using System;
using System.Collections.Generic;
using System.Text;

namespace Zhuang.Data.SqlCommands.Store
{

    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    public sealed class SqlCommandResourceAttribute : Attribute
    {
        private string _path = string.Empty;

        public string Path
        {
            get
            {
                return _path;
            }

            set
            {
                _path = value;
            }
        }

        public SqlCommandResourceAttribute(string path)
        {
            _path = path;
        }
    }
}
