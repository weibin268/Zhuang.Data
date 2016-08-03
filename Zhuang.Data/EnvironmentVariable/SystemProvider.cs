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

            dicResult.Add("date", new MyFunc<string>(() => { return DateTime.Now.ToString("yyyy-MM-dd"); }));

            dicResult.Add("time", new MyFunc<string>(() => { return DateTime.Now.ToString("HH:mm:ss"); }));

            dicResult.Add("datetime", new MyFunc<string>(() => { return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); }));

            return dicResult;
        }
    }
}
