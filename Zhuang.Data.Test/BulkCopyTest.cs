using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Data.SqlClient;

namespace Zhuang.Data.Test
{
    [TestClass]
    public class BulkCopyTest
    {
        DbAccessor _dba = DbAccessor.Get();


        [TestMethod]
        public void BulkWriteToServer()
        {
            DataTable dt= _dba.BulkGetEmptyDataTable("Sys_Product");

            for (int i = 0; i < 3; i++)
            {
                var dr = dt.NewRow();

                dr["ProductName"] = "BulkWriteToServer";
                dr["ProductCode"] = "BulkWriteToServer";

                dt.Rows.Add(dr);
            }

            _dba.BulkWriteToServer(dt);
            _dba.BulkWriteToServer(dt,0,new SqlBulkCopyColumnMapping("ProductName", "ProductName"));

        }
    }
}
