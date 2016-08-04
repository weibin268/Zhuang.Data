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
        internal const string LEFT_BRACE_DUBLE_QUESTION_MARK = "{??";//视值为“null”、“DBNull”和“空白字符串”的参数为“有效”参数
        internal const string LEFT_BRACE_QUESTION_MARK = "{?";//视值为“null”、“DBNull”和“空白字符串”的参数为“无效”参数
        internal const string RIGHT_BRACE = "}";

        private Stack _removeClauseStack = new Stack();

        public void Parse(SqlCommand sqlCommand)
        {
            if (!IsCanParse(sqlCommand.Text)) return;

            RecursiveFindDynamicClause(RegexPattern.DynamicClausePattern.Matches(sqlCommand.Text), sqlCommand);

            foreach (var remove in _removeClauseStack)
            {
                sqlCommand.Text = sqlCommand.Text.Replace(remove.ToString(), "");
            }

            sqlCommand.Text = sqlCommand.Text.Replace(LEFT_BRACE_DUBLE_QUESTION_MARK, "").Replace(LEFT_BRACE_QUESTION_MARK, "").Replace(RIGHT_BRACE, "");
        }

        private string RecursiveFindDynamicClause(MatchCollection matchs, SqlCommand sqlCommand)
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
                        //当值不为“null”、“DBNull”和“空白字符串”时参数值才算是有效
                        if (c.Value != null && c.Value != DBNull.Value && c.Value.ToString().Trim() != string.Empty)
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
                    _removeClauseStack.Push(result);
                }
            }
            return result;
        }

        private bool IsCanParse(string strSql)
        {
            return strSql.Contains(LEFT_BRACE_QUESTION_MARK);
        }

    }
}
