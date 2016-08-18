using System;
using System.Collections.Generic;
using System.Text;
using Zhuang.Data.EnvironmentVariable;

namespace Zhuang.Data.Common
{
    class EnvValService
    {
        const string DefaultDbAccessorHashCode_Key = "DefaultDbAccessorHashCode";

        private static object _objLock = new object();

        //public static void SetDbAccessorDbProviderName(DbAccessor dba, DbProviderName dbProviderName)
        //{
        //    EnvValRepository.Instance.AddEvnVal(dba.GetHashCode().ToString(), dbProviderName.ToString());
        //}

        //public static string GetDbAccessorDbProviderName(DbAccessor dba)
        //{
        //    var result = EnvValRepository.Instance.GetEvnVal(dba.GetHashCode().ToString());
        //    return result == null ? null : result.ToString();
        //}

        public static void SetDefaultDbAccessorDbProviderName(DbAccessor dba)
        {
            if (GetDefaultDbAccessorDbProviderName() != null) return;

            lock (_objLock)
            {
                if (GetDefaultDbAccessorDbProviderName() == null)
                {
                    EnvValRepository.Instance.AddEvnVal(DefaultDbAccessorHashCode_Key, dba.DbProviderName.ToString());
                }
            }
        }

        public static string GetDefaultDbAccessorDbProviderName()
        {
            var result = EnvValRepository.Instance.GetEvnVal(DefaultDbAccessorHashCode_Key);
            return result == null ? null : result.ToString();
        }

    }
}
