using System;
using System.Collections.Generic;
using System.Text;

namespace Zhuang.Data.Annotations
{
    /// <summary>
    /// 用于标识是否为主键
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class KeyAttribute : Attribute
    {
        public KeyAttribute()
        {
        }
    }
}
