using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DevCompilersLW2
{
    public class OutputTextFileWriter
    {
        private string _tokenTextFileName;
        private string _symbolTableTextFileName;
        
        public OutputTextFileWriter(string parTokenTextFileName, string parSymbolTableTextFileName)
        {
            _tokenTextFileName = parTokenTextFileName;
            _symbolTableTextFileName = parSymbolTableTextFileName;
        }
        public void WriteTokenTextFile(LexicalErrorAnalyzer parTokenizer)
        {
            using (StreamWriter writer = new StreamWriter(_tokenTextFileName))
            {
                foreach (Token token in parTokenizer.Tokens)
                {
                    writer.WriteLine(token.TokenType.GetTokenTypeDescription(token));
                }
            }
             
        }
        public void WriteSymbolTableTextFile(LexicalErrorAnalyzer parTokenizer)
        {
            using (StreamWriter writer = new StreamWriter(_symbolTableTextFileName))
            {
                foreach (AttributeVariable attributeVariable in parTokenizer.SymbolTable.AttributeVariables)
                {
                    writer.WriteLine(attributeVariable.Id + " - " + attributeVariable.Name);
                }
            }

        }
    }
}
