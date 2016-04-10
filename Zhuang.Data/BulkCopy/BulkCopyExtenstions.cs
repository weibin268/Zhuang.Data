using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Zhuang.Data.BulkCopy;

namespace Zhuang.Data
{
    public static class BulkCopyExtenstions
    {
        public static void BulkWriteToServer(this DbAccessor db, DataTable table, int batchSize = 0, params BulkCopyColumnMapping[] columnMappings)
        {
            DbBulkCopy dbBulkCopy = DbBulkCopyFactory.GetDbBulkCopy(db, db.ConnectionString);
            dbBulkCopy.BulkCopyTimeout = db.CommandTimeout;
            dbBulkCopy.DbTransaction = db.DbTransaction;
            dbBulkCopy.WriteToServer(table.TableName, table, batchSize, columnMappings);
        }

        public static DataTable BulkGetEmptyDataTable(this DbAccessor db, string tableName)
        {
            DataTable dt = db.QueryDataTable(string.Format("select * from {0} where 1=2", tableName));
            dt.TableName = tableName;
            return dt;
        }
    }
}
