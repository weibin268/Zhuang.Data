using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace Zhuang.Data.DbProviders.MySql
{
    public class MySqlAccessor : DbAccessor
    {

        public MySqlAccessor(string connectionString)
            : base(MySqlClientFactory.Instance, connectionString)
        {
        }

    }
}
