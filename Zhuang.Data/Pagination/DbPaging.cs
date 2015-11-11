using System;
using System.Data;
using System.Data.Common;
using Zhuang.Data.SqlCommands;
using Zhuang.Data.SqlCommands.Store;
using Zhuang.Data.Utility;

namespace Zhuang.Data.Pagination
{

    public class PagingOptions
    {
        public string Sql { get; set; }
        public string OrderClause { get; set; }
        public int StartRowIndex { get; set; }
        public int RowCount { get; set; }
    }


    /// <summary> 
    /// 根据各种不同数据库生成不同分页语句的辅助类 Paging
    /// </summary> 
    public abstract class DbPaging
    {
        public abstract string GetPageSql(string sql, string orderClause, int startRowIndex, int rowCount);

        public string GetCountSql(string strSql)
        {
            strSql = RetrieveSql(strSql);

            string tempSql = SqlUtil.RemoveOrderByClause(strSql);
            return " select count(1) from (\n" + tempSql + "\n) tt";
        }

        protected string RetrieveSql(string strSql)
        {
            SqlCommand sqlCmd = SqlCommandRepository.Instance.GetSqlCommand(strSql);

            if (sqlCmd != null)
            {
                strSql = sqlCmd.Text;
            }

            return strSql;
        }
    }
} 
