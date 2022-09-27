using DevCompilersLW2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DevCompilersLW1
{
    class Program
    {
        private static int _lineQuantity = 0;
        private static int _min = 0;
        private static int _max = 0;
        private static int _parameterQuantity = 0;
        private static bool _go = false;
        private static ArithmetiqueExpresionGenerator _arithmetiqueExpresionGenerator;
        private static ArithmetiqueExpresionTranslator _arithmetiqueExpresionTranslator;

        static void Main(string[] args)
        {
            /*_go = args.Length > 0;
            if (_go)
            {
                SetParameterQuantity(args);
              
                if (CheckInputData(args))
                {
                    _arithmetiqueExpresionGenerator = new ArithmetiqueExpresionGenerator(_lineQuantity, _min, _max, args[1].ToString());
                    _arithmetiqueExpresionTranslator = new ArithmetiqueExpresionTranslator(args[1].ToString(), args[2].ToString());
                    RealizeMainTask(args);
                }
            }
            else
            {
                Console.WriteLine("Execute me via CMD with input arguments -)");
            }*/
            string v = "va.r1_+(99..5.2-5var+.t-o.*(var2-0..658+.0))/_var3+1.*23.525+var2_+var_25e";
            //string v = "var1+25*8(25-89)+2.9@";
            LexicalErrorAnalyzer le = new LexicalErrorAnalyzer(v);
            le.IsCorrectExpresionLexicaly();
            
            //Console.WriteLine(le.SplitExpresion(le.Expresion).Length);

        }
        private static bool CheckInputData(string[] args)
        {
            
            if(args.Length!=5 && args.Length != 3)
            {
                Console.WriteLine("Incorrect input number of parameters");
                return false;
            }
            else if(!args[0].Equals("T") && !args[0].Equals("t") && !args[0].Equals("G") && !args[0].Equals("g"))
            {
                Console.WriteLine("Incorrect input work'mode \nT or t for Translation's mode\nG or g for Generation's mode");
                return false;
            }
            else if (!IsCorrectTextFileName(args[1].ToString()))
            {
                Console.WriteLine("Incorrect input generation's output text file name");
                return false;
            }
            else if((args[0].Equals("T") || args[0].Equals("t")))
            {
                if (!IsCorrectTextFileName(args[2].ToString()))
                {
                    Console.WriteLine("Incorrect input translation's output text file name");
                    return false;
                }
                else if (!IsExistTextFileInDirectory(args[1].ToString()))
                {
                    Console.WriteLine("Input text file for translation does exist in program's directory");
                    return false;
                }
            }
            else if (!IsCorrectNumericParameters(args))
            {
                Console.WriteLine("Incorrect input numeric parameter's values\n" +
                    "Generation's mode correct format :programName.exe G or g outputFileName.txt _lineQuantity minValueOperand maxValueOperand\n" +
                        "Tranlate's mode correct format :programName.exe T or t outputFileName.txt outputFileNameForTranslation.txt ");
                return false;
            }
            return true;

        }
        private static void RealizeMainTask(string[] args)
        {
            
            switch (_parameterQuantity)
            {
                case 5:
                    _arithmetiqueExpresionGenerator.RealizeGenerationMode();
                    break;
                case 3:
                    _arithmetiqueExpresionTranslator.RealizeTranslationMode(args);
                    break;

            }
        }
        
        
        private static bool IsCorrectInputParameters(string[] args)
        {
            bool result = args.Length == _parameterQuantity;
            switch (_parameterQuantity)
            {
                case 5:
                    return result && IsCorrectNumericParameters(args);
                case 3:
                    return result && IsCorrectTextFileName(args[2]) && IsExistTextFileInDirectory(args[1]);
                default:
                    return false;
            }
        }
        private static bool IsCorrectNumericParameters(string[] args)
        {
            return Int32.TryParse(args[2], out _lineQuantity) &&
                     Int32.TryParse(args[3], out _min) &&
                     Int32.TryParse(args[4], out _max) && IsPositivesNumericParameters(_lineQuantity, _min, _max); 
        }
        private static bool IsPositivesNumericParameters(int parLineQuantity, int parMin, int parMax)
        {
            return parLineQuantity > 0 && parMin > 0 && parMax > 0 && parMin <= parMax;
        }
        
        private static bool IsCorrectTextFileName(string parFileName)
        {
            return parFileName.EndsWith(".txt");
        }

        private static bool IsExistTextFileInDirectory(string parFileName)
        {
            return File.Exists(Directory.GetCurrentDirectory() +"\\"+ parFileName);
        }

        private static void SetParameterQuantity(string[] args)
        {
            if((args[0].Equals("G")|| args[0].Equals("g")) && args.Length==5)
            {
                _parameterQuantity = args.Length;
            }
            else if((args[0].Equals("T") || args[0].Equals("t")) && args.Length==3)
            {
                _parameterQuantity = args.Length;
            }
            
        }
        
    }
}
