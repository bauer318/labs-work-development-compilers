﻿using System;
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
        INCORRECT_DEFAULT_IDENTIFICATOR,
        CORRECT_DEFAULT_IDENTIFICATOR,
        INCORRECT_DECIMAL_CONSTANT,
        CORRECT_DECIMAL_CONSTANT,
        CORRECT_INTEGER_IDENTIFICATOR,
        CORRECT_DECIMAL_IDENTIFICATOR,
        INCORRECT_TYPE_IDENTIFICATOR,
        OPEN_SQUARE_BRAKET,
        CLOSE_SQUARE_BRAKET
    }
}
