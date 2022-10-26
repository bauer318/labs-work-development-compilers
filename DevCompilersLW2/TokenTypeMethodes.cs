using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW2
{
    public static class TokenTypeMethodes
    {
        private static string GetTokenIdentificatorTypeDescription(TokenType parTokenType)
        {
            switch (parTokenType)
            {
                case TokenType.CORRECT_INTEGER_IDENTIFICATOR:
                    return " целого типа";
                default:
                    return " вещественного типа";
            }
        }
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
                case TokenType.CORRECT_DECIMAL_IDENTIFICATOR:
                case TokenType.CORRECT_INTEGER_IDENTIFICATOR:
                    return "<id, " + parToken.AttributeValue + "> - идентификатор с именем " + parToken.Lexeme + GetTokenIdentificatorTypeDescription(parTokenType);
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
            switch (parTokenType)
            {
            case TokenType.INCORRECT_DECIMAL_CONSTANT:
                    return "Лексическая ошибка! Неправильно задана константа <<"+parCurrentLexeme+">> на позиции "+currentPosition;
                case TokenType.INCORRECT_DEFAULT_IDENTIFICATOR:
                    return "Лексическая ошибка! Идентификатор <<"+parCurrentLexeme+">> не может начинаться с цифры на позиции " + currentPosition;
                case TokenType.OPEN_SQUARE_BRAKET:
                case TokenType.CLOSE_SQUARE_BRAKET:
                    return "Семантическая ошибка! Пустые квадратный(е) скобки для типов " + "\"" + parCurrentLexeme + "\"" + " на позиции " + currentPosition;
                case TokenType.INCORRECT_TYPE_IDENTIFICATOR:
                    return "Семантическая ошибка! Непральный тип для идентификатора <<" + parCurrentLexeme + ">> на позиции " + currentPosition;
                case TokenType.IDENTIFICATOR_WITHOUT_TYPE:
                    return "Семантическая ошибка! Не указан тип для идентификатора <<" + parCurrentLexeme + ">> на позиции " + currentPosition;
                default:
                    if (parCurrentLexeme.Length == 1)
                        return "Лексическая ошибка! Недопустимый символ " + "\"" + parCurrentLexeme + "\"" + " на позиции " + currentPosition;
                    else
                        return "Семантическая ошибка! Неправильный идентификатор или тпи переменной <<" + parCurrentLexeme+">> на позиции "+currentPosition;
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
                case TokenType.CORRECT_DECIMAL_IDENTIFICATOR:
                case TokenType.CORRECT_INTEGER_IDENTIFICATOR:
                    return "<id, " + parToken.AttributeValue + ">";
                case TokenType.INT_2_FLOAT:
                    return "Int2Float";
            }
            return "";
        }
        public static TokenType GetIdentificatorTokenType(this TokenType parTokenType)
        {
            switch (parTokenType)
            {
                case TokenType.CORRECT_INTEGER_IDENTIFICATOR:
                case TokenType.CORRECT_DEFAULT_IDENTIFICATOR:
                    return TokenType.CORRECT_INTEGER_IDENTIFICATOR;
                default:
                    return TokenType.CORRECT_DECIMAL_IDENTIFICATOR;

            }
        }
        public static string GetIdentificatorTypeInBracketDescription(this TokenType parTOkenType)
        {
            switch (parTOkenType)
            {
                case TokenType.CORRECT_DECIMAL_IDENTIFICATOR:
                    return "[вещественный]";
                default:
                    return "[целый]";
            }
        }
    }
}
