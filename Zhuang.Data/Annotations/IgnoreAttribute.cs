using System;
using System.Collections.Generic;
using System.Text;

namespace Zhuang.Data.Annotations
{
    /// <summary>
    /// 用于标识忽略实体的属性，使该属性将不参数sql语句的生成
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class IgnoreAttribute : Attribute
    {
        public IgnoreAttribute()
        {
        }
    }
}
