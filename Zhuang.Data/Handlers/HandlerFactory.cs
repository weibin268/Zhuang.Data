using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Zhuang.Data.Handlers
{
    public class HandlerFactory
    {
        public static IEnumerable<IDbExecuteHandler> GetDbExecuteHandlers()
        {
            yield return new SqlCommandHandler();
            yield return new DbParameterHandler();
            //yield return new TraceHandler();
        }
    }
}
