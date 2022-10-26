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
                    AnalysisRegimeWorker.RealizeLexicalSyntaxicalAnalysis(expresion, inputParametersChecker);
                }
                else
                {
                    Console.WriteLine("Incorrect input work's regime\nTapes Lex or lex for Lexical analysis\nSyn or sys for syntaxical analysis");
                }
            }
            else if (args.Length == 0)
            {
                Console.WriteLine("Execute me via CMD with input arguments -)");
            }
            else
            {
                Console.WriteLine("Incorrect input data\nCorrect format input data:\nprogram.exe LEX or lex inputExpr.txt [Tokens.txt] [symbols.txt] for lexical analysis\n" +
                    "programe.exe SYN or syn inputExpr.txt [Tokens.txt] [symbols.txt] [syntax_tree.txt] for syntaxical analysis\n");
            }*/
            string expression = "var[f]+var2[i]*60"; //var1[i]*60+var2[f]
            LexicalErrorAnalyzer lexicalErrorAnalyzer = new LexicalErrorAnalyzer();
            if (lexicalErrorAnalyzer.IsLexicalyCorrectExpresion(expression))
            {
               /* for(int i=0; i < lexicalErrorAnalyzer.Tokens.Count; i++)
                {
                    Console.WriteLine(lexicalErrorAnalyzer.Tokens[i].TokenType);
                }*/
               SyntacticalErrorAnalyzer syntactical = new SyntacticalErrorAnalyzer(lexicalErrorAnalyzer.Tokens, lexicalErrorAnalyzer.SymbolTable);
                if (syntactical.IsSyntaxicalyCorrectExpression() && lexicalErrorAnalyzer.CanBuildSyntaxTree)
                {
                    Parser parser = new Parser(syntactical);
                    parser.ParseExpression();
                    //parser.PrintTokenNode();
                    //if(parser.is)
                    SemanticErrorAnalyzer semantic = new SemanticErrorAnalyzer(parser.GetAbstractSyntaxTree(), syntactical.SymbolTable);
                    //semantic.GoToLast(parser.GetAbstractSyntaxTree());
                    //semantic.Create();
                    /*List<string> t = semantic.GetSemanticTreeTextList();
                    foreach (string str in t)
                    {
                        Console.WriteLine(str);
                    }*/
                    Console.WriteLine();
                    //semantic.PrintVerifiResult();
                    semantic.CompareOut();
                    //semantic.Print2();
                    //semantic.CompareOut();
                    List<string> t = semantic.GetSemanticTreeTextList();
                    foreach(string str in t)
                    {
                        Console.WriteLine(str);
                    }
                    //semantic.PrintListToken();
                    //semantic.Run(parser.GetAbstractSyntaxTree());
                }
                
            }
        }
    }
}
