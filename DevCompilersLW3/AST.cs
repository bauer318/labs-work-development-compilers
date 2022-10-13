using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW3
{
    public abstract class AST
    {
        public abstract decimal Eval();
        public abstract string GetNodeHead();
        public abstract AST GetLeftNode();
        public abstract AST GetRightNode();
    }
}
