using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW3
{
    public class TokenNode<Token> : ICloneable
    {
        public TokenNode<Token> LeftNode { get; set; }
        public TokenNode<Token> RightNode { get; set; }

        public Token Value { get; set; }
        public TokenNode(Token parValue, TokenNode<Token> parLeftNode, TokenNode<Token> parRightNode)
        {
            LeftNode = parLeftNode;
            RightNode = parRightNode;
            Value = parValue;
        }
        public TokenNode(Token parValue)
        {
            Value = parValue;
            LeftNode = null;
            RightNode = null;
        }
        public static bool IsLeafToken(TokenNode<Token> parToken)
        {
            if (parToken == null)
            {
                return false;
            }
            return parToken.LeftNode == null && parToken.RightNode == null;

        }
        public object Clone()
        {
            return new TokenNode<Token>(this.Value, this.LeftNode, this.RightNode);
        }
    }
}
