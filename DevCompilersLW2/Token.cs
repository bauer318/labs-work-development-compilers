using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW2
{
    public class Token
    {
        public TokenType TokenType { get; set; }
        public string Lexeme { get; set; }
        public int AttributeValue { get; set; }
        public Token(TokenType tokenType)
        {
            TokenType = tokenType;
            Lexeme = string.Empty;
        }

        public Token(TokenType parTokenType, string parLexeme,int parAttributeValue = 0)
        {
            TokenType = parTokenType;
            Lexeme = parLexeme;
            AttributeValue = parAttributeValue;
        }
        
        public string GetTokenName()
        {
            switch (TokenType)
            {
                case TokenType.CorrectIdentificator:
                    return "id";

                default:
                    return Lexeme;
            } 
        }
    }
}
