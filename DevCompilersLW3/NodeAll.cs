using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW3
{
    public class NodeAll : TokenNode
    {
        public NodeAll(string parValue, TokenNode parLeftNode, TokenNode parRightNode) : base(parValue, parLeftNode, parRightNode)
        {

        }
        public NodeAll(string parValue):base(parValue)
        {

        }
    }
}
