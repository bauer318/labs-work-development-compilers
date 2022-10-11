using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DevCompilersLW2
{
    
    public enum TokenType
    {
        INVALID,
        INTEGER_CONSTANT,
        OPEN_PARENTHESIS,
        CLOSE_PARENTHESIS,
        ADDITION_SIGN,
        SOUSTRACTION_SIGN,
        MULTIPLICATION_SIGN,
        DIVISION_SIGN,
        INCORRECT_IDENTIFICATOR,
        CORRECT_IDENTIFICATOR,
        INCORRECT_DECIMAL_CONSTANT,
        CORRECT_DECIMAL_CONSTANT,
        EQUAL_SIGN
    }
}
