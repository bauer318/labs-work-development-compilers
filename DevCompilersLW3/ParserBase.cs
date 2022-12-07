using DevCompilersLW2;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW3
{
    public abstract class ParserBase
    {
        public List<TokenType> _termItems = new List<TokenType>() { TokenType.ADDITION_SIGN, TokenType.SOUSTRACTION_SIGN };
        public List<TokenType> _factorItems = new List<TokenType>() { TokenType.MULTIPLICATION_SIGN, TokenType.DIVISION_SIGN };
        public List<Token> _tokens;

        public int _currentTokenIndex = 0;
        public Token _currentToken = null;
        public List<TokenNode<Token>> _tokenNodes = new List<TokenNode<Token>>();
        public int whiteSpaceCount = 0;
        public TokenNode<Token> AbstractSyntaxTree = null;
        public List<string> _astTexts = new List<string>();

        public bool DivideByZeroRunTimeException { get; set; }

        public ParserBase(SyntacticalErrorAnalyzer parSyntacticalErrorAnalyzer)
        {
            DivideByZeroRunTimeException = false;
            _tokens = parSyntacticalErrorAnalyzer.Tokens;
            GetNextToken();
        }
        public void GetNextToken()
        {
            if (_currentTokenIndex < _tokens.Count)
            {
                _currentToken = _tokens[_currentTokenIndex];
                _currentTokenIndex++;
            }
        }
        public abstract TokenNode<Token> ParseExpression();
        public abstract TokenNode<Token> Factor();

        public TokenNode<Token> Term()
        {
            TokenNode<Token> term = null;
            if (_currentToken.TokenType == TokenType.OPEN_PARENTHESIS)
            {
                GetNextToken();
                term = ParseExpression();
            }
            else if (TokenWorker.IsOperand(_currentToken.TokenType))
            {
                term = new TokenNode<Token>(_currentToken);
            }
            GetNextToken();
            return term;
        }
        public TokenNode<Token> GetAbstractSyntaxTree()
        {
            return BuildTreeRecursive(_tokenNodes[_tokenNodes.Count - 1]);
        }
        public List<string> GetSyntaxTreeNodeTextArray()
        {
            AbstractSyntaxTree = GetAbstractSyntaxTree();
            TraverserPreOrder("", "", AbstractSyntaxTree);
            return _astTexts;
        }
        public void PrintTokenNode()
        {
            foreach (TokenNode<Token> t in _tokenNodes)
            {
                Console.WriteLine(t.LeftNode.Value.Lexeme + " " + t.Value.Lexeme + " " + t.RightNode.Value.Lexeme);
            }
        }

        private TokenNode<Token> BuildTreeRecursive(TokenNode<Token> parNode)
        {
            if (TokenNode<Token>.IsLeafToken(parNode))
            {
                return new TokenNode<Token>(parNode.Value);
            }
            TokenNode<Token> next = new TokenNode<Token>(parNode.Value);
            next.LeftNode = BuildTreeRecursive(parNode.LeftNode);
            next.RightNode = BuildTreeRecursive(parNode.RightNode);
            return next;
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
