using System;
using System.Collections.Generic;
using System.Text;
using Zhuang.Data.Entity.Mapping;
using Zhuang.Data.Entity.Sql;
using System.Reflection;

namespace Zhuang.Data
{
    public static class EntityExtenstions
    {
        public static T Select<T>(this DbAccessor db, object objIdParameters)
        {
            var tableMapping = new TableMapping(typeof(T));
            ISqlBuilder sqlBuilder = new DefaultSqlBuilder(tableMapping);

            #region 处理参数为基础类型，即直接为主键的值
            if (objIdParameters.GetType() == typeof(string) || objIdParameters.GetType().IsPrimitive)
            {
                var dicParam = new Dictionary<string, object>();

                var keyColumns = tableMapping.GetKeyColumns();

                if (keyColumns.Count == 0)
                    throw new Exceptions.EntityException("实体没有设置主键！");

                string keyColumnName = keyColumns[0].ColumnName;

                dicParam.Add(keyColumnName, objIdParameters);
                objIdParameters = dicParam;
            }
            #endregion

            return db.QueryEntity<T>(sqlBuilder.BuildSelect(), objIdParameters);

        }

        public static int Delete(this DbAccessor db, object objEntity)
        {
            ISqlBuilder sqlBuilder = new DefaultSqlBuilder(new TableMapping(objEntity.GetType()));
            return db.ExecuteNonQuery(sqlBuilder.BuildDelete(), objEntity);
        }

        public static int Delete<T>(this DbAccessor db, object objIdParameters)
        {

            var tableMapping = new TableMapping(typeof(T));
            ISqlBuilder sqlBuilder = new DefaultSqlBuilder(tableMapping);

            #region 处理参数为基础类型，即直接为主键的值
            if (objIdParameters.GetType() == typeof(string) || objIdParameters.GetType().IsPrimitive)
            {
                var dicParam = new Dictionary<string, object>();

                var keyColumns = tableMapping.GetKeyColumns();

                if (keyColumns.Count == 0)
                    throw new Exceptions.EntityException("实体没有设置主键！");

                string keyColumnName = keyColumns[0].ColumnName;

                dicParam.Add(keyColumnName, objIdParameters);
                objIdParameters = dicParam;
            }
            #endregion

            return db.ExecuteNonQuery(sqlBuilder.BuildDelete(), objIdParameters);
        }

        public static int Insert(this DbAccessor db, object objEntity)
        {
            ISqlBuilder sqlBuilder = new DefaultSqlBuilder(new TableMapping(objEntity.GetType()));
            return db.ExecuteNonQuery(sqlBuilder.BuildInsert(), objEntity);
        }

        public static int Insert<T>(this DbAccessor db, object objParameters)
        {
            var mapping = new TableMapping(typeof(T));
            mapping.FilterColumn(objParameters);
            ISqlBuilder sqlBuilder = new DefaultSqlBuilder(mapping);
            return db.ExecuteNonQuery(sqlBuilder.BuildInsert(), objParameters);
        }

        public static int Update(this DbAccessor db, object objEntity)
        {
            ISqlBuilder sqlBuilder = new DefaultSqlBuilder(new TableMapping(objEntity.GetType()));
            return db.ExecuteNonQuery(sqlBuilder.BuildUpdate(), objEntity);
        }

        public static int Update<T>(this DbAccessor db, object objParameters)
        {
            var mapping = new TableMapping(typeof(T));
            mapping.FilterColumn(objParameters);
            ISqlBuilder sqlBuilder = new DefaultSqlBuilder(mapping);
            return db.ExecuteNonQuery(sqlBuilder.BuildUpdate(), objParameters);
        }

        public static int UpdateFields(this DbAccessor db, object objEntity, params string[] inclusiveFields)
        {


            List<string> lsInclusiveFields = new List<string>();
            lsInclusiveFields.AddRange(inclusiveFields);
            var mapping = new TableMapping(objEntity.GetType());
            foreach (var item in mapping.GetKeyColumns())
            {
                lsInclusiveFields.Add(item.ColumnName);
            }
            mapping.FilterColumn(lsInclusiveFields.ToArray());
            ISqlBuilder sqlBuilder = new DefaultSqlBuilder(mapping);
            return db.ExecuteNonQuery(sqlBuilder.BuildUpdate(), objEntity);
        }
    }
}
