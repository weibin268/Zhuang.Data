using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zhuang.Data.Annotations;
using Zhuang.Data.Handlers;
using Zhuang.Data.Utility;

namespace Zhuang.Data.Test
{
   

    [TestClass]
    public class EntityTest
    {
        DbAccessor _dba = DbAccessor.Get();

        public EntityTest()
        {
            _dba.PreCommandExecute += (c) =>
            {
                Console.WriteLine(c.DbCommand.CommandText);
            };
            _dba.PreCommandExecute += new TraceHandler().PrintSqlByConsole;

        }


        [TestMethod]
        public void Select()
        {
            var products = _dba.Select<SysProduct>(new { ProductId = 1 });
            Console.WriteLine(products.ProductName);
        }

        [TestMethod]
        public void Insert()
        {
            SysProduct pro = new SysProduct();
            
            pro.ProductName = "zwb";
            pro.RecordStatus = "Active";

            _dba.Insert(pro);

            var dt = _dba.QueryDataTable("select * from sys_product where ProductName=@ProductName",
                new { ProductName ="zwb"});
            Console.WriteLine(DataTableUtil.ToString(dt));
        }

        [TestMethod]
        public void Update()
        {
            SysProduct pro = new SysProduct();
            pro.ProductId = 59;
            pro.ProductName = "zwb";
            pro.RecordStatus = "Inactive";

            _dba.Update(pro);

            var dt = _dba.QueryDataTable("select * from sys_product where ProductName=@ProductName",
                new { ProductName = "zwb" });
            Console.WriteLine(DataTableUtil.ToString(dt));
        }

        [TestMethod]
        public void Delete()
        {
            SysProduct pro = new SysProduct();
            pro.ProductId = 58;

            _dba.Delete(pro);

            var dt = _dba.QueryDataTable("select * from sys_product where ProductName=@ProductName",
                new { ProductName = "zwb" });
            Console.WriteLine(DataTableUtil.ToString(dt));
        }

    }
}
