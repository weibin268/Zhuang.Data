using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Zhuang.Data.Common;
using Zhuang.Data.Utility;

namespace Zhuang.Data.SqlCommands.Parser
{
    /*
    string strSql = @"select * from sys_product 
    where 1=1 {? and ProductId=@ProductId {?? or ProductId=@ProductId {? or ProductId=@ProductId}}} {?? and ProductName=@ProductName} ";
    */

    public class DynamicClauseParser : ISqlCommandParser
    {
        internal const string LEFT_BRACE_DUBLE_QUESTION_MARK = "{??";//视值为null或DBNull参数为“有效”参数
        internal const string LEFT_BRACE_QUESTION_MARK = "{?";//视值为null或DBNull参数“无效”参数
        internal const string RIGHT_BRACE = "}";

        private Stack removeClauseStack = new Stack();

        public SqlCommand Parse(SqlCommand rawSqlCommand)
        {
            RecursiveFindDynamicClause(RegexPattern.DynamicClausePattern.Matches(rawSqlCommand.Text), rawSqlCommand);

            foreach (var remove in removeClauseStack)
            {
                rawSqlCommand.Text = rawSqlCommand.Text.Replace(remove.ToString(), "");
            }

            rawSqlCommand.Text = rawSqlCommand.Text.Replace(LEFT_BRACE_DUBLE_QUESTION_MARK, "").Replace(LEFT_BRACE_QUESTION_MARK, "").Replace(RIGHT_BRACE, "");
            return rawSqlCommand;
        }

        public string RecursiveFindDynamicClause(MatchCollection matchs, SqlCommand sqlCommand)
        {
            string result = string.Empty;
            if (matchs.Count < 1)
                return result;

            foreach (Match match in matchs)
            {
                result = match.Value;
                string reResult = RecursiveFindDynamicClause(RegexPattern.DynamicClausePattern.Matches(match.Groups["DynamicClause"].Value), sqlCommand);
                if (!sqlCommand.Parameters.Exists((c) =>
                {
                    bool r = false;
                    string tempResult = string.IsNullOrEmpty(reResult) ? result : result.Replace(reResult, "");
                    if (SqlUtil.ContainsParameterName(tempResult, c.ParameterName))
                    {
                        if (c.Value != null && c.Value != DBNull.Value)
                        {
                            r = true;
                        }

                        if (result.StartsWith(LEFT_BRACE_DUBLE_QUESTION_MARK))
                        {
                            r = true;
                        }
                    }
                    return r;
                }))
                {
                    removeClauseStack.Push(result);
                }
            }
            return result;
        }

        public static bool IsCanParse(string strSql)
        {
            return strSql.Contains(LEFT_BRACE_QUESTION_MARK);
        }

    }
}
