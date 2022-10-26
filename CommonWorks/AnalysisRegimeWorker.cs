using DevCompilersLW2;
using DevCompilersLW3;
using DevCompilersLW4;
using System;

namespace CommonWorks
{
    public class AnalysisRegimeWorker
    {
        public static void RealizeSemanticAnalysis(string parExpression, InputParametersChecker parInputParametersChecker)
        {
            LexicalErrorAnalyzer lexicalErrorAnalyzer = new LexicalErrorAnalyzer();
            if (parInputParametersChecker.GetAnalysisRegime() == 0)
            {
                if (lexicalErrorAnalyzer.IsLexicalyCorrectExpresion(parExpression))
                {
                    string tokenTextFileName = parInputParametersChecker.GetTextFileOrEmpty(2);
                    string symbolTableTextFileName = parInputParametersChecker.GetTextFileOrEmpty(3);
                    string syntaxTreeTextFileName = parInputParametersChecker.GetTextFileOrEmpty(4);
                    string syntaxTreeModTextFileName = parInputParametersChecker.GetTextFileOrEmpty(5);
                    OutputTextFileWriter outputTextFileWriter = new OutputTextFileWriter(tokenTextFileName, 
                        symbolTableTextFileName, syntaxTreeTextFileName, syntaxTreeModTextFileName);
                    outputTextFileWriter.WriteTokenTextFile(lexicalErrorAnalyzer);
                    outputTextFileWriter.WriteSymbolTableTextFile(lexicalErrorAnalyzer);
                    SyntacticalErrorAnalyzer syntacticalErrorAnalyser =
                        new SyntacticalErrorAnalyzer(lexicalErrorAnalyzer.Tokens, lexicalErrorAnalyzer.SymbolTable);
                    Parser parser = TryToBuildSyntaxTree(syntacticalErrorAnalyser, syntaxTreeTextFileName, lexicalErrorAnalyzer.CanBuildSyntaxTree);
                    if (parser != null)
                    {
                        TryToBuildSyntaxModTree(parser, lexicalErrorAnalyzer, outputTextFileWriter);
                    }

                }
            }
        }
        private static Parser TryToBuildSyntaxTree(SyntacticalErrorAnalyzer parSyntacticalErrorAnalyzer,
            string syntaxTreeTextFileName,
            bool parIsLexicalyCorrectExpression)
        {
            
            if (parSyntacticalErrorAnalyzer.IsSyntaxicalyCorrectExpression() && parIsLexicalyCorrectExpression)
            {
                Parser parser = new Parser(parSyntacticalErrorAnalyzer);
                parser.ParseExpression();
                OutputTextFileWriter outputTextFileWriter = new OutputTextFileWriter(syntaxTreeTextFileName);
                outputTextFileWriter.WriteAbstractSyntaxTreeTextFile(parser);
                return parser;
            }
            return null;
        }
        public static void TryToBuildSyntaxModTree(Parser parParser, LexicalErrorAnalyzer parLexicalErrorAnalyzer, OutputTextFileWriter parOutputTextFileWriter)
        {
            SyntacticalTreeModificator syntacticalTreeModificator = new SyntacticalTreeModificator(parParser.GetAbstractSyntaxTree(),
                parLexicalErrorAnalyzer.SymbolTable);
            syntacticalTreeModificator.RealizeSyntaxTreeModification();
            SemanticErrorAnalyzer semanticErrorAnalyzer = new SemanticErrorAnalyzer(syntacticalTreeModificator.SyntaxTreeModified);
            semanticErrorAnalyzer.CheckingDivisionByZero();
            if (semanticErrorAnalyzer.CanWriteSyntaxTreeModFileText)
            {
                parOutputTextFileWriter.WriteSyntaxTreeMdifiedTextFile(syntacticalTreeModificator);
            }
        }
    }
}
