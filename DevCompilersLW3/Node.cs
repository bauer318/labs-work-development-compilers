using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW3
{
    public class Node<String>:TokenNode
    {
        public Node(string parOperator,TokenNode parLeft, TokenNode parRight):base(parOperator, parLeft,parRight)
        {
           
        }
        public Node(string parOperator)
        {
            value = parOperator;
            
        }
        public static bool isLeaf(TokenNode parAST)
        {
            return parAST.GetType()==typeof(ASTLeaf);
        }
    }
}
