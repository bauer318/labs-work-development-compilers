using DevCompilersLW2;
using DevCompilersLW3;
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
            /*if (args.Length==3)
            {
                if (CheckInputData(args))
                {
                    string expresion = File.ReadAllText(args[0].ToString());
                    LexicalErrorAnalyzer lexicalErrorAnalyzer = new LexicalErrorAnalyzer();
                    if (lexicalErrorAnalyzer.IsLexicalyCorrectExpresion(expresion))
                    {
                        OutputTextFileWriter outputTextFileWriter = new OutputTextFileWriter(args[1].ToString(), args[2].ToString());
                        outputTextFileWriter.WriteTokenTextFile(lexicalErrorAnalyzer);
                        outputTextFileWriter.WriteSymbolTableTextFile(lexicalErrorAnalyzer);
                    }  
                }
            }
            else if(args.Length==0)
            {
                Console.WriteLine("Execute me via CMD with input arguments -)");
            }
            else
            {
                Console.WriteLine("Incorrect input data\nCorrect format input data:\nprogram.exe inputExpr.txt Tokens.txt symbols.txt");
            }*/

            string expr = "2-(3+5)*4+(2+5)"; //And 2+(2+5)+(6/8) (((2-5)+2*(4-6))-3+5/(4/2))

            //ASTWorker parser = new ASTWorker();

            LexicalErrorAnalyzer lexicalErrorAnalyzer = new LexicalErrorAnalyzer();
            if (lexicalErrorAnalyzer.IsLexicalyCorrectExpresion(expr))
            {
                //OutputTextFileWriter outputTextFileWriter = new OutputTextFileWriter(args[1].ToString(), args[2].ToString());
                //outputTextFileWriter.WriteTokenTextFile(lexicalErrorAnalyzer);
                //outputTextFileWriter.WriteSymbolTableTextFile(lexicalErrorAnalyzer);
                SyntaxicalErrorAnalyzer c = new SyntaxicalErrorAnalyzer(lexicalErrorAnalyzer.Tokens);
                if (c.IsSyntaxicalyCorrectExpression())
                {
                    Parser parser = new Parser(lexicalErrorAnalyzer.Tokens);
                    parser.DoTask();
                    /*Console.Write("Before ");
                    parser.PrintList(parser._tokens);
                    Console.WriteLine();
                    parser.SetTokenPriority();
                    Console.WriteLine("Prio seted");
                    parser.PrintList(parser._tokens);
                    parser.SortByPriority();
                    Console.WriteLine("Sorted");
                    parser.PrintList(parser._tokens);*/
                    // parser.CreateByPriority();
                    //parser.DoTask();
                    //Console.Write("After ");
                    //parser.PrintList(parser._tokens);
                    //Console.WriteLine();
                    /*Console.WriteLine("Can Arrange "+parser.CanArrangeExpression(lexicalErrorAnalyzer.Tokens));
                    Console.WriteLine("Can Delete " + parser.CanDeleteBrace(lexicalErrorAnalyzer.Tokens));
                    List<Token> l = parser.DeleteExternDoubleBrace(lexicalErrorAnalyzer.Tokens);
                    parser.PrintList(l);*/
                    //parser.Test(lexicalErrorAnalyzer.Tokens, 0);
                    //parser.Do();
                    //parser.ParseExp();
                    //parser.Print2();
                    //parser.Print();

                }
                else
                {
                    Console.WriteLine("Negatif");
                }
            }

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
