using System;
using System.Collections.Generic;
using System.Text;

namespace Zhuang.Data.Annotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TableAttribute : Attribute
    {
        private readonly string _name;

        public string Name
        {
            get
            {
                return this._name;
            }
        }

        public TableAttribute(string name)
        {
            this._name = name;
        }
    }
}
