using System;
using System.Collections.Generic;
using System.Text;
using Zhuang.Data.Common;

namespace Zhuang.Data.EnvironmentVariable
{
    public class EnvValRepository
    {
        private Dictionary<string, object> _dicEvnVal;
        private IList<IEnvValStoreProvider> _storeProviders;
        private static EnvValRepository _instance;
        private static object _objLock = new object();

        public static EnvValRepository Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_objLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new EnvValRepository();
                            _instance.AddStoreProvider(new SystemProvider());
                        }
                    }
                }
                return _instance;
            }
        }

        public void AddStoreProvider(IEnvValStoreProvider storeProvider)
        {
            if (storeProvider == null) return;

            _storeProviders.Add(storeProvider);

            var evnVals = storeProvider.GetEnvironmentVariables();
            foreach (var evnVal in evnVals)
            {
                if (!_dicEvnVal.ContainsKey(evnVal.Key))
                {
                    _dicEvnVal.Add(evnVal.Key, evnVal.Value);
                }
            }

        }

        public EnvValRepository()
        {
            _dicEvnVal = new Dictionary<string, object>();
            _storeProviders = new List<IEnvValStoreProvider>();
        }

        public object GetEvnVal(string key)
        {
            if (_dicEvnVal.ContainsKey(key))
            {
                object objValue = _dicEvnVal[key];
                if (typeof(MyFunc<string>) == objValue.GetType())
                {
                    return ((MyFunc<string>)objValue)();
                }
                else
                {
                    return objValue;
                }
            }
            else
            {
                return null;
            }
        }

        public void AddEvnVal(string key, object value)
        {
            if (!_dicEvnVal.ContainsKey(key))
            {
                _dicEvnVal.Add(key, value);
            }
        }

    }
}
