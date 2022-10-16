using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW3
{
    public class ASTMinus : AST
    {
        
        public ASTMinus(AST leftNode, AST rightNode) : base("-",leftNode, rightNode)
        {
        }
        public override decimal Eval()
        {
            return this._leftNode.Eval() - this._rightNode.Eval();
        }
        public override string ToString()
        {
            return String.Format("({0} - {1})", this._leftNode.ToString(), this._rightNode.ToString());
        }
    }
}
