using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW3
{
    public abstract class AST
    {
        public AST _leftNode { get; set; }
        public AST _rightNode { get; set; }
        public string Operator { get; set; }
        public abstract decimal Eval();
        public AST(string parOperator, AST parLeftNode, AST parRightNode)
        {
            _leftNode = parLeftNode;
            _rightNode = parRightNode;
            Operator = parOperator;
        }
        public AST(decimal num)
        {
            _leftNode = null;
            _rightNode = null;
        }
        public AST()
        {

        }
    }
}
