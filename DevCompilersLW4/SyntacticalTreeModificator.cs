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
        int t = 0;
        public void Comp(TokenNode<Token> n)
        {
            Console.WriteLine("To bimi "+GetTokenConvertedType(n.LeftNode.RightNode.LeftNode, n.LeftNode.RightNode.RightNode).Value.TokenType); //+
        }

        private TokenType GetTokenNodeType(TokenNode<Token> parNode)
        {
            if (TokenNode<Token>.IsLeafToken(parNode))
            {
                return parNode.Value.TokenType;
            }
            else
            {
                return GetTokenConvertedType(parNode.LeftNode, parNode.RightNode).Value.TokenType;
            }
            
        }
        private TokenType GetOp(TokenNode<Token> parNode)
        {
            TokenType result = parNode.Value.TokenType;
            if (!TokenNode<Token>.IsLeafToken(parNode))
            {
                if(TokenNode<Token>.IsLeafToken(parNode.LeftNode) && TokenNode<Token>.IsLeafToken(parNode.RightNode))
                {
                    //Differents type pour tous les deux liste
                    if (!TokenWorker.IsTokenOperandEqualType(parNode.LeftNode.Value.TokenType, parNode.RightNode.Value.TokenType))
                    {
                        if (TokenWorker.IsTokenOperandDecimalType(parNode.LeftNode.Value.TokenType))
                        {
                            /*TokenNode<Token> temp = parNode.RightNode.Clone() as TokenNode<Token>;
                            parNode.RightNode.Value = new Token(TokenType.INT_2_FLOAT, "aaa");
                            parNode.RightNode.LeftNode = temp;
                            parNode.RightNode.RightNode = null;*/
                            result = TokenType.CORRECT_DECIMAL_CONSTANT;
                        }
                        else
                        {
                            /*TokenNode<Token> temp = parNode.LeftNode.Clone() as TokenNode<Token>;
                            parNode.LeftNode.Value = new Token(TokenType.INT_2_FLOAT, "aaa");
                            parNode.LeftNode.LeftNode = temp;
                            parNode.LeftNode.RightNode = null;*/
                            result = TokenType.CORRECT_DECIMAL_CONSTANT;
                        }
                    }
                    //Meme type
                    else
                    {
                        result = parNode.LeftNode.Value.TokenType;
                    }
                }
                else
                {
                    
                }
            }
            return result;
        }
        private TokenNode<Token> GetTokenConvertedType(TokenNode<Token> parLeftNode, TokenNode<Token> parRightNode)
        {
            /*Console.WriteLine("Compare " + parLeftNode.Value.Lexeme + " and " + parRightNode.Value.Lexeme);*/
            TokenNode<Token> result = new TokenNode<Token>(new Token(TokenType.INVALID, "CONVERTED"));
            if (TokenNode<Token>.IsLeafToken(parLeftNode) && TokenNode<Token>.IsLeafToken(parRightNode))
            {
                if(!TokenWorker.IsTokenOperandEqualType(parLeftNode.Value.TokenType, parRightNode.Value.TokenType))
                {
                    /*Console.WriteLine("Diferents type " + parLeftNode.Value.Lexeme + " and " + parRightNode.Value.Lexeme +" "
                        +parLeftNode.Value.TokenType+ " and "+parRightNode.Value.TokenType);
                    if (TokenWorker.IsTokenOperandDecimalType(parLeftNode.Value.TokenType))
                    {
                        TokenNode<Token> temp = parRightNode.Clone() as TokenNode<Token>;
                        parRightNode.Value = new Token(TokenType.INT_2_FLOAT, temp.Value.Lexeme, temp.Value.AttributeValue);
                        parRightNode.LeftNode = temp;
                        parRightNode.RightNode = null;
                        result = parRightNode;
                    }
                    else //if(TokenWorker.IsTokenOperandDecimalType(parRightNode.Value.TokenType))
                    {
                        TokenNode<Token> temp = parLeftNode.Clone() as TokenNode<Token>;
                        parLeftNode.Value = new Token(TokenType.INT_2_FLOAT, temp.Value.Lexeme, temp.Value.AttributeValue);
                        parLeftNode.LeftNode = temp;
                        parLeftNode.RightNode = null;
                        result = parLeftNode;
                    }*/
                    //result = parLeftNode;
                    result = new TokenNode<Token>(new Token(TokenType.INT_2_FLOAT, "CONVERTED"));
                }
                else
                {
                    /*Console.WriteLine("Equals type " + parLeftNode.Value.Lexeme + " and " + parRightNode.Value.Lexeme + " "
                       + parLeftNode.Value.TokenType + " and " + parRightNode.Value.TokenType);*/
                    result = parLeftNode;
                }
            }
            else
            {
                if (TokenNode<Token>.IsLeafToken(parLeftNode))
                {
                    result = GetTokenConvertedType(parLeftNode, GetTokenConvertedType(parRightNode.LeftNode, parRightNode.RightNode));
                }
                else if (TokenNode<Token>.IsLeafToken(parRightNode))
                {
                    result = GetTokenConvertedType(GetTokenConvertedType(parLeftNode.LeftNode, parLeftNode.RightNode), parRightNode);
                    
                }
                else
                {
                    result = GetTokenConvertedType(GetTokenConvertedType(parLeftNode.LeftNode, parLeftNode.RightNode),
                        GetTokenConvertedType(parRightNode.LeftNode, parRightNode.RightNode));
                }
            }

            return result;
        }
        private TokenType GetOperatorResultType(TokenNode<Token> parNode)
        {
            Console.WriteLine("Enter with " + parNode.Value.Lexeme);
            TokenNode<Token> leftNode = parNode.LeftNode;
            TokenNode<Token> rightNode = parNode.RightNode;
            Console.WriteLine("Op left " + leftNode.Value.Lexeme + " -> " + leftNode.Value.TokenType + " right " + rightNode.Value.Lexeme + " -> " + rightNode.Value.TokenType);
            if (TokenNode<Token>.IsLeafToken(leftNode) && TokenNode<Token>.IsLeafToken(rightNode))
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
                    Console.WriteLine("Ici " + leftNode.Value.Lexeme + " -> " + leftNode.Value.TokenType + " right " + rightNode.Value.Lexeme + " -> " + rightNode.Value.TokenType);
                    return GetOperatorResultType(leftNode);
                }
                else
                {
                    return ModifieSyntaxTree(parNode);
                }
            }
        }
        public void V()
        {
            BottomTop(SyntaxTreeModified);
        }
        private void BottomTop(TokenNode<Token> parNode)
        {
            t++;
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
                        //TokenNode<Token> temp = parNode.RightNode.Clone() as TokenNode<Token>;
                        parNode.RightNode.Value = new Token(TokenType.INT_2_FLOAT, rightNodeCloned.Value.Lexeme, rightNodeCloned.Value.AttributeValue);
                        parNode.RightNode.LeftNode = rightNodeCloned;
                        parNode.RightNode.RightNode = null;
                    }
                    else
                    {
                        //TokenNode<Token> temp = parNode.LeftNode.Clone() as TokenNode<Token>;
                        parNode.LeftNode.Value = new Token(TokenType.INT_2_FLOAT, leftNodeCloned.Value.Lexeme, leftNodeCloned.Value.AttributeValue);
                        parNode.LeftNode.LeftNode = leftNodeCloned;
                        parNode.LeftNode.RightNode = null;
                    }
                }
                /*
                */
                if (!TokenNode<Token>.IsLeafToken(leftNodeCloned))
                {
                    BottomTop(leftNodeCloned);
                }
                if (!TokenNode<Token>.IsLeafToken(rightNodeCloned))
                {
                    BottomTop(rightNodeCloned);
                }
                
            }
        }
        private TokenType ModifieSyntaxTree(TokenNode<Token> parNode)
        {
            Console.WriteLine("Enter with " + parNode.Value.Lexeme);
            TokenType leftType = GetTokenNodeType(parNode.LeftNode);
            TokenType rightType = GetTokenNodeType(parNode.RightNode);
            Console.WriteLine("Gen left " +parNode.LeftNode.Value.Lexeme+" -> "+ parNode.LeftNode.Value.TokenType + 
                " right " + parNode.RightNode.Value.Lexeme + " -> "+ parNode.RightNode.Value.TokenType);
            if (!TokenWorker.IsTokenOperandEqualType(leftType, rightType))
            {
                Console.WriteLine("Diff");
                if (TokenWorker.IsTokenOperandDecimalType(leftType))
                {
                    /*Console.WriteLine("IN Bef Gen " + parNode.LeftNode.Value.Lexeme + " -> " + parNode.LeftNode.Value.TokenType +
                    " right " + parNode.RightNode.Value.Lexeme + " -> " + parNode.RightNode.Value.TokenType);*/
                    TokenNode<Token> temp = parNode.RightNode.Clone() as TokenNode<Token>;
                   // parNode.RightNode = new TokenNode<Token>(new Token(TokenType.INT_2_FLOAT, temp.Value.Lexeme, temp.Value.AttributeValue), temp);
                    parNode.RightNode.Value = new Token(TokenType.INT_2_FLOAT, temp.Value.Lexeme, temp.Value.AttributeValue);
                    parNode.RightNode.LeftNode = temp;
                    parNode.RightNode.RightNode = null;
                    /*Console.WriteLine("IN Af Gen " + parNode.LeftNode.Value.Lexeme + " -> " + parNode.LeftNode.Value.TokenType +
                    " right " + parNode.RightNode.Value.Lexeme + " -> " + parNode.RightNode.Value.TokenType);*/

                }
                else if(TokenWorker.IsTokenOperandDecimalType(rightType))
                {
                    /*Console.WriteLine("IN Bef Gen " + parNode.LeftNode.Value.Lexeme + " -> " + parNode.LeftNode.Value.TokenType +
                    " right " + parNode.RightNode.Value.Lexeme + " -> " + parNode.RightNode.Value.TokenType);*/
                    TokenNode<Token> temp = parNode.LeftNode.Clone() as TokenNode<Token>;
                    //parNode.LeftNode = new TokenNode<Token>(new Token(TokenType.INT_2_FLOAT, temp.Value.Lexeme, temp.Value.AttributeValue), temp);
                    parNode.LeftNode.Value = new Token(TokenType.INT_2_FLOAT, temp.Value.Lexeme, temp.Value.AttributeValue);
                    parNode.LeftNode.LeftNode = temp;
                    parNode.RightNode.RightNode = null;
                    /*Console.WriteLine("IN Af Gen " + parNode.LeftNode.Value.Lexeme + " -> " + parNode.LeftNode.Value.TokenType +
                    " right " + parNode.RightNode.Value.Lexeme + " -> " + parNode.RightNode.Value.TokenType);*/
                }
                Console.WriteLine("Out Gen " + parNode.LeftNode.Value.Lexeme + " -> " + parNode.LeftNode.Value.TokenType +
                    " right " + parNode.RightNode.Value.Lexeme + " -> " + parNode.RightNode.Value.TokenType);
            }
                return rightType;
        }
        public void RealizeSyntaxTreeModification()
        {
            ModifieSyntaxTree(SyntaxTreeModified);
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
