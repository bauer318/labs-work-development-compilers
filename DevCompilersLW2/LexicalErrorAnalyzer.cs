using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DevCompilersLW2
{
    public class LexicalErrorAnalyzer
    {
        private string _expresion;
        public Dictionary<string, int> dic = new Dictionary<string, int>();

        public string Expresion
        {
            get
            {
                return _expresion;
            }
            set
            {
                _expresion = value;
            }
        }
        public LexicalErrorAnalyzer(string parExpresion)
        {
            _expresion = parExpresion;
        }

        public bool IsCorrectExpresionLexicaly()
        {
            bool result = true;
            if (string.IsNullOrEmpty(Expresion))
            {
                Console.WriteLine("Error! Empty expresion");
                return false;
            }

            Dictionary<string, string> dictionary = GetLexicalDictionary();
            List<List<string>> list = CreateListExpresionAlphabet();
            if (HasInvalidCharacterInExpresion(dictionary))
            {
                result = false;
            }
            if (AreInvalidIdentificatorOrDecimalConstant(list))
            {
                PrintInvaliIdentificatorDecimalConstantErrorMessage(list);
                result = false;
            }
            return result;
        }
        public string GetExpresionWithWhitespace()
        {
            string result = "";
            for(var i=0; i<_expresion.Length; i++)
            {
                string currentText = _expresion.ElementAt(i).ToString();
                if (IsExpresionSeparator(currentText))
                {
                    result += " " + currentText+" ";
                }
                else
                {
                    result += currentText;
                }
            }
            return result.Trim();
        }

        private bool IsExpresionSeparator(string parText)
        {
            string[] separatorSymbolArray = { "+", "/", "*", "-", "(", ")" };
            bool result = false;
            for(var i = 0; i< separatorSymbolArray.Length; i++)
            {
                if (parText.Equals(separatorSymbolArray[i]))
                {
                    result = true;
                }
            }
            return result;
        }
        public Dictionary<string, string> GetLexicalDictionary()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            AddAlphabet(dictionary);
            AddOperation(dictionary);
            AddSymboles(dictionary);
            AddNumbers(dictionary);
            
            return dictionary;
        }
        private void AddAlphabet(Dictionary<string, string> parDictionary)
        {
            string[] str1 = {"A","B","C","D","E","F","G","H","I", "J", "K", "L",
                "M","N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            for (var i=0; i<str1.Length;i++)
            {
                parDictionary.Add(str1[i], str1[i]);
                parDictionary.Add(str1[i].ToLower(), str1[i].ToLower());
            }
            parDictionary.Add(" ", " ");
            
        }
        private void AddOperation(Dictionary<string, string> parDictionary)
        {
            string[] operationSymbols = { "-", "*", "/","+" };
            string[] operationNames = {"операция вычитания", "операция умножения", "операция деления", "операция сложения" };
            for(var i = 0; i < operationSymbols.Length; i++)
            {
                parDictionary.Add(operationSymbols[i], operationNames[i]);
            }
        }
        private void AddSymboles(Dictionary<string, string> parDictionary)
        {
            string[] symbolArray = { "(", ")", "_","." };
            string[] symbolNames = {"открывающая скобка", "закрывающая скобка","знак нижнее подчеркивание","точка" };
            for (var i = 0; i < symbolArray.Length; i++)
            {
                parDictionary.Add(symbolArray[i], symbolNames[i]);
            }
        }
        private void AddNumbers(Dictionary<string,string> parDictionary)
        {
            for(var i = 0; i<=9; i++)
            {
                parDictionary.Add(i.ToString(), i.ToString());
            }
        }
        private string ReplaceWhitespace(string input)
        {
            return Regex.Replace(input, @"\s+", "");
        }
        public string[] SplitExpresion()
        {
  
            return _expresion.Split(new char[] { '+','-','/','*','(',')',' '});
        }
        
        public List<List<string>> CreateListExpresionAlphabet()
        {
            string[] expresionSplited = SplitExpresion();
            List<List<string>> result = new List<List<string>>();
            List<string> correctVariableNameList = new List<string>();
            List<string> incorrectVariableNameList = new List<string>();
            List<string> correctDecimalConstantList = new List<string>();
            List<string> incorrectDecimalConstantList = new List<string>();
            List<string> integerConstantList = new List<string>();
            Regex constantDecimalRegex = new Regex("^(\\.|\\d)+\\.+(\\.|\\d)*$");
            Regex variableNameRegex = new Regex("^[_\\.a-zA-Z0-9]+$");
            Regex correctNameVariableRegex = new Regex("^[_a-zA-Z]");
            Regex correctConstantDecimalRegex = new Regex("^\\d+\\.{1}\\d+$");
            var id = 1;
            for(var i=0; i < expresionSplited.Length; i++)
            {
                var currentText = expresionSplited[i];
                if (string.IsNullOrEmpty(currentText))
                {
                    continue;
                }
                if (constantDecimalRegex.IsMatch(currentText))
                {
                    
                    if (correctConstantDecimalRegex.IsMatch(currentText))
                    {
                        correctDecimalConstantList.Add(currentText);
                       
                    }
                    else
                    {
                        incorrectDecimalConstantList.Add(currentText);
                        
                    }
                }
                if (int.TryParse(currentText,out var tempInt))
                {
                    integerConstantList.Add(currentText);
                    
                }
                if (variableNameRegex.IsMatch(currentText) && !constantDecimalRegex.IsMatch(currentText) &&
                    !int.TryParse(currentText, out var tempIntS))
                {
                    if (correctNameVariableRegex.IsMatch(currentText))
                    {
                        if (!correctVariableNameList.Contains(currentText))
                        {
                            correctVariableNameList.Add(currentText);
                            if (!dic.ContainsKey(currentText))
                            {
                                dic.Add(currentText, id);
                                id++;
                            }
                           
                        }
                    }
                    else
                    {
                        if (!incorrectVariableNameList.Contains(currentText))
                        {
                            incorrectVariableNameList.Add(currentText);
                        }
                        
                    }
                }
            }
            result.Add(incorrectVariableNameList);
            result.Add(incorrectDecimalConstantList);
            result.Add(correctVariableNameList);
            result.Add(correctDecimalConstantList);
            result.Add(integerConstantList);
            return result;
        }
       
        private bool AreInvalidIdentificatorOrDecimalConstant(List<List<string>> parExpressionCharactersList)
        {
            return (parExpressionCharactersList[0].Count != 0 || parExpressionCharactersList[1].Count != 0);   
        }
        
        private bool HasInvalidCharacterInExpresion(Dictionary<string, string> parDictionary)
        {
            bool result = false;
            for (var i = 0; i < _expresion.Length; i++)
            {
                if (!parDictionary.ContainsKey(_expresion.ElementAt(i).ToString()))
                {
                    Console.WriteLine(GetInvalidCharacterErrorMessage(_expresion.ElementAt(i).ToString()));
                    result = true;
                }
            }
            return result;
        }
        private string GetInvalidCharacterErrorMessage(string currentText)
        {
            return "Лексическая ошибка! Недопустимый символ " +"\""+currentText+"\""+" на позиции "+_expresion.IndexOf(currentText);
        }
        private void PrintInvaliIdentificatorDecimalConstantErrorMessage(List<List<string>> list)
        {
            Console.WriteLine(GetInvalidVariableNameErrorMessage(list[0]));
            Console.WriteLine(GetInvalidDecimalConstantErrorMessage(list[1]));
        }
        private string GetInvalidVariableNameErrorMessage(List<string> parIncorrectVariableNameList)
        {
            string result = "";
            Regex variableNameBeginWithNumberRegex = new Regex("^\\d+");
            for(var i=0; i < parIncorrectVariableNameList.Count; i++)
            {
                string currentIdentificator = parIncorrectVariableNameList[i];
                result += "Лексическая ошибка! Идентификатор <<"+ currentIdentificator + ">>";
                if (variableNameBeginWithNumberRegex.IsMatch(currentIdentificator))
                {
                    result += " не может начиаться с цифры ";
                } else if (currentIdentificator.Contains("."))
                {
                    result += " не может содержить символ точки ";
                }
                result += " на позиции " + _expresion.IndexOf(currentIdentificator)+"\n";
            }
            return result;
        }
        private string GetInvalidDecimalConstantErrorMessage(List<string> parIncorrectDecimalConstantList)
        {
            string result = "";
            for(var i=0; i < parIncorrectDecimalConstantList.Count; i++)
            {
                string currentDecimalConstant = parIncorrectDecimalConstantList[i];
                result += "Лексическая ошибка! неправильно задана константа <<"
                    + currentDecimalConstant + ">> на позиции "+_expresion.IndexOf(currentDecimalConstant)+"\n";
            }
            return result;
        }
        
        
    }
}
