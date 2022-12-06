using DevCompilersLW2;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW3
{
    public class ParserOpt:ParserBase
    {
        public ParserOpt(SyntacticalErrorAnalyzer parSyntacticalErrorAnalyzer):base(parSyntacticalErrorAnalyzer)
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
                    if(TokenWorker.IsConstantType(rigthNode.Value.TokenType) && TokenWorker.IsConstantType(result.Value.TokenType))
                    {
                        result = GetOpAdd(result, rigthNode);
                    }
                    else
                    {
                        result = new TokenNode<Token>(tokenPlus, result, rigthNode);
                    }
                    _tokenNodes.Add(result);
                }
                else if (_currentToken.TokenType == TokenType.SOUSTRACTION_SIGN)
                {
                    GetNextToken();
                    TokenNode<Token> rigthNode = Factor();
                    Token tokenMinus = new Token(TokenType.SOUSTRACTION_SIGN, "-");
                    if (TokenWorker.IsConstantType(rigthNode.Value.TokenType) && TokenWorker.IsConstantType(result.Value.TokenType))
                    {
                        result = GetOpSub(result, rigthNode);
                    }
                    else
                    {
                        result = new TokenNode<Token>(tokenMinus, result, rigthNode);
                    }
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
                    if (TokenWorker.IsConstantType(rigthNode.Value.TokenType) && TokenWorker.IsConstantType(factor.Value.TokenType))
                    {
                        factor = GetOpMul(factor, rigthNode);
                    }
                    else
                    {
                        factor = new TokenNode<Token>(tokenMultiply, factor, rigthNode);
                    }
                    _tokenNodes.Add(factor);
                }
                else if (_currentToken.TokenType == TokenType.DIVISION_SIGN)
                {
                    GetNextToken();
                    TokenNode<Token> rigthNode = Term();
                    Token tokenDivision = new Token(TokenType.DIVISION_SIGN, "/");
                    if (TokenWorker.IsConstantType(rigthNode.Value.TokenType) && TokenWorker.IsConstantType(factor.Value.TokenType))
                    {
                        factor = GetOpDiv(factor, rigthNode);
                    }
                    else
                    {
                        factor = new TokenNode<Token>(tokenDivision, factor, rigthNode);
                    }
                    _tokenNodes.Add(factor);
                }
            }
            return factor;
        }
        private bool IsDecimalTokenConstant(TokenNode<Token> tokenNode)
        {
            return tokenNode.Value.TokenType == TokenType.CORRECT_DECIMAL_CONSTANT;
        }
        private TokenNode<Token> GetOpAdd(TokenNode<Token> leftNode, TokenNode<Token> rightNode)
        {
            Token leafToken = null;
            TokenNode<Token> result = null;
            if (leftNode.Value.TokenType == TokenType.CORRECT_DECIMAL_CONSTANT ||
                rightNode.Value.TokenType == TokenType.CORRECT_DECIMAL_CONSTANT)
            {
                double resultValue = Convert.ToDouble(leftNode.Value.Lexeme.Replace('.', ','))
                    + Convert.ToDouble(rightNode.Value.Lexeme.Replace('.', ','));
                string lexeme = resultValue.ToString().Replace(',', '.');
                leafToken = new Token(TokenType.CORRECT_DECIMAL_CONSTANT, lexeme);
                result = new TokenNode<Token>(leafToken);
            }
            else
            {
                int resultValue = Convert.ToInt32(leftNode.Value.Lexeme) +
                    Convert.ToInt32(rightNode.Value.Lexeme);
                string lexeme = resultValue.ToString();
                leafToken = new Token(TokenType.INTEGER_CONSTANT, lexeme);
                result = new TokenNode<Token>(leafToken);
            }
            return result;
        }
        private TokenNode<Token> GetOpSub(TokenNode<Token> leftNode, TokenNode<Token> rightNode)
        {
            Token leafToken = null;
            TokenNode<Token> result = null;
            if (leftNode.Value.TokenType == TokenType.CORRECT_DECIMAL_CONSTANT ||
                rightNode.Value.TokenType == TokenType.CORRECT_DECIMAL_CONSTANT)
            {
                double resultValue = Convert.ToDouble(leftNode.Value.Lexeme.Replace('.', ','))
                    - Convert.ToDouble(rightNode.Value.Lexeme.Replace('.', ','));
                string lexeme = resultValue.ToString().Replace(',', '.');
                leafToken = new Token(TokenType.CORRECT_DECIMAL_CONSTANT, lexeme);
                result = new TokenNode<Token>(leafToken);
            }
            else
            {
                int resultValue = Convert.ToInt32(leftNode.Value.Lexeme) -
                    Convert.ToInt32(rightNode.Value.Lexeme);
                string lexeme = resultValue.ToString();
                leafToken = new Token(TokenType.INTEGER_CONSTANT, lexeme);
                result = new TokenNode<Token>(leafToken);
            }
            return result;
        }
        private TokenNode<Token> GetOpMul(TokenNode<Token> leftNode, TokenNode<Token> rightNode)
        {
            Token leafToken = null;
            TokenNode<Token> result = null;
            if (leftNode.Value.TokenType == TokenType.CORRECT_DECIMAL_CONSTANT ||
                rightNode.Value.TokenType == TokenType.CORRECT_DECIMAL_CONSTANT)
            {
                double resultValue = Convert.ToDouble(leftNode.Value.Lexeme.Replace('.', ','))
                    * Convert.ToDouble(rightNode.Value.Lexeme.Replace('.', ','));
                string lexeme = resultValue.ToString().Replace(',', '.');
                leafToken = new Token(TokenType.CORRECT_DECIMAL_CONSTANT, lexeme);
                result = new TokenNode<Token>(leafToken);
            }
            else
            {
                int resultValue = Convert.ToInt32(leftNode.Value.Lexeme) *
                    Convert.ToInt32(rightNode.Value.Lexeme);
                string lexeme = resultValue.ToString();
                leafToken = new Token(TokenType.INTEGER_CONSTANT, lexeme);
                result = new TokenNode<Token>(leafToken);
            }
            return result;
        }
        private TokenNode<Token> GetOpDiv(TokenNode<Token> leftNode, TokenNode<Token> rightNode)
        {
            Token leafToken = null;
            TokenNode<Token> result = null;
            if(rightNode.Value.Lexeme.Equals("0.0") || rightNode.Value.Lexeme.Equals("0"))
            {
                throw new ArithmeticException("Division by zero");
                
            }
            if (leftNode.Value.TokenType == TokenType.CORRECT_DECIMAL_CONSTANT ||
                rightNode.Value.TokenType == TokenType.CORRECT_DECIMAL_CONSTANT)
            {
                double resultValue = Convert.ToDouble(leftNode.Value.Lexeme.Replace('.', ','))
                    / Convert.ToDouble(rightNode.Value.Lexeme.Replace('.', ','));
                string lexeme = resultValue.ToString().Replace(',', '.');
                leafToken = new Token(TokenType.CORRECT_DECIMAL_CONSTANT, lexeme);
                result = new TokenNode<Token>(leafToken);
            }
            else
            {
                int resultValue = Convert.ToInt32(leftNode.Value.Lexeme) /
                    Convert.ToInt32(rightNode.Value.Lexeme);
                string lexeme = resultValue.ToString();
                leafToken = new Token(TokenType.INTEGER_CONSTANT, lexeme);
                result = new TokenNode<Token>(leafToken);
            }
            return result;
        }
    }
}
