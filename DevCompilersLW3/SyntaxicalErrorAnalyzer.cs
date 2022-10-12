using DevCompilersLW2;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevCompilersLW3
{
    public class SyntaxicalErrorAnalyzer
    {
        private List<Token> tokens = new List<Token>();
        private AutomatState automatState;
           
        public SyntaxicalErrorAnalyzer(List<Token> parTokens)
        {
            tokens = parTokens;
        }
        public bool IsSyntaxicalyCorrectExpression()
        {
            Console.WriteLine("Expression ");
            for(var i = 0; i<tokens.Count;i++)
            {
                Console.Write(tokens[i].Lexeme);
            }
            Console.WriteLine(" ");
            AutomateStateMethodes.e = 0;
            AutomateStateMethodes.i = 0;
            AutomateStateMethodes.k = 0;
            automatState = AutomatState.OPENED_BRACE_OPERAND;
            if (tokens.Count == 1)
            {
                AutomateStateMethodes.OneLexemeCase(tokens[0]);
            }
            else
            {
                while (automatState != AutomatState.END_EXPRESSION)
                {
                    automatState = automatState.Swip(tokens);
                }
            }
            return AutomateStateMethodes.Can_Continue;
        }
    }
}
