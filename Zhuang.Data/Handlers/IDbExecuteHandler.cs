using System;
using System.Collections.Generic;
using System.Text;

namespace Zhuang.Data.Handlers
{
    public interface IDbExecuteHandler
    {
        void HandleExecute(DbAccessorContext context);
    }
}
