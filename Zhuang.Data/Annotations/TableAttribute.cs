using System;
using System.Collections.Generic;
using System.Text;

namespace Zhuang.Data.Annotations
{

    /// <summary>
    /// 用于标识实体所对应的表名
    /// </summary>
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
