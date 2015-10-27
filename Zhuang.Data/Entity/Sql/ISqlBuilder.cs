using System;
using System.Collections.Generic;
using System.Text;

namespace Zhuang.Data.Entity.Sql
{
    public interface ISqlBuilder
    {
        string BuildSelect();

        string BuildInsert();

        string BuildUpdate();

        string BuildDelete();
    }
}
