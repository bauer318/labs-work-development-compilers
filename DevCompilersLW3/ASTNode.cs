using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW3
{
    public class ASTNode
    {
        private Object _value;
        private ASTNode _left;
        private ASTNode _right;
        public Object Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }
        public ASTNode Left
        {
            get
            {
                return _left;
            }
            set
            {
                _left = value;
            }
        }
        public ASTNode Right
        {
            get
            {
                return _right;
            }
            set
            {
                _right = value;
            }
        }
        public ASTNode(Object parValue)
        {
            _value = parValue;
        }
    }
}
