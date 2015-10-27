using System;
using System.Collections.Generic;
using System.Text;
using Zhuang.Data.Annotations;

namespace Zhuang.Data.Entity.Mapping
{
    public class TableMapping
    {
        private Type _typeEntity;
        public Type TypeEntity
        {
            get
            {
                return _typeEntity;
            }
        }

        private string _tableName = string.Empty;
        public string TableName
        {
            get
            {
                return _tableName;
            }
        }

        private IList<ColumnMapping> _columns = new List<ColumnMapping>();
        public IList<ColumnMapping> Columns
        {
            get
            {
                return _columns;
            }
            set
            {
                _columns = value;
            }
        }

        public TableMapping(Type typeEntity)
        {
            _typeEntity = typeEntity;

            Init();
        }

        private void Init()
        {
            var tableAttributes = _typeEntity.GetCustomAttributes(typeof(TableAttribute), false);
            if (tableAttributes.Length > 0)
            {
                _tableName = ((TableAttribute)tableAttributes[0]).Name;
            }
            else
            {
                _tableName = _typeEntity.Name;
            }

            foreach (var pi in _typeEntity.GetProperties())
            {

                var ignoreAttributes = pi.GetCustomAttributes(typeof(IgnoreAttribute), false);
                if (ignoreAttributes.Length > 0)
                {
                    continue;
                }

                ColumnMapping column = new ColumnMapping();
                column.PropertyName = pi.Name;

                var columnAttributes = pi.GetCustomAttributes(typeof(ColumnAttribute), false);
                if (columnAttributes.Length > 0)
                {
                    column.ColumnName = ((ColumnAttribute)columnAttributes[0]).Name;
                }
                else
                {
                    column.ColumnName = pi.Name;
                }

                var keyAttributes = pi.GetCustomAttributes(typeof(KeyAttribute), false);
                if (keyAttributes.Length > 0)
                {
                    column.IsPrimaryKey = true;
                }

                var autoAttributes = pi.GetCustomAttributes(typeof(AutoGenerateAttribute), false);
                if (autoAttributes.Length > 0)
                {
                    column.IsAutoGenerate = true;
                }

                Columns.Add(column);

            }


        }

        public IList<ColumnMapping> GetKeyColumns()
        {
            IList<ColumnMapping> result = new List<ColumnMapping>();

            foreach (var col in Columns)
            {
                if (col.IsPrimaryKey)
                {
                    result.Add(col);
                }
            }

            return result;
        }

        public IList<ColumnMapping> GetInsertColumns()
        {
            IList<ColumnMapping> result = new List<ColumnMapping>();

            foreach (var col in Columns)
            {
                if (!col.IsAutoGenerate)
                {
                    result.Add(col);
                }
            }

            return result;
        }

        public IList<ColumnMapping> GetUpdateColumns()
        {
            return GetInsertColumns();
        }

        public void FilterColumn(object objEntity)
        {
            var inclusiveColumnNames = new List<string>();

            if (objEntity.GetType() == typeof(Dictionary<string, object>))
            {
                foreach (var key in ((Dictionary<string, object>)objEntity).Keys)
                {
                    inclusiveColumnNames.Add(key);
                }
            }
            else
            {
                foreach (var pi in objEntity.GetType().GetProperties())
                {
                    inclusiveColumnNames.Add(pi.Name);
                }
            }

            FilterColumn(inclusiveColumnNames.ToArray());
        }

        public void FilterColumn(string[] inclusiveColumnNames)
        {
            var tempColumns = new List<ColumnMapping>();
            foreach (var colName in inclusiveColumnNames)
            {
                var col = Array.Find<ColumnMapping>(((List<ColumnMapping>)_columns).ToArray(), c =>
                {
                    return c.ColumnName == colName;
                });

                if (col != null)
                {
                    tempColumns.Add(col);
                }
            }
            _columns = tempColumns;
        }

    }
}
