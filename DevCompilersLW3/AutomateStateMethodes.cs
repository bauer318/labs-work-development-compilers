using DevCompilersLW2;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW3
{
    public static class AutomateStateMethodes
    {
        public static int k = 0;
        public static int i = 0;
        public static int e = 0;
        public static bool  Can_Continue = true;
        private static AutomatState AutomateStateFrom = AutomatState.OPENED_BRACE_OPERAND;
        private static AutomatState nextAutomateState = AutomatState.OPENED_BRACE_OPERAND;
        public static AutomatState Swip(this AutomatState parAutomatState,List<Token> parTokens)
        {
            Token currentToken = parTokens[i];
            TokenType currentTokenType = currentToken.TokenType;
            var tokensNumber = parTokens.Count;
            switch (parAutomatState)
            {
                case AutomatState.OPENED_BRACE_OPERAND:
                    if (i < tokensNumber - 1)
                    {
                        switch (currentTokenType)
                        {
                            case TokenType.OPEN_PARENTHESIS:
                                OpenBrace();
                                NextToken();
                                break;
                            case TokenType.CORRECT_DECIMAL_CONSTANT:
                            case TokenType.CORRECT_IDENTIFICATOR:
                            case TokenType.INTEGER_CONSTANT:
                                NextToken();
                                nextAutomateState = AutomatState.CLOSED_BRACE_OPERATOR;
                                break;
                            default:
                                CannotGenerateSyntaxThree();
                                switch (currentTokenType)
                                {
                                    case TokenType.ADDITION_SIGN:
                                    case TokenType.SOUSTRACTION_SIGN:
                                    case TokenType.DIVISION_SIGN:
                                    case TokenType.MULTIPLICATION_SIGN:
                                        if (TokenWorker.IsTokenTypeOperatorLeft(i, parTokens))
                                        {
                                            TokenWorker.PrintMessage("Successive operator ", i);
                                        }
                                        else
                                        {
                                            TokenWorker.PrintMessage("Not operand for ", i, currentToken);
                                        }
                                        break;
                                    case TokenType.EQUAL_SIGN:
                                        AddEqualSign();
                                        if (e > 1)
                                        {
                                            TokenWorker.PrintMessage("They are many equal sign in the expression ", i, currentToken);
                                        }
                                        else if (TokenWorker.IsCorrectEqualSignPosition(i, parTokens))
                                        {
                                            TokenWorker.PrintMessage("Not identificator for ", i, currentToken);
                                        }
                                        else
                                        {
                                            TokenWorker.PrintMessage("Wrong possition for ", i, currentToken);
                                        }
                                        break;
                                    case TokenType.CLOSE_PARENTHESIS:
                                        CloseBrace();
                                        if (k < 0)
                                        {
                                            k = 0;
                                        }
                                        else if (k == 0 || TokenWorker.IsOpenedBraceLeft(i, parTokens))
                                        {
                                            TokenWorker.PrintMessage("Empty braces at ", i);
                                        }
                                        else if (TokenWorker.IsTokenTypeOperatorLeft(i, parTokens))
                                        {
                                            TokenWorker.PrintMessage("Not operand for ", i, parTokens[i - 1]);
                                        }
                                        else
                                        {
                                            TokenWorker.PrintMessage("Not operator for ", i, parTokens[i + 1]);
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
                        AutomateStateFrom = AutomatState.OPENED_BRACE_OPERAND;
                    }
                    break;
                case AutomatState.CLOSED_BRACE_OPERATOR:
                    if (i < tokensNumber - 1)
                    {
                        switch(currentTokenType)
                        {
                            case TokenType.CLOSE_PARENTHESIS:
                                CloseBrace();
                                if (k != 0)
                                {
                                    CannotGenerateSyntaxThree();
                                    k = 0;
                                }
                             
                                break;
                            case TokenType.OPEN_PARENTHESIS:
                                OpenBrace();
                                TokenWorker.PrintMessage("Not operator befor ", i, currentToken);
                                nextAutomateState = AutomatState.OPENED_BRACE_OPERAND;
                                CannotGenerateSyntaxThree();
                                break;
                            default:
                                if (currentTokenType == TokenType.EQUAL_SIGN)
                                {
                                    AddEqualSign();
                                    if (TokenWorker.IsCorrectEqualSignPosition(i, parTokens))
                                    {
                                        if (parTokens[i - 1].TokenType != TokenType.CORRECT_IDENTIFICATOR)
                                        {
                                            CannotGenerateSyntaxThree();
                                            TokenWorker.PrintMessage("Not identificator for ", i, currentToken);
                                        }
                                    }
                                    else if (e > 1)
                                    {
                                        CannotGenerateSyntaxThree();
                                        TokenWorker.PrintMessage("They are many equal sign in the expression ", i, currentToken);
                                    }
                                    else
                                    {
                                        CannotGenerateSyntaxThree();
                                        TokenWorker.PrintMessage("Not identificator for ", i, currentToken);
                                    }
                                }
                                nextAutomateState = AutomatState.OPENED_BRACE_OPERAND;
                                break;
                        }
                        NextToken();
                    }
                    else
                    {
                        nextAutomateState = AutomatState.CLOSED_BRACE_OPERAND;
                        AutomateStateFrom = AutomatState.CLOSED_BRACE_OPERATOR;
                    }
                    break;
                case AutomatState.CLOSED_BRACE_OPERAND:
                    switch (currentTokenType)
                    {
                        case TokenType.CLOSE_PARENTHESIS:
                            CloseBrace();
                            if (k != 0)
                            {
                                CannotGenerateSyntaxThree();
                            }
                            break;
                        case TokenType.OPEN_PARENTHESIS:
                            CannotGenerateSyntaxThree();
                            break;
                        case TokenType.EQUAL_SIGN:
                            AddEqualSign();
                            CannotGenerateSyntaxThree();
                            if (TokenWorker.IsCorrectEqualSignPosition(i, parTokens))
                            {
                                if (parTokens[i - 1].TokenType == TokenType.CORRECT_IDENTIFICATOR)
                                {
                                    TokenWorker.PrintMessage("Not expression's definition for ", i, currentToken);
                                }
                                else
                                {
                                    TokenWorker.PrintMessage("Not identificator for ", i, currentToken);
                                }
                            }
                            else if (e > 1)
                            {
                                TokenWorker.PrintMessage("They are many equal sign in the expression ", i, currentToken);
                            }
                            else
                            {
                                TokenWorker.PrintMessage("Wrong possition for ", i, currentToken);
                            }
                            break;
                    }
                    if (k != 0)
                    {
                        if (k > 0)
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
                            if (TokenWorker.IsTokenTypeOperator(currentToken))
                            {
                                CannotGenerateSyntaxThree();
                                TokenWorker.PrintMessage("Successive operator ", i);
                            }
                            break;
                        case AutomatState.CLOSED_BRACE_OPERATOR:
                            if (TokenWorker.IsTokenTypeOperator(currentToken) && currentTokenType != TokenType.EQUAL_SIGN)
                            {
                                CannotGenerateSyntaxThree();
                                TokenWorker.PrintMessage("Not operand for ", i, currentToken);
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
            switch (parToken.TokenType)
            {
                case TokenType.SOUSTRACTION_SIGN:
                case TokenType.ADDITION_SIGN:
                case TokenType.DIVISION_SIGN:
                case TokenType.MULTIPLICATION_SIGN:
                case TokenType.EQUAL_SIGN:
                    TokenWorker.PrintMessage("Not operand for ", i, parToken);
                    break;
                case TokenType.OPEN_PARENTHESIS:
                case TokenType.CLOSE_PARENTHESIS:
                    TokenWorker.PrintMessage("Not expression for ", i, parToken);
                    break;
                case TokenType.CORRECT_DECIMAL_CONSTANT:
                case TokenType.CORRECT_IDENTIFICATOR:
                case TokenType.INTEGER_CONSTANT:
                    TokenWorker.PrintMessage("Not operator for ", i, parToken);
                    break;

            }
        }
        private static void CannotGenerateSyntaxThree()
        {
            Can_Continue = false;
        }
        private static void NextToken()
        {
            i++;
        }
        private static void OpenBrace()
        {
            k++;
        }
        private static void CloseBrace()
        {
            k--;
        }
        private static void AddEqualSign()
        {
            e++;
        }
      
    }
}
