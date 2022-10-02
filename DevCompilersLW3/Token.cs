using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW3
{
    public enum Token
    {
        NUMBER = 0,


        ADD, // +
        MINUS, // -
        MULTIPLY, // *
        DIVISION, // /

        RBRACE, // (
        LBRACE, // )

        EOF // END OF FILE
    }
}
