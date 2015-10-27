using System;
using System.Collections.Generic;
using System.Text;

namespace Zhuang.Data.Annotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class KeyAttribute : Attribute
    {
        public KeyAttribute()
        {
        }
    }
}
