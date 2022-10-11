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
                case TokenType.CORRECT_IDENTIFICATOR:
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
                case TokenType.EQUAL_SIGN:
                    return "<" + parToken.Lexeme + "> - равенство ";
                default:
                    return "INVALID TOKEN TYPE";
            }
        }
        public static string GetIncorrectTokenTypeDescrition(this TokenType parTokenType, string parCurrentLexeme, string inputExpresion)
        {
            var currentPosition = inputExpresion.IndexOf(parCurrentLexeme);
            switch (parTokenType)
            {
            case TokenType.INCORRECT_DECIMAL_CONSTANT:
                    return "Лексическая ошибка! Неправильно задана константа <<"+parCurrentLexeme+">> на позиции "+currentPosition;
                case TokenType.INCORRECT_IDENTIFICATOR:
                    return "Лексическая ошибка! Идентификатор <<"+parCurrentLexeme+">> не может начинаться с цифры на позиции " + currentPosition;
                default:
                    return "Лексическая ошибка! Недопустимый символ " + "\"" + parCurrentLexeme + "\"" + " на позиции "+currentPosition;
            }
        }
    }
}
