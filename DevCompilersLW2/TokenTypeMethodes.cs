using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW2
{
    public static class TokenTypeMethodes
    {
        public static string GetTokenTypeDescription(this TokenType parTokenType, Token parToken)
        {
            switch (parTokenType)
            {
                case TokenType.ADDITION_SIGN:
                    return "<" + parToken.Lexeme + "> - операция сложения";
                case TokenType.CLOSE_PARENTHESIS:
                    return "<" + parToken.Lexeme + "> - закрывающая скобка";
                case TokenType.CORRECT_DECIMAL_CONSTANT:
                    return "<" + parToken.Lexeme + "> - константа вещественного типа";
                case TokenType.CORRECT_DEFAULT_IDENTIFICATOR:
                    return "<id, "+parToken.AttributeValue+"> - идентификатор с именем "+parToken.Lexeme;
                case TokenType.DIVISION_SIGN:
                    return "<" + parToken.Lexeme + "> - операция деления";
                case TokenType.INTEGER_CONSTANT:
                    return "<" + parToken.Lexeme + "> - константа целого типа";
                case TokenType.MULTIPLICATION_SIGN:
                    return "<" + parToken.Lexeme + "> - операция умножения";
                case TokenType.OPEN_PARENTHESIS:
                    return "<" + parToken.Lexeme + "> - открывающая скобка";
                case TokenType.SOUSTRACTION_SIGN:
                    return "<" + parToken.Lexeme + "> - операция вычитания";
                default:
                    return "INVALID TOKEN TYPE";
            }
        }
        public static string GetIncorrectTokenTypeDescrition(this TokenType parTokenType, string parCurrentLexeme, string inputExpresion)
        {
            var currentPosition = inputExpresion.IndexOf(parCurrentLexeme);
            if (currentPosition == -1)
            {
                //Console.WriteLine(parCurrentLexeme.Split(new char[] { '0','','','','','','','','','','','' })[0]);
                currentPosition = 0;// inputExpresion.IndexOf(parCurrentLexeme.Split(new char[] { ' ' })[0]);
            }
            switch (parTokenType)
            {
            case TokenType.INCORRECT_DECIMAL_CONSTANT:
                    return "Лексическая ошибка! Неправильно задана константа <<"+parCurrentLexeme+">> на позиции "+currentPosition;
                case TokenType.INCORRECT_DEFAULT_IDENTIFICATOR:
                    return "Лексическая ошибка! Идентификатор <<"+parCurrentLexeme+">> не может начинаться с цифры на позиции " + currentPosition;
                default:
                    if (parCurrentLexeme.Length == 1)
                        return "Лексическая ошибка! Недопустимый символ " + "\"" + parCurrentLexeme + "\"" + " на позиции " + currentPosition;
                    else
                        return "Неправильное имя переменной или неправильый тип переменной <<"+parCurrentLexeme+">> на позиции "+currentPosition;
            }
        }
        public static string GetTokenNodeDescription(this TokenType parTokenType,Token parToken)
        {
            switch (parTokenType)
            {
                case TokenType.ADDITION_SIGN:
                case TokenType.SOUSTRACTION_SIGN:
                case TokenType.MULTIPLICATION_SIGN:
                case TokenType.DIVISION_SIGN:
                case TokenType.CORRECT_DECIMAL_CONSTANT:
                case TokenType.INTEGER_CONSTANT:
                case TokenType.OPEN_PARENTHESIS:
                case TokenType.CLOSE_PARENTHESIS:
                    return "<"+parToken.Lexeme+">";
                case TokenType.CORRECT_DEFAULT_IDENTIFICATOR:
                    return "<id, " + parToken.AttributeValue + ">";
            }
            return "";
        }
    }
}
