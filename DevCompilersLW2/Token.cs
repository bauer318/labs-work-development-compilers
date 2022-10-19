using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW2
{
    public class Token
    {
        public readonly TokenType TokenType;
        public readonly string Lexeme ;
        public readonly int AttributeValue;
        public int TokenPriority;
        public Token(TokenType parTokenType, string parLexeme,int parAttributeValue = 0)
        {
            TokenType = parTokenType;
            Lexeme = parLexeme;
            AttributeValue = parAttributeValue;
            TokenPriority = 0;
        }
        public Token(Token parToken, int parPriority)
        {
            TokenType = parToken.TokenType;
            Lexeme = parToken.Lexeme;
            AttributeValue = parToken.AttributeValue;
            TokenPriority = parPriority;
        }
    }
}
