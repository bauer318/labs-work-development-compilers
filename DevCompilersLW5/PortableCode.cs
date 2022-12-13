using DevCompilersLW2;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW5
{
    [System.Serializable]
    public class PortableCode
    {
        public string OperationCode { get; set; }
        public Token Result { get; set; }
        public List<Token> OperandList { get;}

        public int Id { get; }
        
        public PortableCode(string parOperationCode, Token parResult, List<Token> parOperandList, int parId)
        {
            OperationCode = parOperationCode;
            Result = parResult;
            OperandList = parOperandList;
            Id = parId;
        }
        public PortableCode(string parOperationCode,Token parResult,int parId)
        {
            Result = parResult;
            OperationCode = parOperationCode;
            OperandList = new List<Token>();
            Id = parId;
        }
        public PortableCode(Token parResult)
        {
            Result = parResult;
            OperationCode = string.Empty;
            OperandList = new List<Token>();
            Id = parResult.AttributeValue;
        }
        public String toString()
        {
            String res = "Code " + this.OperationCode + " Result " + this.Result.Lexeme + " Operands ";
            foreach(Token t in this.OperandList)
            {
                res += " " + t.Lexeme;
            }
            return res + " id "+this.Id;
        }
        
        public static PortableCode GetPortableCodeResultById(int parId, List<PortableCode> portableCodes)
        {
            foreach(PortableCode portable in portableCodes)
            {
                if (portable.Id == parId)
                {
                    return portable;
                }
            }
            return null;
        }
    }
}
