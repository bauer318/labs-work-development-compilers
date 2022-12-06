using DevCompilersLW2;
using DevCompilersLW3;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW5
{
    public class IntermediateCodeGenerator
    {
        public TokenNode<Token> SemanticTree { get; }
        public SymbolTable SymbolTable { get; }
        public List<PortableCode> PortableCodes {get; }
        private int _lastVariableId;
        public List<Token> PostFixExpressionSequence { get; }
        private int id = 1;
        public IntermediateCodeGenerator(TokenNode<Token> parSemanticTree, SymbolTable parSymboleTable)
        {
            SemanticTree = parSemanticTree;
            SymbolTable = parSymboleTable;
            PortableCodes = new List<PortableCode>();
            PostFixExpressionSequence = new List<Token>();
            InitFirstPortableCodeResultId();
        }
       
        private void InitFirstPortableCodeResultId()
        {
            _lastVariableId = SymbolTable.AttributeVariables.Count;
        }
        private void AddPostFixExpression(TokenNode<Token> parSemanticTree)
        {
            if(parSemanticTree != null)
            {
                if (!TokenNode<Token>.IsLeafToken(parSemanticTree))
                {
                    if(parSemanticTree.LeftNode != null)
                    {
                        AddPostFixExpression(parSemanticTree.LeftNode);
                    }
                    if(parSemanticTree.RightNode != null)
                    {
                        AddPostFixExpression(parSemanticTree.RightNode);
                    }
                }
                PostFixExpressionSequence.Add(parSemanticTree.Value);
            }
        }
        public List<string> GetPostFixExpressionText()
        {
            List<string> result = new List<string>();
            AddPostFixExpression(SemanticTree);
            foreach(Token token in PostFixExpressionSequence)
            {
                result.Add(TokenTypeMethodes.GetPostFixTokenNodeDescription(token.TokenType, token));
            }
            return result;
        }
        private void AddPortableCodeResultInSymbolTable()
        {
            var portableCodeResultLexemeIndex = 0;
            foreach(PortableCode portableCode in PortableCodes)
            {
                portableCode.Result.AttributeValue = ++_lastVariableId;
                SymbolTable.AttributeVariables.Add(new AttributeVariable(portableCode.Result.AttributeValue,
                    TokenWorker.GetPortableCodeResultLexeme(++portableCodeResultLexemeIndex),portableCode.Result.TokenType));
            }
        }
        private void CreatePortableCodeList(TokenNode<Token> parSemanticTree)
        {
            
            if (parSemanticTree != null)
            {
                if (!TokenNode<Token>.IsLeafToken(parSemanticTree))
                {
                    string operationCode = TokenTypeMethodes.GetOperationCodeDesignation(parSemanticTree.Value.TokenType);
                    Token result = parSemanticTree.Value;
                    result.TokenType = TokenWorker.GetTokenNodeType(parSemanticTree);
                    List<Token> operands = new List<Token>();
                    if(parSemanticTree.LeftNode != null)
                    {
                        operands.Add(parSemanticTree.LeftNode.Value);
                        CreatePortableCodeList(parSemanticTree.LeftNode);
                    }
                    if(parSemanticTree.RightNode != null)
                    {
                        operands.Add(parSemanticTree.RightNode.Value);
                        CreatePortableCodeList(parSemanticTree.RightNode);
                    }
                    PortableCodes.Add(new PortableCode(operationCode, result, operands,id));
                    id++;
                } 
            }
        }
        public List<string> GetPortableCodeText()
        {
            CreatePortableCodeList(SemanticTree);
            AddPortableCodeResultInSymbolTable();
            List<string> result = new List<string>();
            foreach (PortableCode portable in PortableCodes)
            {
                string currentPortableCodeText = portable.OperationCode + " ";
                currentPortableCodeText += TokenWorker.GetPortableCodeNodeDescription(portable.Result);
                foreach (Token token in portable.OperandList)
                {
                    currentPortableCodeText += " " + TokenWorker.GetPortableCodeNodeDescription(token);
                }
                result.Add(currentPortableCodeText);
            }
            return result;
        }
        public List<string> GetSymboleTableText()
        {
            List<string> result = new List<string>();
            foreach (AttributeVariable attribute in SymbolTable.AttributeVariables)
            {
                result.Add(TokenTypeMethodes.GetPortableCodeTokenSymboleTableDescription(attribute.TokenType, attribute));
            }
            return result;
        }
    }
}
