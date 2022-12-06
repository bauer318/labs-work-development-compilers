﻿using CommonWorks;
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
            string expr = "A + B[f] * (60 - 6/2)"; //   9-(5+2) (a+b)*(c+d)-25/0   A + B * (60 - 6/(2+2))
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
                    /*Console.WriteLine("Symbol table");
                    foreach (string str in gen.GetSymboleTableText())
                    {
                        Console.WriteLine(str);
                    }*/
                    /*Console.WriteLine("Post fix");
                    foreach (string str in gen.GetPostFixExpressionText())
                    {
                        Console.Write(str);
                    }*/
                    Console.WriteLine();
                    /*foreach(PortableCode portableCode in gen.PortableCodes)
                    {
                        Console.WriteLine(portableCode.OperationCode + " " + portableCode.Result.Lexeme + " "
                            + PortableCodeWorker.CanOperate(portableCode));
                    }*/
                    
                    PortableCodeOptimizator portableCodeOptimizator = new PortableCodeOptimizator(gen.PortableCodes,
                        gen.SymbolTable);
                    PortableCode portable = PortableCodeWorker.GetPortableCodeByIdResult(gen.PortableCodes, 3);
                    Console.Write("Portable "+portable.Result.Lexeme+" ");
                    foreach(Token token in portable.OperandList)
                    {
                        Console.Write(" " + token.Lexeme);
                    }
                    //PortableCodeOptimizator.GetTokenResult(gen.SemanticTree);
                    /*List<PortableCode> result = portableCodeOptimizator.Go();
                    foreach (PortableCode p in result)
                    {
                        Console.WriteLine(p.toString());
                    }*/
                    //result = PortableCodeWorker.GetLastList(result);
                    /*Console.WriteLine();
                    foreach (PortableCode p in result)
                    {
                        Console.WriteLine(p.toString());
                    }*/
                    /*result = PortableCodeWorker.PortableCodesOpt;
                    foreach(PortableCode p in result)
                    {
                        Console.WriteLine(p.toString());
                    }*/
                    //Console.WriteLine("-----------");
                    /* foreach(PortableCode portableCode in result)
                     {
                         if (!PortableCodeWorker.IsResultValueToken(portableCode))
                             Console.WriteLine(portableCode.Result.Lexeme);
                         Console.WriteLine("Code op " + portableCode.OperationCode + " op1 " + portableCode.OperandList[0].Lexeme +
                             " op2 " + portableCode.OperandList[1].Lexeme);
                         else
                             Console.WriteLine("result value " + portableCode.Result.Lexeme);
                     }*/
                    /*List<Token> listt = new List<Token>();
                    listt.RealizeAdd(new Token(TokenType.INTEGER_CONSTANT, "6"));
                    listt.RealizeAdd(new Token(TokenType.CORRECT_DECIMAL_CONSTANT, "2.5"));*/
                    //PortableCodeOptimizator.PlusOperation(new PortableCode("plus", new Token(TokenType.DIVISION_SIGN, "/"), listt));
                    /*foreach(PortableCode portableCode in gen.PortableCodes)
                     {
                         if (PortableCodeWorker.IsConstantInt2Float(portableCode))
                         {
                             PortableCodeWorker.RealizeInt2Float(portableCode);
                         }
                     }*/

                }
            }
        }
        
    }
}
