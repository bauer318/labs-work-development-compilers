using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW2
{
    public enum TokenType
    {
        Invalid,
        IntegerConstant,
        OpenParenthesis,
        CloseParenthesis,
        AdditionSign,
        SoustractionSign,
        MultiplicationSign,
        DivisionSign,
        IncorrectIdentificator,
        CorrectIdentificator,
        IncorrectDecimalConstant,
        CorrectDecimalConstant
    }
}
