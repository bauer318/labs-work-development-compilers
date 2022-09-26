using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DevCompilersLW1
{
    public class ArithmetiqueExpresionGenerator
    {
        private int _lineQuantity = 0;
        private int _min = 0;
        private int _max = 0;
        private string _outPutFileName = "";
        public int Min
        {
            get
            {
                return _min;
            }
            set
            {
                _min = value;
            }
        }
        public int Max
        {
            get
            {
                return _max;
            }
            set
            {
                _max = value;
            }
        }
        public string OutputFileName
        {
            get
            {
                return _outPutFileName;
            }
            set
            {
                _outPutFileName = value;
            }
        }
        public int LineQuantity
        {
            get
            {
                return _lineQuantity;
            }
            set
            {
                _lineQuantity = value;
            }
        }
        
        public ArithmetiqueExpresionGenerator(int parLineQuantity, int parMin, int parMax, string parOutputFileName)
        {
            _min = parMin;
            _max = parMax;
            _outPutFileName = parOutputFileName;
            _lineQuantity = parLineQuantity;
        }
        private  string GenerateArithmetiqueExpresion()
        {
            Random rnd = new Random();
            int operandQuantity = rnd.Next(_min, _max);
            string result = "";
            for (var i = 0; i <= operandQuantity; i++)
            {
                if (i < operandQuantity)
                {
                    result += GetRandomNumber() + GetRandomOperation();
                }
                else
                {
                    result += GetRandomNumber();
                }
            }
            return result;
        }
        private  string GetRandomNumber()
        {
            Random rnd = new Random();
            return rnd.Next(1, 10).ToString();
        }
        private  string GetRandomOperation()
        {
            Random rnd = new Random();
            char[] operationsAlphabet = { '+', '-', '*', '/' };
            return operationsAlphabet[rnd.Next(0, 4)].ToString();
        }
        public  void RealizeGenerationMode()
        {
            using (var sw = new StreamWriter(_outPutFileName))
            {
                for (var i = 0; i < _lineQuantity; i++)
                {
                    sw.WriteLine(GenerateArithmetiqueExpresion());
                }
            }
        }
        

    }
}
