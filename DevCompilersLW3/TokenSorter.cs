using DevCompilersLW2;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW3
{
    public class TokenSorter
    {
        public List<Token> Tokens;
        public int TokenPriority;

        public TokenSorter(List<Token> parTokens, int parPriority)
        {
            Tokens = parTokens;
            TokenPriority = parPriority;
        }
        public void PrintTokenSorter()
        {
            Console.Write("Priority " + this.TokenPriority+" ");
            foreach(Token token in this.Tokens)
            {
                Console.Write(token.Lexeme + " ");
            }
            Console.WriteLine();

        }
    }
}
