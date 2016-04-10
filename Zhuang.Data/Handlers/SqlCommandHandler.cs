using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Zhuang.Data.Common;
using Zhuang.Data.Handlers;
using Zhuang.Data.SqlCommands;
using Zhuang.Data.SqlCommands.Parser;
using Zhuang.Data.SqlCommands.Store;

namespace Zhuang.Data.Handlers
{
    public class SqlCommandHandler : IDbExecuteHandler
    {
        public void HandleExecute(DbAccessorContext context)
        {
            RetrieveSqlCommand(context);

            ParseSqlCommand(context);
        }

        private void RetrieveSqlCommand(DbAccessorContext context)
        {
            if (!context.DbCommand.CommandText.Trim().Contains(" "))
            {
                SqlCommand sqlCmd = SqlCommandRepository.Instance.GetSqlCommand(context.DbCommand.CommandText);
                if (sqlCmd != null)
                {
                    context.DbCommand.CommandText = sqlCmd.Text;
                }
            }

        }
      
        private void ParseSqlCommand(DbAccessorContext context)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.DbAccessor = context.DbAccessor;
            foreach (DbParameter p in context.DbCommand.Parameters)
            {
                cmd.Parameters.Add(p);
            }
            cmd.Text = context.DbCommand.CommandText;

            IList<ISqlCommandParser> parsers = new List<ISqlCommandParser>();
            parsers.Add(new NormalParameterParser());
            parsers.Add(new ValueParameterParser());
            parsers.Add(new EvnValParameterParser());
            parsers.Add(new DynamicClauseParser());

            foreach (var p in parsers)
            {
                p.Parse(cmd);
            }

            context.DbCommand.CommandText = cmd.Text;

        }
    }
}
