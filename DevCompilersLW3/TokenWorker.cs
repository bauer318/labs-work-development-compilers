using DevCompilersLW2;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW3
{
    public static class TokenWorker
    {
        
        public static bool IsTokenTypeOperatorLeft(int parCurrentTokenIndex, List<Token> parTokens)
        {
            bool result = false;
            
            if (parCurrentTokenIndex - 1 >= 0)
            {
                return IsTokenTypeOperator(parTokens[parCurrentTokenIndex - 1]);
            }
            return result;
        }
        
        public static bool IsTokenOperandEqualType(TokenType parLeftTokenType, TokenType parRightTokenType)
        {
            return (IsTokenOperandDecimalType(parLeftTokenType) && IsTokenOperandDecimalType(parRightTokenType)) ||
                (IsTokenOperandIntegerType(parLeftTokenType) && IsTokenOperandIntegerType(parRightTokenType));
        }
        public static bool IsTokenOperandDecimalType(TokenType parTokenType)
        {
            return parTokenType == TokenType.CORRECT_DECIMAL_CONSTANT || parTokenType == TokenType.CORRECT_DECIMAL_IDENTIFICATOR ||
                parTokenType == TokenType.INT_2_FLOAT;
        }
        public static bool IsTokenOperandIntegerType(TokenType parTokenType)
        {
            return parTokenType == TokenType.INTEGER_CONSTANT || parTokenType == TokenType.CORRECT_INTEGER_IDENTIFICATOR;
        }
        public static bool IsTokenTypeOperator(Token parToken)
        {
            TokenType tokenType = parToken.TokenType;
            return tokenType == TokenType.ADDITION_SIGN ||
                    tokenType == TokenType.DIVISION_SIGN ||
                    tokenType == TokenType.MULTIPLICATION_SIGN ||
                    tokenType == TokenType.SOUSTRACTION_SIGN;
        }
        public static bool IsOperand(TokenType parTokenType)
        {
            return parTokenType == TokenType.CORRECT_DECIMAL_IDENTIFICATOR ||
                parTokenType == TokenType.CORRECT_INTEGER_IDENTIFICATOR||
                parTokenType == TokenType.CORRECT_DECIMAL_CONSTANT ||
                parTokenType == TokenType.INTEGER_CONSTANT;
        }
        public static bool IsOpenedBraceLeft(int parCurrentTokenIndex, List<Token> parTokens)
        {
            if (parCurrentTokenIndex - 1 >= 0)
            {
                return parTokens[parCurrentTokenIndex - 1].TokenType == TokenType.OPEN_PARENTHESIS;
            }
            return false;
        }
        public static int GetPositionLastOpenedBrace(List<Token> parTokens)
        {
            for(var i = parTokens.Count-1; i >= 0; i--)
            {
                if (parTokens[i].TokenType == TokenType.OPEN_PARENTHESIS)
                {
                    return i;
                }
            }
            return -1;
        }
        public static void PrintErrorNotOpenedBrace(List<Token> parTokens)
        {
            var count = 0;
            for (var j = 0; j < parTokens.Count; j++)
            {
                if (parTokens[j].TokenType == TokenType.OPEN_PARENTHESIS)
                {
                    count++;
                }
                else if (parTokens[j].TokenType == TokenType.CLOSE_PARENTHESIS)
                {
                    count--;
                    if (count < 0)
                    {
                        PrintMessage("У закрывающей скобки ", "отсутвует открывающая скобка", j, parTokens[j]);
                        count = 0;
                    }
                }
            }
        }
        public static void PrintErrorNotClosedBrace(List<Token> parTokens)
        {
            var count = 0;
            for (var j = parTokens.Count - 1; j >= 0; j--)
            {
                if (parTokens[j].TokenType == TokenType.CLOSE_PARENTHESIS)
                {
                    count++;
                }
                else if (parTokens[j].TokenType == TokenType.OPEN_PARENTHESIS)
                {
                    count--;
                    if (count < 0)
                    {
                        PrintMessage("У открывающей скобки ","отсутвует закрывающая скобка", j, parTokens[j]);
                        count = 0;
                    }
                }
            }
        }
        
        public static void PrintMessage(string parBeginErrorMessage,string parEndErrorMessage, int parCurrentTokenIndex, Token parToken)
        {
            Console.WriteLine("Синтаксическая ошибка! " + parBeginErrorMessage + parToken.TokenType.GetTokenNodeDescription(parToken) +
                " на позиции " + parCurrentTokenIndex+" "+parEndErrorMessage);
        }
        public static void PrintMessage(string parMessage, int parCurrentTokenIndex)
        {
            Console.WriteLine("Синтаксическая ошибка! " + parMessage + " на позиции " + parCurrentTokenIndex);
        }
        public static void PrintMessage(string parMessage)
        {
            Console.WriteLine("Синтаксическая ошибка! " + parMessage);
        }
        private static TokenNode<Token> GetTokenOperatorResultType(TokenNode<Token> parLeftNode, TokenNode<Token> parRightNode)
        {
            TokenNode<Token> result = new TokenNode<Token>(new Token(TokenType.INVALID, "CONVERTED"));
            if (TokenNode<Token>.IsLeafToken(parLeftNode) && TokenNode<Token>.IsLeafToken(parRightNode))
            {
                if (!TokenWorker.IsTokenOperandEqualType(parLeftNode.Value.TokenType, parRightNode.Value.TokenType))
                {
                    result = new TokenNode<Token>(new Token(TokenType.INT_2_FLOAT, "CONVERTED"));
                }
                else
                {
                    result = parLeftNode;
                }
            }
            else
            {
                if (TokenNode<Token>.IsLeafToken(parLeftNode))
                {
                    if(parRightNode != null)
                    result = GetTokenOperatorResultType(parLeftNode, GetTokenOperatorResultType(parRightNode.LeftNode, parRightNode.RightNode));
                }
                else if (TokenNode<Token>.IsLeafToken(parRightNode))
                {
                    result = GetTokenOperatorResultType(GetTokenOperatorResultType(parLeftNode.LeftNode, parLeftNode.RightNode), parRightNode);

                }
                else
                {
                    if (parRightNode != null)
                        result = GetTokenOperatorResultType(GetTokenOperatorResultType(parLeftNode.LeftNode, parLeftNode.RightNode),
                            GetTokenOperatorResultType(parRightNode.LeftNode, parRightNode.RightNode));
                    else
                        result = GetTokenOperatorResultType(parLeftNode.LeftNode, parLeftNode.RightNode);
                }
            }

            return result;
        }
        public static TokenType GetTokenNodeType(TokenNode<Token> parNode)
        {
            if (TokenNode<Token>.IsLeafToken(parNode) || parNode.Value.TokenType == TokenType.INT_2_FLOAT)
            {
                return parNode.Value.TokenType;
            }
            else
            {
                return GetTokenOperatorResultType(parNode.LeftNode, parNode.RightNode).Value.TokenType;
            }

        }
        public static string GetPortableCodeResultLexeme(int parId)
        {
            return "T" + parId;
        }
        public static string GetPortableCodeNodeDescription(Token parToken)
        {
            return parToken.AttributeValue == 0 ? parToken.Lexeme : "<id," + parToken.AttributeValue.ToString() + ">"; ;
        }
        public static bool IsTokenConstantType(Token parToken)
        {
            return parToken.TokenType == TokenType.CORRECT_DECIMAL_CONSTANT || parToken.TokenType == TokenType.INTEGER_CONSTANT;
        }
    }
}
