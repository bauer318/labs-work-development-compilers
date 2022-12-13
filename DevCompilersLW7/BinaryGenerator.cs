using DevCompilersLW2;
using DevCompilersLW5;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW7
{
    public class BinaryGenerator
    {
        private List<PortableCode> _portableCodes;
        private SymbolTable _symbolTable;
        public List<PortableCode> PortableCodes
        {
            get
            {
                return _portableCodes;
            }
        }

        public SymbolTable SymbolTable
        {
            get
            {
                return _symbolTable;
            }
        }

        public BinaryGenerator(List<PortableCode> parPortableCodes, SymbolTable parSymbolTable)
        {
            _portableCodes = parPortableCodes;
            _symbolTable = parSymbolTable;
        }

    }
}
