using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW2
{
    public class SymbolTableCreator
    {
        private LexicalErrorAnalyzer _lexicalErrorAnalyzer;
        private string _symbolTableOutFileName;
        public LexicalErrorAnalyzer LexicalErrorAnalyzer
        {
            get
            {
                return _lexicalErrorAnalyzer;
            }
            set
            {
                _lexicalErrorAnalyzer = value;
            }
        }
        public string SymbolTableOutFileName
        {
            get
            {

            }
        }
    }
}
