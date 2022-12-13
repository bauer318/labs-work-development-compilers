using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CommonWorks
{
    public class InputParametersChecker
    {
        string[] _args;
        public int index;
        public bool needOptimize;

        public InputParametersChecker(string[] parArgs)
        {
            index = 1;
            needOptimize = false;
            _args = parArgs;
        }
        public bool CheckInputData()
        {
            if (!(_args.Length >= 2 && _args.Length <= 7))
            {
                return false;
            }
            if (IsExistParameterWithIndex(1))
            {
                needOptimize = _args[1].ToString().ToLower().Equals("opt");
                if (needOptimize)
                {
                    index = 2;
                }
            }
            if (!IsCorrectTextFileName(_args[index].ToString()))
            {
                Console.WriteLine("Incorrect input expresion's text file name");
                return false;
            }
            if (IsExistParameterWithIndex(index+1))
            {
                if (!IsCorrectTextFileName(_args[index+1].ToString()))
                {
                    Console.WriteLine("Incorrect input Tokens's output text file name");
                    return false;
                }
            }
            if (IsExistParameterWithIndex(index+2))
            {
                if (!IsCorrectTextFileName(_args[index+2].ToString()))
                {
                    Console.WriteLine("Incorrect input symbol table's output text file name");
                    return false;
                }
            }
            if (IsExistParameterWithIndex(index+3))
            {
                if (!IsCorrectTextFileName(_args[index+2].ToString()))
                {
                    Console.WriteLine("Incorrect input syntax tree's output text file name");
                    return false;
                }
            }
            if (IsExistParameterWithIndex(index+4))
            {
                if (!IsCorrectTextFileName(_args[index+3].ToString()))
                {
                    Console.WriteLine("Incorrect input syntax tree modified's output text file name");
                    return false;
                }
            }
            if (IsExistParameterWithIndex(index+5))
            {
                if (!IsCorrectTextFileName(_args[index+4].ToString()))
                {
                    Console.WriteLine("Incorrect input portable code or postfix's output text file name");
                    return false;
                }
            }
            if (!IsExistTextFileInDirectory(_args[index].ToString()))
            {
                Console.WriteLine("expresion's text file doesn't exite in the current directory");
                return false;
            }
            return true;

        }

        public bool IsExistParameterWithIndex(int parIndex)
        {
            return _args.Length >= parIndex + 1;
        }

        public string GetTextFileOrEmpty(int parIndex)
        {
            string result = IsExistParameterWithIndex(parIndex) ? _args[parIndex].ToString() : string.Empty;
            return result;
        }
        public bool NeedOptimize()
        {
            return false;
        }

        public int GetAnalysisRegime()
        {
            switch (_args[0].ToString().ToLower())
            {
                case "lex":
                    return 0;
                case "syn":
                    return 1;
                case "sem":
                    return 2;
                case "gen1":
                    return 3;
                case "gen2":
                    return 4;
                case "gen3":
                    return 5;
                default:
                    return -1;
            }
        }

        private bool IsCorrectTextFileName(string parFileName)
        {
            return parFileName.EndsWith(".txt");
        }

        private bool IsExistTextFileInDirectory(string parFileName)
        {
            return File.Exists(Directory.GetCurrentDirectory() + "\\" + parFileName);
        }
    }
}
