using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Zhuang.Data.Common;
using Zhuang.Data.EnvironmentVariable;
using Zhuang.Data.Utility;

namespace Zhuang.Data.SqlCommands.Parser
{
    public class ParameterNameParser : ISqlCommandParser
    {


        public SqlCommand Parse(SqlCommand rawSqlCommand)
        {
            string parameterNamePrefix = SqlUtil.PARAMETER_NAME_PREFIX_AT;

            string strDbProviderName = EvnValRepository.Instance.GetEvnVal(typeof(DbProviderName).FullName).ToString();
            if (strDbProviderName == DbProviderName.Oracle.ToString())
            {
                parameterNamePrefix = SqlUtil.PARAMETER_NAME_PREFIX_COLON;
            }

            IList<Replacement> lsReplacement = new List<Replacement>();

            foreach (Match match in RegexPattern.ParameterPattern.Matches(rawSqlCommand.Text))
            {
                string namedParam = match.Groups["NamedParam"].Value.Trim();
                if (string.IsNullOrEmpty(namedParam)) continue;

                lsReplacement.Add(new Replacement()
                {
                    OldText = match.Value,
                    NewText = parameterNamePrefix + namedParam
                });
            }

            foreach (var replacement in lsReplacement)
            {
                rawSqlCommand.Text = rawSqlCommand.Text.Replace(replacement.OldText, replacement.NewText);
            }

            return null;
        }
    }
}
