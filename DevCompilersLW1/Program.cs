using CommonWorks;
using DevCompilersLW2;
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
            InputParametersChecker inputParametersChecker = new InputParametersChecker(args);
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
            }
        }
    }
}
