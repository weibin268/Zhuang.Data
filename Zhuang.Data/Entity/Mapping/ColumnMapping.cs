using System;
using System.Collections.Generic;
using System.Text;

namespace Zhuang.Data.Entity.Mapping
{
    public class ColumnMapping
    {
        public string ColumnName { get; set; }

        public bool IsPrimaryKey { get; set; }

        public string PropertyName { get; set; }

        public bool IsAutoGenerate { get; set; }
    }
}
