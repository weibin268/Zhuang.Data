using System;
using System.Collections.Generic;
using System.Text;

namespace Zhuang.Data.EnvironmentVariable
{
    public interface IEnvValStoreProvider
    {
        Dictionary<string, object> GetEnvironmentVariables();
    }
}
