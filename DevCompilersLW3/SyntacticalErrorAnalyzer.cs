using DevCompilersLW2;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevCompilersLW3
{
    public class SyntacticalErrorAnalyzer
    {
        public List<Token> Tokens = new List<Token>();
        public SymbolTable SymbolTable;
        private AutomatState automatState;
           
        public SyntacticalErrorAnalyzer(List<Token> parTokens, SymbolTable parSymbolTable)
        {
            Tokens = parTokens;
            SymbolTable = parSymbolTable;
        }
        public bool IsSyntaxicalyCorrectExpression()
        {
            AutomateStateMethodes.currentTokenIndex = 0;
            AutomateStateMethodes.braceCount = 0;
            automatState = AutomatState.OPENED_BRACE_OPERAND;
            if (Tokens.Count == 1)
            {
                AutomateStateMethodes.OneLexemeCase(Tokens[0]);
            }
            else
            {
                while (automatState != AutomatState.END_EXPRESSION)
                {
                    automatState = automatState.Swip(Tokens);
                }
            }
            return AutomateStateMethodes.Can_Continue;
        }
    }
}
