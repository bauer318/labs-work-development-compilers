using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW3
{
    public class ASTPlus : AST
    {
        public ASTPlus(AST leftNode, AST rightNode) : base("+",leftNode, rightNode)
        {
        }
        public override decimal Eval()
        {
            // Perform the evaluation
            return this._leftNode.Eval() + this._rightNode.Eval();
        }

        public override string ToString()
        {
            return String.Format("({0} + {1})", this._leftNode.ToString(), this._rightNode.ToString());
        }
    }
}
