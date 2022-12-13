using DevCompilersLW2;
using DevCompilersLW3;
using DevCompilersLW4;
using DevCompilersLW5;
using DevCompilersLW7;
using System;

namespace CommonWorks
{
    public class AnalysisRegimeWorker
    {
        private static InputParametersChecker inputParametersChecker;
        public static void RealizeLexicalSyntacticalSemanticAnalysis(string parExpression, InputParametersChecker parInputParametersChecker)
        {
            inputParametersChecker = parInputParametersChecker;
            LexicalErrorAnalyzer lexicalErrorAnalyzer = new LexicalErrorAnalyzer();
            string tokenTextFileName = parInputParametersChecker.GetTextFileOrEmpty(parInputParametersChecker.index+1);
            string symbolTableTextFileName = parInputParametersChecker.GetTextFileOrEmpty(parInputParametersChecker.index + 2);
            string syntaxTreeTextFileName = parInputParametersChecker.GetTextFileOrEmpty(parInputParametersChecker.index + 3);
            string syntaxTreeModTextFileName = parInputParametersChecker.GetTextFileOrEmpty(parInputParametersChecker.index + 4);
            string portableCodeTextFileName = parInputParametersChecker.GetTextFileOrEmpty(parInputParametersChecker.index + 5);
            string postfixExpressionTextFileName = parInputParametersChecker.GetTextFileOrEmpty(parInputParametersChecker.index + 5);
            OutputFileWriter outputTextFileWriter = new OutputFileWriter(tokenTextFileName,
                        symbolTableTextFileName, syntaxTreeTextFileName, syntaxTreeModTextFileName,portableCodeTextFileName,postfixExpressionTextFileName);
            ParserBase parser = null;
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
                    if (parser != null && !parser.DivideByZeroRunTimeException)
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
                    if(parser != null && !parser.DivideByZeroRunTimeException)
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
                case 5:
                    lexicalErrorAnalyzer.IsLexicalyCorrectExpresion(parExpression);
                    syntacticalErrorAnalyser = new SyntacticalErrorAnalyzer(lexicalErrorAnalyzer.Tokens, lexicalErrorAnalyzer.SymbolTable);
                    parser = TryToBuildSyntaxTree(syntacticalErrorAnalyser, syntaxTreeTextFileName, lexicalErrorAnalyzer.CanBuildSyntaxTree);
                    if (parser != null && !parser.DivideByZeroRunTimeException)
                        syntacticalTreeModificator = TryToBuildSyntaxModTree(parser, lexicalErrorAnalyzer, outputTextFileWriter);
                    IntermediateCodeGenerator intermediateCode = GeneratesIntermediateCode(syntacticalTreeModificator, parInputParametersChecker, outputTextFileWriter);
                    if(intermediateCode != null)
                    {
                        BinaryGenerator binaryGenerator = new BinaryGenerator(intermediateCode.PortableCodes,
                            intermediateCode.SymbolTable);
                        outputTextFileWriter.WriteBinaryFile(binaryGenerator);
                    }
                    break;

            }
          }
        private static IntermediateCodeGenerator GeneratesIntermediateCode(SyntacticalTreeModificator syntacticalTreeModificator,
            InputParametersChecker parInputParametersChecker, OutputFileWriter outputFileWriter)
        {
            IntermediateCodeGenerator intermediateCodeGenerator = null;
            if (syntacticalTreeModificator != null)
            {
                intermediateCodeGenerator = GenerateIntermediateCode(syntacticalTreeModificator);
                outputFileWriter.WritePortableCodeTextFile(intermediateCodeGenerator);
                outputFileWriter.WritePortableCodeSymbolTable(intermediateCodeGenerator);
            }
            return intermediateCodeGenerator;
        }
        private static ParserBase TryToBuildSyntaxTree(SyntacticalErrorAnalyzer parSyntacticalErrorAnalyzer,
            string syntaxTreeTextFileName,
            bool parIsLexicalyCorrectExpression)
        {
            
            if (parSyntacticalErrorAnalyzer.IsSyntaxicalyCorrectExpression() && parIsLexicalyCorrectExpression)
            {
                ParserBase parser = new Parser(parSyntacticalErrorAnalyzer);
                if (inputParametersChecker.needOptimize)
                {
                    parser = new ParserOpt(parSyntacticalErrorAnalyzer);
                }
                parser.ParseExpression();
                if (!parser.DivideByZeroRunTimeException)
                {
                    OutputFileWriter outputTextFileWriter = new OutputFileWriter(syntaxTreeTextFileName);
                    outputTextFileWriter.WriteAbstractSyntaxTreeTextFile(parser);
                }
                return parser;
            }
            return null;
        }
        private static SyntacticalTreeModificator TryToBuildSyntaxModTree(ParserBase parParser, LexicalErrorAnalyzer parLexicalErrorAnalyzer, OutputFileWriter parOutputTextFileWriter)
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
