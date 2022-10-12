using DevCompilersLW2;
using DevCompilersLW3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
                Console.WriteLine("Incorrect input data\nCorrect format input data:\nprogram.exe inputExpr.txt tokens.txt symbols.txt");
            }*/
            //Console.WriteLine("Welcome to Calcy, a nifty and easy to use math interpreter.");
            /*while (true)
            {
                Console.Write(">> ");
                string input = Console.ReadLine();

                if (input == "exit()")
                {
                    break;
                }
                Console.WriteLine(">> {0}", input);
                Lexer lexer = new Lexer(input);
                List<Tokens> tokens = lexer.Get_Tokens();
                Console.WriteLine(">> {0}", lexer.ToString());
                Parser parser = new Parser(tokens);
                AST astObj = parser.ParseExp();
                if (astObj == null)
                {
                    continue;
                }
                Console.WriteLine(">> {0}", astObj.Eval());
            }*/
            string expr = "+";
           /* char[] digits = { '0', '1', '2',
            '3', '4', '5', '6', '7', '8', '9' };
            char[] letters = {'_','A','B','C','D','E','F','G','H', 'I', 'J', 'K', 'L'
        ,'M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z','a','b','c','d','e','f'
        ,'g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z'};
            char[] op = { '+', '-', '*', '/' };
            Class1 c2 = new Class1(op,letters,digits, expr);
            c2.MainMethode();*/

            //string expresion = File.ReadAllText(args[0].ToString());
           
            LexicalErrorAnalyzer lexicalErrorAnalyzer = new LexicalErrorAnalyzer();
            if (lexicalErrorAnalyzer.IsLexicalyCorrectExpresion(expr))
            {
                //OutputTextFileWriter outputTextFileWriter = new OutputTextFileWriter(args[1].ToString(), args[2].ToString());
                //outputTextFileWriter.WriteTokenTextFile(lexicalErrorAnalyzer);
                //outputTextFileWriter.WriteSymbolTableTextFile(lexicalErrorAnalyzer);
                Class1 c = new Class1(lexicalErrorAnalyzer.Tokens);
                c.Rt();
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
