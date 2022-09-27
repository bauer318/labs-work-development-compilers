using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DevCompilersLW2
{
    public class TokenSequenceCreator
    {
        private LexicalErrorAnalyzer _lexicalErrorAnalyzer;
        private string _tokenFileName;
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
        public string TokenFileName
        {
            get
            {
                return _tokenFileName;
            }
            set
            {
                _tokenFileName = value;
            }
        }
        public TokenSequenceCreator(LexicalErrorAnalyzer parLexicalErrorAnalyzer, string parTokenFileName)
        {
            _lexicalErrorAnalyzer = parLexicalErrorAnalyzer;
            _tokenFileName = parTokenFileName;
        }
        public void CreateTokenSequenceFile()
        {
            List<string> identificatorList = _lexicalErrorAnalyzer.CreateListExpresionAlphabet()[2];
            List<string> decimalConstantList = _lexicalErrorAnalyzer.CreateListExpresionAlphabet()[3];
            List<string> integerList = _lexicalErrorAnalyzer.CreateListExpresionAlphabet()[4];
            Dictionary<string, string> expresionDictionary = _lexicalErrorAnalyzer.GetLexicalDictionary();
            string[] expresionArray = _lexicalErrorAnalyzer.GetExpresionWithWhitespace().Split(" ");
            WriteTokenFile(identificatorList, decimalConstantList, integerList, expresionDictionary, expresionArray);

        }

        private void WriteTokenFile(List<string> parIdentificatorList,
            List<string> parDecimalConstantList,
            List<string> parIntegerList,
            Dictionary<string, string> parExpresionDictionary,
            string[] parExpresionArray)
        {

            using (StreamWriter writer = new StreamWriter(_tokenFileName)) 
            {
                int id = 1;
                for(var i=0; i<parExpresionArray.Length; i++)
                {
                    string currentExpresionElement = parExpresionArray[i];
                    if (parIdentificatorList.Contains(currentExpresionElement))
                    {
                        writer.WriteLine("<id,"+id+"> - идентификатор с инменем "+currentExpresionElement);
                        id++;
                    }
                    if (parDecimalConstantList.Contains(currentExpresionElement))
                    {
                        writer.WriteLine("<" + currentExpresionElement + "> - константа вещественного типа");
                    }
                    if (parIntegerList.Contains(currentExpresionElement))
                    {
                        writer.WriteLine("<" + currentExpresionElement + "> - константа целого типа");
                    }
                    if (parExpresionDictionary.ContainsKey(currentExpresionElement) && !parIntegerList.Contains(currentExpresionElement))
                    {
                        writer.WriteLine("<"+currentExpresionElement+"> - "+parExpresionDictionary.GetValueOrDefault(currentExpresionElement));
                    }
                }
            }
        }
        
        
    }
}
