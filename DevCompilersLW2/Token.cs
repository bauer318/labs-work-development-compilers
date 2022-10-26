using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW2
{
    public class Token
    {
        public TokenType TokenType { get; set; }
        public readonly string Lexeme ;
        public readonly int AttributeValue;
        public Token(TokenType parTokenType, string parLexeme,int parAttributeValue = 0)
        {
            TokenType = parTokenType;
            Lexeme = parLexeme;
            AttributeValue = parAttributeValue;
        }
    }
}
