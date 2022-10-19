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
            if (lexicalErrorAnalyzer.IsLexicalyCorrectExpresion(parExpression))
            {
                string tokenTextFileName = parInputParametersChecker.GetTextFileOrEmpty(2);
                string symbolTableTextFileName = parInputParametersChecker.GetTextFileOrEmpty(3);
                OutputTextFileWriter outputTextFileWriter = new OutputTextFileWriter(tokenTextFileName, symbolTableTextFileName);
                outputTextFileWriter.WriteTokenTextFile(lexicalErrorAnalyzer);
                outputTextFileWriter.WriteSymbolTableTextFile(lexicalErrorAnalyzer);
                if (parInputParametersChecker.GetAnalysisRegime() == 1)
                {
                    RealizeSyntaxicalAnalysis(lexicalErrorAnalyzer, parInputParametersChecker);
                }
            }
        }
        public static void RealizeSyntaxicalAnalysis(LexicalErrorAnalyzer parLexicalErrorAnalyzer, InputParametersChecker parInputParametersChecker)
        {
            SyntaxicalErrorAnalyzer syntaxicalErrorAnalyser = new SyntaxicalErrorAnalyzer(parLexicalErrorAnalyzer.Tokens);
            if (syntaxicalErrorAnalyser.IsSyntaxicalyCorrectExpression())
            {
                Parser parser = new Parser(parLexicalErrorAnalyzer.Tokens, parLexicalErrorAnalyzer.SymbolTable);
                parser.ParseExpression();
                string syntaxTreeTextFileName = parInputParametersChecker.GetTextFileOrEmpty(4);
                OutputTextFileWriter outputTextFileWriter = new OutputTextFileWriter(syntaxTreeTextFileName);
                outputTextFileWriter.WriteAbstractSyntaxTreeTextFile(parser);
            }
        }
    }
}
