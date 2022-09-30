using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW2
{
    public class SymbolTable
    {

        public readonly Token Token;
        
        public SymbolTable(Token parToken)
        {
            Token = parToken;
        }
    }
}
