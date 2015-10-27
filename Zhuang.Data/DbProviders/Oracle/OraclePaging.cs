using System;
using System.Collections.Generic;
using System.Text;
using Zhuang.Data.Pagination;
using Zhuang.Data.Utility;

namespace Zhuang.Data.DbProviders.Oracle
{
    public class OraclePaging : DbPaging
    {
        public override string GetPageSql(string sql, string orderClause, int startRowIndex, int rowCount)
        {
            sql = SqlUtil.RemoveOrderByClause(sql);
            StringBuilder stringBuilder = new StringBuilder(sql.Length + 100);
            stringBuilder.Append("select * from (select * from (select row_.*, rownum rownum_ from ( ");
            stringBuilder.Append(sql);
            if (!string.IsNullOrEmpty(orderClause))
            {
                stringBuilder.Append(" Order By ").Append(orderClause);
            }
            stringBuilder.Append("\n ) row_) where rownum_ <= ").Append(startRowIndex + rowCount - 1).Append(") where rownum_ >= ").Append(startRowIndex);
            return stringBuilder.ToString();
        }
    }
}
