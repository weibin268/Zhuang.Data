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

        private bool _hasArgs;

        public EnvValParameterParser(bool hasArgs)
        {
            _hasArgs = hasArgs;
        }

        public void Parse(SqlCommand sqlCommand)
        {

            IList<Replacement> lsReplacement = new List<Replacement>();

            foreach (Match match in RegexPattern.ParameterPattern.Matches(sqlCommand.Text))
            {
                string envParam = match.Groups["EnvParam"].Value.Trim();

                if (string.IsNullOrEmpty(envParam)) continue;

                if (_hasArgs)
                {
                    if (!envParam.Contains(":")) continue;
                }
                else
                {
                    if (envParam.Contains(":")) continue;
                }

                var envVal = EnvValRepository.Instance.GetEvnVal(envParam);
                if (!string.IsNullOrEmpty(envVal == null ? null : envVal.ToString()))
                {
                    lsReplacement.Add(new Replacement()
                    {
                        OldText = match.Value,
                        NewText = envVal.ToString()
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
