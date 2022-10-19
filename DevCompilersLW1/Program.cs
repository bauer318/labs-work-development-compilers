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
            /*if (args.Length>=2 && args.Length<=5)
            {
                if (CheckInputData(args))
                {
                    
                    LexicalErrorAnalyzer lexicalErrorAnalyzer = new LexicalErrorAnalyzer();
                    if (lexicalErrorAnalyzer.IsLexicalyCorrectExpresion(expresion))
                    {
                        OutputTextFileWriter outputTextFileWriter = new OutputTextFileWriter(args[1].ToString(), args[2].ToString());
                        outputTextFileWriter.WriteTokenTextFile(lexicalErrorAnalyzer);
                        outputTextFileWriter.WriteSymbolTableTextFile(lexicalErrorAnalyzer);
                    }  
                }
            }*/


            /*string expr = "2-(3+5)*4+(2+5)"; //And 2+(2+5)+(6/8) (((2-5)+2*(4-6))-3+5/(4/2))

            //ASTWorker parser = new ASTWorker();

            LexicalErrorAnalyzer lexicalErrorAnalyzer = new LexicalErrorAnalyzer();
            if (lexicalErrorAnalyzer.IsLexicalyCorrectExpresion(expr))
            {
                //Case 1
                //OutputTextFileWriter outputTextFileWriter = new OutputTextFileWriter(args[1].ToString(), args[2].ToString());
                //outputTextFileWriter.WriteTokenTextFile(lexicalErrorAnalyzer);
                //outputTextFileWriter.WriteSymbolTableTextFile(lexicalErrorAnalyzer);
                //Case 2

                SyntaxicalErrorAnalyzer c = new SyntaxicalErrorAnalyzer(lexicalErrorAnalyzer.Tokens);
                if (c.IsSyntaxicalyCorrectExpression())
                {
                    Parser parser = new Parser(lexicalErrorAnalyzer.Tokens, lexicalErrorAnalyzer.SymbolTable);
                    parser.ParseExpression();
                    
                }
                else
                {
                    Console.WriteLine("Negatif");
                }
            }*/

        }
        
        private static bool CheckInputData(string[] args)
        {
            
            if(!IsCorrectTextFileName(args[0].ToString()))
            {
                Console.WriteLine("Incorrect input expresion's text file name");
                return false;
            }
            else if(!IsCorrectTextFileName(args[1].ToString()))
            {
                Console.WriteLine("Incorrect input Tokens's output text file name");
                return false;
            }
            else if (!IsCorrectTextFileName(args[2].ToString()))
            {
                Console.WriteLine("Incorrect input symbol table's output text file name");
                return false;
            }
            else if(!IsExistTextFileInDirectory(args[0].ToString()))
            {
                Console.WriteLine("expresion's text file doesn't exite in the current directory");
                return false;
            }
            return true;

        }
        
        private static bool IsCorrectTextFileName(string parFileName)
        {
            return parFileName.EndsWith(".txt");
        }

        private static bool IsExistTextFileInDirectory(string parFileName)
        {
            return File.Exists(Directory.GetCurrentDirectory() +"\\"+ parFileName);
        }
    }
}
