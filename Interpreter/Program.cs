using DevCompilersLW2;
using DevCompilersLW5;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Interpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                var binaryFileName = args[0];
                if (IsExistTextFileInDirectory(binaryFileName))
                {
                    Interpreter interpreter = CreateInterpreterByBinaryFileName(binaryFileName);
                    interpreter.InputValues();
                    interpreter.ProcessCalcul();
                    interpreter.PrintResult();
                }
                else
                {
                    Console.WriteLine("binary file doesn't exite in the current directory");
                }
                
            }
            else
            {
                Console.WriteLine("Execute me via CMD with input arguments -)");
            }
           
        }
        public static Interpreter CreateInterpreterByBinaryFileName(string parBinaryFileName)
        {
            var file = new FileStream(parBinaryFileName, FileMode.Open, FileAccess.Read);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            var portableCodes = (List<PortableCode>)binaryFormatter.Deserialize(file);
            var symbolTable = (SymbolTable)binaryFormatter.Deserialize(file);
            file.Close();
            return new Interpreter(portableCodes, symbolTable);
        }
        private static bool IsExistTextFileInDirectory(string parFileName)
        {
            return File.Exists(Directory.GetCurrentDirectory() + "\\" + parFileName);
        }
    }
}
