using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Zhuang.Data.Common
{
    struct RegexPattern
    {
        public static readonly Regex ParameterPattern = new Regex("#(?<NamedParam>.+?)#|\\$(?<ValueParam>.+?)\\$|\\{(?<EvnParam>.+?)\\}");
        public static readonly Regex DynamicClausePattern = new Regex("\\{\\?(?<DynamicClause>(?>[^\\{\\}]+|\\{\\?(?<Clause>)|\\}(?<-Clause>))*(?(Clause)(?!)))\\}", RegexOptions.Singleline);
        public static readonly Regex ActionClausePattern = new Regex("@(?<ActionName>[a-zA-Z0-9_-]+)\\{(?<ActionText>.+?)\\}", RegexOptions.Singleline);
        public static readonly Regex DbNamedParamPattern = new Regex("[@:](?<Name>[a-zA-Z0-9_:-]+)");
    }
}
