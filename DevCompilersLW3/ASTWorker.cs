using DevCompilersLW2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevCompilersLW3
{
    public class ASTWorker
    {
       
        public ASTWorker()
        {

        }
        public List<int> GetOperatorsPosition(List<Token> parTokens)
        {
            List<int> result = new List<int>();
            for (var i = 0; i < parTokens.Count; i++)
            {
                if (TokenWorker.IsTokenTypeOperator(parTokens[i]) || parTokens[i].TokenType == TokenType.EQUAL_SIGN)
                {
                    result.Add(i);
                }
            }

            return result;
        }
        private Token ToLeft(int currentOperatorIndex, List<Token> parTokens)
        {
            for (var i = currentOperatorIndex - 1; i >= 0; i--)
            {
                if (TokenWorker.IsOperand(parTokens[i].TokenType))
                {
                    return parTokens[i];
                }
            }
            return null;
        }
        private Token ToRight(int currentOperatorIndex, List<Token> parTokens)
        {
            for (var i = currentOperatorIndex + 1; i < parTokens.Count; i++)
            {
                if (TokenWorker.IsOperand(parTokens[i].TokenType))
                {
                    return parTokens[i];
                }
            }
            return null;
        }
        public void Print(List<int> listPos, List<Token> parTokens)
        {
            for (var i = 0; i < listPos.Count - 1; i++)
            {
                if (i == 0)
                {
                    Console.WriteLine(GetWhitespace((i + 1) * 3) + "<" + parTokens[listPos[i]].Lexeme + ">");
                }
                Console.WriteLine(GetWhitespace(i * 6) + "/---|---\\");
                Console.WriteLine(GetWhitespace(i * 6) + ToLeft(listPos[i], parTokens).Lexeme + GetWhitespace((7)) + "<" + parTokens[listPos[i + 1]].Lexeme + ">");
            }
            Console.WriteLine(GetWhitespace((listPos.Count - 1) * 6) + "/---|---\\");
            Console.WriteLine(GetWhitespace((listPos.Count - 1) * 6) + ToLeft(listPos[listPos.Count - 1], parTokens).Lexeme +
                GetWhitespace(((listPos.Count - 1) * 3)) + ToRight(listPos[listPos.Count - 1], parTokens).Lexeme);
        }
        private string GetWhitespace(int number)
        {
            string result = "";
            for (var i = 0; i <= number; i++)
            {
                result += " ";
            }
            return result;
        }
    }
}
