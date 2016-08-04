using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using Zhuang.Data.Utility;

namespace Zhuang.Data.Extensions
{
    public static class DataReaderExtensions
    {
        public static IList<IDictionary<string, object>> ReadDictionaries(this IDataReader reader)
        {
            if (reader.IsClosed)
            {
                throw new InvalidOperationException("reader has been closed");
            }
            List<IDictionary<string, object>> list = new List<IDictionary<string, object>>();
            IDictionary<string, object> item;
            while ((item = reader.ReadDictionary()) != null)
            {
                list.Add(item);
            }
            return list;
        }

        public static IDictionary<string, object> ReadDictionary(this IDataReader reader)
        {
            if (reader.Read())
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    dictionary.Add(reader.GetName(i), reader.GetValue(i));
                }
                return dictionary;
            }
            return null;
        }

        public static IList<T> ReadEntities<T>(this IDataReader reader)
        {
            IList<T> lsResult = new List<T>();
            T item = default(T);
            while ((item = reader.ReadEntity<T>()) != null)
            {
                lsResult.Add(item); 
            }
            return lsResult;
        }

        public static T ReadEntity<T>(this IDataReader reader)
        {
            T entity = default(T);
            if (reader.Read())
            {
                entity = (T)Activator.CreateInstance(typeof(T));
                Type entityType = typeof(T);

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string fieldName = reader.GetName(i);
                    var entityProperties = entityType.GetProperties();
                    PropertyInfo pi = Array.Find<PropertyInfo>(entityProperties, (c) =>
                    {
                        return c.Name.ToLower() == fieldName.ToLower();
                    });
                    //PropertyInfo pi = entityType.GetProperty(reader.GetName(i));
                    if (pi != null)
                    {
                        var objValue = reader.GetValue(i);
                        var value = objValue.GetType() == typeof(DBNull) ? null : objValue;

                        object valueChanged;
                        //类型转换,特殊处理
                        valueChanged = ConvertTypeSpecialHandle(value, pi);

                        //类型转换,通用处理
                        valueChanged = ConvertTypeCommonHandle(valueChanged, pi);

                        pi.SetValue(entity, valueChanged, null);
                    }
                }
                return entity;
            }
            return entity;
        }

        private static object ConvertTypeSpecialHandle(object targetValue, PropertyInfo sourcePropertyInfo)
        {

            Type sourceType = sourcePropertyInfo.PropertyType;

            if (targetValue == null) goto End;

            //如果数据库字段值为int64而实体属性类型为int32则将字段值转为int32
            if (targetValue.GetType() == typeof(Int64) &&
                (sourceType == typeof(Int32) || sourceType == typeof(Nullable<Int32>)))
            {
                return Convert.ToInt32(targetValue);
            }

            End:
            return targetValue;
        }


        private static object ConvertTypeCommonHandle(object targetValue, PropertyInfo sourcePropertyInfo)
        {
            object result;

            if (sourcePropertyInfo.PropertyType.IsGenericType && sourcePropertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var typeGeneric = sourcePropertyInfo.PropertyType.GetGenericTypeDefinition()
                    .MakeGenericType(new Type[] { Nullable.GetUnderlyingType(sourcePropertyInfo.PropertyType) });
                object nullableValue = Activator.CreateInstance(typeGeneric);
                nullableValue = targetValue;
                result = nullableValue;
            }
            else
            {
                result = Convert.ChangeType(targetValue, sourcePropertyInfo.PropertyType);
            }

            return result;
        }

    }
}
