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
        private int countDifferentsOperandType = 0;
        public SyntacticalTreeModificator(TokenNode<Token> parAbstractSyntaxTree, SymbolTable parSymbolTable)
        {
            SyntaxTreeModified = parAbstractSyntaxTree;
            _symbolTable = parSymbolTable;
        }
        
        public void RealizeSyntaxTreeModification()
        {
            RealizeVerificationDifferentType();
            if (countDifferentsOperandType > 0)
            {
                ModifieSyntaxtTree(SyntaxTreeModified.LeftNode, SyntaxTreeModified.RightNode);
                RealizeSyntaxTreeModification();
            }
        }
   
        private void RealizeVerificationDifferentType()
        {
            countDifferentsOperandType = 0;
            CheckDifferentOperandType(SyntaxTreeModified.LeftNode, SyntaxTreeModified.RightNode);
        }
        private TokenNode<Token> CheckDifferentOperandType(TokenNode<Token> parLeftNode, TokenNode<Token> parRightNode)
        {
            TokenNode<Token> result = null;
            if(TokenNode<Token>.IsLeafToken(parLeftNode) && TokenNode<Token>.IsLeafToken(parRightNode))
            {
                if(!TokenWorker.IsTokenOperandEqualType(parLeftNode.Value.TokenType, parRightNode.Value.TokenType))
                {
                    countDifferentsOperandType++;
                }
                return parLeftNode;
            }
            else
            {
                if (TokenNode<Token>.IsLeafToken(parLeftNode))
                {
                    result = CheckDifferentOperandType(parLeftNode, CheckDifferentOperandType(parRightNode.LeftNode, parRightNode.RightNode));
                }
                else if(TokenNode<Token>.IsLeafToken(parRightNode))
                {
                    result = CheckDifferentOperandType(parRightNode, CheckDifferentOperandType(parLeftNode.LeftNode, parLeftNode.RightNode));
                }
            }
            return result;
        }
        private TokenNode<Token> ModifieSyntaxtTree(TokenNode<Token> parLeftNode, TokenNode<Token> parRightNode)
        { 
            TokenNode<Token> nextComp = null;
             if(TokenNode<Token>.IsLeafToken(parLeftNode) && TokenNode<Token>.IsLeafToken(parRightNode))
            {
                if (!TokenWorker.IsTokenOperandEqualType(parLeftNode.Value.TokenType,parRightNode.Value.TokenType))
                {
                    if (TokenWorker.IsTokenOperandDecimalType(parLeftNode.Value.TokenType))
                    {
                        //Int2Float for right node
                        TokenNode<Token> temp = parRightNode.Clone() as TokenNode<Token>;
                        TokenNode<Token> convertedTokenNode = new TokenNode<Token>(new Token(temp.Value.TokenType, temp.Value.Lexeme, temp.Value.AttributeValue));
                        parRightNode.Value.TokenType = parLeftNode.Value.TokenType;
                        parRightNode.Value = new Token(TokenType.INT_2_FLOAT, convertedTokenNode.Value.Lexeme,convertedTokenNode.Value.AttributeValue);
                        parRightNode.ConvertedTokenNode = convertedTokenNode;
                        return parRightNode;
                        
                        
                    }
                    else
                    {
                        //Int2Float for left node
                        TokenNode<Token> temp = parLeftNode.Clone() as TokenNode<Token>;
                        TokenNode<Token> convertedTokenNode = new TokenNode<Token>(new Token(temp.Value.TokenType, temp.Value.Lexeme,temp.Value.AttributeValue));
                        parLeftNode.Value.TokenType = parRightNode.Value.TokenType;
                        parLeftNode.Value = new Token(TokenType.INT_2_FLOAT, convertedTokenNode.Value.Lexeme, convertedTokenNode.Value.AttributeValue);
                        parLeftNode.ConvertedTokenNode = convertedTokenNode;
                        return parLeftNode;       
                    }
                }
                else
                {
                    return parLeftNode;
                }
                
            }
            else
            {
                if (TokenNode<Token>.IsLeafToken(parLeftNode))
                {
                    nextComp = ModifieSyntaxtTree(parLeftNode,ModifieSyntaxtTree(parRightNode.LeftNode, parRightNode.RightNode));
                }
                else if (TokenNode<Token>.IsLeafToken(parRightNode))
                {
                    nextComp =ModifieSyntaxtTree(ModifieSyntaxtTree(parLeftNode.LeftNode, parLeftNode.RightNode), parRightNode);
                }
            }
            return nextComp;
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
                if (parAbstractSyntaxTree.Value.TokenType == TokenType.INT_2_FLOAT)
                {
                    parPointer = "     |---";
                    _astTexts.Add(paddingForBoth.Remove(0,5)+ parPointer + 
                    parAbstractSyntaxTree.ConvertedTokenNode.Value.TokenType.GetTokenNodeDescription(parAbstractSyntaxTree.ConvertedTokenNode.Value));
                }
                TraverserPreOrder(paddingForBoth, pointerForLeft, parAbstractSyntaxTree.LeftNode);
                TraverserPreOrder(paddingForBoth, pointerForRight, parAbstractSyntaxTree.RightNode); ;
            }
        }

    }
}
