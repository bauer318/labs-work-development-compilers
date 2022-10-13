using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW3
{
    public class ASTMultiply : AST
    {
        public readonly AST _leftNode;
        public readonly AST _rightNode;

        public ASTMultiply(AST leftNode, AST rightNode)
        {
            this._leftNode = leftNode;
            this._rightNode = rightNode;
        }
        public override AST GetLeftNode()
        {
            return _leftNode;
        }
        public override AST GetRightNode()
        {
            return _rightNode;
        }
        public override decimal Eval()
        {
            return this._leftNode.Eval() * this._rightNode.Eval();
        }

        public override string GetNodeHead()
        {
            return "<*>";
        }

        public override string ToString()
        {
            return String.Format("({0} * {1})", this._leftNode.ToString(), this._rightNode.ToString());
        }
    }
}
