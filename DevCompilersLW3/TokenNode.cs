using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW3
{
    public class TokenNode<Token>:ICloneable
    {
        public TokenNode<Token> LeftNode { get; set; }
        public TokenNode<Token> RightNode { get; set; }
        public TokenNode<Token> ConvertedTokenNode { get; set; }
        public Token Value { get; set; }
        public TokenNode(Token parValue, TokenNode<Token> parLeftNode, TokenNode<Token> parRightNode)
        {
            LeftNode = parLeftNode;
            RightNode = parRightNode;
            Value = parValue;
            ConvertedTokenNode = null;
        }
        public TokenNode(Token parValue)
        {
            Value = parValue;
            LeftNode = null;
            RightNode = null;
            ConvertedTokenNode = null;
        }
        /// <summary>
        /// Copy's contructor
        /// </summary>
        /// <param name="parTokenNode"></param>
        public TokenNode(TokenNode<Token> parTokenNode)
        {
            this.Value = parTokenNode.Value;
            this.LeftNode = parTokenNode.LeftNode;
            this.RightNode = parTokenNode.RightNode;
            this.ConvertedTokenNode = parTokenNode.ConvertedTokenNode;
        }

        /*public TokenNode<Token> Clone()
        {
            return new TokenNode<Token>(this);
        }*/

        public static bool IsTokenNodeOperator(TokenNode<Token> parTokenNode)
        {
            return parTokenNode.LeftNode != null && parTokenNode.RightNode !=null;
        }
        public static bool IsLeafToken(TokenNode<Token> parToken)
        {
            return (parToken.LeftNode == null && parToken.RightNode == null)|
                IsConvertedToken(parToken);
        }
        public static bool IsConvertedToken(TokenNode<Token> parTokenNode)
        {
            return parTokenNode.LeftNode != null && parTokenNode.RightNode == null;
        }

        public object Clone()
        {
            return new TokenNode<Token>(this.Value, this.LeftNode, this.RightNode);
        }
    }
}
