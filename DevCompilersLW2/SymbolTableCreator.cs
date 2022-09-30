using System;
using System.Collections.Generic;
using System.IO;
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
                return _symbolTableOutFileName;
            }
            set
            {
                _symbolTableOutFileName = value;
            }
        }
        public SymbolTableCreator(LexicalErrorAnalyzer parLexicalErrorAnalyzer, string parSymbolTableOutFileName)
        {
            _lexicalErrorAnalyzer = parLexicalErrorAnalyzer;
            _symbolTableOutFileName = parSymbolTableOutFileName;
        }
        public void CreateSymbolTableFile()
        {
            List<string> identificatorList = _lexicalErrorAnalyzer.CreateListExpresionAlphabet()[2];
            using(StreamWriter writer = new StreamWriter(_symbolTableOutFileName))
            {
                for(var i=0; i<identificatorList.Count; i++)
                {
                    writer.WriteLine(_lexicalErrorAnalyzer.dic.GetValueOrDefault(identificatorList[i])+ " - " + identificatorList[i]);
                }
            }
        }
    }
}
