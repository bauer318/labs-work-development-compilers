using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW2
{
    public class Token
    {
        public readonly TokenType TokenType;
        public readonly string name ;
        public readonly int AttributeValue;
        public Token(TokenType parTokenType, string parName,int parAttributeValue = 0)
        {
            TokenType = parTokenType;
            name = parName;
            AttributeValue = parAttributeValue;
        }
    }
}
