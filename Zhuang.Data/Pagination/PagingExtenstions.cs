using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using Zhuang.Data.Pagination;

namespace Zhuang.Data
{
    public static class PagingExtenstions
    {
        public static DataTable PageQueryDataTable(this DbAccessor db,
            string sql, string orderClause, int pageIndex, int rowCount, out int totalRowCount,
            object objParameters = null)
        {
            DbPaging paging = DbPagingFactory.GetDbPaging(db);

            if (objParameters == null)
            {
                object objTotalRowCount = db.ExecuteScalar<object>(paging.GetCountSql(sql));
                totalRowCount = Convert.ToInt32(objTotalRowCount);
                int startRowIndex = 1 + (pageIndex - 1) * rowCount;
                return db.QueryDataTable(paging.GetPageSql(sql, orderClause, startRowIndex, rowCount));
            }
            else
            {
                object objTotalRowCount = db.ExecuteScalar<object>(paging.GetCountSql(sql), objParameters);
                totalRowCount = Convert.ToInt32(objTotalRowCount);
                int startRowIndex = 1 + (pageIndex - 1) * rowCount;
                return db.QueryDataTable(paging.GetPageSql(sql, orderClause, startRowIndex, rowCount), objParameters);
            }
        }

        public static IList<T> PageQueryEntities<T>(this DbAccessor db,
            string sql, string orderClause, int pageIndex, int rowCount, out int totalRowCount,
            object objParameters = null)
        {
            DbPaging paging = DbPagingFactory.GetDbPaging(db);

            if (objParameters == null)
            {
                object objTotalRowCount = db.ExecuteScalar<object>(paging.GetCountSql(sql));
                totalRowCount = Convert.ToInt32(objTotalRowCount);
                int startRowIndex = 1 + (pageIndex - 1) * rowCount;
                return db.QueryEntities<T>(paging.GetPageSql(sql, orderClause, startRowIndex, rowCount));
            }
            else
            {
                object objTotalRowCount = db.ExecuteScalar<object>(paging.GetCountSql(sql), objParameters);
                totalRowCount = Convert.ToInt32(objTotalRowCount);
                int startRowIndex = 1 + (pageIndex - 1) * rowCount;
                return db.QueryEntities<T>(paging.GetPageSql(sql, orderClause, startRowIndex, rowCount), objParameters);
            }
        }

    }
}
