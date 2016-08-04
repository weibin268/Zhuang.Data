using System;
using System.Collections.Generic;
using System.Text;
using Zhuang.Data.EnvironmentVariable;

namespace Zhuang.Data.Common
{
    class EnvValService
    {
        const string DefaultDbAccessorHashCode_Key = "DefaultDbAccessorHashCode";

        public static void SetDbAccessorDbProviderName(DbAccessor dba, DbProviderName dbProviderName)
        {
            EnvValRepository.Instance.AddEvnVal(dba.GetHashCode().ToString(), dbProviderName.ToString());
        }

        public static string GetDbAccessorDbProviderName(DbAccessor dba)
        {
            var result = EnvValRepository.Instance.GetEvnVal(dba.GetHashCode().ToString());
            return result == null ? null : result.ToString();
        }

        public static void SetDefaultDbAccessorHashCode(DbAccessor dba)
        {
            EnvValRepository.Instance.AddEvnVal(DefaultDbAccessorHashCode_Key, dba.GetHashCode().ToString());
        }

        public static string GetDefaultDbAccessorHashCode()
        {
            var result = EnvValRepository.Instance.GetEvnVal(DefaultDbAccessorHashCode_Key);
            return result == null ? null : result.ToString();
        }

    }
}
