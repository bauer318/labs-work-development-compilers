using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW2
{
    public class SymbolTableWorker
    {
        public static SymbolTable SymbolTable { get; set; }

        public static string GetVariableNameById(int parId)
        {
            for (var i = 0; i < SymbolTable.AttributeVariables.Count; i++)
            {
                if (SymbolTable.AttributeVariables[i].Id == parId)
                {
                    return SymbolTable.AttributeVariables[i].Name;
                }
            }
            return string.Empty;
        }
        public static TokenType GetVariableTypeById(int parId)
        {
            for (var i = 0; i < SymbolTable.AttributeVariables.Count; i++)
            {
                if (SymbolTable.AttributeVariables[i].Id == parId)
                {
                    return SymbolTable.AttributeVariables[i].TokenType;
                }
            }
            return TokenType.INVALID;
        }
        public static int GetVariableIdByName(string parName)
        {
            for (var i = 0; i < SymbolTable.AttributeVariables.Count; i++)
            {
                if (SymbolTable.AttributeVariables[i].Name.Equals(parName))
                {
                    return SymbolTable.AttributeVariables[i].Id;
                }
            }
            return 0;
        }


    }
}
