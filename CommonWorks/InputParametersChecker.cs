using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CommonWorks
{
    public class InputParametersChecker
    {
        string[] _args;

        public InputParametersChecker(string[] parArgs)
        {
            _args = parArgs;
        }
        public bool CheckInputData()
        {
            if (!(_args.Length >= 2 && _args.Length <= 6))
            {
                return false;
            }
            if (!IsCorrectTextFileName(_args[1].ToString()))
            {
                Console.WriteLine("Incorrect input expresion's text file name");
                return false;
            }
            if (IsExistParameterWithIndex(2))
            {
                if (!IsCorrectTextFileName(_args[2].ToString()))
                {
                    Console.WriteLine("Incorrect input Tokens's output text file name");
                    return false;
                }
            }
            if (IsExistParameterWithIndex(3))
            {
                if (!IsCorrectTextFileName(_args[3].ToString()))
                {
                    Console.WriteLine("Incorrect input symbol table's output text file name");
                    return false;
                }
            }
            if (IsExistParameterWithIndex(4))
            {
                if (!IsCorrectTextFileName(_args[3].ToString()))
                {
                    Console.WriteLine("Incorrect input syntax tree's output text file name");
                    return false;
                }
            }
            if (IsExistParameterWithIndex(5))
            {
                if (!IsCorrectTextFileName(_args[4].ToString()))
                {
                    Console.WriteLine("Incorrect input syntax tree modified's output text file name");
                    return false;
                }
            }
            if (!IsExistTextFileInDirectory(_args[1].ToString()))
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

        public int GetAnalysisRegime()
        {
            switch (_args[0].ToString().ToLower())
            {
                case "sem":
                    return 0;
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
