using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DevCompilersLW2
{
    public class LexicalyErrorAnalyzer
    {
        private List<TokenDefinition> _tokenDefinitions;
        public readonly List<Token> Tokens = new List<Token>();
        public readonly List<SymbolTable> SymbolTables = new List<SymbolTable>();
        public LexicalyErrorAnalyzer()
        {
            _tokenDefinitions = new List<TokenDefinition>();
            _tokenDefinitions.Add(new TokenDefinition(TokenType.AdditionSign, "^\\+$"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.SoustractionSign, "^\\-$"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.MultiplicationSign, "^\\*$"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.DivisionSign, "^\\/$"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.OpenParenthesis, "^\\($"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.CloseParenthesis, "^\\)$"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.IntegerConstant, "^[0-9]+$"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.IncorrectDecimalConstant, "^[0-9.]*\\.*\\..*\\..*[0-9.]*$"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.CorrectDecimalConstant, "^\\d+\\.{1}\\d+$"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.IncorrectIdentificator, "^[0-9]+[_a-zA-Z0-9]+$"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.CorrectIdentificator, "^[_a-zA-Z]+[0-9]*$"));

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
            for (var i = 0; i < expresionSplited.Length; i++)
            {
                var currentText = expresionSplited[i];
                var match = FindMatch(currentText);
                var currentPosition = parExpresion.IndexOf(currentText);
                switch (match.TokenType)
                {
                    case TokenType.IncorrectDecimalConstant:
                        Console.WriteLine("Лексическая ошибка! неправильно задана константа <<" + currentText +
                            ">> на позиции " + currentPosition);
                        result = false;
                        break;
                    case TokenType.IncorrectIdentificator:
                        Console.WriteLine("Лексическая ошибка! Идентификатор <<" + currentText +
                            ">> не может начиаться с цифры на позиции " + currentPosition);
                        result = false;
                        break;
                    case TokenType.Invalid:
                        if (!string.IsNullOrEmpty(currentText))
                        {
                            Console.WriteLine("Лексическая ошибка! Недопустимый символ " + "\"" + currentText + "\"" + " на позиции "
                                + currentPosition);
                            result = false;
                        }
                        break;
                    case TokenType.CorrectIdentificator:
                        if (!tokenLexemes.Contains(match.Lexeme))
                        {
                            attributeValue++;
                            tokenLexemes.Add(match.Lexeme);
                            SymbolTables.Add(new SymbolTable(new Token(match.TokenType, match.Lexeme, attributeValue)));
                        }
                        Tokens.Add(new Token(match.TokenType, match.Lexeme, attributeValue));
                        break;
                    default:
                        Tokens.Add(new Token(match.TokenType, match.Lexeme));
                        break;
                }
            }
            return result;
        }

        private TokenMatch FindMatch(string lqlText)
        {
            foreach (var tokenDefinition in _tokenDefinitions)
            {
                var match = tokenDefinition.Match(lqlText);
                if (match.IsMatch)
                    return match;
            }

            return new TokenMatch() { IsMatch = false };
        }

        private bool IsWhitespace(string lqlText)
        {
            return Regex.IsMatch(lqlText, "^\\s+");
        }

        private TokenMatch CreateInvalidTokenMatch(string lqlText)
        {
            var match = Regex.Match(lqlText, "(^\\S+\\s)|^\\S+");
            if (match.Success)
            {
                return new TokenMatch()
                {
                    IsMatch = true,
                    TokenType = TokenType.Invalid,
                    Lexeme = match.Value.Trim()
                };
            }

            throw new DslParserException("Failed to generate invalid Tokens");
        }
    }
}
