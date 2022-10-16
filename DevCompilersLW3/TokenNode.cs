using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW3
{
    public class TokenNode
    {
        public TokenNode _leftNode { get; set; }
        public TokenNode _rightNode { get; set; }
        public string value { get; set; }
        public TokenNode(string parValue, TokenNode parLeftNode, TokenNode parRightNode)
        {
            _leftNode = parLeftNode;
            _rightNode = parRightNode;
            this.value = parValue;
        }
        public TokenNode(string parValue)
        {
            value = parValue;
            _leftNode = null;
            _rightNode = null;
        }
        public TokenNode()
        {

        }
    }
}
