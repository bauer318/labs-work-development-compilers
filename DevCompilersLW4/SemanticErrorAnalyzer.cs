using DevCompilersLW2;
using DevCompilersLW3;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW4
{
    public class SemanticErrorAnalyzer
    {
        private TokenNode<Token> _modifiedSyntaxTree;
        public SemanticErrorAnalyzer(TokenNode<Token> parModifiedSyntaxTree)
        {
            _modifiedSyntaxTree = parModifiedSyntaxTree;
        }
        private bool IsTokenDivOperator(TokenNode<Token> parTokenNode)
        {
            return parTokenNode.Value.TokenType == TokenType.DIVISION_SIGN;
        }
        private bool IsTokenConstantZero(TokenNode<Token> parTokenNode)
        {
            if(parTokenNode.Value.TokenType==TokenType.INTEGER_CONSTANT ||
                parTokenNode.Value.TokenType == TokenType.CORRECT_DECIMAL_CONSTANT)
            {
                return IsLexemeConstantZero(parTokenNode.Value.Lexeme);
            }
            if(parTokenNode.Value.TokenType == TokenType.INT_2_FLOAT)
            {
                TokenNode<Token> convertedTokenNode = parTokenNode.ConvertedTokenNode;
                return IsLexemeConstantZero(convertedTokenNode.Value.Lexeme);
            }
            return false;
        }
        private bool IsLexemeConstantZero(string parLexeme)
        {
            return parLexeme.Equals("0") || parLexeme.Equals("0.0");
        }
        private bool IsDivisionByZero(TokenNode<Token> parTokenNode)
        {
            return IsTokenDivOperator(parTokenNode) && IsTokenConstantZero(parTokenNode.RightNode);
        }
        public void CheckingDivisionByZero()
        {
            CheckDivisionByZero(_modifiedSyntaxTree);
        }
        private void CheckDivisionByZero(TokenNode<Token> parRoot)
        {
            if (parRoot != null)
            {
                if (!TokenNode<Token>.IsLeafToken(parRoot))
                {
                    if (IsDivisionByZero(parRoot))
                    {
                        Console.WriteLine("Div by zero");
                    }
                    else
                    {
                        if (TokenNode<Token>.IsLeafToken(parRoot.LeftNode))
                        {
                            CheckDivisionByZero(parRoot.RightNode);
                        }
                        else if (TokenNode<Token>.IsLeafToken(parRoot.RightNode))
                        {
                            CheckDivisionByZero(parRoot.LeftNode);
                        }
                    }
                }
            }
        }
    }
}
