using System;
using System.Collections.Generic;
using System.Text;
using Zhuang.Data.Common;

namespace Zhuang.Data.EnvironmentVariable
{
    public class EvnValRepository
    {
        private Dictionary<string, object> _dicEvnVal;
        private IList<IEvnValStoreProvider> _storeProviders;
        private static EvnValRepository _instance;
        private static object _objLock = new object();

        public static EvnValRepository Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_objLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new EvnValRepository();
                            _instance.AddStoreProvider(new SystemProvider());
                        }
                    }
                }
                return _instance;
            }
        }

        public void AddStoreProvider(IEvnValStoreProvider storeProvider)
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

        public EvnValRepository()
        {
            _dicEvnVal = new Dictionary<string, object>();
            _storeProviders = new List<IEvnValStoreProvider>();
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
            _dicEvnVal.Add(key, value);
        }

    }
}
