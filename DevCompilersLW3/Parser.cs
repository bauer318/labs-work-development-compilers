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
        private readonly List<Token> _tokens;
        private int _currentTokenIndex = 0;
        private Token _currentToken = null;
        private List<TokenNode<Token>> _tokenNodes = new List<TokenNode<Token>>();
        private int whiteSpaceCount = 0;
        private TokenNode<Token> _abstractSyntaxTree = null;
        

        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
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
        public TokenNode<Token> ParseExp()
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
        public TokenNode<Token> Factor()
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
        public TokenNode<Token> Term()
        {
            TokenNode<Token> term = null;
            if (_currentToken.TokenType == TokenType.OPEN_PARENTHESIS)
            {
                GetNextToken();
                term = ParseExp();
            }
            else if (TokenWorker.IsOperand(_currentToken.TokenType))
            {
                term = new TokenNode<Token>(_currentToken);   
            }
            GetNextToken();
            return term;
        }
        public void Print2()
        {
            _abstractSyntaxTree = BuildeTree(_tokenNodes[_tokenNodes.Count - 1]);
            StringBuilder sb = new StringBuilder();
            TraverserPreOrder(sb,"","" ,_abstractSyntaxTree);
            Console.WriteLine(sb);
        }
        private TokenNode<Token> BuildeTree(TokenNode<Token> parNode)
        {
            if (TokenNode<Token>.IsLeafToken(parNode))
            {
                return new TokenNode<Token>(parNode.Value);
            }
            TokenNode<Token> next = new TokenNode<Token>(parNode.Value);
            next.LeftNode = BuildeTree(parNode.LeftNode);
            next.RightNode = BuildeTree(parNode.RightNode);
            return next;
        }
        
        private void TraverserPreOrder(StringBuilder parSb, string parPadding, string parPointer, TokenNode<Token> parTree)
        {
            if(parTree != null)
            {
                parSb.Append(parPadding);
                parSb.Append(parPointer);
                parSb.Append(parTree.Value.TokenType.GetTokenNodeDescription(parTree.Value));
                parSb.Append("\n");
                StringBuilder paddingBuilder = new StringBuilder(parPadding);
                if (whiteSpaceCount > 0)
                {
                    paddingBuilder.Append("     ");
                }
                whiteSpaceCount++;
                string paddingForBoth = paddingBuilder.ToString();
                string pointerForRight = " |---";
                string pointerForLeft = " |---";
                TraverserPreOrder(parSb,paddingForBoth,pointerForLeft, parTree.LeftNode);
                TraverserPreOrder(parSb, paddingForBoth, pointerForRight, parTree.RightNode); ;
            }
        }

    }

}
