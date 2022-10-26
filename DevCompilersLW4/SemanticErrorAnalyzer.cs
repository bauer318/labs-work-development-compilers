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
        int t = 0;
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
        public void PrintType()
        {
            
        }
        public void CompareOut()
        {
            Compare(_abstractSyntaxTree.LeftNode, _abstractSyntaxTree.RightNode);
        }
       
        
        public TokenNode<Token> Compare(TokenNode<Token> parLeftNode, TokenNode<Token> parRightNode)
        { 
            t++;
            TokenNode<Token> nextComp = null;
             if(TokenNode<Token>.IsLeafToken(parLeftNode) && TokenNode<Token>.IsLeafToken(parRightNode))
            {
                
                TokenNode<Token> convertedTokenNode = null;
                if (!TokenWorker.IsTokenOperandEqualType(parLeftNode.Value.TokenType,parRightNode.Value.TokenType))
                {
                    Console.WriteLine("Dif type " + parLeftNode.Value.TokenType + " and " + parRightNode.Value.TokenType);
                    if (TokenWorker.IsTokenOperandDecimalType(parLeftNode.Value.TokenType))
                    {
                        //Int2Float for right node
                        TokenNode<Token> temp = parRightNode.Clone() as TokenNode<Token>;
                        convertedTokenNode = new TokenNode<Token>(new Token(temp.Value.TokenType, temp.Value.Lexeme, temp.Value.AttributeValue));
                        parRightNode.Value.TokenType = parLeftNode.Value.TokenType;
                        parRightNode.Value = new Token(TokenType.INT_2_FLOAT, convertedTokenNode.Value.Lexeme,convertedTokenNode.Value.AttributeValue);
                        parRightNode.ConvertedTokenNode = convertedTokenNode;
                        return parRightNode;
                        
                        
                    }
                    else
                    {
                        //Int2Float for left node
                        TokenNode<Token> temp = parLeftNode.Clone() as TokenNode<Token>;
                        convertedTokenNode = new TokenNode<Token>(new Token(temp.Value.TokenType, temp.Value.Lexeme,temp.Value.AttributeValue));
                        parLeftNode.Value.TokenType = parRightNode.Value.TokenType;
                        parLeftNode.Value = new Token(TokenType.INT_2_FLOAT, convertedTokenNode.Value.Lexeme, convertedTokenNode.Value.AttributeValue);
                        parLeftNode.ConvertedTokenNode = convertedTokenNode;
                        return parLeftNode;       
                    }
                }
                else
                {
                    //Console.WriteLine("Meme type " + parLeftNode.Value.TokenType + " and " + parRightNode.Value.TokenType);
                    return parRightNode;
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
                    nextComp =Compare(parRightNode, Compare(parLeftNode.LeftNode, parLeftNode.RightNode));
                }
            }
            return nextComp;
        }
        //private TokenType()
        private TokenType TP(TokenNode<Token> parNode)
        {
            if (TokenNode<Token>.IsLeafToken(parNode))
            {
                return parNode.Value.TokenType;
            }
            return TokenType.INVALID;
        }
        public void GoToLast(TokenNode<Token> parNode)
        {
            t++;
            Console.Write(t + " " + parNode.Value.Lexeme);
            if (parNode.LeftNode != null)
            {
                Console.Write(" " + parNode.LeftNode.Value.Lexeme);
            }
            if (parNode.RightNode != null)
            {
                Console.Write(" " + parNode.RightNode.Value.Lexeme);
            }
            Console.WriteLine();
            if (!IsLastFirstNode(parNode))
            {
                if (!TokenNode<Token>.IsLeafToken(parNode.LeftNode))
                {
                    GoToLast(parNode.LeftNode);
                }
                if (!TokenNode<Token>.IsLeafToken(parNode.RightNode))
                {
                    GoToLast(parNode.RightNode);
                }
            }
            
        }
      
        private bool IsLastFirstNode(TokenNode<Token> parNode)
        {
            
            return TokenWorker.IsOperand(parNode.LeftNode.Value.TokenType) &&
                TokenWorker.IsOperand(parNode.RightNode.Value.TokenType);
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
