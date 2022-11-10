using DevCompilersLW2;
using DevCompilersLW3;
using DevCompilersLW4;
using DevCompilersLW5;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CommonWorks
{
    public class OutputTextFileWriter
    {
        private string _tokenTextFileName;
        private string _symbolTableTextFileName;
        private string _syntaxTreeTextFileName;
        private string _syntaxTreeModTextFileName;
        private string _portableCodeTextFileName;
        private string _postfixTextFileName;

        public OutputTextFileWriter(string parTokenTextFileName, string parSymbolTableTextFileName, string parSyntaxTreeTextFileName)
        {
            _tokenTextFileName = string.IsNullOrEmpty(parTokenTextFileName)? "Tokens.txt":parTokenTextFileName;
            _symbolTableTextFileName = string.IsNullOrEmpty(parSymbolTableTextFileName)? "symbols.txt" :parSymbolTableTextFileName;
        }
        public OutputTextFileWriter(string parSyntaxTreeTextFileName)
        {
            _syntaxTreeTextFileName = string.IsNullOrEmpty(parSyntaxTreeTextFileName)? "syntax_tree.txt" : parSyntaxTreeTextFileName;
        }
        public OutputTextFileWriter(string parTokenTextFileName, 
            string parSymbolTableTextFileName, 
            string parSyntaxTreeTextFileName,
            string parSyntaxTreeModTextFileName) : this(parTokenTextFileName, parSymbolTableTextFileName,parSyntaxTreeTextFileName)
        {
            _syntaxTreeModTextFileName = string.IsNullOrEmpty(parSyntaxTreeModTextFileName) ? "syntax_tree_mod.txt" : parSyntaxTreeModTextFileName;   
        }
        public OutputTextFileWriter(string parTokenTextFileName,
            string parSymbolTableTextFileName,
            string parSyntaxTreeTextFileName,
            string parSyntaxTreeModTextFileName,
            string parPortableCodeTextFileName,
            string parPostfixTextFileName) : this(parTokenTextFileName, parSymbolTableTextFileName, parSyntaxTreeTextFileName, parSyntaxTreeModTextFileName)
        {
            _portableCodeTextFileName = string.IsNullOrEmpty(parPortableCodeTextFileName) ? "portable_code.txt" : parPortableCodeTextFileName;
            _postfixTextFileName = string.IsNullOrEmpty(parPostfixTextFileName) ? "postfix.txt" : parPostfixTextFileName;
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
                    writer.WriteLine(attributeVariable.Id + " - " + 
                        attributeVariable.Name+attributeVariable.TokenType.GetIdentificatorTypeInBracketDescription());
                }
            }
        }
        public void WriteAbstractSyntaxTreeTextFile(Parser parParser)
        {
            using (StreamWriter writer = new StreamWriter(_syntaxTreeTextFileName))
            {
                foreach(string nodeText in parParser.GetSyntaxTreeNodeTextArray())
                writer.WriteLine(nodeText);
            }
        }
        public void WriteSyntaxTreeMdifiedTextFile(SyntacticalTreeModificator parSyntacticalTreeModificator)
        {
            using(StreamWriter writer = new StreamWriter(_syntaxTreeModTextFileName))
            {
                foreach(string nodeText in parSyntacticalTreeModificator.GetSemanticTreeTextList())
                {
                    writer.WriteLine(nodeText);
                }
            }
        }
        public void WritePortableCodeTextFile(IntermediateCodeGenerator intermediateCodeGenerator)
        {
            using(StreamWriter writer = new StreamWriter(_portableCodeTextFileName))
            {
                foreach(string treeAddressCode in intermediateCodeGenerator.GetPortableCodeText())
                {
                    writer.WriteLine(treeAddressCode);
                }
            }
        }
        public void WritePostfixExpressionTextFile(IntermediateCodeGenerator intermediateCodeGenerator)
        {
            using(StreamWriter writer = new StreamWriter(_postfixTextFileName))
            {
                foreach(string tokenInPostfixExpression in intermediateCodeGenerator.GetPostFixExpressionText())
                {
                    writer.Write(tokenInPostfixExpression);
                }
            }
        }
        public void WritePortableCodeSymbolTable(IntermediateCodeGenerator intermediateCodeGenerator)
        {
            using(StreamWriter writer = new StreamWriter(_symbolTableTextFileName))
            {
                foreach(string symboleTable in intermediateCodeGenerator.GetSymboleTableText())
                {
                    writer.WriteLine(symboleTable);
                }
            }
        }
    }
}
