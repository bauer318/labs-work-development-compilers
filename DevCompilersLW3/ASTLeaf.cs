using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW3
{
    public class ASTLeaf : TokenNode
    {
        public Node<String> last { get; set; }
        
        public ASTLeaf(string parValue)
        {
            last = new Node<string>(parValue);
        }

    }
}
