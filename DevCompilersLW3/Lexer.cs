using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW3
{
    public class Lexer
    {
        private readonly List<Tokens> tokens;
        private readonly string _input;
        private Int32 pos = 0;
        private char curr_input;
        // LIST CHECKER
        private List<char> NumberList = new List<char> { '.', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        public Lexer(string input)
        {
            this._input = input;
            tokens = new List<Tokens>();
            this.curr_input = input.Length > 0 ? this._input[pos] : '\0'; // set first char

        }
        private void Get_Next()
        {
            if (pos < this._input.Length - 1)
            {
                pos++;
                this.curr_input = this._input[pos];

            }
            else
            {
                curr_input = '\0';
            }
        }
        public List<Tokens> Get_Tokens()
        {

            while (true)
            {
                if (curr_input == ' ' || curr_input == '\t')
                {
                    // Skip empty space
                    Get_Next();
                    continue;
                }
                else if (NumberList.Contains(curr_input))
                {
                    Tokens numberToken = Generate_Number();
                    tokens.Add(numberToken);
                }
                else if (curr_input == '+')
                {
                    Tokens additionToken = new Tokens(TokenLab03.ADD, null);
                    tokens.Add(additionToken);
                    Get_Next();
                }
                else if (curr_input == '-')
                {
                    Tokens minusToken = new Tokens(TokenLab03.MINUS, null);
                    tokens.Add(minusToken);
                    Get_Next();
                }
                else if (curr_input == '*')
                {
                    Tokens multiplyToken = new Tokens(TokenLab03.MULTIPLY, null);
                    tokens.Add(multiplyToken);
                    Get_Next();
                }
                else if (curr_input == '/')
                {
                    Tokens divideToken = new Tokens(TokenLab03.DIVISION, null);
                    tokens.Add(divideToken);
                    Get_Next();
                }
                else if (curr_input == '(')
                {
                    Tokens lbraceToken = new Tokens(TokenLab03.LBRACE, null);
                    tokens.Add(lbraceToken);
                    Get_Next();
                }
                else if (curr_input == ')')
                {
                    Tokens rbraceToken = new Tokens(TokenLab03.RBRACE, null);
                    tokens.Add(rbraceToken);
                    Get_Next();
                }
                else if (curr_input == '\0')
                {
                    Tokens eofToken = new Tokens(TokenLab03.EOF, null);
                    tokens.Add(eofToken);
                    break;
                }
            }

            return tokens;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var token in tokens)
            {
                sb.Append(token.ToString());
            }

            return sb.ToString();
        }
        private Tokens Generate_Number()
        {
            int decimal_count = 0;
            StringBuilder sb = new StringBuilder();
            while (NumberList.Contains(curr_input))
            {
                if (curr_input == '.' && decimal_count <= 1)
                {
                    decimal_count++;
                }

                if (sb.Length < 1 && decimal_count > 0)
                {
                    // You have a decimal place starting
                    // with no preceding number i.e .6767 = 0.6767
                    sb.Append("0");
                }
                sb.Append(curr_input);
                Get_Next();
            }

            string str = sb.ToString();
            decimal val;
            if (decimal.TryParse(str, out val))
            {
                ;
            }
            return new Tokens(TokenLab03.NUMBER, val);
        }
    }

}
