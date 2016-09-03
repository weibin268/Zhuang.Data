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
                            _instance.AddStoreProvider(new DefaultEnvValStoreProvider());
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

        /// <summary>
        /// 获取环境变量
        /// </summary>
        /// <param name="key">格式如：datetime:yyyy-MM-dd</param>
        /// <returns></returns>
        public object GetEvnVal(string key)
        {
            var envValText = new EnvValName(key);

            return GetEvnVal(envValText);
        }

        public object GetEvnVal(EnvValName envValText)
        {

            var keyValue = envValText.GetName();
            string keyParams = envValText.GetArgs();

            if (_dicEvnVal.ContainsKey(keyValue))
            {
                object objValue = _dicEvnVal[keyValue];
                if (typeof(MyEnvFunc) == objValue.GetType())
                {
                    return ((MyEnvFunc)objValue)(keyParams);
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
