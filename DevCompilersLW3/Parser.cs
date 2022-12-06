using DevCompilersLW2;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW3
{
    public class Parser:ParserBase
    {
        public Parser(SyntacticalErrorAnalyzer parSyntacticalErrorAnalyzer):base(parSyntacticalErrorAnalyzer)
        {
            
        }
        public override TokenNode<Token> ParseExpression()
        {
            TokenNode<Token> result = Factor();
            while (result != null && _termItems.Contains(_currentToken.TokenType))
            {
                if (_currentToken.TokenType == TokenType.ADDITION_SIGN)
                {
                    GetNextToken();
                    TokenNode<Token> rigthNode = Factor();
                    Token tokenPlus = new Token(TokenType.ADDITION_SIGN, "+");
                    result = new TokenNode<Token>(tokenPlus, result, rigthNode);
                    _tokenNodes.Add(result);
                }
                else if (_currentToken.TokenType == TokenType.SOUSTRACTION_SIGN)
                {
                    GetNextToken();
                    TokenNode<Token> rigthNode = Factor();
                    Token tokenMinus = new Token(TokenType.SOUSTRACTION_SIGN, "-");
                    result = new TokenNode<Token>(tokenMinus, result, rigthNode);
                    _tokenNodes.Add(result);
                }
            }

            return result;
        }
        public override TokenNode<Token> Factor()
        {
            TokenNode<Token> factor = Term();
            while (factor != null && _factorItems.Contains(_currentToken.TokenType))
            {
                if (_currentToken.TokenType == TokenType.MULTIPLICATION_SIGN)
                {
                    GetNextToken();
                    TokenNode<Token> rigthNode = Term();
                    Token tokenMultiply = new Token(TokenType.MULTIPLICATION_SIGN, "*");
                    factor = new TokenNode<Token>(tokenMultiply, factor, rigthNode);
                    _tokenNodes.Add(factor);
                }
                else if (_currentToken.TokenType == TokenType.DIVISION_SIGN)
                {
                    GetNextToken();
                    TokenNode<Token> rigthNode = Term();
                    Token tokenDivision = new Token(TokenType.DIVISION_SIGN, "/");
                    factor = new TokenNode<Token>(tokenDivision, factor, rigthNode);
                    _tokenNodes.Add(factor);
                }
            }
            return factor;
        }
    }

}
