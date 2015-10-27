using System;
using System.Collections.Generic;
using System.Text;
using Zhuang.Data.Entity.Mapping;
using Zhuang.Data.Entity.Sql;

namespace Zhuang.Data
{
    public static class EntityExtenstions
    {
        public static T Select<T>(this DbAccessor db, object objIdParameters)
        {
            ISqlBuilder sqlBuilder = new DefaultSqlBuilder(new TableMapping(typeof(T)));
            return db.QueryEntity<T>(sqlBuilder.BuildSelect(), objIdParameters);
        }

        public static int Delete(this DbAccessor db, object objEntity)
        {
            ISqlBuilder sqlBuilder = new DefaultSqlBuilder(new TableMapping(objEntity.GetType()));
            return db.ExecuteNonQuery(sqlBuilder.BuildDelete(), objEntity);
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
            var mapping = new TableMapping(objEntity.GetType());
            mapping.FilterColumn(inclusiveFields);
            ISqlBuilder sqlBuilder = new DefaultSqlBuilder(mapping);
            return db.ExecuteNonQuery(sqlBuilder.BuildUpdate(), objEntity);
        }
    }
}
