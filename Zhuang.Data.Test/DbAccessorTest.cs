using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zhuang.Data.Utility;
using Zhuang.Data.Handlers;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Zhuang.Data.Test
{
    [TestClass]
    public class DbAccessorTest
    {
        DbAccessor _dba = DbAccessor.Get();

        public DbAccessorTest()
        {
            _dba.PreCommandExecute += (c) =>
            {
                Console.WriteLine(c.DbCommand.CommandText);
            };
            _dba.PreCommandExecute += new TraceHandler().PrintSqlByConsole;

        }

        [TestMethod]
        public void QueryDataTable()
        {

            var dt = _dba.QueryDataTable("select top 10 * from sys_product");
            Console.WriteLine(DataTableUtil.ToString(dt));
        }

        [TestMethod]
        public void QueryDictionary()
        {
            IDictionary<string,object> dic =  _dba.QueryDictionary("select top 1 * from sys_product");

            Console.WriteLine(dic["ProductId"]);
        }

        [TestMethod]
        public void QueryDictionaries()
        {
            var dics = _dba.QueryDictionaries("select top 10 * from sys_product");

            Console.WriteLine(dics[5]["ProductId"]);
        }

        [TestMethod]
        public void Trace()
        {
            DbAccessor dba = DbAccessor.Create();
            dba.PreCommandExecute += new TraceHandler().PrintSqlByConsole;

            var dt = _dba.QueryDataTable("select top 10 * from sys_product");
            Console.WriteLine(DataTableUtil.ToString(dt));

        }

        [TestMethod]
        public void SmartParameter()
        {
            var dt = _dba.QueryDataTable("select * from sys_product where productid=#id#", new { id = 1 });
            Console.WriteLine(DataTableUtil.ToString(dt));
        }

        [TestMethod]
        public void ObjectParameter()
        {
            var dt2 = _dba.QueryDataTable("select * from sys_product where productid=#id#", new { id = 1 });
            Console.WriteLine(DataTableUtil.ToString(dt2));

            var dicParam = new Dictionary<string, object>();
            dicParam.Add("id", 2);
            var dt3 = _dba.QueryDataTable("select * from sys_product where productid=#id#", dicParam);
            Console.WriteLine(DataTableUtil.ToString(dt3));

            var dbParam = _dba.DbProviderFactory.CreateParameter();
            dbParam.ParameterName = "@Id";
            dbParam.DbType = DbType.Int32;
            dbParam.Value = 3;
            var dt4 = _dba.QueryDataTable("select '$DateTimeNow$' as aa,* from sys_product where productid=#id#",
                new DbParameter[] { dbParam });
            Console.WriteLine(DataTableUtil.ToString(dt4));

        }

        [TestMethod]
        public void BeginTran()
        {
            using (DbAccessor dba = DbAccessor.Create())
            {
                dba.BeginTran();
                dba.ExecuteNonQuery("update sys_product set ProductName='abc' where ProductId=#ProductId#", new { ProductId = 1 });
                dba.RollbackTran();
            }
        }

    }
}
