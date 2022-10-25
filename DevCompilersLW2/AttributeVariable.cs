using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW2
{
    public class AttributeVariable
    {
        private int _id;
        private string _name;
        private TokenType _tokenType;
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        public TokenType TokenType
        {
            get
            {
                return _tokenType;
            }
            set
            {
                _tokenType = value;
            }
        }
        public AttributeVariable(int parId, string parName)
        {
            _id = parId;
            _name = parName;
        }
        public AttributeVariable(int parId, string parName, TokenType parTokenType):this(parId,parName)
        {
            _tokenType = parTokenType;
        }
    }
}
