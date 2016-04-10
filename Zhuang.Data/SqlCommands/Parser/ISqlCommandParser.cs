using System;
using System.Collections.Generic;
using System.Text;

namespace Zhuang.Data.SqlCommands.Parser
{
    public interface ISqlCommandParser
    {
        void Parse(SqlCommand sqlCommand);
    }
}
