using DevCompilersLW2;
using DevCompilersLW3;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW4
{
    public class SyntacticalTreeModificator
    {
        public TokenNode<Token> SyntaxTreeModified { get; }
        private SymbolTable _symbolTable;
        private List<string> _astTexts = new List<string>();
        private int whiteSpaceCount = 0;
        public SyntacticalTreeModificator(TokenNode<Token> parAbstractSyntaxTree, SymbolTable parSymbolTable)
        {
            SyntaxTreeModified = parAbstractSyntaxTree;
            _symbolTable = parSymbolTable;
        }
        private TokenType GetTokenNodeType(TokenNode<Token> parNode)
        {
            if (TokenNode<Token>.IsLeafToken(parNode))
            {
                return parNode.Value.TokenType;
            }
            else
            {
                return GetTokenOperatorResultType(parNode.LeftNode, parNode.RightNode).Value.TokenType;
            }
            
        }
        private TokenNode<Token> GetTokenOperatorResultType(TokenNode<Token> parLeftNode, TokenNode<Token> parRightNode)
        {
            TokenNode<Token> result = new TokenNode<Token>(new Token(TokenType.INVALID, "CONVERTED"));
            if (TokenNode<Token>.IsLeafToken(parLeftNode) && TokenNode<Token>.IsLeafToken(parRightNode))
            {
                if(!TokenWorker.IsTokenOperandEqualType(parLeftNode.Value.TokenType, parRightNode.Value.TokenType))
                { 
                    result = new TokenNode<Token>(new Token(TokenType.INT_2_FLOAT, "CONVERTED"));
                }
                else
                {
                    result = parLeftNode;
                }
            }
            else
            {
                if (TokenNode<Token>.IsLeafToken(parLeftNode))
                {
                    result = GetTokenOperatorResultType(parLeftNode, GetTokenOperatorResultType(parRightNode.LeftNode, parRightNode.RightNode));
                }
                else if (TokenNode<Token>.IsLeafToken(parRightNode))
                {
                    result = GetTokenOperatorResultType(GetTokenOperatorResultType(parLeftNode.LeftNode, parLeftNode.RightNode), parRightNode);
                    
                }
                else
                {
                    result = GetTokenOperatorResultType(GetTokenOperatorResultType(parLeftNode.LeftNode, parLeftNode.RightNode),
                        GetTokenOperatorResultType(parRightNode.LeftNode, parRightNode.RightNode));
                }
            }

            return result;
        }
        public void RealizeTopBottomSyntaxTreeModification()
        {
            TopBottomModifieSyntaxTree(SyntaxTreeModified);
        }
        private void TopBottomModifieSyntaxTree(TokenNode<Token> parNode)
        {
            if(parNode != null)
            {
                TokenType leftType = GetTokenNodeType(parNode.LeftNode);
                TokenType rightType = GetTokenNodeType(parNode.RightNode);
                TokenNode<Token> leftNodeCloned = parNode.LeftNode.Clone() as TokenNode<Token>;
                TokenNode<Token> rightNodeCloned = parNode.RightNode.Clone() as TokenNode<Token>;
                if (!TokenWorker.IsTokenOperandEqualType(leftType, rightType))
                {
                    if (TokenWorker.IsTokenOperandDecimalType(leftType))
                    {
                        parNode.RightNode.Value = new Token(TokenType.INT_2_FLOAT, rightNodeCloned.Value.Lexeme, rightNodeCloned.Value.AttributeValue);
                        parNode.RightNode.LeftNode = rightNodeCloned;
                        parNode.RightNode.RightNode = null;
                    }
                    else
                    {
                        parNode.LeftNode.Value = new Token(TokenType.INT_2_FLOAT, leftNodeCloned.Value.Lexeme, leftNodeCloned.Value.AttributeValue);
                        parNode.LeftNode.LeftNode = leftNodeCloned;
                        parNode.LeftNode.RightNode = null;
                    }
                }
                if (!TokenNode<Token>.IsLeafToken(leftNodeCloned))
                {
                    TopBottomModifieSyntaxTree(leftNodeCloned);
                }
                if (!TokenNode<Token>.IsLeafToken(rightNodeCloned))
                {
                    TopBottomModifieSyntaxTree(rightNodeCloned);
                }
                
            }
        }
        public List<string> GetSemanticTreeTextList()
        {
            TraverserPreOrder("", "", SyntaxTreeModified);
            return _astTexts;
        }
        private void TraverserPreOrder(string parPadding, string parPointer, TokenNode<Token> parAbstractSyntaxTree)
        {
            if (parAbstractSyntaxTree != null)
            {
                _astTexts.Add(parPadding + parPointer + parAbstractSyntaxTree.Value.TokenType.GetTokenNodeDescription(parAbstractSyntaxTree.Value));
                StringBuilder paddingBuilder = new StringBuilder(parPadding);
                if (whiteSpaceCount > 0)
                {
                    paddingBuilder.Append("     ");
                }
                whiteSpaceCount++;
                string paddingForBoth = paddingBuilder.ToString();
                string pointerForRight = " |---";
                string pointerForLeft = " |---";
                TraverserPreOrder(paddingForBoth, pointerForLeft, parAbstractSyntaxTree.LeftNode);
                TraverserPreOrder(paddingForBoth, pointerForRight, parAbstractSyntaxTree.RightNode); ;
            }
        }

    }
}
