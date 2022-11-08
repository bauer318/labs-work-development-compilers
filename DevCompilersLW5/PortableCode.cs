using DevCompilersLW2;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW5
{
    public class PortableCode
    {
        public string OperationCode { get; }
        public Token Result { get; }
        public List<Token> OperandList { get;}
        
        public PortableCode(string parOperationCode, Token parResult, List<Token> parOperandList)
        {
            OperationCode = parOperationCode;
            Result = parResult;
            OperandList = parOperandList;
        }
    }
}
