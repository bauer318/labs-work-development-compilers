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

                    switch (token.TokenType)
                    {
                        case TokenType.CorrectIdentificator:
                            writer.WriteLine("<" + token.GetTokenName() + "," + token.AttributeValue + ">");
                            break;
                        default:
                            writer.WriteLine("<" + token.GetTokenName() + ">");
                            break;
                    }
                }
            }
             
        }
        public void WriteSymbolTableTextFile(LexicalErrorAnalyzer parTokenizer)
        {
            using (StreamWriter writer = new StreamWriter(_symbolTableTextFileName))
            {
                foreach (SymbolTable symbolTable in parTokenizer.SymbolTables)
                {
                    writer.WriteLine(symbolTable.Token.AttributeValue + " - " + symbolTable.Token.Lexeme);
                }
            }

        }
    }
}
