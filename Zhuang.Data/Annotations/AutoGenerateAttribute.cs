using System;
using System.Collections.Generic;
using System.Text;

namespace Zhuang.Data.Annotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class AutoGenerateAttribute : Attribute
    {
        public AutoGenerateAttribute()
        {
        }
    }
}
