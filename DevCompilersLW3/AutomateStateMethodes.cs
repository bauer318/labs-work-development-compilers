using DevCompilersLW2;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW3
{
    public static class AutomateStateMethodes
    {
        public static int k = 0;
        private static int i = 0;
        public static int e;
        public static bool Can_Continue = true;
        private static AutomatState AutomateStateFrom = AutomatState.OPENED_BRACE_OPERAND;
        public static AutomatState Swip(this AutomatState parAutomatState,List<Token> parTokens)
        {
            Token currentToken = parTokens[i];
            TokenType currentTokenType = currentToken.TokenType;
            var tokensNumber = parTokens.Count;
            AutomatState nextAutomateState = AutomatState.OPENED_BRACE_OPERAND;
            switch (parAutomatState)
            {
                case AutomatState.OPENED_BRACE_OPERAND:
                    if (i < tokensNumber - 1)
                    {
                        if (currentTokenType == TokenType.OPEN_PARENTHESIS)
                        {
                            OpenBrace();
                            NextToken();
                        }
                        else if (!TokenWorker.IsTokenTypeConstIdentificator(currentTokenType))
                        {
                            CannotGenerateSyntaxThree();
                            if (TokenWorker.IsTokenTypeOperator(currentToken))
                            {
                                if (TokenWorker.IsTokenTypeOperatorLeft(i, parTokens))
                                {
                                    TokenWorker.PrintMessage("Successive operator at ", i);
                                }
                                else
                                {
                                    TokenWorker.PrintMessage("Not operand for ", i, currentToken);
                                }
                                       
                            }
                            else if (currentTokenType == TokenType.EQUAL_SIGN)
                            {
                                AddEqualSign();
                                if (e > 1)
                                {
                                    TokenWorker.PrintMessage("They are many equal sign in the expression ", i, currentToken);
                                }
                                else if (TokenWorker.IsCorrectEqualSignPosition(i, e))
                                {
                                    TokenWorker.PrintMessage("Not identificator for ", i, currentToken);
                                }
                                else
                                {
                                    TokenWorker.PrintMessage("Wrong possition for ", i, currentToken);
                                }
                            }
                            else if (currentTokenType == TokenType.CLOSE_PARENTHESIS)
                            {
                                CloseBrace();
                                if (k < 0)
                                {
                                    TokenWorker.PrintErrorNotOpenedBrace(parTokens);
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
                            }
                            NextToken();
                        }
                        else
                        {
                            nextAutomateState = AutomatState.CLOSED_BRACE_OPERATOR;
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
                        if (currentTokenType == TokenType.CLOSE_PARENTHESIS)
                        {
                            CloseBrace();
                            if (k > 0)
                            {
                                TokenWorker.PrintErrorNotClosedBrace(parTokens);
                            }
                            else if (k < 0)
                            {
                                TokenWorker.PrintErrorNotOpenedBrace(parTokens);
                            }
                            k = k != 0 ? 0 : k; 
                        }
                        else if (currentTokenType == TokenType.OPEN_PARENTHESIS)
                        {
                            k++;
                            TokenWorker.PrintMessage("Not expression after ", i, currentToken);
                        }
                        else
                        {
                            if (currentTokenType == TokenType.EQUAL_SIGN)
                            {
                                AddEqualSign();
                                if (TokenWorker.IsCorrectEqualSignPosition(i, e))
                                {
                                    if (parTokens[i - 1].TokenType != TokenType.CORRECT_IDENTIFICATOR)
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
                                    TokenWorker.PrintMessage("Not identificator for ", i, currentToken);
                                }
                            }
                            nextAutomateState = AutomatState.OPENED_BRACE_OPERAND;
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
                    if (currentTokenType == TokenType.CLOSE_PARENTHESIS)
                    {
                        CloseBrace();
                        if (k != 0)
                        {
                            TokenWorker.PrintMessage("Not opened brace for ", i, currentToken);
                        }
                    }
                    else if (currentTokenType == TokenType.OPEN_PARENTHESIS)
                    {
                        TokenWorker.PrintMessage("Not closed brace for ", i, currentToken);
                    }
                    else if (currentTokenType == TokenType.EQUAL_SIGN)
                    {
                        AddEqualSign();
                        CannotGenerateSyntaxThree();
                        if (TokenWorker.IsCorrectEqualSignPosition(i, e))
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
                    }
                    else if(k!=0)
                    {
                        var lastOpenedBraceIndex = TokenWorker.GetPositionLastOpenedBrace(parTokens);
                        TokenWorker.PrintMessage("Not closed brace for ", lastOpenedBraceIndex, parTokens[lastOpenedBraceIndex]);
                    }
                    if (AutomateStateFrom == AutomatState.OPENED_BRACE_OPERAND)
                    {
                        if (TokenWorker.IsTokenTypeOperator(currentToken))
                        {
                            TokenWorker.PrintMessage("Successive operator at ", i, currentToken);
                        }
                    }
                    else if(AutomateStateFrom == AutomatState.CLOSED_BRACE_OPERATOR)
                    {
                        if(!TokenWorker.IsTokenTypeOperator(currentToken) && currentTokenType != TokenType.EQUAL_SIGN)
                        {
                            TokenWorker.PrintMessage("Not operand for ", i, currentToken);
                        }
                    }
                    nextAutomateState = AutomatState.END_EXPRESSION;
                    break;

            }
            return nextAutomateState;
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
