using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Data.SqlClient;
using Zhuang.Data.Utility;

namespace Zhuang.Data.Test
{
    [TestClass]
    public class ParameterParserTest
    {
        DbAccessor _dba = DbAccessor.Get();


        [TestMethod]
        public void Test()
        {
            var dt = _dba.QueryDataTable("select top 10 '{time}' as a,'$time$' as b from sys_product", new { time = "ttttt" });
            Console.WriteLine(DataTableUtil.ToString(dt));

        }
    }
}
