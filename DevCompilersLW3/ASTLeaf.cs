using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW3
{
    public class ASTLeaf : AST
    {
        public decimal _num { get; }

        public Node<String> last { get; set; }
        public ASTLeaf(decimal num):base(num)
        {
            this._num = num;
            last = new Node<string>(num.ToString());
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
