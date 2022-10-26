using CommonWorks;
using DevCompilersLW2;
using DevCompilersLW3;
using DevCompilersLW4;
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
                    AnalysisRegimeWorker.RealizeSemanticAnalysis(expresion, inputParametersChecker);
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
            string expression = "var[]+4-var3[f]*60.3+2"; //var1[i]*60+var2[f] var[] + 4 - var3[f]*60.3+var3[F]
            LexicalErrorAnalyzer lexicalErrorAnalyzer = new LexicalErrorAnalyzer();
            if (lexicalErrorAnalyzer.IsLexicalyCorrectExpresion(expression))
            {
               SyntacticalErrorAnalyzer syntactical = new SyntacticalErrorAnalyzer(lexicalErrorAnalyzer.Tokens, lexicalErrorAnalyzer.SymbolTable);
                if (syntactical.IsSyntaxicalyCorrectExpression() && lexicalErrorAnalyzer.CanBuildSyntaxTree)
                {
                    Parser parser = new Parser(syntactical);
                    parser.ParseExpression();
                    
                    SyntacticalTreeModificator semantic = new SyntacticalTreeModificator(parser.GetAbstractSyntaxTree(), syntactical.SymbolTable);
                    
                    Console.WriteLine();
                    
                    semantic.RealizeSyntaxTreeModification();
                  
                   /* List<string> t = semantic.GetSemanticTreeTextList();
                    foreach(string str in t)
                    {
                        Console.WriteLine(str);
                    }*/
                    //SemanticErrorAnalyzer semanticErrorAnalyzer = new SemanticErrorAnalyzer(semantic.SyntaxTreeModified);
                    //semanticErrorAnalyzer.CheckingDivisionByZero();
                   
                }   
            }
        }
    }
}
