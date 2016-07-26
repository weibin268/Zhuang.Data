using System;
using System.Collections.Generic;

namespace Zhuang.Data.Exceptions
{
    public class MyException : Exception
    {

        public MyException() : base()//调用基类的构造器
        {

        }

        public MyException(string message) : base(message)//调用基类的构造器
        {

        }

        public MyException(string message, Exception innerException) : base(message, innerException)//调用基类的构造器
        {

        }

    }
}
