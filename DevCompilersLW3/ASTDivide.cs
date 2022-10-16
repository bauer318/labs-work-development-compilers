using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW3
{
    public class ASTDivide : AST
    {
        //public  AST _leftNode { get; }
        //public  AST _rightNode { get; }

        public ASTDivide(AST leftNode, AST rightNode):base("/",leftNode,rightNode)
        {  
        }
        public override decimal Eval()
        {
            return this._leftNode.Eval() / this._rightNode.Eval();
        }

        public override string ToString()
        {
            return String.Format("({0} / {1})", this._leftNode.ToString(), this._rightNode.ToString());
        }
    }
}
