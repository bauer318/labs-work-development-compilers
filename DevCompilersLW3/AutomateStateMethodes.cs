using DevCompilersLW2;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW3
{
    public static class AutomateStateMethodes
    {
        public static int braceCount = 0;
        public static int currentTokenIndex = 0;
        public static bool  Can_Continue = true;
        private static AutomatState AutomateStateFrom = AutomatState.OPENED_BRACE_OPERAND;
        private static AutomatState nextAutomateState = AutomatState.OPENED_BRACE_OPERAND;
        public static AutomatState Swip(this AutomatState parAutomatState,List<Token> parTokens)
        {
            Token currentToken = parTokens[currentTokenIndex];
            TokenType currentTokenType = currentToken.TokenType;
            var tokensNumber = parTokens.Count;
            switch (parAutomatState)
            {
                case AutomatState.OPENED_BRACE_OPERAND:
                    if (currentTokenIndex < tokensNumber - 1)
                    {
                        switch (currentTokenType)
                        {
                            case TokenType.OPEN_PARENTHESIS:
                                OpenBrace();
                                NextToken();
                                AutomateStateFrom = AutomatState.OPENED_BRACE_OPERAND;
                                break;
                            case TokenType.CORRECT_DECIMAL_CONSTANT:
                            case TokenType.CORRECT_DEFAULT_IDENTIFICATOR:
                            case TokenType.INTEGER_CONSTANT:
                                NextToken();
                                nextAutomateState = AutomatState.CLOSED_BRACE_OPERATOR;
                                AutomateStateFrom = AutomatState.CLOSED_BRACE_OPERATOR;
                                break;
                            default:
                                CannotGenerateSyntaxThree();
                                AutomateStateFrom = AutomatState.CLOSED_BRACE_OPERATOR;
                                switch (currentTokenType)
                                {
                                    case TokenType.ADDITION_SIGN:
                                    case TokenType.SOUSTRACTION_SIGN:
                                    case TokenType.DIVISION_SIGN:
                                    case TokenType.MULTIPLICATION_SIGN:
                                        if (TokenWorker.IsTokenTypeOperatorLeft(currentTokenIndex, parTokens))
                                        {
                                            TokenWorker.PrintMessage("последовательные операторы друг за другом ", currentTokenIndex);
                                        }
                                        else
                                        {
                                            
                                            TokenWorker.PrintMessage("У операция ", " отсутствует операнд", currentTokenIndex, currentToken);
                                        }
                                        break;
                                    case TokenType.CLOSE_PARENTHESIS:
                                        CloseBrace();
                                        if (braceCount < 0)
                                        {
                                            braceCount = 0;
                                        }
                                        else if (braceCount == 0 || TokenWorker.IsOpenedBraceLeft(currentTokenIndex, parTokens))
                                        {
                                            TokenWorker.PrintMessage("Пустые скобки ", currentTokenIndex);
                                        }
                                        else if (TokenWorker.IsTokenTypeOperatorLeft(currentTokenIndex, parTokens))
                                        {
                                            TokenWorker.PrintMessage("У операция ", " отсутствует операнд", currentTokenIndex, parTokens[currentTokenIndex - 1]);
                                        }
                                        else
                                        {
                                            var helpText = "у константа";
                                            if (currentToken.TokenType == TokenType.CORRECT_DEFAULT_IDENTIFICATOR)
                                            {
                                                helpText = "у идентификатора ";
                                            }

                                            TokenWorker.PrintMessage(helpText, " отсутвует операция ", currentTokenIndex, parTokens[currentTokenIndex + 1]);
                                        }
                                        nextAutomateState = AutomatState.CLOSED_BRACE_OPERATOR;
                                        break;
                                }
                                NextToken();
                                break;

                        }
                    }
                    else
                    {
                        nextAutomateState = AutomatState.CLOSED_BRACE_OPERAND;
                       
                    }
                    break;
                case AutomatState.CLOSED_BRACE_OPERATOR:
                    
                    if (currentTokenIndex < tokensNumber - 1)
                    {
                        switch(currentTokenType)
                        {
                            case TokenType.CLOSE_PARENTHESIS:
                                CloseBrace();
                                AutomateStateFrom = AutomatState.CLOSED_BRACE_OPERATOR;
                                break;
                            case TokenType.OPEN_PARENTHESIS:
                                OpenBrace();
                                TokenWorker.PrintMessage("перед отрывающей скобки ", " отсутвует операция ", currentTokenIndex, currentToken);
                                nextAutomateState = AutomatState.OPENED_BRACE_OPERAND;
                                AutomateStateFrom = AutomatState.OPENED_BRACE_OPERAND;
                                CannotGenerateSyntaxThree();
                                break;
                            default:
                                nextAutomateState = AutomatState.OPENED_BRACE_OPERAND;
                                AutomateStateFrom = AutomatState.OPENED_BRACE_OPERAND;
                                break;
                        }
                        NextToken();
                    }
                    else
                    {
                        nextAutomateState = AutomatState.CLOSED_BRACE_OPERAND;
                    }
                    break;
                case AutomatState.CLOSED_BRACE_OPERAND:
                    switch (currentTokenType)
                    {
                        case TokenType.CLOSE_PARENTHESIS:
                            CloseBrace();
                            if (braceCount != 0)
                            {
                                CannotGenerateSyntaxThree();
                            }
                            break;
                        case TokenType.OPEN_PARENTHESIS:
                            OpenBrace();
                            CannotGenerateSyntaxThree();
                            break;
                    }
                    if (braceCount != 0)
                    {
                        if (braceCount > 0)
                        {
                            TokenWorker.PrintErrorNotClosedBrace(parTokens);
                        }
                        else
                        {
                           
                            TokenWorker.PrintErrorNotOpenedBrace(parTokens);
                        }
                        CannotGenerateSyntaxThree();
                    }
                    switch (AutomateStateFrom)
                    {
                        case AutomatState.OPENED_BRACE_OPERAND:
                            if (!TokenWorker.IsOperand(currentToken.TokenType))
                            { 
                                CannotGenerateSyntaxThree();
                                if (currentToken.TokenType == TokenType.CLOSE_PARENTHESIS)
                                {
                                    TokenWorker.PrintMessage("Перед закрывающей скобке ", " отсутствует операнд ", currentTokenIndex, currentToken);
                                }
                                else if (currentToken.TokenType == TokenType.OPEN_PARENTHESIS)
                                {
                                    TokenWorker.PrintMessage("Перед открывающей скобке ", " отсутствует операнд ", currentTokenIndex, currentToken);
                                }
                                else if (TokenWorker.IsOperand(currentToken.TokenType))
                                {
                                    TokenWorker.PrintMessage("последовательные операторы друг за другом ", currentTokenIndex);
                                }
                            }
                            break;
                        case AutomatState.CLOSED_BRACE_OPERATOR:
                            if (currentToken.TokenType != TokenType.CLOSE_PARENTHESIS)
                            {
                                CannotGenerateSyntaxThree();
                                if (TokenWorker.IsOperand(currentToken.TokenType))
                                {
                                    
                                    TokenWorker.PrintMessage("У операнда ", " отсутствует операция ", currentTokenIndex, currentToken);
                                }
                                else if (TokenWorker.IsTokenTypeOperator(currentToken))
                                {
                                    
                                    TokenWorker.PrintMessage("У операция ", " отсутствует операнд ", currentTokenIndex, currentToken);
                                }
                                else if (currentToken.TokenType == TokenType.OPEN_PARENTHESIS)
                                {
                                    
                                    TokenWorker.PrintMessage("Перед открывающей скобке ", " отсутствует операция ", currentTokenIndex, currentToken);
                                }
                            }
                            break;
                    }
                    nextAutomateState = AutomatState.END_EXPRESSION;
                    break;

            }
            return nextAutomateState;
        }
        public static void OneLexemeCase(Token parToken)
        {
            CannotGenerateSyntaxThree();
            switch (parToken.TokenType)
            {
                case TokenType.SOUSTRACTION_SIGN:
                case TokenType.ADDITION_SIGN:
                case TokenType.DIVISION_SIGN:
                case TokenType.MULTIPLICATION_SIGN:
                    TokenWorker.PrintMessage("У операция ", " отсутствует операнд", currentTokenIndex, parToken);
                    break;
                case TokenType.OPEN_PARENTHESIS:
                case TokenType.CLOSE_PARENTHESIS:
                    TokenWorker.PrintMessage("У скобки ", " отсутствует выражение ", currentTokenIndex, parToken);
                    break;
                case TokenType.CORRECT_DECIMAL_CONSTANT:
                case TokenType.CORRECT_DEFAULT_IDENTIFICATOR:
                case TokenType.INTEGER_CONSTANT:
                    var helpText = "у константа ";
                    if (parToken.TokenType == TokenType.CORRECT_DEFAULT_IDENTIFICATOR)
                    {
                        helpText = "у идентификатора ";
                    }
                    TokenWorker.PrintMessage(helpText, " отсутвует операция ", currentTokenIndex, parToken);
                    break;

            }
        }
        private static void CannotGenerateSyntaxThree()
        {
            Can_Continue = false;
        }
        private static void NextToken()
        {
            currentTokenIndex++;
        }
        private static void OpenBrace()
        {
            braceCount++;
        }
        private static void CloseBrace()
        {
            braceCount--;
        }
      
    }
}
