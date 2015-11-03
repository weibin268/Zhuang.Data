using System;
using System.Collections.Generic;
using System.Text;
using Zhuang.Data.Common;

namespace Zhuang.Data.EnvironmentVariable
{

    public enum SystemEvnValKey
    {
        DateNow,
        TimeNow,
        DateTimeNow
    }

    class SystemProvider : IEvnValStoreProvider
    {
        public Dictionary<string, object> GetEnvironmentVariables()
        {
            Dictionary<string, object> dicResult = new Dictionary<string, object>();

            dicResult.Add(SystemEvnValKey.DateNow.ToString(), new Fun<string>(() => { return DateTime.Now.ToString("yyyy-MM-dd"); }));

            dicResult.Add(SystemEvnValKey.TimeNow.ToString(), new Fun<string>(() => { return DateTime.Now.ToString("hh:mm:ss"); }));

            dicResult.Add(SystemEvnValKey.DateTimeNow.ToString(), new Fun<string>(() => { return DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"); }));

            return dicResult;
        }
    }
}
