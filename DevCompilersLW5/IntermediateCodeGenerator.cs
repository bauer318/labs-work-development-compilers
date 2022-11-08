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
        public IntermediateCodeGenerator(TokenNode<Token> parSemanticTree, SymbolTable parSymboleTable)
        {
            SemanticTree = parSemanticTree;
            SymbolTable = parSymboleTable;
            PortableCodes = new List<PortableCode>();
            InitFirstPortableCodeResultId();
        }
       
        private void InitFirstPortableCodeResultId()
        {
            _lastVariableId = SymbolTable.AttributeVariables.Count;
        }
        public void GoOne(TokenNode<Token> parSemanticTree)
        {
            //i++;
            if(parSemanticTree != null)
            {
                if (!TokenNode<Token>.IsLeafToken(parSemanticTree))
                {
                    if(parSemanticTree.LeftNode != null)
                    {
                        GoOne(parSemanticTree.LeftNode);
                    }
                    if(parSemanticTree.RightNode != null)
                    {
                        GoOne(parSemanticTree.RightNode);
                    }
                }
               Console.WriteLine("value " + parSemanticTree.Value.Lexeme+ " type "+parSemanticTree.Value.TokenType);
            }
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
                    PortableCodes.Add(new PortableCode(operationCode, result, operands));
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
        public List<string> GetPortableCodeSymboleTableText()
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
