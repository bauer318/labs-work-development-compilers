using DevCompilersLW2;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter
{
    public static class InterpreterWorker
    {
        public static string GetVariableTypeDescription(TokenType tokenType)
        {
            switch (tokenType)
            {
                case TokenType.CORRECT_DECIMAL_IDENTIFICATOR:
                    return "float";
                case TokenType.CORRECT_INTEGER_IDENTIFICATOR:
                    return "int";
            }
            return "unknow";
        }
    }
}
