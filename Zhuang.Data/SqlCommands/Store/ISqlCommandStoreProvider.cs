using System;
using System.Collections.Generic;
using System.Text;

namespace Zhuang.Data.SqlCommands.Store
{
    public interface ISqlCommandStoreProvider
    {
        IEnumerable<SqlCommand> GetSqlCommands();
    }
}
