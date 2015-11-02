using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Zhuang.Data
{
    public sealed class DbAccessorContext
    {
        public DbAccessorContext(DbAccessor dbAccessor, DbCommand dbCommand)
        {
            DbCommand = dbCommand;
            DbAccessor = dbAccessor;
        }

        public DbCommand DbCommand { get; }

        public DbAccessor DbAccessor { get; }
    }
}
