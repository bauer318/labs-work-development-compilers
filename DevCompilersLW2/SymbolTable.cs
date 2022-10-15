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
        public string GetVariableNameById(int parId)
        {
            for (var i = 0; i < _attributeVariables.Count; i++)
            {
                if (_attributeVariables[i].Id == parId)
                {
                    return _attributeVariables[i].Name;
                }
            }
            return string.Empty;
        }
    }
}
