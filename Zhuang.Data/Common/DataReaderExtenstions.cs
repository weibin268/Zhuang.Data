using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using Zhuang.Data.Utility;

namespace Zhuang.Data.Common
{
    public static class DataReaderExtenstions
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
                        var tempValue = objValue.GetType() == typeof(DBNull) ? null : objValue;
                        var value = SqlUtil.ConvertDbFieldValueByEntityPropertyType(tempValue, pi.PropertyType);
                        object valueChanged;
                        if (pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            var typeGeneric = pi.PropertyType.GetGenericTypeDefinition()
                                .MakeGenericType(new Type[] { Nullable.GetUnderlyingType(pi.PropertyType) });
                            object nullableValue = Activator.CreateInstance(typeGeneric);
                            nullableValue = value;
                            valueChanged = nullableValue;
                        }
                        else
                        {
                            valueChanged = Convert.ChangeType(value, pi.PropertyType);
                        }

                        pi.SetValue(entity, valueChanged, null);
                    }
                }
                return entity;
            }
            return entity;
        }
    }
}
