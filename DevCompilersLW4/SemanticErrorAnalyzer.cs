using DevCompilersLW2;
using DevCompilersLW3;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW4
{
    public class SemanticErrorAnalyzer
    {
        private TokenNode<Token> _abstractSyntaxTree;
        private SymbolTable _symbolTable;
        private List<string> _astTexts = new List<string>();
        private int whiteSpaceCount = 0;
        public TokenNode<Token> G { get; set; }
        private List<TokenNode<Token>> l = new List<TokenNode<Token>>();
        private int t = 0;
        private int countDifferentsOperandType = 0;
        public SemanticErrorAnalyzer(TokenNode<Token> parAbstractSyntaxTree, SymbolTable parSymbolTable)
        {
            _abstractSyntaxTree = parAbstractSyntaxTree;
            _symbolTable = parSymbolTable;
        }
        public void Print()
        {
            Console.WriteLine("Root " + _abstractSyntaxTree.Value.Lexeme);
        }
        public List<string> GetSemanticTreeTextList()
        {
            //Create();
            TraverserPreOrder("", "", _abstractSyntaxTree);
            return _astTexts;
        }
        private void Create()
        {
            G = BuildSemanticTreeRecursive(_abstractSyntaxTree);
        }
        private TokenNode<Token> BuildSemanticTreeRecursive(TokenNode<Token> parNode)
        {
            if (TokenNode<Token>.IsLeafToken(parNode))
            {
                return new TokenNode<Token>(parNode.Value);
            }
            TokenNode<Token> next = new TokenNode<Token>(parNode.Value);
            next.LeftNode = BuildSemanticTreeRecursive(parNode.LeftNode);
            next.RightNode = BuildSemanticTreeRecursive(parNode.RightNode);
            return next;
        }
        private void Create2(TokenNode<Token> parNode)
        {
            if (parNode != null)
            {
                
                //Console.WriteLine(parNode.Value.Lexeme);
                l.Add(parNode);
                Create2(parNode.LeftNode);
                Create2(parNode.RightNode);
            }
        }
        public void Print2()
        {
            l = new List<TokenNode<Token>>();
            Create2(_abstractSyntaxTree);
            foreach (TokenNode<Token> t in l)
            {
                Console.WriteLine("lexeme " + t.Value.Lexeme + " type " + t.Value.TokenType);
            }
            Console.WriteLine("-----------------------");
        }
        public void CompareOut()
        {
            PrintVerifiResult();
            if (countDifferentsOperandType > 0)
            {
                Compare(_abstractSyntaxTree.LeftNode, _abstractSyntaxTree.RightNode);
                CompareOut();
            }
        }
       
        public void PrintVerifiResult()
        {
            countDifferentsOperandType = 0;
            Verifie(_abstractSyntaxTree.LeftNode, _abstractSyntaxTree.RightNode);
            //Console.WriteLine("Count dif " + countDifferentsOperandType);
        }
        private TokenNode<Token> Verifie(TokenNode<Token> parLeftNode, TokenNode<Token> parRightNode)
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
                    result = Verifie(parLeftNode, Verifie(parRightNode.LeftNode, parRightNode.RightNode));
                }
                else if(TokenNode<Token>.IsLeafToken(parRightNode))
                {
                    result = Verifie(parRightNode, Verifie(parLeftNode.LeftNode, parLeftNode.RightNode));
                }
            }
            return result;
        }
        public TokenNode<Token> Compare(TokenNode<Token> parLeftNode, TokenNode<Token> parRightNode)
        { 
            t++;
            TokenNode<Token> nextComp = null;
             if(TokenNode<Token>.IsLeafToken(parLeftNode) && TokenNode<Token>.IsLeafToken(parRightNode))
            {
                Console.WriteLine(t + " Enter left " + parLeftNode.Value.Lexeme + " right " + parRightNode.Value.Lexeme);
                if (!TokenWorker.IsTokenOperandEqualType(parLeftNode.Value.TokenType,parRightNode.Value.TokenType))
                {
                    Console.WriteLine(t+" Dif type " + parLeftNode.Value.TokenType + " and " + parRightNode.Value.TokenType);
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
                    Console.WriteLine(t+" Meme type " + parLeftNode.Value.TokenType + " and " + parRightNode.Value.TokenType);
                    return parLeftNode;
                }
                
            }
            else
            {
                if (TokenNode<Token>.IsLeafToken(parLeftNode))
                {
                    nextComp = Compare(parLeftNode,Compare(parRightNode.LeftNode, parRightNode.RightNode));
                }
                else if (TokenNode<Token>.IsLeafToken(parRightNode))
                {
                    nextComp =Compare(Compare(parLeftNode.LeftNode, parLeftNode.RightNode), parRightNode);
                }
            }
            return nextComp;
        }
        
        public void Run(TokenNode<Token> root)
        {
            BuildSemanticTreeRecursive(root);
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
