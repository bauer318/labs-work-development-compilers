using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DevCompilersLW2
{
    public class TokenDefinition
    {
        private Regex _regex;
        private TokenType _returnsToken;
        public TokenDefinition(TokenType returnsToken, string regexPattern)
        {
            _regex = new Regex(regexPattern, RegexOptions.IgnoreCase);
            _returnsToken = returnsToken;
        }
        public TokenMatch Match(string inputString)
        {
            var match = _regex.Match(inputString);
            if (match.Success)
            {
                var bracketIndex = match.Value.IndexOf('[');
                var lex = match.Value;
                if (bracketIndex != -1)
                {
                    lex = lex.Remove(bracketIndex);
                }
                return new TokenMatch()
                {
                    IsMatch = true,
                    TokenType = _returnsToken,
                    Lexeme = lex
                };
            }
            else
            {
                return new TokenMatch() { IsMatch = false };
            }

        }
    }
    
}
