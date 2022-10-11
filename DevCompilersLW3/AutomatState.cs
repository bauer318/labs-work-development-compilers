using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW3
{
    public enum AutomatState
    {
        OPENED_BRACE_OPERAND,
        CLOSED_BRACE_OPERATOR,
        CLOSED_BRACE_OPERAND,
        END_EXPRESSION
    }
}
