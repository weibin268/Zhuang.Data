using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Zhuang.Data.Utility
{
    public static class DataTableUtil
    {
        internal const string COLUMN_SEPARATOR = "|";

        public static string ToString(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();

            if (dt == null)
                return "Null";

            foreach (DataColumn dc in dt.Columns)
            {
                if (dt.Columns.IndexOf(dc) != (dt.Columns.Count - 1))
                {
                    sb.Append(dc.ColumnName + COLUMN_SEPARATOR);
                }
                else
                {
                    sb.Append(dc.ColumnName + "\n");
                }
            }

            foreach (DataRow dr in dt.Rows)
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    if (dt.Columns.IndexOf(dc) != (dt.Columns.Count - 1))
                    {
                        sb.Append(dr[dc.ColumnName] + COLUMN_SEPARATOR);
                    }
                    else
                    {
                        sb.Append(dr[dc.ColumnName] + "\n");
                    }
                }
            }

            return sb.ToString();
        }
    }
}
