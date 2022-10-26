﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW3
{
    public class TokenNode<Token>
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
        public TokenNode(Token parValue, TokenNode<Token> parConvertedTokenNode)
        {
            Value = parValue;
            LeftNode = parConvertedTokenNode;
            RightNode = null;
        }

        public static bool IsTokenNodeOperator(TokenNode<Token> parTokenNode)
        {
            return parTokenNode.LeftNode != null && parTokenNode.RightNode !=null;
        }
        public static bool IsLeafToken(TokenNode<Token> parToken)
        {
            return parToken.LeftNode == null && parToken.RightNode == null;
        }
        public static bool IsConvertedToken(TokenNode<Token> parTokenNode)
        {
            return parTokenNode.LeftNode != null && parTokenNode.RightNode == null;
        }
    }
}
