using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Zhuang.Data.Common;
using Zhuang.Data.EnvironmentVariable;

namespace Zhuang.Data.SqlCommands.Parser
{
    public class EnvValParameterParser : ISqlCommandParser
    {
        public void Parse(SqlCommand sqlCommand)
        {

            IList<Replacement> lsReplacement = new List<Replacement>();

            foreach (Match match in RegexPattern.ParameterPattern.Matches(sqlCommand.Text))
            {
                string evnParam = match.Groups["EvnParam"].Value.Trim();
                if (string.IsNullOrEmpty(evnParam)) continue;

                var evnVal = EnvValRepository.Instance.GetEvnVal(evnParam);
                if (!string.IsNullOrEmpty(evnVal == null ? null : evnVal.ToString()))
                {
                    lsReplacement.Add(new Replacement()
                    {
                        OldText = match.Value,
                        NewText = evnVal.ToString()
                    });
                    continue;
                }
            }

            foreach (var replacement in lsReplacement)
            {
                sqlCommand.Text = sqlCommand.Text.Replace(replacement.OldText, replacement.NewText);
            }
        }
    }
}
