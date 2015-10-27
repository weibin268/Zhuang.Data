using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Zhuang.Data
{
    public sealed class DbAccessorContext
    {
        public DbAccessorContext(DbCommand dbCommand)
        {
            DbCommand = dbCommand;
        }

        public DbCommand DbCommand { get; }
    }
}
