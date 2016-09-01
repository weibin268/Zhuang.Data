using System;
using System.Collections.Generic;
using System.Text;
using Zhuang.Data.Common;

namespace Zhuang.Data.EnvironmentVariable
{

    class SystemProvider : IEnvValStoreProvider
    {
        public Dictionary<string, object> GetEnvironmentVariables()
        {
            Dictionary<string, object> dicResult = new Dictionary<string, object>();

            dicResult.Add("date", new MyEnvFunc((c) => { return DateTime.Now.ToString("yyyy-MM-dd"); }));

            dicResult.Add("time", new MyEnvFunc((c) => { return DateTime.Now.ToString("HH:mm:ss"); }));

            dicResult.Add("datetime", new MyEnvFunc((c) =>
            {
                string format = "yyyy-MM-dd HH:mm:ss";

                if (!string.IsNullOrEmpty(c))
                {
                    format = c;
                }

                return DateTime.Now.ToString(format);
            }));

            return dicResult;
        }
    }
}
