using DevCompilersLW2;
using DevCompilersLW3;
using DevCompilersLW4;
using DevCompilersLW5;
using DevCompilersLW7;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace CommonWorks
{
    public class OutputFileWriter
    {
        private string _tokenTextFileName;
        private string _symbolTableTextFileName;
        private string _syntaxTreeTextFileName;
        private string _syntaxTreeModTextFileName;
        private string _portableCodeTextFileName;
        private string _postfixTextFileName;
        private string _binaryFileName = @"C:\Users\Bauer\source\repos\DevCompilersLW1\Interpreter\bin\Debug\netcoreapp3.1\post_code.bin";

        public OutputFileWriter(string parTokenTextFileName, string parSymbolTableTextFileName, string parSyntaxTreeTextFileName)
        {
            _tokenTextFileName = string.IsNullOrEmpty(parTokenTextFileName)? "Tokens.txt":parTokenTextFileName;
            _symbolTableTextFileName = string.IsNullOrEmpty(parSymbolTableTextFileName)? "symbols.txt" :parSymbolTableTextFileName;
        }
        public OutputFileWriter(string parSyntaxTreeTextFileName)
        {
            _syntaxTreeTextFileName = string.IsNullOrEmpty(parSyntaxTreeTextFileName)? "syntax_tree.txt" : parSyntaxTreeTextFileName;
        }
        public OutputFileWriter(string parTokenTextFileName, 
            string parSymbolTableTextFileName, 
            string parSyntaxTreeTextFileName,
            string parSyntaxTreeModTextFileName) : this(parTokenTextFileName, parSymbolTableTextFileName,parSyntaxTreeTextFileName)
        {
            _syntaxTreeModTextFileName = string.IsNullOrEmpty(parSyntaxTreeModTextFileName) ? "syntax_tree_mod.txt" : parSyntaxTreeModTextFileName;   
        }
        public OutputFileWriter(string parTokenTextFileName,
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
        public void WriteAbstractSyntaxTreeTextFile(ParserBase parParser)
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
        public void WriteBinaryFile(BinaryGenerator binaryGenerator)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(_binaryFileName, FileMode.OpenOrCreate);
            formatter.Serialize(fileStream, binaryGenerator.PortableCodes);
            formatter.Serialize(fileStream, binaryGenerator.SymbolTable);
            fileStream.Close();
        }
    }
}
