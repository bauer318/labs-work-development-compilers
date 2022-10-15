using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DevCompilersLW2
{
    public class LexicalErrorAnalyzer
    {
        private List<TokenDefinition> _tokenDefinitions;
        public readonly List<Token> Tokens = new List<Token>();
        public SymbolTable SymbolTable;
        public LexicalErrorAnalyzer()
        {
            _tokenDefinitions = new List<TokenDefinition>();
            _tokenDefinitions.Add(new TokenDefinition(TokenType.ADDITION_SIGN, "^\\+$"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.SOUSTRACTION_SIGN, "^\\-$"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.MULTIPLICATION_SIGN, "^\\*$"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.DIVISION_SIGN, "^\\/$"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.OPEN_PARENTHESIS, "^\\($"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.CLOSE_PARENTHESIS, "^\\)$"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.INTEGER_CONSTANT, "^[0-9]+$"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.INCORRECT_DECIMAL_CONSTANT, "^[0-9.]*\\.*\\..*\\..*[0-9.]*$"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.CORRECT_DECIMAL_CONSTANT, "^\\d+\\.{1}\\d+$"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.INCORRECT_IDENTIFICATOR, "^[0-9]+[_a-zA-Z0-9]+$"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.CORRECT_IDENTIFICATOR, "^[_a-zA-Z]+[0-9]*$"));

        }
        public string[] SplitExpresion(string parExpresion)
        {
            return PutWhitespace(RemoveAllWhitespace(parExpresion)).Split(new char[] { ' ' });
        }
        public string RemoveAllWhitespace(string parExpresion)
        {
            return Regex.Replace(parExpresion, @"\s+", "");
        }
        public string PutWhitespace(string parExpresion)
        {
            string result = "";
            for (var i = 0; i < parExpresion.Length; i++)
            {
                string currentText = parExpresion.ElementAt(i).ToString();
                if (IsExpresionSeparator(currentText))
                {
                    result += " " + currentText + " ";
                }
                else
                {
                    result += currentText;
                }
            }
            return result.Trim();
        }
        public Dictionary<string, string> GetLexicalDictionary()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            AddAlphabet(dictionary);
            AddNumbers(dictionary);
            return dictionary;
        }
        private void AddAlphabet(Dictionary<string, string> parDictionary)
        {
            string[] str1 = {"A","B","C","D","E","F","G","H","I", "J", "K", "L",
                "M","N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            for (var i = 0; i < str1.Length; i++)
            {
                parDictionary.Add(str1[i], str1[i]);
                parDictionary.Add(str1[i].ToLower(), str1[i].ToLower());
            }
            parDictionary.Add(".", ".");
            parDictionary.Add("_", "_");

        }
        private void AddNumbers(Dictionary<string, string> parDictionary)
        {
            for (var i = 0; i <= 9; i++)
            {
                parDictionary.Add(i.ToString(), i.ToString());
            }
        }
        private bool IsExpresionSeparator(string parText)
        {
            string[] separatorSymbolArray = { "+", "/", "*", "-", ")", "(" };
            Dictionary<string, string> dictionary = GetLexicalDictionary();
            bool result = false;
            for (var i = 0; i < separatorSymbolArray.Length; i++)
            {
                if (parText.Equals(separatorSymbolArray[i]) || !dictionary.ContainsKey(parText))
                {
                    result = true;
                }
            }
            return result;
        }
        public bool IsLexicalyCorrectExpresion(string parExpresion)
        {
            bool result = true;
            string[] expresionSplited = SplitExpresion(parExpresion);
            var attributeValue = 0;
            List<string> tokenLexemes = new List<string>();
            List<AttributeVariable> attributeVariables = new List<AttributeVariable>();
            for (var i = 0; i < expresionSplited.Length; i++)
            {
                var currentText = expresionSplited[i];
                var match = FindMatch(currentText);
                switch (match.TokenType)
                {
                    case TokenType.INCORRECT_DECIMAL_CONSTANT:
                    case TokenType.INCORRECT_IDENTIFICATOR:
                        Console.WriteLine(match.TokenType.GetIncorrectTokenTypeDescrition(currentText,parExpresion));
                        result = false;
                        break;
                    case TokenType.INVALID:
                        if (!string.IsNullOrEmpty(currentText))
                        {
                            Console.WriteLine(match.TokenType.GetIncorrectTokenTypeDescrition(currentText, parExpresion));
                            result = false;
                        }
                        break;
                    case TokenType.CORRECT_IDENTIFICATOR:
                        if (!tokenLexemes.Contains(match.Lexeme))
                        {
                            attributeValue++;
                            tokenLexemes.Add(match.Lexeme);
                            attributeVariables.Add(new AttributeVariable(attributeValue, match.Lexeme));
                        }
                        Tokens.Add(new Token(match.TokenType, match.Lexeme, attributeValue));
                        break;
                    default:
                        Tokens.Add(new Token(match.TokenType, match.Lexeme));
                        break;
                }
            }
            SymbolTable = new SymbolTable(attributeVariables);
            return result;
        }

        private TokenMatch FindMatch(string parExpresionElement)
        {
            foreach (var tokenDefinition in _tokenDefinitions)
            {
                var match = tokenDefinition.Match(parExpresionElement);
                if (match.IsMatch)
                {
                    return match;
                }
            }
            return new TokenMatch() { IsMatch = false };
        }
    }
}
