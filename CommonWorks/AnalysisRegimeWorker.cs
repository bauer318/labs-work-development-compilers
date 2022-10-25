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
                    SyntacticalErrorAnalyzer syntacticalErrorAnalyser = 
                        new SyntacticalErrorAnalyzer(lexicalErrorAnalyzer.Tokens, lexicalErrorAnalyzer.SymbolTable);
                    TryToBuildSyntaxTree(syntacticalErrorAnalyser, parInputParametersChecker,lexicalErrorAnalyzer.CanBuildSyntaxTree);
                    break;

            }
        }
        public static void TryToBuildSyntaxTree(SyntacticalErrorAnalyzer parSyntacticalErrorAnalyzer,
            InputParametersChecker parInputParametersChecker,
            bool parIsLexicalyCorrectExpression)
        {
            
            if (parSyntacticalErrorAnalyzer.IsSyntaxicalyCorrectExpression() && parIsLexicalyCorrectExpression)
            {
                Parser parser = new Parser(parSyntacticalErrorAnalyzer);
                parser.ParseExpression();
                string syntaxTreeTextFileName = parInputParametersChecker.GetTextFileOrEmpty(4);
                OutputTextFileWriter outputTextFileWriter = new OutputTextFileWriter(syntaxTreeTextFileName);
                outputTextFileWriter.WriteAbstractSyntaxTreeTextFile(parser);
            }
        }
    }
}
