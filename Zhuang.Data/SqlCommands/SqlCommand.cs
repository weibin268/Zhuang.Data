using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Zhuang.Data.Common;

namespace Zhuang.Data.SqlCommands
{
    public class SqlCommand
    {
        public string Key { get; set; }

        public string Text { get; set;}

        public List<DbParameter> Parameters { get; set; }

        public DbAccessor DbAccessor { get; set; }

        public SqlCommand()
        {
            Parameters = new List<DbParameter>();
        }
    }
}
