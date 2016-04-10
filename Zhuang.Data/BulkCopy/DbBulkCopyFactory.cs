using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Zhuang.Data.DbProviders.MySql;
using Zhuang.Data.DbProviders.Oracle;
using Zhuang.Data.DbProviders.SqlServer;

namespace Zhuang.Data.BulkCopy
{
    public class DbBulkCopyFactory
    {
        public static DbBulkCopy GetDbBulkCopy(DbAccessor dba,string connectionString)
        {
            DbBulkCopy dbBulkCopy = null;

            if (dba.GetType() == typeof(SqlServerAccessor))
            {
                dbBulkCopy = new SqlServerBulkCopy(connectionString);
            }
            else if (dba.GetType() == typeof(OracleAccessor))
            {
                dbBulkCopy = new Oracle_BulkCopy(connectionString);
            }

            if (dbBulkCopy != null)
            {
                return dbBulkCopy;
            }
            else
            {
                throw new Exception(string.Format("“{0}”没有对应的DbBulkCopy实现！", dba.GetType()));
            }


        }
    }
}
