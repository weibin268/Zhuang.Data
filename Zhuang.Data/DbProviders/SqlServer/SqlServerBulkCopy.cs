using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using Zhuang.Data.BulkCopy;
using Zhuang.Data.Common;

namespace Zhuang.Data.DbProviders.SqlServer
{
    public class SqlServerBulkCopy : DbBulkCopy
    {
        public SqlServerBulkCopy(string connectionString) : base(connectionString)
        { }

        public override void WriteToServer(string destinationTableName, DataTable table,
            int batchSize = 0, params SqlBulkCopyColumnMapping[] columnMappings)
        {
            SqlBulkCopy bulkCopy = null;
            try
            {
                if (DbTransaction != null)
                {
                    bulkCopy = new SqlBulkCopy((SqlConnection)DbTransaction.Connection, SqlBulkCopyOptions.Default, (SqlTransaction)DbTransaction);
                }
                else
                {
                    bulkCopy = new SqlBulkCopy(_connectionString);
                }

                bulkCopy.DestinationTableName = destinationTableName;
                if (batchSize != 0)
                {
                    bulkCopy.BatchSize = batchSize;
                }
                if (BulkCopyTimeout != (int)CommandTimeoutValue.None)
                {
                    bulkCopy.BulkCopyTimeout = BulkCopyTimeout;
                }
                if (columnMappings != null)
                {
                    foreach (var map in columnMappings)
                    {
                        bulkCopy.ColumnMappings.Add(map);
                    }
                }
                bulkCopy.WriteToServer(table);
            }
            finally
            {
                if (bulkCopy != null) { bulkCopy.Close(); }
            }
        }
    }
}
