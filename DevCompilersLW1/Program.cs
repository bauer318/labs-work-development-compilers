using CommonWorks;
using DevCompilersLW2;
using DevCompilersLW3;
using DevCompilersLW4;
using DevCompilersLW5;
using DevCompilersLW6;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DevCompilersLW1
{
    class Program
    {
        static void Main(string[] args)
        {
            /*InputParametersChecker inputParametersChecker = new InputParametersChecker(args);
            if (inputParametersChecker.CheckInputData())
            {
                string expresion = File.ReadAllText(args[1].ToString());
                if(inputParametersChecker.GetAnalysisRegime()!=-1)
                {
                    AnalysisRegimeWorker.RealizeLexicalSyntacticalSemanticAnalysis(expresion, inputParametersChecker);
                }
                else
                {
                    Console.WriteLine("Incorrect input work's regime\nTapes SEM or sem for Semantical analysis");
                }
            }
            else if (args.Length == 0)
            {
                Console.WriteLine("Execute me via CMD with input arguments -)");
            }
            else
            {
                Console.WriteLine("Incorrect input data\nCorrect format input data:\nprogram.exe SEM or sem inputExpr.txt [Tokens.txt] [symbols.txt] [syntax_tree.txt] " +
                    "[syntax_tree_mod.txt] for semantic analysis");
            }*/
            LexicalErrorAnalyzer lex = new LexicalErrorAnalyzer();
            string expr = "A + B * (60 - 6/2)"; //   9-(5+2) (a+b)*(c+d)-25/0
            if (lex.IsLexicalyCorrectExpresion(expr))
            {
                SyntacticalErrorAnalyzer syn = new SyntacticalErrorAnalyzer(lex.Tokens, lex.SymbolTable);
                Parser parser = new Parser(syn);
                parser.ParseExpression();
                Console.WriteLine("Syntax");
                foreach (string str in parser.GetSyntaxTreeNodeTextArray())
                {
                    Console.WriteLine(str);
                }
                SyntacticalTreeModificator synT = new SyntacticalTreeModificator(parser.GetAbstractSyntaxTree(), lex.SymbolTable);
                synT.RealizeTopBottomSyntaxTreeModification();
                SemanticErrorAnalyzer sem = new SemanticErrorAnalyzer(synT.SyntaxTreeModified);
                if (sem.CanWriteSyntaxTreeModFileText)
                {
                    Console.WriteLine("Sem");
                    foreach (string str in synT.GetSemanticTreeTextList())
                    {
                        Console.WriteLine(str);
                    }
                    IntermediateCodeGenerator gen = new IntermediateCodeGenerator(synT.SyntaxTreeModified, synT.SymboleTable);
                    Console.WriteLine("Portable code");
                    foreach (string str in gen.GetPortableCodeText())
                    {
                        Console.WriteLine(str);
                    }
                    Console.WriteLine("Symbol table");
                    foreach (string str in gen.GetSymboleTableText())
                    {
                        Console.WriteLine(str);
                    }
                    Console.WriteLine("Post fix");
                    foreach (string str in gen.GetPostFixExpressionText())
                    {
                        Console.Write(str);
                    }
                    Console.WriteLine();
                    foreach(PortableCode portableCode in gen.PortableCodes)
                    {
                        Console.WriteLine(portableCode.OperationCode + " " + portableCode.Result.Lexeme + " "
                            + PortableCodeWorker.CanOperate(portableCode));
                    }

                }
            }
        }
        
    }
}
