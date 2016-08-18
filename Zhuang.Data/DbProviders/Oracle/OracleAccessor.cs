using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zhuang.Data.DbProviders.Oracle
{
    public class OracleAccessor : DbAccessor
    {
        public OracleAccessor(string connectionString)
            : base(OracleClientFactory.Instance, connectionString, Common.DbProviderName.Oracle)
        {
        }

    }
}
