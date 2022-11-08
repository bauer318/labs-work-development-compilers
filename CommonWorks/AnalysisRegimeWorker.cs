using DevCompilersLW2;
using DevCompilersLW3;
using DevCompilersLW4;
using DevCompilersLW5;
using System;

namespace CommonWorks
{
    public class AnalysisRegimeWorker
    {
        public static void RealizeLexicalSyntacticalSemanticAnalysis(string parExpression, InputParametersChecker parInputParametersChecker)
        {
            LexicalErrorAnalyzer lexicalErrorAnalyzer = new LexicalErrorAnalyzer();
            string tokenTextFileName = parInputParametersChecker.GetTextFileOrEmpty(2);
            string symbolTableTextFileName = parInputParametersChecker.GetTextFileOrEmpty(3);
            string syntaxTreeTextFileName = parInputParametersChecker.GetTextFileOrEmpty(4);
            string syntaxTreeModTextFileName = parInputParametersChecker.GetTextFileOrEmpty(5);
            string portableCodeTextFileName = parInputParametersChecker.GetTextFileOrEmpty(6);
            string postfixExpressionTextFileName = parInputParametersChecker.GetTextFileOrEmpty(6);
            OutputTextFileWriter outputTextFileWriter = new OutputTextFileWriter(tokenTextFileName,
                        symbolTableTextFileName, syntaxTreeTextFileName, syntaxTreeModTextFileName,portableCodeTextFileName,postfixExpressionTextFileName);
            Parser parser = null;
            SyntacticalErrorAnalyzer syntacticalErrorAnalyser = null;
            SyntacticalTreeModificator syntacticalTreeModificator = null;
            switch (parInputParametersChecker.GetAnalysisRegime())
            {
                case 0:
                    if (lexicalErrorAnalyzer.IsLexicalyCorrectExpresion(parExpression))
                    {
                        outputTextFileWriter.WriteTokenTextFile(lexicalErrorAnalyzer);
                        outputTextFileWriter.WriteSymbolTableTextFile(lexicalErrorAnalyzer);
                    }
                    break;
                case 1:
                    bool go =lexicalErrorAnalyzer.IsLexicalyCorrectExpresion(parExpression);
                    syntacticalErrorAnalyser = new SyntacticalErrorAnalyzer(lexicalErrorAnalyzer.Tokens, lexicalErrorAnalyzer.SymbolTable);
                    if (go)
                    {
                        parser = TryToBuildSyntaxTree(syntacticalErrorAnalyser, syntaxTreeTextFileName, lexicalErrorAnalyzer.CanBuildSyntaxTree);
                    }
                    break;
                case 2:
                    lexicalErrorAnalyzer.IsLexicalyCorrectExpresion(parExpression);
                    syntacticalErrorAnalyser = new SyntacticalErrorAnalyzer(lexicalErrorAnalyzer.Tokens, lexicalErrorAnalyzer.SymbolTable);
                    parser = TryToBuildSyntaxTree(syntacticalErrorAnalyser, syntaxTreeTextFileName, lexicalErrorAnalyzer.CanBuildSyntaxTree);
                    if (parser != null)
                    {
                        outputTextFileWriter.WriteTokenTextFile(lexicalErrorAnalyzer);
                        outputTextFileWriter.WriteSymbolTableTextFile(lexicalErrorAnalyzer);
                        TryToBuildSyntaxModTree(parser, lexicalErrorAnalyzer, outputTextFileWriter);
                    }
                    break;
                case 3:
                case 4:
                    lexicalErrorAnalyzer.IsLexicalyCorrectExpresion(parExpression);
                    syntacticalErrorAnalyser = new SyntacticalErrorAnalyzer(lexicalErrorAnalyzer.Tokens, lexicalErrorAnalyzer.SymbolTable);
                    parser = TryToBuildSyntaxTree(syntacticalErrorAnalyser, syntaxTreeTextFileName, lexicalErrorAnalyzer.CanBuildSyntaxTree);
                    if(parser != null)
                    syntacticalTreeModificator = TryToBuildSyntaxModTree(parser, lexicalErrorAnalyzer, outputTextFileWriter);
                    if(syntacticalTreeModificator != null)
                    {
                        IntermediateCodeGenerator intermediateCodeGenerator = GenerateIntermediateCode(syntacticalTreeModificator);
                        switch (parInputParametersChecker.GetAnalysisRegime())
                        {
                            case 3:
                                outputTextFileWriter.WritePortableCodeTextFile(intermediateCodeGenerator);
                                break;
                            default:
                                outputTextFileWriter.WritePostfixExpressionTextFile(intermediateCodeGenerator);
                                break;
                        }
                        outputTextFileWriter.WritePortableCodeSymbolTable(intermediateCodeGenerator);
                    }
                    
                    break;

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
        private static SyntacticalTreeModificator TryToBuildSyntaxModTree(Parser parParser, LexicalErrorAnalyzer parLexicalErrorAnalyzer, OutputTextFileWriter parOutputTextFileWriter)
        {
            SyntacticalTreeModificator syntacticalTreeModificator = new SyntacticalTreeModificator(parParser.GetAbstractSyntaxTree(),
                parLexicalErrorAnalyzer.SymbolTable);
            syntacticalTreeModificator.RealizeTopBottomSyntaxTreeModification();
            SemanticErrorAnalyzer semanticErrorAnalyzer = new SemanticErrorAnalyzer(syntacticalTreeModificator.SyntaxTreeModified);
            semanticErrorAnalyzer.CheckingDivisionByZero();
            if (semanticErrorAnalyzer.CanWriteSyntaxTreeModFileText)
            {
                parOutputTextFileWriter.WriteSyntaxTreeMdifiedTextFile(syntacticalTreeModificator);
                return syntacticalTreeModificator;
            }
            return null;   
        }
        private static IntermediateCodeGenerator GenerateIntermediateCode(SyntacticalTreeModificator parSyntacticalTreeModificator)
        {
            return new IntermediateCodeGenerator(parSyntacticalTreeModificator.SyntaxTreeModified,
                            parSyntacticalTreeModificator.SymboleTable);
        }
    }
}
