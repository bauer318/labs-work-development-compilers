using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW3
{
    public class ASTLeaf : AST
    {
        public readonly decimal _num;
        public ASTLeaf(decimal num)
        {
            this._num = num;
        }
        public override decimal Eval()
        {
            return this._num;
        }

        public override string ToString()
        {
            return this._num.ToString();
        }
    }
}
