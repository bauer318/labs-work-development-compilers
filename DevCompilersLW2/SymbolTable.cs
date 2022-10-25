using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW2
{
    public class SymbolTable
    {

        private List<AttributeVariable> _attributeVariables;
        public List<AttributeVariable> AttributeVariables
        {
            get
            {
                return _attributeVariables;
            }
            set
            {
                _attributeVariables = value;
            }
        }

        public SymbolTable(List<AttributeVariable> parAttributeVariables)
        {
            _attributeVariables = parAttributeVariables;
        }
        
    }
}
