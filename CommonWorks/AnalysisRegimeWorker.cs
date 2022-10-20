using DevCompilersLW2;
using DevCompilersLW3;
using System;

namespace CommonWorks
{
    public class AnalysisRegimeWorker
    {
        public static void RealizeLexicalSyntaxicalAnalysis(string parExpression, InputParametersChecker parInputParametersChecker)
        {
            LexicalErrorAnalyzer lexicalErrorAnalyzer = new LexicalErrorAnalyzer();
            switch (parInputParametersChecker.GetAnalysisRegime())
            {
                case 0:
                    if (lexicalErrorAnalyzer.IsLexicalyCorrectExpresion(parExpression))
                    {
                        string tokenTextFileName = parInputParametersChecker.GetTextFileOrEmpty(2);
                        string symbolTableTextFileName = parInputParametersChecker.GetTextFileOrEmpty(3);
                        OutputTextFileWriter outputTextFileWriter = new OutputTextFileWriter(tokenTextFileName, symbolTableTextFileName);
                        outputTextFileWriter.WriteTokenTextFile(lexicalErrorAnalyzer);
                        outputTextFileWriter.WriteSymbolTableTextFile(lexicalErrorAnalyzer);
                    }
                    break;
                case 1:
                    lexicalErrorAnalyzer.IsLexicalyCorrectExpresion(parExpression);
                    SyntaxicalErrorAnalyzer syntaxicalErrorAnalyser = 
                        new SyntaxicalErrorAnalyzer(lexicalErrorAnalyzer.Tokens, lexicalErrorAnalyzer.SymbolTable);
                    TryToBuildSyntaxTree(syntaxicalErrorAnalyser, parInputParametersChecker,lexicalErrorAnalyzer.CanBuildSyntaxTree);
                    break;

            }
        }
        public static void TryToBuildSyntaxTree(SyntaxicalErrorAnalyzer parSyntaxicalErrorAnalyzer,
            InputParametersChecker parInputParametersChecker,
            bool parIsLexicalyCorrectExpression)
        {
            
            if (parSyntaxicalErrorAnalyzer.IsSyntaxicalyCorrectExpression() && parIsLexicalyCorrectExpression)
            {
                Parser parser = new Parser(parSyntaxicalErrorAnalyzer);
                parser.ParseExpression();
                string syntaxTreeTextFileName = parInputParametersChecker.GetTextFileOrEmpty(4);
                OutputTextFileWriter outputTextFileWriter = new OutputTextFileWriter(syntaxTreeTextFileName);
                outputTextFileWriter.WriteAbstractSyntaxTreeTextFile(parser);
            }
        }
    }
}
