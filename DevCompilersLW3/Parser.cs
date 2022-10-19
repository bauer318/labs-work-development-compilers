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
        public List<Token> _tokens { get; set; }
        private int _currentTokenIndex = 0;
        private Token _currentToken = null;
        private List<TokenNode<Token>> _tokenNodes = new List<TokenNode<Token>>();
        private int whiteSpaceCount = 0;
        private TokenNode<Token> _abstractSyntaxTree = null;
        private List<List<int>> _expressionInBracePosition = new List<List<int>>();
        private List<Token> _subTokens = new List<Token>();
        private List<int> _excList = new List<int>();
        private int k = 0;
        private int maxPriority = 0;
        private List<int> tokenPrioritie = new List<int>();
        private int _deleteFrom = 0;
        private int _currentDoubleOpenedBrace = 0;
        private List<TokenSorter> tokenSorters = new List<TokenSorter>();
        

        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
            GetNextToken();
        }

        public void CreateP()
        {
            var priority = -1;
            List<int> alreadyInserted = new List<int>();
            for(var i = 0; i<_tokens.Count;i++)
            {
                if (!alreadyInserted.Contains(i))
                {
                    List<Token> tokens = new List<Token>();
                    if (!TokenWorker.IsTokenTypeOperator(_tokens[i]))
                    {
                        priority = _tokens[i].TokenPriority;
                        if (_tokens[i].TokenType == TokenType.OPEN_PARENTHESIS)
                        {
                            tokens.Add(_tokens[i]);

                            for (var k = i + 1; k < _tokens.Count; k++)
                            {
                                alreadyInserted.Add(k);
                                tokens.Add(_tokens[k]);
                                if (_tokens[k].TokenType == TokenType.CLOSE_PARENTHESIS)
                                {
                                    break;
                                }
                            }
                        }
                        else
                        {
                            tokens.Add(_tokens[i]);
                        }
                    }
                    else
                    {
                        tokens.Add(_tokens[i]);
                        priority = -1;

                    }
                    tokenSorters.Add(new TokenSorter(tokens, priority));
                }
            }
        }

        private void SorteTokenCreatedP()
        {
            for(var i=0; i < tokenSorters.Count-2; i += 2)
            {
                var maxIndex = i;
                for(var j=i+2; j < tokenSorters.Count; j += 2)
                {
                    if(tokenSorters[maxIndex].TokenPriority < tokenSorters[j].TokenPriority)
                    {
                        maxIndex = j;
                    }
                }
                
                    TokenSorter temp = tokenSorters[maxIndex];
                    tokenSorters[maxIndex] = tokenSorters[i];
                    tokenSorters[i] = temp;
                
            }
        }
        private void OpenSortedTokens()
        {
            List<Token> results = new List<Token>();
            for(var i=0; i < tokenSorters.Count; i++)
            {
                for(var j=0; j < tokenSorters[i].Tokens.Count; j++)
                {
                    results.Add(tokenSorters[i].Tokens[j]);
                }
            }
            _tokens = results;
        }

        public void SetTokenPriority()
        {
            var open_brace_count = 0;
            List<Token> result = new List<Token>();
            for(var i = 0; i < _tokens.Count; i++)
            {
                if(_tokens[i].TokenType == TokenType.OPEN_PARENTHESIS)
                {
                    open_brace_count++;
                }
                result.Add(new Token(_tokens[i], open_brace_count));
                if (_tokens[i].TokenType == TokenType.CLOSE_PARENTHESIS)
                {
                    open_brace_count--;
                }
                if (open_brace_count > maxPriority)
                {
                    maxPriority = open_brace_count;
                }
                
            }
            _tokens = result;
        }
        public void PrintPriority()
        {
            for (var i = 0; i < _tokens.Count; i++)
            {
                Console.Write(_tokens[i].Lexeme);
                //Console.WriteLine(_tokens[i].TokenPriority);
            }
            Console.WriteLine();
            for (var i = 0; i < _tokens.Count; i++)
            {
                Console.Write(_tokens[i].TokenPriority);
            }
        }

        public void CreateByPriority()
        {
            List<Token> result = new List<Token>();
            List<int> excluIndex = new List<int>();
            bool canAddOperator = true;
            bool operand = true;
            bool operato = false;
            for(var i = maxPriority; i >= 0; i--)
            {
                for(var j=0; j < _tokens.Count; j++)
                {
                    Token currentToken = _tokens[j];
                    
                    if (currentToken.TokenType == TokenType.OPEN_PARENTHESIS)
                    {
                        if (j>0 && TokenWorker.IsTokenTypeOperator(_tokens[j - 1]) && currentToken.TokenPriority == i)
                        {
                            if (_tokens[j - 1].TokenPriority == i - 1 && !excluIndex.Contains(j-1) && !canAddOperator)
                            {
                                result.Add(_tokens[j - 1]);
                                excluIndex.Add(j - 1);
                            }
                        }
                        if (TokenWorker.IsOperand(_tokens[j + 1].TokenType) && currentToken.TokenPriority==i)
                        {
                            result.Add(currentToken);
                        }
                    }
                    if(currentToken.TokenType==TokenType.CLOSE_PARENTHESIS && 
                        currentToken.TokenPriority == i - 1 && 
                        TokenWorker.IsOperand(_tokens[j-1].TokenType))
                    {
                        result.Add(currentToken);
                    }
                    if ((TokenWorker.IsTokenTypeOperator(currentToken)||TokenWorker.IsOperand(currentToken.TokenType)) 
                        && currentToken.TokenPriority==i && !excluIndex.Contains(j))
                    {
                       /* if((TokenWorker.IsTokenTypeOperator(currentToken) && operato) || (TokenWorker.IsOperand(currentToken.TokenType) && operand))
                        {*/
                            result.Add(currentToken);
                       // }  
                    }
                    if((TokenWorker.IsTokenTypeOperator(currentToken) || TokenWorker.IsOperand(currentToken.TokenType)) && currentToken.TokenPriority == i - 1)
                    {
                        canAddOperator = !canAddOperator;
                    }
                    if (TokenWorker.IsTokenTypeOperator(currentToken))
                    {
                        operato = !operato;
                        operand = true;
                    }
                    if (TokenWorker.IsOperand(currentToken.TokenType))
                    {
                        operand = !operand;
                        operato = true;
                    }
                   

                }
            }
            _tokens = result;
        }
        public void SortByPriority()
        {
            for(var i=0; i < _tokens.Count-1; i++)
            {
                var maxIndex = i;
                for(var j = i+1; j<_tokens.Count; j++)
                {
                    if (_tokens[i].TokenPriority < _tokens[j].TokenPriority)
                    {
                        maxIndex = j;
                    }
                }
                    Token temp = _tokens[maxIndex];
                    _tokens[maxIndex] = _tokens[i];
                    _tokens[i] = temp;
                
                
            }
        }
        public void DoTask()
        {
            PrintList(_tokens);
            SetTokenPriority(); 
            while (CanDeleteBrace())
            {
                _tokens = DeleteExternDoubleBrace(_tokens);
            }
            CreateP();
            Console.WriteLine();
            SorteTokenCreatedP();
            OpenSortedTokens();
            PrintList(_tokens);
        }

        public void CreateArrangedTokenList()
        {
            GetExpressionInBracePosition(_tokens, 0);
            List<int> parExpressionInBracePositions = new List<int>();
            List<Token> tokens = new List<Token>();
            for (var i = 0; i < _expressionInBracePosition.Count; i++)
            {
                for(var j = 0; j < _expressionInBracePosition[i].Count; j++)
                {
                    parExpressionInBracePositions.Add(_expressionInBracePosition[i][j]);
                }
            }
            for(var i =0; i < _tokens.Count; i++)
            {
                if (parExpressionInBracePositions.Contains(i))
                {
                    tokens.Add(_tokens[i]);
                }  
            }
            for (var i = 0; i < _tokens.Count; i++)
            {
                if (!parExpressionInBracePositions.Contains(i))
                {
                    tokens.Add(_tokens[i]);
                }
            }
            _tokens = tokens;
        }
        
        public bool CanArrangeExpression()
        {
            var braceCount = 0;
            bool operandOutPassed = false;
            for(var i=0; i < _tokens.Count; i++)
            {
                if (_tokens[i].TokenType == TokenType.OPEN_PARENTHESIS)
                {
                    braceCount++;
                    if (operandOutPassed)
                    {
                        return true;
                    }
                }
                if(_tokens[i].TokenType == TokenType.CLOSE_PARENTHESIS)
                {
                    braceCount--;
                }
                if(TokenWorker.IsOperand(_tokens[i].TokenType) && braceCount==0)
                {
                    operandOutPassed = true;
                }
                
            }
            return false;
        }
        public bool CanDeleteBrace()
        {
            var braceCount = 0;
            _currentDoubleOpenedBrace = 0;
            for (var i = 0; i < _tokens.Count; i++)
            {
                if (_tokens[i].TokenType == TokenType.OPEN_PARENTHESIS)
                {

                    braceCount++;
                }
                if (_tokens[i].TokenType == TokenType.CLOSE_PARENTHESIS)
                {
                    braceCount--;
                }
                if (braceCount > 1)
                {
                    _currentDoubleOpenedBrace = i;
                    return true;
                }

            }
            return false;
        }
        private int GetLastIndexOpenedBraceBefore(int parIndex)
        {
            var braceCount = 0;
            for(var i=parIndex-1; i >=0; i--)
            {
                if (_tokens[i].TokenType == TokenType.OPEN_PARENTHESIS)
                {
                    braceCount++;
                    if (braceCount == 1)
                    {
                        return i;
                        
                    }
                }
                if(_tokens[i].TokenType == TokenType.CLOSE_PARENTHESIS)
                {
                    braceCount--;
                }
            }
            return 0;
        }

        private int GetLastIndexClosedBraceAfter(int parIndex)
        {
            var braceCount = 0;
            for(var i = parIndex; i >= 0; i--)
            {
                if (_tokens[i].TokenType == TokenType.CLOSE_PARENTHESIS)
                {
                    braceCount--;
                }
                if (_tokens[i].TokenType == TokenType.OPEN_PARENTHESIS)
                {
                    braceCount++;
                }
            }
            for(var i = parIndex + 1; i < _tokens.Count; i++)
            {
                if (_tokens[i].TokenType == TokenType.CLOSE_PARENTHESIS)
                {
                    braceCount--;
                    if (braceCount == 0)
                    {
                        return i;
                    }
                }
                if (_tokens[i].TokenType == TokenType.OPEN_PARENTHESIS)
                {
                    braceCount++;
                }
            }

            return 0;
        }

        
        public List<Token> DeleteExternDoubleBrace(List<Token> parTokens)
        {
            List<Token> result = new List<Token>();
            var lastClosedBraceIndex = GetLastIndexClosedBraceAfter(_currentDoubleOpenedBrace);
            var firstOpenedBraceIndex = GetLastIndexOpenedBraceBefore(_currentDoubleOpenedBrace);
            for(var i =0; i < parTokens.Count; i++)
            {
                if(i!=firstOpenedBraceIndex && i != lastClosedBraceIndex)
                {
                    result.Add(parTokens[i]);
                }
            }
            return result;
        }

        public void PrintList(List<Token> parTokens)
        {
            for(var i =0; i<parTokens.Count; i++)
            {
                Console.Write(parTokens[i].Lexeme);
            }
        }

        public void Do(List<Token> parTokens)
        {
            TokenType currentTokenType;
            _subTokens = new List<Token>();
            for(var i=0; i < parTokens.Count; i++)
            {
                currentTokenType = parTokens[i].TokenType;
                if (currentTokenType == TokenType.OPEN_PARENTHESIS)
                {
                    if (i > 0 && k==0)
                    {
                        if (TokenWorker.IsTokenTypeOperator(parTokens[i - 1]))
                        {
                            _subTokens.Add(parTokens[i - 1]);
                            _excList.Add(i - 1);
                        }
                    }
                    _subTokens.Add(parTokens[i]);
                    _excList.Add(i);
                    k++;
                }
                else if (currentTokenType == TokenType.CLOSE_PARENTHESIS)
                {
                    k--;
                    _subTokens.Add(parTokens[i]);
                    _excList.Add(i);
                }
                else
                {
                    if (k > 0)
                    {
                        _subTokens.Add(parTokens[i]);
                        _excList.Add(i);
                    }
                }
            }
            _tokens = _subTokens;
            /*for(var i = 0; i < _subTokens.Count; i++)
            {
                Console.Write(_subTokens[i].Lexeme);
            }*/
        }

        private void GetExpressionInBracePosition(List<Token> parTokens, int parStartIndex)
        {
            var open_brace_count = 0;
            TokenType current_token_type;
            List<int> result = new List<int>();
            for(var i=parStartIndex; i < parTokens.Count; i++)
            {
                current_token_type = parTokens[i].TokenType;
                if (current_token_type == TokenType.OPEN_PARENTHESIS)
                {
                    open_brace_count++;
                }
                if (open_brace_count == 1)
                {
                    var j = i + 1;
                    result.Add(i);
                    while(open_brace_count > 0)
                    {
                        result.Add(j);
                        if (parTokens[j].TokenType == TokenType.CLOSE_PARENTHESIS)
                        {
                            open_brace_count--;
                        }
                        if (parTokens[j].TokenType == TokenType.OPEN_PARENTHESIS)
                        {
                            open_brace_count++;
                        }
                        j++;
                    }
                    i = j;
                    _expressionInBracePosition.Add(result);
                    if (j < parTokens.Count)
                    {
                        GetExpressionInBracePosition(parTokens, j + 1);
                        break;
                    }
                }
            }
        }
        public void Test(List<Token> parTokens, int parStartIndex)
        {
           GetExpressionInBracePosition(parTokens, parStartIndex);
           for(var i=0; i < _expressionInBracePosition.Count; i++)
            {
                for(var j=0; j < _expressionInBracePosition[i].Count; j++)
                {
                    Console.Write(_expressionInBracePosition[i][j] + " ");
                }
                Console.WriteLine();
            }
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
        public void Print()
        {
            //Console.WriteLine(_tokenNodes.Count);
            for(var i=0; i < _tokenNodes.Count; i++)
            {
                Console.WriteLine(_tokenNodes[i].Value.Lexeme);
                Console.WriteLine(" "+_tokenNodes[i].LeftNode.Value.Lexeme);
                Console.WriteLine(" " + _tokenNodes[i].RightNode.Value.Lexeme);
                Console.WriteLine();
            }
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
