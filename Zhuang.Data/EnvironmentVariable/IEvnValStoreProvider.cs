using System;
using System.Collections.Generic;
using System.Text;

namespace Zhuang.Data.EnvironmentVariable
{
    public interface IEvnValStoreProvider
    {
        Dictionary<string, object> GetEnvironmentVariables();
    }
}
