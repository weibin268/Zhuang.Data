using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Zhuang.Data.Test
{
    [TestClass]
    public class PaginationTest
    {

        DbAccessor _dba = DbAccessor.Get();

        [TestMethod]
        public void PageQueryDataTable()
        {
            int totalRowCount;
            _dba.PageQueryDataTable("top8", "ProductId",1,3,out totalRowCount);

            _dba.PageQueryDataTable("select * from sys_product", "ProductId", 1, 3, out totalRowCount);

        }
    }
}
