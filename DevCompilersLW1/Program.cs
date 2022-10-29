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

            string expr = "((2+7)*2.5)+(7-8)/2";//   (8+4)*(5+2)/9.5   (8+4)*(5+2.5)/9
            //string expr = "(8+4)*(5+2)/9.5"; //(8+4.4)*(5+2.5)/9
            LexicalErrorAnalyzer lex = new LexicalErrorAnalyzer();
            if (lex.IsLexicalyCorrectExpresion(expr))
            {
                SyntacticalErrorAnalyzer syn = new SyntacticalErrorAnalyzer(lex.Tokens, lex.SymbolTable);
                if(syn.IsSyntaxicalyCorrectExpression() && lex.CanBuildSyntaxTree)
                {
                    Parser parser = new Parser(syn);
                    parser.ParseExpression();
                    SyntacticalTreeModificator treeMod = new SyntacticalTreeModificator(parser.GetAbstractSyntaxTree(), lex.SymbolTable);
                    treeMod.V();
                    //treeMod.Comp(treeMod.SyntaxTreeModified);
                    //treeMod.RealizeSyntaxTreeModification();
                    List<string> str = treeMod.GetSemanticTreeTextList();
                    foreach(string s in str)
                    {
                        Console.WriteLine(s);
                    }
                    //SemanticErrorAnalyzer semanticErrorAnalyzer = new SemanticErrorAnalyzer(treeMod.SyntaxTreeModified);
                   // semanticErrorAnalyzer.CheckingDivisionByZero();
                }
                
                
                
            }

        }
        
    }
}
