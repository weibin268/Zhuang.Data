using System;
using System.Collections.Generic;
using System.Text;
using Zhuang.Data.DbProviders.MySql;
using Zhuang.Data.DbProviders.Oracle;
using Zhuang.Data.DbProviders.SqlServer;

namespace Zhuang.Data.Pagination
{
    public class DbPagingFactory
    {
        public static DbPaging GetDbPaging(DbAccessor dba)
        {
            DbPaging paging = null;

            if (dba.GetType() == typeof(SqlServerAccessor))
            {
                paging = new SqlServerPaging();
            }
            else if (dba.GetType() == typeof(OracleAccessor))
            {
                paging = new OraclePaging();
            }
            else if (dba.GetType() == typeof(MySqlAccessor))
            {
                paging = new MySqlPaging();
            }

            if (paging != null)
            {
                return paging;
            }
            else
            {
                throw new Exception(string.Format("“{0}”没有对应的分页实现！", dba.GetType()));
            }


        }
    }
}
