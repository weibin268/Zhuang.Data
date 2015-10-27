using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zhuang.Data.Annotations;
using Zhuang.Data.Entity.Mapping;
using Zhuang.Data.Entity.Sql;

namespace Zhuang.Data.Test
{

    [TestClass]
    public class MappingTest
    {
        public object DefaultSqlBuilder { get; private set; }

        [TestMethod]
        public void TableMapping()
        {
            TableMapping table = new TableMapping(typeof(SysProduct));
            Console.WriteLine(table.TableName);

            foreach (var col in table.Columns)
            {
                Console.WriteLine(col.ColumnName+" "+col.IsPrimaryKey);
            }

            DefaultSqlBuilder sqlb = new DefaultSqlBuilder(table);
            Console.WriteLine(sqlb.BuildDelete());

            Console.WriteLine(sqlb.BuildInsert());

            Console.WriteLine(sqlb.BuildUpdate());
        }
    }
}
