using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace Zhuang.Data.DbProviders.SqlServer
{
    public class SqlServerAccessor : DbAccessor
    {
        public SqlServerAccessor(string connectionString)
            : base(SqlClientFactory.Instance, connectionString)
        {

        }
    }
}
