using System;
using System.Collections.Generic;
using System.Text;

namespace Zhuang.Data.Annotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class ColumnAttribute : Attribute
    {
        private readonly string _name;
        private string _typeName;
        private int _order = -1;

        public string TypeName
        {
            get
            {
                return _typeName;
            }

            set
            {
                _typeName = value;
            }
        }

        public int Order
        {
            get
            {
                return _order;
            }

            set
            {
                _order = value;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public ColumnAttribute(string name)
        {
            this._name = name;
        }
    }
}
