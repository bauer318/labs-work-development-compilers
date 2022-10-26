using DevCompilersLW2;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW3
{
    public class Parser
    {
        private List<TokenType> _termItems = new List<TokenType>() { TokenType.ADDITION_SIGN, TokenType.SOUSTRACTION_SIGN };
        private List<TokenType> _factorItems = new List<TokenType>() { TokenType.MULTIPLICATION_SIGN, TokenType.DIVISION_SIGN};
        private List<Token> _tokens;

        private int _currentTokenIndex = 0;
        private Token _currentToken = null;
        private List<TokenNode<Token>> _tokenNodes = new List<TokenNode<Token>>();
        private int whiteSpaceCount = 0;
        public TokenNode<Token> AbstractSyntaxTree = null;
        private List<string> _astTexts = new List<string>();
        
        public Parser(SyntacticalErrorAnalyzer parSyntacticalErrorAnalyzer)
        {
            _tokens = parSyntacticalErrorAnalyzer.Tokens;
            GetNextToken();
        }
        
        private void GetNextToken()
        {
            if (_currentTokenIndex < _tokens.Count)
            {
                _currentToken = _tokens[_currentTokenIndex];
                _currentTokenIndex++;
            }
        }
        public TokenNode<Token> ParseExpression()
        {
            TokenNode<Token> result = Factor();
            while (result != null && _termItems.Contains(_currentToken.TokenType))
            {
                if (_currentToken.TokenType == TokenType.ADDITION_SIGN)
                {
                    GetNextToken();
                    TokenNode<Token> rigthNode = Factor();
                    Token tokenPlus = new Token(TokenType.ADDITION_SIGN, "+");
                    result = new TokenNode<Token>(tokenPlus, result, rigthNode);
                    _tokenNodes.Add(result);
                }
                else if (_currentToken.TokenType == TokenType.SOUSTRACTION_SIGN)
                {
                    GetNextToken();
                    TokenNode<Token> rigthNode = Factor();
                    Token tokenMinus = new Token(TokenType.SOUSTRACTION_SIGN, "-");
                    result = new TokenNode<Token>(tokenMinus, result, rigthNode);
                    _tokenNodes.Add(result);
                }
            }

            return result;
        }
        private TokenNode<Token> Factor()
        {
            TokenNode<Token> factor = Term();
            while (factor != null && _factorItems.Contains(_currentToken.TokenType))
            {
                if (_currentToken.TokenType == TokenType.MULTIPLICATION_SIGN)
                {
                    GetNextToken();
                    TokenNode<Token> rigthNode = Term();
                    Token tokenMultiply = new Token(TokenType.MULTIPLICATION_SIGN, "*");
                    factor = new TokenNode<Token>(tokenMultiply, factor, rigthNode);
                    _tokenNodes.Add(factor);
                }
                else if (_currentToken.TokenType == TokenType.DIVISION_SIGN)
                {
                    GetNextToken();
                    TokenNode<Token> rigthNode = Term();
                    Token tokenDivision = new Token(TokenType.DIVISION_SIGN, "/");
                    factor = new TokenNode<Token>(tokenDivision, factor, rigthNode);
                    _tokenNodes.Add(factor);
                }
            }
            return factor;
        }
        
        private TokenNode<Token> Term()
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
            TraverserPreOrder("","" ,AbstractSyntaxTree);
            return _astTexts;
        }
        public void PrintTokenNode()
        {
            foreach(TokenNode<Token> t in _tokenNodes)
            {
                Console.WriteLine(t.LeftNode.Value.Lexeme+" "+t.Value.Lexeme+" "+t.RightNode.Value.Lexeme);
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
            if(parAbstractSyntaxTree != null)
            {
                _astTexts.Add(parPadding+parPointer+ parAbstractSyntaxTree.Value.TokenType.GetTokenNodeDescription(parAbstractSyntaxTree.Value));
                StringBuilder paddingBuilder = new StringBuilder(parPadding);
                if (whiteSpaceCount > 0)
                {
                    paddingBuilder.Append("     ");
                }
                whiteSpaceCount++;
                string paddingForBoth = paddingBuilder.ToString();
                string pointerForRight = " |---";
                string pointerForLeft = " |---";
                TraverserPreOrder(paddingForBoth,pointerForLeft, parAbstractSyntaxTree.LeftNode);
                TraverserPreOrder(paddingForBoth, pointerForRight, parAbstractSyntaxTree.RightNode); ;
            }
        }

    }

}
