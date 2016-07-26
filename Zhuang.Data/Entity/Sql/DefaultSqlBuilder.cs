using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Zhuang.Data.Entity.Mapping;
using Zhuang.Data.Exceptions;

namespace Zhuang.Data.Entity.Sql
{
    public class DefaultSqlBuilder : ISqlBuilder
    {
        private TableMapping _tableMapping;

        public DefaultSqlBuilder(TableMapping tableMapping)
        {
            _tableMapping = tableMapping;
        }

        public string BuildSelect()
        {
            StringBuilder sbSql = new StringBuilder();
            var selectColumns = _tableMapping.Columns;
            var keyColumns = _tableMapping.GetKeyColumns();
            var lsSelect = new List<string>();
            var lsWhere = new List<string>();

            if (keyColumns.Count < 1)
                throw new EntityException("实体没有设置主键！");

            foreach (var col in selectColumns)
            {
                lsSelect.Add(string.Format("{0} as {1}", col.ColumnName, col.PropertyName));
            }

            foreach (var keyCol in keyColumns)
            {
                lsWhere.Add(string.Format("{0}=#{1}#", keyCol.ColumnName, keyCol.PropertyName));
            }

            sbSql.Append(string.Format("Select {0} \n from {1} ",
                    string.Join(" , ", lsSelect.ToArray()), _tableMapping.TableName))
                .Append(" Where ")
                .Append(string.Join(" And ", lsWhere.ToArray()));

            return sbSql.ToString();
        }

        public string BuildSelectList()
        {
            StringBuilder sbSql = new StringBuilder();
            var selectColumns = _tableMapping.Columns;
            var wherecolumns = _tableMapping.Columns;

            var lsSelect = new List<string>();
            var lsWhere = new List<string>();


            foreach (var col in selectColumns)
            {
                lsSelect.Add(string.Format("{0} as {1}", col.ColumnName, col.PropertyName));
            }

            lsWhere.Add("1=1");
            foreach (var col in wherecolumns)
            {
                lsWhere.Add("{? " + string.Format("and {0}=#{1}#", col.ColumnName, col.PropertyName) + " }");
            }

            sbSql.Append(string.Format("Select {0} \n from {1} ",
                    string.Join(" , ", lsSelect.ToArray()), _tableMapping.TableName))
                .Append(" Where ")
                .Append(string.Join(" ", lsWhere.ToArray()));

            return sbSql.ToString();
        }

        public string BuildDelete()
        {
            StringBuilder sbSql = new StringBuilder();
            var keyColumns = _tableMapping.GetKeyColumns();
            var lsWhere = new List<string>();
            if (keyColumns.Count < 1)
                throw new EntityException("实体没有设置主键！");


            foreach (var keyCol in keyColumns)
            {
                lsWhere.Add(string.Format("{0}=#{1}#", keyCol.ColumnName, keyCol.PropertyName));
            }

            sbSql.Append("delete from ")
                .Append(_tableMapping.TableName)
                .Append(" where ")
                .Append(string.Join(" And ", lsWhere.ToArray()));

            return sbSql.ToString();
        }

        public string BuildInsert()
        {
            StringBuilder sbSql = new StringBuilder();
            var insertColumns = _tableMapping.GetInsertColumns();
            var lsCoumnNames = new List<string>();
            var lsParameterNames = new List<string>();

            if (insertColumns.Count < 1)
                throw new EntityException("实体没有对应要插入到数据的属性！");

            foreach (var col in insertColumns)
            {
                lsCoumnNames.Add(col.ColumnName);
                lsParameterNames.Add(string.Format("#{0}#", col.PropertyName));
            }

            sbSql.AppendLine(string.Format("Insert into {0}({1})",
                _tableMapping.TableName,
                string.Join(",", lsCoumnNames.ToArray())))
                .Append(string.Format("values({0})",
                string.Join(",", lsParameterNames.ToArray())));


            return sbSql.ToString();
        }

        public string BuildUpdate()
        {
            StringBuilder sbSql = new StringBuilder();
            var updateColumns = _tableMapping.GetUpdateColumns();
            var keyColumns = _tableMapping.GetKeyColumns();
            var lsUpdateSet = new List<string>();
            var lsWhere = new List<string>();

            if (keyColumns.Count < 1)
                throw new EntityException("实体没有设置主键！");

            if (updateColumns.Count < 1)
                throw new EntityException("实体没有对应要更新到数据的属性！");

            foreach (var col in updateColumns)
            {
                lsUpdateSet.Add(string.Format("{0}=#{1}#", col.ColumnName, col.PropertyName));
            }

            foreach (var keyCol in keyColumns)
            {
                lsWhere.Add(string.Format("{0}=#{1}#", keyCol.ColumnName, keyCol.PropertyName));
            }

            sbSql.Append(string.Format("Update {0} set ", _tableMapping.TableName))
                .AppendLine(string.Join(" , ", lsUpdateSet.ToArray()))
                .Append(" Where ")
                .Append(string.Join(" And ", lsWhere.ToArray()));


            return sbSql.ToString();
        }
    }
}
