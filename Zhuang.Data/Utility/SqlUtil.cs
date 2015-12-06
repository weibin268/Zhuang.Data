using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Text.RegularExpressions;

namespace Zhuang.Data.Utility
{
    public static class SqlUtil
    {
        internal const string PARAMETER_NAME_PREFIX_AT = "@";
        internal const string PARAMETER_NAME_PREFIX_COLON = ":";

        public static DbType GetDbTypeByType(Type type)
        {
            DbType dbType;

            if (type == typeof(bool))
            {
                dbType = DbType.Boolean;
            }
            else if (type == typeof(Nullable<bool>))
            {
                dbType = DbType.Boolean;
            }
            else if (type == typeof(decimal))
            {
                dbType = DbType.Decimal;
            }
            else if (type == typeof(double))
            {
                dbType = DbType.Double;
            }
            else if (type == typeof(string))
            {
                dbType = DbType.String;
            }
            else if (type == typeof(Int16))
            {
                dbType = DbType.Int16;
            }
            else if (type == typeof(Int32))
            {
                dbType = DbType.Int32;
            }
            else if (type == typeof(Nullable<Int32>))
            {
                dbType = DbType.Int32;
            }
            else if (type == typeof(Int64))
            {
                dbType = DbType.Int64;
            }
            else if (type == typeof(DateTime))
            {
                dbType = DbType.DateTime;
            }
            else if (type == typeof(Nullable<DateTime>))
            {
                dbType = DbType.DateTime;
            }
            else if (type == typeof(Guid))
            {
                dbType = DbType.Guid;
            }
            else
            {
                dbType = DbType.Object;
            }

            return dbType;
        }

        public static object ConvertDbFieldValueByEntityPropertyType(object dbFieldValue,Type entityPropertyType)
        {
            if (dbFieldValue == null) goto End;

            if (dbFieldValue.GetType() == typeof(Int64) &&
                (entityPropertyType == typeof(Int32) || entityPropertyType == typeof(Nullable<Int32>)))
            {
                return Convert.ToInt32(dbFieldValue);
            }

            End:
            return dbFieldValue;
        }

        public static string RemoveOrderByClause(string sql)
        {
            Regex regex = new Regex("[{]\\s*[?]\\s*order\\s+by[\\d|\\w|\\s|$|#|@|:|-|_]*[}]|order\\s+by[\\d|\\w|\\s|$|#|@|:|-|_]*", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            sql = regex.Replace(sql.Trim(), string.Empty);
            return sql;
        }

        public static string GetSqlFromDbCommand(DbCommand dbCommand)
        {
            string strSql = dbCommand.CommandText;

            foreach (DbParameter dp in dbCommand.Parameters)
            {
                string strReplace = string.Empty;

                if (dp.Value == DBNull.Value)
                {
                    strReplace = "NULL";
                }
                else if (dp.DbType == DbType.Int32)
                {
                    strReplace = string.Format("{0}", dp.Value);
                }
                else
                {
                    strReplace = string.Format("'{0}'", dp.Value);
                }

                strSql = strSql.Replace(PARAMETER_NAME_PREFIX_AT + dp.ParameterName, strReplace);
                strSql = strSql.Replace(PARAMETER_NAME_PREFIX_COLON + dp.ParameterName, strReplace);
            }

            return strSql;
        }

        public static bool ContainsParameterName(string strSql, string parameterName)
        {
            strSql = strSql.ToLower();
            parameterName = parameterName.ToLower()
                .Replace(PARAMETER_NAME_PREFIX_COLON,"").Replace(PARAMETER_NAME_PREFIX_AT,"");

            if (strSql.Contains(PARAMETER_NAME_PREFIX_COLON + parameterName) || strSql.Contains(PARAMETER_NAME_PREFIX_AT + parameterName))
                return true;
            else
                return false;
        }
    }
}
