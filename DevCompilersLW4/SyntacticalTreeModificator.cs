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
                return GetOperatorResultType(parNode);
            }
            
        }
        private TokenType GetOperatorResultType(TokenNode<Token> parNode)
        {
            TokenNode<Token> leftNode = parNode.LeftNode;
            TokenNode<Token> rightNode = parNode.RightNode;
            if(TokenNode<Token>.IsLeafToken(leftNode) && TokenNode<Token>.IsLeafToken(rightNode))
            {
                if (!TokenWorker.IsTokenOperandEqualType(leftNode.Value.TokenType, rightNode.Value.TokenType))
                {
                    if (TokenWorker.IsTokenOperandDecimalType(leftNode.Value.TokenType))
                    {
                        TokenNode<Token> temp = rightNode.Clone() as TokenNode<Token>;
                        rightNode.Value = new Token(TokenType.INT_2_FLOAT, temp.Value.Lexeme, temp.Value.AttributeValue);
                        rightNode.LeftNode = temp;
                        rightNode.RightNode = null;
                    }
                    else 
                    {
                        TokenNode<Token> temp = leftNode.Clone() as TokenNode<Token>;
                        leftNode.Value = new Token(TokenType.INT_2_FLOAT, temp.Value.Lexeme, temp.Value.AttributeValue);
                        leftNode.LeftNode = temp;
                        leftNode.RightNode = null;
                        
                    }
                }
                    return leftNode.Value.TokenType;
            }
            else
            {
                if (TokenNode<Token>.IsLeafToken(leftNode))
                {
                    return GetOperatorResultType(rightNode);
                }else if (TokenNode<Token>.IsLeafToken(rightNode))
                {
                    return GetOperatorResultType(leftNode);
                }
                else
                {
                    return CompareTokenType(parNode);
                }
            }
        }
        private TokenType CompareTokenType(TokenNode<Token> parNode)
        {
            TokenType leftType = GetTokenNodeType(parNode.LeftNode);
            TokenType rightType = GetTokenNodeType(parNode.RightNode);
            if (!TokenWorker.IsTokenOperandEqualType(leftType, rightType))
            {
     
                if (TokenWorker.IsTokenOperandDecimalType(leftType))
                {
                    TokenNode<Token> temp = parNode.RightNode.Clone() as TokenNode<Token>;
                    parNode.RightNode = new TokenNode<Token>(new Token(TokenType.INT_2_FLOAT, temp.Value.Lexeme, temp.Value.AttributeValue), temp);
                }
                else if(TokenWorker.IsTokenOperandDecimalType(rightType))
                {
                    TokenNode<Token> temp = parNode.LeftNode.Clone() as TokenNode<Token>;
                    parNode.LeftNode = new TokenNode<Token>(new Token(TokenType.INT_2_FLOAT, temp.Value.Lexeme, temp.Value.AttributeValue), temp);
                }
            }
                return leftType;
        }
        public void RealizeSyntaxTreeModification()
        {
            CompareTokenType(SyntaxTreeModified);
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
