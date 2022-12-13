using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter
{
    internal class DivByZeroCustomException:Exception
    {
        public string Message { get; }
        public DivByZeroCustomException(string message)
        {
            Message = message;
        }
    }
}
