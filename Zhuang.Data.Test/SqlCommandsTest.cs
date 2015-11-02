using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zhuang.Data.Utility;

namespace Zhuang.Data.Test
{
    [TestClass]
    public class SqlCommandsTest
    {
        DbAccessor _dba = DbAccessor.Get();

        [TestMethod]
        public void ConfigFileNameWithDbProviderName()
        {
            var dt = _dba.QueryDataTable("top8");

            Console.WriteLine(DataTableUtil.ToString(dt));

        }
    }
}
