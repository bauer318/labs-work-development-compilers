using DevCompilersLW2;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW3
{
    public static class TokenWorker
    {
        public static bool IsCorrectEqualSignPosition(int parCurrentTokenIndex,List<Token> parTokens)
        {
            int res = 0;
            for(var i = parCurrentTokenIndex-1; i >= 0; i--)
            {
                if(parTokens[i].TokenType==TokenType.CORRECT_IDENTIFICATOR || parTokens[i].TokenType == TokenType.OPEN_PARENTHESIS)
                {
                    if(parTokens[i].TokenType == TokenType.CORRECT_IDENTIFICATOR)
                    {
                        res++;
                    }
                }
                else
                {
                    return false;
                }
            }
            return res == 1;
        }

        public static bool IsTokenTypeOperatorLeft(int parCurrentTokenIndex, List<Token> parTokens)
        {
            bool result = false;
            
            if (parCurrentTokenIndex - 1 >= 0)
            {
                return IsTokenTypeOperator(parTokens[parCurrentTokenIndex - 1]) ||
                    parTokens[parCurrentTokenIndex - 1].TokenType == TokenType.EQUAL_SIGN;
            }
            return result;
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
            return parTokenType == TokenType.CORRECT_IDENTIFICATOR ||
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
                        PrintMessage("lexical error! Not opened brace for ", j, parTokens[j]);
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
                        PrintMessage("lexical error! Not closed brace for ", j, parTokens[j]);
                        count = 0;
                    }
                }
            }
        }
        
        public static void PrintMessage(string parMessage, int parCurrentTokenIndex, Token parToken)
        {
            Console.WriteLine("lexical error! " + parMessage + parToken.Lexeme + " at " + parCurrentTokenIndex);
        }
        public static void PrintMessage(string parMessage, int parCurrentTokenIndex)
        {
            Console.WriteLine("lexical error! "+parMessage + " at " + parCurrentTokenIndex);
        }
        public static void PrintMessage(string parMessage)
        {
            Console.WriteLine("lexical error! "+parMessage);
        }
    }
}
