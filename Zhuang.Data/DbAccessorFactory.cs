using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using Zhuang.Data.Common;
using Zhuang.Data.DbProviders.MySql;
using Zhuang.Data.DbProviders.Oracle;
using Zhuang.Data.DbProviders.SqlServer;
using Zhuang.Data.EnvironmentVariable;
using Zhuang.Data.Handlers;

namespace Zhuang.Data
{
    public static class DbAccessorFactory
    {
        public static string DefaultDbName
        {
            get {
                string defaultDbName = ConfigurationManager.AppSettings[AppSettingsKey.DefaultDbName];
                return defaultDbName == null ? "DefaultDb" : defaultDbName;
            }
        }


        public static string ProviderName
        {
            get
            {
                string providerName = ConfigurationManager.AppSettings[AppSettingsKey.ProviderName];
                return providerName;
            }
        }

        private static DbAccessor _dba;

        private static object _objLock = new object();

        public static DbAccessor GetDbAccessor()
        {
            if (_dba == null)
            {
                lock (_objLock)
                {
                    if (_dba == null)
                    {
                        _dba = CreateDbAccessor();
                        _dba.IsSingleton = true;
                    }
                }
            }
            return _dba;
        }

        public static DbAccessor CreateDbAccessor()
        {
            return CreateDbAccessor(DefaultDbName);
        }

        public static DbAccessor CreateDbAccessor(string name)
        {
            ConnectionStringSettings csSettings = ConfigurationManager.ConnectionStrings[name];

            if (csSettings == null)
                throw new Exception(string.Format("请检查配置文件的数据库连接配置，找不到名称为{0}的ConnectionString！", name));

            if (ProviderName != null)
                csSettings.ProviderName = ProviderName;

            return CreateDbAccessor(csSettings.ConnectionString, csSettings.ProviderName);

        }

        public static DbAccessor CreateDbAccessor(string connectionString, string providerName)
        {
            DbAccessor dba = NewDbAccessor(connectionString, providerName);

            if (dba != null)
            {
                IEnumerable<IDbExecuteHandler> dbExecuteHandlers = HandlerFactory.GetDbExecuteHandlers();

                foreach (IDbExecuteHandler handler in dbExecuteHandlers)
                {
                    dba.PreCommandExecute += handler.HandleExecute;
                }
            }

            return dba;
        }

        public static DbAccessor NewDbAccessor(string connectionString, string providerName)
        {
            DbAccessor dba = null;

            if (string.IsNullOrEmpty(providerName)
                || providerName == "System.Data.SqlClient"
                || providerName.ToLower() == DbProviderName.SqlServer.ToString().ToLower())
            {
                dba = new SqlServerAccessor(connectionString);
                EvnValRepository.Instance.AddEvnVal(typeof(DbProviderName).FullName, DbProviderName.SqlServer.ToString());
            }
            else if (providerName.ToLower() == DbProviderName.Oracle.ToString().ToLower())
            {
                dba = new OracleAccessor(connectionString);
                EvnValRepository.Instance.AddEvnVal(typeof(DbProviderName).FullName, DbProviderName.Oracle.ToString());
            }
            else if (providerName.ToLower() == DbProviderName.MySql.ToString().ToLower())
            {
                dba = new MySqlAccessor(connectionString);
                EvnValRepository.Instance.AddEvnVal(typeof(DbProviderName).FullName, DbProviderName.MySql.ToString());
            }
            else
            {
                Type tProviderName = Type.GetType(providerName);
                if (tProviderName == null)
                {
                    throw new Exception(string.Format("ConnectionString（{0}）的ProviderName（{1}）找不到该类型！", connectionString, providerName));
                }
                else if (!(tProviderName.IsSubclassOf(typeof(DbAccessor))))
                {
                    throw new Exception(string.Format("ConnectionString（{0}）的ProviderName（{1}）该类型不是DbAccessor的实现类！", connectionString, providerName));
                }
                object oProviderName = Activator.CreateInstance(tProviderName, connectionString);
                dba = oProviderName as DbAccessor;

            }

            return dba;
        }
    }
}
