using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW3
{
    public class Tokens
    {
        public readonly Token _tokenType;
        public readonly object _value;

        public Tokens(Token tokenType, object value)
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
