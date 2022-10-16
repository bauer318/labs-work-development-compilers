using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW3
{
    public class Tree<Token> : List<Tree<Token>>
    {
        public Token Node { get; set; }
        public Tree<Token> Parent { get; set; }
        public new void Add(Tree<Token> item)
        {
            if(item==null)
            {
                return;
            }
            base.Add(item);
            item.Parent = this;
        }
    }
}
