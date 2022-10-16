using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW3
{
    public class Tokens
    {
        public readonly TokenLab03 _tokenType;
        public readonly object _value;

        public Tokens(TokenLab03 tokenType, object value)
        {
            this._tokenType = tokenType;
            this._value = value;
        }


        public override string ToString()
        {
            return " " + this._tokenType + ":" + this._value;
        }
    }

}
