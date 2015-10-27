using System;
using System.Collections.Generic;
using System.Text;

namespace Zhuang.Data.SqlCommands.Store
{
    public class SqlCommandRepository
    {
        private Dictionary<string, SqlCommand> _dicSqlCommands;
        private IList<ISqlCommandStoreProvider> _storeProviders ;
        private static SqlCommandRepository _instance;
        private static object _objLock = new object();

        public static SqlCommandRepository Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_objLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new SqlCommandRepository();
                            var configFilesProvider = new ConfigFilesProvider();
                            configFilesProvider.EnableFileSystemWatcher();
                            _instance.AddStoreProvider(configFilesProvider);
                            _instance.AddStoreProvider(new AssemblyResourceProvider());
                        }
                    }
                }
                return _instance;
            }
        }

        public void AddStoreProvider(ISqlCommandStoreProvider storeProvider)
        {
            if (storeProvider == null) return;

            _storeProviders.Add(storeProvider);

            var sqlCmds = storeProvider.GetSqlCommands();
            foreach (var sqlcmd in sqlCmds)
            {
                if (!_dicSqlCommands.ContainsKey(sqlcmd.Key))
                {
                    _dicSqlCommands.Add(sqlcmd.Key, sqlcmd);
                }
            }
        }

        public SqlCommandRepository()
        {
            _dicSqlCommands = new Dictionary<string, SqlCommand>();
            _storeProviders = new List<ISqlCommandStoreProvider>();
        }

        public SqlCommand GetSqlCommand(string key)
        {
            if (_dicSqlCommands.ContainsKey(key))
            {
                return _dicSqlCommands[key];
            }
            else
            {
                return null;
            }
        }

        public void AddSqlCommand(SqlCommand sqlCmd)
        {
            _dicSqlCommands.Add(sqlCmd.Key, sqlCmd);
        }

        public void AddOrReplaceSqlCommand(SqlCommand sqlCmd)
        {
            if (_dicSqlCommands.ContainsKey(sqlCmd.Key))
            {
                _dicSqlCommands[sqlCmd.Key] = sqlCmd;
            }
            else
            {
                _dicSqlCommands.Add(sqlCmd.Key,sqlCmd);
            }
        }

    }
}
