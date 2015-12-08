using System;
using System.Collections.Generic;
using System.Text;

namespace Zhuang.Data.Annotations
{
    /// <summary>
    /// 主要用于标识自动生成值的主键字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class AutoGenerateAttribute : Attribute
    {
        public AutoGenerateAttribute()
        {
        }
    }
}
