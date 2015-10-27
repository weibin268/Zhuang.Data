using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using Zhuang.Data.Common;

namespace Zhuang.Data.BulkCopy
{
    public abstract class DbBulkCopy
    {
        protected string _connectionString;

        public int BulkCopyTimeout { get; set; }

        public DbTransaction DbTransaction { get; set; }

        public DbBulkCopy(string connectionString)
        {
            _connectionString = connectionString;
            BulkCopyTimeout = (int)CommandTimeoutValue.None;
        }

        public abstract void WriteToServer(string destinationTableName, DataTable table, 
            int BatchSize = 0, params SqlBulkCopyColumnMapping[] columnMappings);

    }
}
