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

            dicResult.Add("{date}", new Fun<string>(() => { return DateTime.Now.ToString("yyyy-MM-dd"); }));

            dicResult.Add("{time}", new Fun<string>(() => { return DateTime.Now.ToString("HH:mm:ss"); }));

            dicResult.Add("{datetime}", new Fun<string>(() => { return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); }));

            return dicResult;
        }
    }
}
