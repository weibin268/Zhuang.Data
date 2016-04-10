using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Zhuang.Data.Common;
using Zhuang.Data.EnvironmentVariable;

namespace Zhuang.Data.SqlCommands.Parser
{
    public class ValueParameterParser : ISqlCommandParser
    {
        public void Parse(SqlCommand sqlCommand)
        {
            IList<Replacement> lsReplacement = new List<Replacement>();

            foreach (Match match in RegexPattern.ParameterPattern.Matches(sqlCommand.Text))
            {
                string valueParam = match.Groups["ValueParam"].Value.Trim();
                if (string.IsNullOrEmpty(valueParam)) continue;

                var param = sqlCommand.Parameters.FindLast((c) => { return c.ParameterName == valueParam.Trim(); });
                if (param != null && param.Value!=null)
                {
                    lsReplacement.Add(new Replacement()
                    {
                        OldText = match.Value,
                        NewText = param.Value.ToString()
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
