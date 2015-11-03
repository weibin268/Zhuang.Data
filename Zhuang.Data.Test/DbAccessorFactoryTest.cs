using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zhuang.Data.Utility;

namespace Zhuang.Data.Test
{
    [TestClass]
    public class DbAccessorFactoryTest
    {
        [TestMethod]
        public void CreateDbAccessor()
        {
            var dba = DbAccessorFactory.CreateDbAccessor("TestDb");

            var dt = dba.QueryDataTable("select top 10 * from sys_product");
            Console.WriteLine(DataTableUtil.ToString(dt));
        }
    }
}
