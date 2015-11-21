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

            ParseParameterName(context);

            ParseReplacement(context);

            ParseDynamicClause(context);

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

        private void ParseDynamicClause(DbAccessorContext context)
        {

            if (!DynamicClauseParser.IsCanParse(context.DbCommand.CommandText)) return;

            SqlCommand cmd = new SqlCommand();

            cmd.DbAccessor = context.DbAccessor;
            cmd.Text = context.DbCommand.CommandText;
            foreach (DbParameter p in context.DbCommand.Parameters)
            {
                cmd.Parameters.Add(p);
            }
            var parser = new DynamicClauseParser();
            cmd = parser.Parse(cmd);
            context.DbCommand.CommandText = cmd.Text;
        }

        private void ParseParameterName(DbAccessorContext context)
        {
            SqlCommand cmd = new SqlCommand();

            cmd.DbAccessor = context.DbAccessor;
            cmd.Text = context.DbCommand.CommandText;
            new ParameterNameParser().Parse(cmd);

            context.DbCommand.CommandText = cmd.Text;
        }

        private void ParseReplacement(DbAccessorContext context)
        {
            SqlCommand cmd = new SqlCommand();

            cmd.DbAccessor = context.DbAccessor;
            foreach (DbParameter p in context.DbCommand.Parameters)
            {
                cmd.Parameters.Add(p);
            }

            cmd.Text = context.DbCommand.CommandText;
            new ReplacementParser().Parse(cmd);

            context.DbCommand.CommandText = cmd.Text;
        }
    }
}
