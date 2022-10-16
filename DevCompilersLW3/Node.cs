using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW3
{
    public class Node<String>:AST
    {
        public Node(string parOperator,AST parLeft, AST parRight):base(parOperator, parLeft,parRight)
        {
           
        }
        public Node(string parOperator)
        {
            Operator = parOperator;
            
        }
        public static bool isLeaf(AST parAST)
        {
            return parAST.GetType()==typeof(ASTLeaf);
        }

        public override decimal Eval()
        {
            throw new NotImplementedException();
        }
    }
}
