using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DevCompilersLW2
{
    public class TokenDefinition
    {
        private Regex _regex;
        private readonly TokenType _returnsToken;
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
                return new TokenMatch()
                {
                    IsMatch = true,
                    TokenType = _returnsToken,
                    Lexeme = match.Value
                };
            }
            else
            {
                return new TokenMatch() { IsMatch = false };
            }

        }
    }
    
}
