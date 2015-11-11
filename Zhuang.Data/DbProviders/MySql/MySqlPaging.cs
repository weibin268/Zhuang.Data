using System;
using System.Collections.Generic;
using System.Text;
using Zhuang.Data.Pagination;
using Zhuang.Data.Utility;

namespace Zhuang.Data.DbProviders.MySql
{
    public class MySqlPaging : DbPaging
    {
        public override string GetPageSql(string sql, string orderClause, int startRowIndex, int rowCount)
        {
            sql = RetrieveSql(sql);

            sql = SqlUtil.RemoveOrderByClause(sql);
            StringBuilder stringBuilder = new StringBuilder(sql.Length + 100);
            if (!string.IsNullOrEmpty(orderClause))
            {
                orderClause = "order by " + orderClause;
                stringBuilder.Append(sql).Append(" ").Append(orderClause).Append(" ").Append("limit ")
                    .Append(startRowIndex - 1).Append(",").Append(rowCount);
                return stringBuilder.ToString();
            }
            throw new Exception("Paged query must set orderBy Clause");
        }
    }
}
