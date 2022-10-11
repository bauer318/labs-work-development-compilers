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
        public static bool Can_Continue = true;
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
                                            TokenWorker.PrintMessage("Case 0 -- Successive operator at ", i);
                                        }
                                        else
                                        {
                                            Console.WriteLine("Case 0 " + i);
                                            TokenWorker.PrintMessage("Case 0 -- Not operand for ", i, currentToken);
                                        }
                                        break;
                                    case TokenType.EQUAL_SIGN:
                                        AddEqualSign();
                                        if (e > 1)
                                        {
                                            TokenWorker.PrintMessage("Case 0 -- They are many equal sign in the expression ", i, currentToken);
                                        }
                                        else if (TokenWorker.IsCorrectEqualSignPosition(i, parTokens))
                                        {
                                            TokenWorker.PrintMessage("Case 0 -- Not identificator for ", i, currentToken);
                                        }
                                        else
                                        {
                                            TokenWorker.PrintMessage("Case 0 -- Wrong possition for ", i, currentToken);
                                        }
                                        break;
                                    case TokenType.CLOSE_PARENTHESIS:
                                        CloseBrace();
                                        if (k < 0)
                                        {
                                            TokenWorker.PrintErrorNotOpenedBrace(parTokens);
                                            k = 0;
                                        }
                                        else if (k == 0 || TokenWorker.IsOpenedBraceLeft(i, parTokens))
                                        {
                                            TokenWorker.PrintMessage("Case 0 -- Empty braces at ", i);
                                        }
                                        if (TokenWorker.IsTokenTypeOperatorLeft(i, parTokens))
                                        {
                                            TokenWorker.PrintMessage("Case *0 -- Not operand for ", i, parTokens[i - 1]);
                                        }
                                        else
                                        {
                                            TokenWorker.PrintMessage("Case 0 -- Not operator for ", i, parTokens[i + 1]);
                                        }
                                        nextAutomateState = AutomatState.CLOSED_BRACE_OPERATOR;
                                        break;
                                }
                                NextToken();
                                break;

                        }
                        /*if (currentTokenType == TokenType.OPEN_PARENTHESIS)
                        {
                            OpenBrace();
                            NextToken();
                        }*/
                        /*else if (!TokenWorker.IsTokenTypeConstIdentificator(currentTokenType))
                        {*/

                            //CannotGenerateSyntaxThree();
                            /*if (TokenWorker.IsTokenTypeOperator(currentToken))
                            {
                                if (TokenWorker.IsTokenTypeOperatorLeft(i, parTokens))
                                {
                                    TokenWorker.PrintMessage("Case 0 -- Successive operator at ", i);
                                }
                                else
                                {
                                    Console.WriteLine("Case 0 "+i);
                                    TokenWorker.PrintMessage("Case 0 -- Not operand for ", i, currentToken);
                                }
                            }*/
                            /*else if (currentTokenType == TokenType.EQUAL_SIGN)
                            {
                                AddEqualSign();
                                if (e > 1)
                                {
                                    TokenWorker.PrintMessage("Case 0 -- They are many equal sign in the expression ", i, currentToken);
                                }
                                else if (TokenWorker.IsCorrectEqualSignPosition(i, e))
                                {
                                    TokenWorker.PrintMessage("Case 0 -- Not identificator for ", i, currentToken);
                                }
                                else
                                {
                                    TokenWorker.PrintMessage("Case 0 -- Wrong possition for ", i, currentToken);
                                }
                            }*/
                            /*else if (currentTokenType == TokenType.CLOSE_PARENTHESIS)
                            {
                                CloseBrace();
                                if (k < 0)
                                {
                                    TokenWorker.PrintErrorNotOpenedBrace(parTokens);
                                }
                                else if (k == 0 || TokenWorker.IsOpenedBraceLeft(i, parTokens))
                                {
                                    TokenWorker.PrintMessage("Case 0 -- Empty braces at ", i);
                                }
                                if (TokenWorker.IsTokenTypeOperatorLeft(i, parTokens))
                                {
                                    TokenWorker.PrintMessage("Case *0 -- Not operand for ", i, parTokens[i - 1]);
                                }
                                else
                                {
                                    TokenWorker.PrintMessage("Case 0 -- Not operator for ", i, parTokens[i + 1]);
                                }
                                nextAutomateState = AutomatState.CLOSED_BRACE_OPERATOR;
                            }*/
                            //NextToken();
                        //}
                        /*else
                        {
                            NextToken();
                            nextAutomateState = AutomatState.CLOSED_BRACE_OPERATOR;
                        }*/
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
                                if (k > 0)
                                {
                                    Console.Write("Case 1 -- ");
                                    TokenWorker.PrintErrorNotClosedBrace(parTokens);
                                    //k = 0;
                                    CannotGenerateSyntaxThree();
                                }
                                else if (k < 0)
                                {
                                    Console.Write("Case 1 -- ");
                                    TokenWorker.PrintErrorNotOpenedBrace(parTokens);
                                    //k = 0;
                                    CannotGenerateSyntaxThree();
                                }
                                k = k != 0 ? 0 : k;
                                break;
                            case TokenType.OPEN_PARENTHESIS:
                                k++;
                                TokenWorker.PrintMessage("Case 1 -- Not expression after ", i, currentToken);
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
                                            TokenWorker.PrintMessage("Case 1 -- Not identificator for ", i, currentToken);
                                        }
                                    }
                                    else if (e > 1)
                                    {
                                        CannotGenerateSyntaxThree();
                                        TokenWorker.PrintMessage("Case 1 -- They are many equal sign in the expression ", i, currentToken);
                                    }
                                    else
                                    {
                                        CannotGenerateSyntaxThree();
                                        TokenWorker.PrintMessage("Case 1 -- Not identificator for ", i, currentToken);
                                    }
                                }
                                nextAutomateState = AutomatState.OPENED_BRACE_OPERAND;
                                break;
                        }
                        NextToken();
                        /*if (currentTokenType == TokenType.CLOSE_PARENTHESIS)
                        {
                            CloseBrace();
                            if (k > 0)
                            {
                                Console.Write("Case 1 -- ");
                                TokenWorker.PrintErrorNotClosedBrace(parTokens);
                                CannotGenerateSyntaxThree();
                            }
                            else if (k < 0)
                            {
                                Console.Write("Case 1 -- ");
                                TokenWorker.PrintErrorNotOpenedBrace(parTokens);
                                CannotGenerateSyntaxThree();
                            }
                            k = k != 0 ? 0 : k; 
                        }*/
                            /*else if (currentTokenType == TokenType.OPEN_PARENTHESIS)
                            {
                                k++;
                                TokenWorker.PrintMessage("Case 1 -- Not expression after ", i, currentToken);
                                nextAutomateState = AutomatState.OPENED_BRACE_OPERAND;
                                CannotGenerateSyntaxThree();
                            }*/
                        /*else
                        {
                            if (currentTokenType == TokenType.EQUAL_SIGN)
                            {
                                AddEqualSign();
                                if (TokenWorker.IsCorrectEqualSignPosition(i, e))
                                {
                                    if (parTokens[i - 1].TokenType != TokenType.CORRECT_IDENTIFICATOR)
                                    {
                                        CannotGenerateSyntaxThree();
                                        TokenWorker.PrintMessage("Case 1 -- Not identificator for ", i, currentToken);
                                    }
                                }
                                else if (e > 1)
                                {
                                    CannotGenerateSyntaxThree();
                                    TokenWorker.PrintMessage("Case 1 -- They are many equal sign in the expression ", i, currentToken);
                                }
                                else
                                {
                                    CannotGenerateSyntaxThree();
                                    TokenWorker.PrintMessage("Case 1 -- Not identificator for ", i, currentToken);
                                }
                            }*/
                            //nextAutomateState = AutomatState.OPENED_BRACE_OPERAND;
                        //}
                        //NextToken();
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
                                TokenWorker.PrintMessage("Case 2 -- Not opened brace for ", i, currentToken);
                            }
                            break;
                        case TokenType.OPEN_PARENTHESIS:
                            CannotGenerateSyntaxThree();
                            TokenWorker.PrintMessage("Case 2 -- Not closed brace for ", i, currentToken);
                            break;
                        case TokenType.EQUAL_SIGN:
                            AddEqualSign();
                            CannotGenerateSyntaxThree();
                            if (TokenWorker.IsCorrectEqualSignPosition(i, parTokens))
                            {
                                if (parTokens[i - 1].TokenType == TokenType.CORRECT_IDENTIFICATOR)
                                {
                                    TokenWorker.PrintMessage("Case 2 -- Not expression's definition for ", i, currentToken);
                                }
                                else
                                {
                                    TokenWorker.PrintMessage("Case 2 -- Not identificator for ", i, currentToken);
                                }
                            }
                            else if (e > 1)
                            {
                                TokenWorker.PrintMessage("Case 2 -- They are many equal sign in the expression ", i, currentToken);
                            }
                            else
                            {
                                TokenWorker.PrintMessage("Case 2 -- Wrong possition for ", i, currentToken);
                            }
                            break;
                    }
                    if (k != 0)
                    {
                        if (k > 0)
                        {
                            /*var lastOpenedBraceIndex = TokenWorker.GetPositionLastOpenedBrace(parTokens);
                            TokenWorker.PrintMessage("Case 2 -- Not closed brace for ", lastOpenedBraceIndex, parTokens[lastOpenedBraceIndex]);*/
                            Console.Write("Case 2 k>0");
                            TokenWorker.PrintErrorNotClosedBrace(parTokens);
                        }
                        else
                        {
                            Console.Write("Case 2 k<0");
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
                                TokenWorker.PrintMessage("Case 2 -- Not operand for ", i, currentToken);
                            }
                            break;
                        case AutomatState.CLOSED_BRACE_OPERATOR:
                            if (TokenWorker.IsTokenTypeOperator(currentToken) && currentTokenType != TokenType.EQUAL_SIGN)
                            {
                                CannotGenerateSyntaxThree();
                                TokenWorker.PrintMessage("Case 2 -- Not operand for ", i, currentToken);
                            }
                            break;


                    }
                    /*if (currentTokenType == TokenType.CLOSE_PARENTHESIS)
                    {
                        CloseBrace();
                        if (k != 0)
                        {
                            CannotGenerateSyntaxThree();
                            TokenWorker.PrintMessage("Case 2 -- Not opened brace for ", i, currentToken);
                        }
                    }*/
                    /*else if (currentTokenType == TokenType.OPEN_PARENTHESIS)
                    {
                        CannotGenerateSyntaxThree();
                        TokenWorker.PrintMessage("Case 2 -- Not closed brace for ", i, currentToken);
                    }*/
                    /*else if (currentTokenType == TokenType.EQUAL_SIGN)
                    {
                        AddEqualSign();
                        CannotGenerateSyntaxThree();
                        if (TokenWorker.IsCorrectEqualSignPosition(i, e))
                        {
                            if (parTokens[i - 1].TokenType == TokenType.CORRECT_IDENTIFICATOR)
                            {
                                TokenWorker.PrintMessage("Case 2 -- Not expression's definition for ", i, currentToken);
                            }
                            else
                            {
                                TokenWorker.PrintMessage("Case 2 -- Not identificator for ", i, currentToken);
                            }
                        }
                        else if (e > 1)
                        {
                            TokenWorker.PrintMessage("Case 2 -- They are many equal sign in the expression ", i, currentToken);
                        }
                        else
                        {
                            TokenWorker.PrintMessage("Case 2 -- Wrong possition for ", i, currentToken);
                        }
                    }*/
                    /*else if(k!=0)
                    {

                        var lastOpenedBraceIndex = TokenWorker.GetPositionLastOpenedBrace(parTokens);
                        TokenWorker.PrintMessage("Case 2 -- Not closed brace for ", lastOpenedBraceIndex, parTokens[lastOpenedBraceIndex]);
                        CannotGenerateSyntaxThree();
                    }*/
                    /*if (AutomateStateFrom == AutomatState.OPENED_BRACE_OPERAND)
                    {
                        if (TokenWorker.IsTokenTypeOperator(currentToken))
                        {
                            CannotGenerateSyntaxThree();
                            TokenWorker.PrintMessage("Case 2 -- Not operand for ", i, currentToken);
                        }
                    }*/
                    /*else if(AutomateStateFrom == AutomatState.CLOSED_BRACE_OPERATOR)
                    {
                        if(TokenWorker.IsTokenTypeOperator(currentToken) && currentTokenType != TokenType.EQUAL_SIGN)
                        {
                            CannotGenerateSyntaxThree();
                            TokenWorker.PrintMessage("Case 2 -- Not operand for ", i, currentToken);
                        }
                    }*/
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
