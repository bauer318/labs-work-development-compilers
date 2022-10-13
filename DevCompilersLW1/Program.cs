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
            string expr = "var2 = 2 * 8 - (2+cont)";
            /*Lexer lexer = new Lexer(expr);
            List<Tokens> tokens = lexer.Get_Tokens();*/
            Parser parser = new Parser();
            /*AST astobj = parser.ParseExp();
            List<AST> aSTs = parser.asts;*/
            //List<int> pos = parser.GetOperatorPos(expr);
            //parser.Print(pos, expr);

            //string expresion = File.ReadAllText(args[0].ToString());
           
            LexicalErrorAnalyzer lexicalErrorAnalyzer = new LexicalErrorAnalyzer();
            if (lexicalErrorAnalyzer.IsLexicalyCorrectExpresion(expr))
            {
                //OutputTextFileWriter outputTextFileWriter = new OutputTextFileWriter(args[1].ToString(), args[2].ToString());
                //outputTextFileWriter.WriteTokenTextFile(lexicalErrorAnalyzer);
                //outputTextFileWriter.WriteSymbolTableTextFile(lexicalErrorAnalyzer);
                SyntaxicalErrorAnalyzer c = new SyntaxicalErrorAnalyzer(lexicalErrorAnalyzer.Tokens);
                if (c.IsSyntaxicalyCorrectExpression())
                {
                    Console.WriteLine("Okay");
                    List<int> pos = parser.GetOperatorsPosition(lexicalErrorAnalyzer.Tokens);
                    parser.Print(pos, lexicalErrorAnalyzer.Tokens);

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
