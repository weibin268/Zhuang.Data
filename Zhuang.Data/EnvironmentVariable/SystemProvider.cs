using System;
using System.Collections.Generic;
using System.Text;
using Zhuang.Data.Common;

namespace Zhuang.Data.EnvironmentVariable
{
    class SystemProvider : IEvnValStoreProvider
    {
        public Dictionary<string, object> GetEnvironmentVariables()
        {
            Dictionary<string, object> dicResult = new Dictionary<string, object>();

            dicResult.Add("DateNow", new Fun<string>(() => { return DateTime.Now.ToString("yyyy-MM-dd"); }));

            dicResult.Add("TimeNow", new Fun<string>(()=> { return DateTime.Now.ToString("hh:mm:ss"); }));

            dicResult.Add("DateTimeNow", new Fun<string>(() => { return DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"); }));

            return dicResult;
        }
    }
}
