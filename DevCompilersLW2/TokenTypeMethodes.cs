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
                    return "<" + parToken.name + "> - операция сложения";
                case TokenType.CLOSE_PARENTHESIS:
                    return "<" + parToken.name + "> - закрывающая скобка";
                case TokenType.CORRECT_DECIMAL_CONSTANT:
                    return "<" + parToken.name + "> - константа вещественного типа";
                case TokenType.CORRECT_IDENTIFICATOR:
                    return "<id, "+parToken.AttributeValue+"> - идентификатор с именем "+parToken.name;
                case TokenType.DIVISION_SIGN:
                    return "<" + parToken.name + "> - операция деления";
                case TokenType.INTEGER_CONSTANT:
                    return "<" + parToken.name + "> - константа целого типа";
                case TokenType.MULTIPLICATION_SIGN:
                    return "<" + parToken.name + "> - операция умножения";
                case TokenType.OPEN_PARENTHESIS:
                    return "<" + parToken.name + "> - открывающая скобка";
                case TokenType.SOUSTRACTION_SIGN:
                    return "<" + parToken.name + "> - операция вычитания";
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
