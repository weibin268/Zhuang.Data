using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using Zhuang.Data.Utility;

namespace Zhuang.Data.Handlers
{
    public class DbParameterHandler:IDbExecuteHandler
    {

        public void HandleDBNull(DbAccessorContext context)
        {
            var dbParameters = context.DbCommand.Parameters;

            if (dbParameters != null)
            {
                foreach (DbParameter p in dbParameters)
                {
                    if (p != null)
                    {
                        // 检查未分配值的输出参数,将其分配以DBNull.Value. 
                        if ((p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Input) &&
                            (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                    }
                }

            }
        }

        public void RemoveUnnecessary(DbAccessorContext context)
        {
            var dbParameters = context.DbCommand.Parameters;
            IList<string> unnecessaryParameterNames = new List<string>();
            foreach (DbParameter p in dbParameters)
            {
                if (!SqlUtil.ContainsParameterName(context.DbCommand.CommandText,p.ParameterName))
                {
                    unnecessaryParameterNames.Add(p.ParameterName);
                }
            }

            foreach (string unnecessaryParameterName in unnecessaryParameterNames)
            {
                dbParameters.RemoveAt(unnecessaryParameterName);
            }
        }

        public void HandleExecute(DbAccessorContext context)
        {
            RemoveUnnecessary(context);
            HandleDBNull(context);
        }
    }
}
