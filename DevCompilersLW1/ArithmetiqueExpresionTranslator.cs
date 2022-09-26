using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace DevCompilersLW1
{
     public class ArithmetiqueExpresionTranslator
    {
        
        private static string _outPutFileName = "";
        private static string _outPutFileName_Trans = "";
        private static Dictionary<string, string> _alphanumericDictionary = new Dictionary<string, string>();
        public ArithmetiqueExpresionTranslator(string outputFileName, string outputTransFileName)
        {
            _outPutFileName = outputFileName;
            _outPutFileName_Trans = outputTransFileName;
        }
        public  void RealizeTranslationMode(string[] args)
        {
            CreateAlphabetTranslation();
            string[] linesReadedArray = File.ReadAllLines(_outPutFileName);
            using (var sw = new StreamWriter(_outPutFileName_Trans))
            {
                for (var i = 0; i < linesReadedArray.Length; i++)
                {
                    if (CheckFile(linesReadedArray[i]))
                    {
                        sw.WriteLine(TranslateArithmeticExpresion(linesReadedArray[i]));
                    }
                    else
                    {
                        Console.WriteLine("Incorrect input data in text file");
                        return;
                    }

                }
            }
        }
        public bool CheckFile(string text)
        {
            for (var i = 0; i < text.Length; i++)
            {
                if (!_alphanumericDictionary.ContainsKey(text.ElementAt(i).ToString()))
                 {
                    return false;
                }
            }
            return true;
        }
        private  string TranslateArithmeticExpresion(string parArithmetiqueExpresion)
        {
            string result = "";
            for (var i = 0; i < parArithmetiqueExpresion.Length; i++)
            {
                result += _alphanumericDictionary.GetValueOrDefault(parArithmetiqueExpresion.ElementAt(i).ToString());
            }
            return result;
        }
        private  void CreateAlphabetTranslation()
        {
            string[] operandOpertionAlhpabet = { " Один ", " Два ", " Три ", " Четыре ", " Пять ", " Шесть ", " Семь ", " Восемь ",
                " Девять "," плюс "," минус "," умножит на "," делить на " };
            string[] operationAlphabet = { "+", "-", "*", "/" };
            var j = 0;
            for (var i = 1; i <= 9; i++)
            {
                _alphanumericDictionary.Add(i.ToString(), operandOpertionAlhpabet[i - 1]);
            }
            for (var i = 9; i <= 12; i++)
            {
                _alphanumericDictionary.Add(operationAlphabet[j], operandOpertionAlhpabet[i]);
                j++;
            }
        }
    }
}
