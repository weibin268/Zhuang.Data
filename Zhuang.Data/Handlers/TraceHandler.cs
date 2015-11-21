using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using Zhuang.Data.Utility;

namespace Zhuang.Data.Handlers
{
    public class TraceHandler : IDbExecuteHandler
    {
        public void HandleExecute(DbAccessorContext context)
        {
            PrintSqlByConsole(context);
        }


        public void PrintSqlByConsole(DbAccessorContext context)
        {
            Console.WriteLine("--<<<------------------------------------------------------------------------------------------------------");
            Console.WriteLine(SqlUtil.GetSqlFromDbCommand(context.DbCommand));
            Console.WriteLine("------------------------------------------------------------------------------------------------------>>>--");
        }
    }
}
