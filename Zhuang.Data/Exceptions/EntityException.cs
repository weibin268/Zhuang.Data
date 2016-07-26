using System;
using System.Collections.Generic;

namespace Zhuang.Data.Exceptions
{
    public class EntityException : Exception
    {

        public EntityException() : base()//调用基类的构造器
        {

        }

        public EntityException(string message) : base(message)//调用基类的构造器
        {

        }

        public EntityException(string message, Exception innerException) : base(message, innerException)//调用基类的构造器
        {

        }

    }
}
