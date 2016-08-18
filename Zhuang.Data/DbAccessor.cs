using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text;
using Zhuang.Data.Common;
using Zhuang.Data.Utility;
using Zhuang.Data.Extensions;

namespace Zhuang.Data
{

    #region 委托定义
    public delegate T CommandExecuteHandler<T>(DbCommand cmd);
    public delegate void ExecuteEventHandler(DbAccessorContext context);
    #endregion

    public abstract class DbAccessor : IDisposable
    {

        protected DbProviderFactory _dbProviderFactory;
        private DbTransaction _dbTransaction;
        private int _commandTimeout = (int)CommandTimeoutValue.None;
        private string _connectionString = string.Empty;
        private DbProviderName _dbProviderName;

        public event ExecuteEventHandler PreCommandExecute;
        public event ExecuteEventHandler AfterCommandExecute;

        public bool IsSingleton { get; set; }

        public bool Transactional
        {
            get { return _dbTransaction == null ? false : true; }
        }

        public DbProviderFactory DbProviderFactory
        {
            get
            {
                return _dbProviderFactory;
            }

            set
            {
                _dbProviderFactory = value;
            }
        }

        public DbTransaction DbTransaction
        {
            get
            {
                return _dbTransaction;
            }

            set
            {
                _dbTransaction = value;
            }
        }

        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }
        }

        public int CommandTimeout
        {
            get
            {
                return _commandTimeout;
            }

            set
            {
                _commandTimeout = value;
            }
        }

        public DbProviderName DbProviderName { get { return _dbProviderName; } }

        public DbAccessor(DbProviderFactory dbProviderFactory, string connectionString, DbProviderName dbProviderName)
        {
            IsSingleton = false;
            _dbProviderFactory = dbProviderFactory;
            _connectionString = connectionString;
            _dbProviderName = dbProviderName;
        }

        public void ResetCommandTimeout()
        {
            _commandTimeout = (int)CommandTimeoutValue.None;
        }

        private T Execute<T>(CommandType commandType, string strSql, CommandExecuteHandler<T> commandExecuteHandler, object objParameters = null)
        {
            DbParameter[] dbParameters = objParameters == null ? null : GetDbParameters(objParameters);

            DbConnection conn = GetDbConnection();
            DbCommand cmd = null;
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                cmd = conn.CreateCommand();

                cmd.CommandType = commandType;

                cmd.CommandText = strSql;

                if (_commandTimeout != (int)CommandTimeoutValue.None)
                {
                    cmd.CommandTimeout = _commandTimeout;
                }

                if (Transactional)
                {
                    cmd.Transaction = _dbTransaction;
                }

                if (dbParameters != null)
                {
                    cmd.Parameters.AddRange(dbParameters);
                }

                DbAccessorContext context = new DbAccessorContext(this, cmd);

                if (PreCommandExecute != null)
                {
                    PreCommandExecute.Invoke(context);
                }

                T result = commandExecuteHandler(cmd);

                if (AfterCommandExecute != null)
                {
                    AfterCommandExecute.Invoke(context);
                }

                return result;
            }
            finally
            {
                if (cmd != null) { cmd.Dispose(); }

                if (!Transactional && !(typeof(T) == typeof(DbDataReader)))
                {
                    conn.Dispose();
                }
            }
        }

        public int ExecuteNonQuery(string strSql, object objParameters = null)
        {
            return Execute<int>(CommandType.Text, strSql,
                delegate (DbCommand cmd)
                {
                    return cmd.ExecuteNonQuery();
                }, objParameters);
        }

        public DbDataReader ExecuteReader(string strSql, object objParameters = null)
        {
            return Execute<DbDataReader>(CommandType.Text, strSql,
                delegate (DbCommand cmd)
                {
                    if (Transactional)
                    {
                        return cmd.ExecuteReader();
                    }
                    else
                    {
                        return cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    }

                }, objParameters);
        }

        public T ExecuteScalar<T>(string strSql, object objParameters = null)
        {
            return Execute<T>(CommandType.Text, strSql,
            delegate (DbCommand cmd)
            {
                return (T)cmd.ExecuteScalar();
            }, objParameters);
        }

        public DataSet QueryDataSet(CommandType commandType, string strSql, object objParameters = null)
        {
            return Execute<DataSet>(commandType, strSql,
             delegate (DbCommand cmd)
             {

                 using (DbDataAdapter da = GetDbDataAdapter(cmd))
                 {
                     DataSet result = new DataSet();
                     da.Fill(result);
                     return result;
                 }

             }, objParameters);

        }

        public DataSet QueryDataSet(string strSql, object objParameters = null)
        {
            return QueryDataSet(CommandType.Text, strSql, objParameters);
        }

        public DataTable QueryDataTable(string strSql, object objParameters = null)
        {
            return QueryDataSet(strSql, objParameters).Tables[0];
        }

        public DataSet QueryProcedure(string spName, object objParameters = null)
        {
            return Execute<DataSet>(CommandType.StoredProcedure, spName,
             delegate (DbCommand cmd)
             {
                 using (DbDataAdapter da = GetDbDataAdapter(cmd))
                 {
                     DataSet result = new DataSet();
                     da.Fill(result);
                     return result;
                 }

             }, objParameters);
        }

        public int ExecuteProcedure(string spName, object objParameters = null)
        {
            return Execute<int>(CommandType.StoredProcedure, spName,
            delegate (DbCommand cmd)
            {
                return cmd.ExecuteNonQuery();
            }, objParameters);
        }

        public IList<T> QueryEntities<T>(string strSql, object objParameters = null)
        {
            IList<T> lsResult = new List<T>();
            using (IDataReader dr = ExecuteReader(strSql, objParameters))
            {
                lsResult = dr.ReadEntities<T>();
            }
            return lsResult;
        }

        public T QueryEntity<T>(string strSql, object objParameters = null)
        {
            T entity = default(T);
            using (IDataReader dr = ExecuteReader(strSql, objParameters))
            {
                entity = dr.ReadEntity<T>();
            }
            return entity;
        }

        public IDictionary<string, object> QueryDictionary(string strSql, object objParameters = null)
        {
            IDictionary<string, object> result;

            using (IDataReader dr = ExecuteReader(strSql, objParameters))
            {
                result  = dr.ReadDictionary();
            }

            return result;
        }

        public IList<IDictionary<string, object>> QueryDictionaries(string strSql, object objParameters = null)
        {
            IList<IDictionary<string, object>> result;

            using (IDataReader dr = ExecuteReader(strSql, objParameters))
            {
                result = dr.ReadDictionaries();
            }

            return result;
        }


        public DbParameter[] GetDbParameters(object objParameters)
        {
            if (objParameters is DbParameter[])
            {
                return (DbParameter[])objParameters;
            }

            List<DbParameter> lsParameters = new List<DbParameter>();

            if (objParameters == null)
                throw new ArgumentNullException("objParameters");

            if (objParameters.GetType() == typeof(Dictionary<string, object>))
            {
                #region Dictionary Object
                Dictionary<string, object> dicParameters = (Dictionary<string, object>)objParameters;

                foreach (var key in dicParameters.Keys)
                {
                    DbParameter dp = _dbProviderFactory.CreateParameter();

                    dp.ParameterName = key;
                    dp.Value = dicParameters[key] == null ? DBNull.Value : dicParameters[key];
                    dp.DbType = SqlUtil.GetDbTypeByType(dp.Value.GetType());

                    lsParameters.Add(dp);
                }
                #endregion
            }
            else
            {
                #region Entity Object
                Type tParameters = objParameters.GetType();
                PropertyInfo[] tProperties = tParameters.GetProperties();

                foreach (PropertyInfo pi in tProperties)
                {
                    DbParameter dp = _dbProviderFactory.CreateParameter();
                    dp.DbType = SqlUtil.GetDbTypeByType(pi.PropertyType);
                    dp.ParameterName = pi.Name;
                    dp.Value = pi.GetValue(objParameters, null);

                    lsParameters.Add(dp);
                }
                #endregion

            }

            return lsParameters.ToArray();
        }

        private DbConnection GetDbConnection()
        {
            DbConnection dbConnection;
            if (!Transactional)
            {
                dbConnection = _dbProviderFactory.CreateConnection();
                dbConnection.ConnectionString = ConnectionString;
            }
            else
            {
                dbConnection = _dbTransaction.Connection;
            }
            return dbConnection;
        }

        private DbDataAdapter GetDbDataAdapter(DbCommand cmd)
        {
            DbDataAdapter da = _dbProviderFactory.CreateDataAdapter();
            da.SelectCommand = cmd;
            //da.DeleteCommand = cmd;
            //da.InsertCommand = cmd;
            //da.UpdateCommand = cmd;
            return da;
        }


        public void BeginTran()
        {
            if (IsSingleton)
                throw new Exception("单例不充许使用事务！");

            if (!Transactional)
            {
                DbConnection dbConnection = GetDbConnection();
                dbConnection.Open();
                _dbTransaction = dbConnection.BeginTransaction();
                //_tran = dbConnection.BeginTransaction(IsolationLevel.ReadCommitted);
            }
        }

        public void CommitTran()
        {
            if (Transactional)
            {
                var conn = _dbTransaction.Connection;
                _dbTransaction.Commit();
                conn.Dispose();
                Dispose();
            }
            else
            {
                throw new Exception("没有要提交的事务！");
            }
        }

        public void RollbackTran()
        {
            if (Transactional)
            {
                var conn = _dbTransaction.Connection;
                _dbTransaction.Rollback();
                conn.Dispose();
                Dispose();
            }
            else
            {
                throw new Exception("没有要回滚的事务！");
            }
        }

        public void Dispose()
        {
            if (Transactional)
            {
                var conn = _dbTransaction.Connection;
                _dbTransaction.Dispose();
                if (conn != null)
                {
                    conn.Dispose();
                }
                _dbTransaction = null;
            }
        }


        public static DbAccessor Get()
        {
            return DbAccessorFactory.GetDbAccessor();
        }

        public static DbAccessor Create()
        {
            return DbAccessorFactory.CreateDbAccessor();
        }
    }
}
