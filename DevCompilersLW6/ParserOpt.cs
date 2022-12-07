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
                    result = GetTokenNodeResultAdditionOperation(result, rigthNode);
                    _tokenNodes.Add(result);
                }
                else if (_currentToken.TokenType == TokenType.SOUSTRACTION_SIGN)
                {
                    GetNextToken();
                    TokenNode<Token> rigthNode = Factor();
                    result = GetTokenNodeResultSubtractionOperation(result, rigthNode);
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
                    factor = GetTokenNodeResultMultiplicationOperation(factor, rigthNode);
                    _tokenNodes.Add(factor);
                }
                else if (_currentToken.TokenType == TokenType.DIVISION_SIGN)
                {
                    GetNextToken();
                    TokenNode<Token> rigthNode = Term();
                    factor = GetTokenNodeResultDivisionOperation(factor, rigthNode);
                    _tokenNodes.Add(factor);
                }
            }
            return factor;
        }
        private bool IsDecimalTokenConstant(TokenNode<Token> tokenNode)
        {
            return tokenNode.Value.TokenType == TokenType.CORRECT_DECIMAL_CONSTANT;
        }
        private bool IsNeutralElementSubAdd(TokenNode<Token> tokenNode)
        {
            string lexeme = tokenNode.Value.Lexeme;
            return lexeme.Equals("0") || lexeme.Equals("0.0");
        }
        private bool IsNeutralElementMulDiv(TokenNode<Token> tokenNode)
        {
            string lexeme = tokenNode.Value.Lexeme;
            return lexeme.Equals("1") || lexeme.Equals("1.0");
        }
        
        private bool IsDecimalTokenVariable(TokenNode<Token> tokenNode)
        {
            return tokenNode.Value.TokenType == TokenType.CORRECT_DECIMAL_IDENTIFICATOR;
        }
        private bool IsIntegerTokenConstant(TokenNode<Token> tokenNode)
        {
            return tokenNode.Value.TokenType == TokenType.INTEGER_CONSTANT;
        }
        private TokenNode<Token> ConvertIntegerTokenConstantToFloat(TokenNode<Token> tokenNode)
        {
            string doubleLexeme = tokenNode.Value.Lexeme + ".0";
            return new TokenNode<Token>(new Token(TokenType.CORRECT_DECIMAL_CONSTANT, doubleLexeme));
        }

        private TokenNode<Token> RealizeConvertTokenConstantToFloatIfNecessary(TokenNode<Token> leftNode, TokenNode<Token> rightNode)
        {
            if (IsDecimalTokenVariable(leftNode) && IsIntegerTokenConstant(rightNode))
            {
                return ConvertIntegerTokenConstantToFloat(rightNode);
            }
            return rightNode;
            
        }

        private TokenNode<Token> GetTokenNodeResultAdditionOperation(TokenNode<Token> leftNode, TokenNode<Token> rightNode)
        {
            Token leafToken = null;
            TokenNode<Token> result = null;
            if(TokenWorker.IsConstantType(leftNode.Value.TokenType) && TokenWorker.IsConstantType(rightNode.Value.TokenType))
            {
                if (IsDecimalTokenConstant(leftNode) || IsDecimalTokenConstant(rightNode))
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
            }
            else
            {
                if (IsNeutralElementSubAdd(leftNode))
                {
                    result = rightNode;
                }
                else if (IsNeutralElementSubAdd(rightNode))
                {
                    result = leftNode;
                }
                else
                {
                    leftNode = RealizeConvertTokenConstantToFloatIfNecessary(rightNode,leftNode);
                    rightNode = RealizeConvertTokenConstantToFloatIfNecessary(leftNode, rightNode);
                    Token tokenPlus = new Token(TokenType.ADDITION_SIGN, "+");
                    result = new TokenNode<Token>(tokenPlus, leftNode, rightNode);
                }
                
            }
           
            return result;
        }
        private TokenNode<Token> GetTokenNodeResultSubtractionOperation(TokenNode<Token> leftNode, TokenNode<Token> rightNode)
        {
            Token leafToken = null;
            TokenNode<Token> result = null;
            if (TokenWorker.IsConstantType(leftNode.Value.TokenType) && TokenWorker.IsConstantType(rightNode.Value.TokenType))
            {
                if (IsDecimalTokenConstant(leftNode) || IsDecimalTokenConstant(rightNode))
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
            }
            else
            {
                if (IsNeutralElementSubAdd(leftNode))
                {
                    result = rightNode;
                }
                else if (IsNeutralElementSubAdd(rightNode))
                {
                    result = leftNode;
                }
                else
                {
                    leftNode = RealizeConvertTokenConstantToFloatIfNecessary(rightNode, leftNode);
                    rightNode = RealizeConvertTokenConstantToFloatIfNecessary(leftNode, rightNode);
                    Token tokenMinus = new Token(TokenType.SOUSTRACTION_SIGN, "-");
                    result = new TokenNode<Token>(tokenMinus, leftNode, rightNode);
                }
                
            }
            
            return result;
        }
        private TokenNode<Token> GetTokenNodeResultMultiplicationOperation(TokenNode<Token> leftNode, TokenNode<Token> rightNode)
        {
            Token leafToken = null;
            TokenNode<Token> result = null;
            if (TokenWorker.IsConstantType(leftNode.Value.TokenType) && TokenWorker.IsConstantType(rightNode.Value.TokenType))
            {
                if (IsDecimalTokenConstant(leftNode) || IsDecimalTokenConstant(rightNode))
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
            }
            else
            {
                if (IsNeutralElementMulDiv(leftNode))
                {
                    result = rightNode;
                }
                else if (IsNeutralElementMulDiv(rightNode))
                {
                    result = leftNode;
                }
                else
                {
                    leftNode = RealizeConvertTokenConstantToFloatIfNecessary(rightNode, leftNode);
                    rightNode = RealizeConvertTokenConstantToFloatIfNecessary(leftNode, rightNode);
                    Token tokenMul = new Token(TokenType.MULTIPLICATION_SIGN, "*");
                    result = new TokenNode<Token>(tokenMul, leftNode, rightNode);
                }
            }
            return result;
        }
        private TokenNode<Token> GetTokenNodeResultDivisionOperation(TokenNode<Token> leftNode, TokenNode<Token> rightNode)
        {
            Token leafToken = null;
            TokenNode<Token> result = null;
            if(rightNode.Value.Lexeme.Equals("0.0") || rightNode.Value.Lexeme.Equals("0"))
            {
                Console.WriteLine("Division by zero");
                DivideByZeroRunTimeException = true;
                return new TokenNode<Token>(new Token(TokenType.INTEGER_CONSTANT, "1"));
                
            }
            if (TokenWorker.IsConstantType(leftNode.Value.TokenType) && TokenWorker.IsConstantType(rightNode.Value.TokenType))
            {
                if (IsDecimalTokenConstant(leftNode) || IsDecimalTokenConstant(rightNode))
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
            }
            else
            {
                if (IsNeutralElementMulDiv(rightNode))
                {
                    result = leftNode;
                }
                else
                {
                    leftNode = RealizeConvertTokenConstantToFloatIfNecessary(rightNode, leftNode);
                    rightNode = RealizeConvertTokenConstantToFloatIfNecessary(leftNode, rightNode);
                    Token tokenDiv = new Token(TokenType.DIVISION_SIGN, "/");
                    result = new TokenNode<Token>(tokenDiv, leftNode, rightNode);
                }
                
            }
            return result;
        }

    }
}
