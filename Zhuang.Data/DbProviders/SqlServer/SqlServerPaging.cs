using System;
using System.Collections.Generic;
using System.Text;
using Zhuang.Data.Pagination;
using Zhuang.Data.Utility;

namespace Zhuang.Data.DbProviders.SqlServer
{
    public class SqlServerPaging : DbPaging
    {

        public SqlServerPaging()
        {

        }

        public override string GetPageSql(string sql, string orderClause, int startRowIndex, int rowCount)
        {
            sql = SqlUtil.RetrieveSql(sql);
            sql = SqlUtil.RemoveOrderByClause(sql);

            StringBuilder stringBuilder = new StringBuilder(sql.Length + 100);
            if (!string.IsNullOrEmpty(orderClause))
            {
                orderClause = "order by " + orderClause;
                stringBuilder.Append("select * from (select row_number() over(").Append(orderClause).Append(") as rownum,* from (\n");
                stringBuilder.Append(sql);
                stringBuilder.Append("\n ) as _table1) as _table2 where (rownum <=").Append(startRowIndex + rowCount - 1).Append(" and rownum>= ").Append(startRowIndex).Append(" ) ");

                return stringBuilder.ToString();
            }
            throw new Exception("Paged query must set orderBy Clause");
        }
    }
}
