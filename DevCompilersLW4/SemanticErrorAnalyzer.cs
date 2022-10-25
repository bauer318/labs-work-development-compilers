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
            TraverserPreOrder("", "", _abstractSyntaxTree);
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
