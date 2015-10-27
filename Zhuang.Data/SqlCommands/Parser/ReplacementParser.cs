using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Zhuang.Data.Common;
using Zhuang.Data.EnvironmentVariable;

namespace Zhuang.Data.SqlCommands.Parser
{
    public class ReplacementParser : ISqlCommandParser
    {
        public SqlCommand Parse(SqlCommand rawSqlCommand)
        {

            IList<Replacement> lsReplacement = new List<Replacement>();

            foreach (Match match in RegexPattern.ParameterPattern.Matches(rawSqlCommand.Text))
            {
                string valueParam = match.Groups["ValueParam"].Value.Trim();
                if (string.IsNullOrEmpty(valueParam)) continue;

                var param = rawSqlCommand.Parameters.FindLast((c) => { return c.ParameterName == valueParam.Trim(); });
                if (param != null && param.Value!=null)
                {
                    lsReplacement.Add(new Replacement()
                    {
                        OldText = match.Value,
                        NewText = param.Value.ToString()
                    });
                    continue;
                }

                var evnVal = EvnValRepository.Instance.GetEvnVal(valueParam);
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
                rawSqlCommand.Text = rawSqlCommand.Text.Replace(replacement.OldText, replacement.NewText);
            }

            return null;
        }
    }
}
