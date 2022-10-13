using DevCompilersLW2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevCompilersLW3
{
    public class ASTWorker
    {
        private List<ASTNode> _astNodes;
        private string _astFileName = "syntax_tree.txt";
        public List<ASTNode> ASTNodes
        {
            get
            {
                return _astNodes;
            }
        }
        public ASTWorker()
        {
            _astNodes = new List<ASTNode>();
        }
        public List<int> GetOperatorsPosition(List<Token> parTokens)
        {
            List<int> result = new List<int>();
            for (var i = 0; i < parTokens.Count; i++)
            {
                if (TokenWorker.IsTokenTypeOperator(parTokens[i]) || parTokens[i].TokenType == TokenType.EQUAL_SIGN)
                {
                    result.Add(i);
                }
            }

            return result;
        }
        private Token ToLeft(int currentOperatorIndex, List<Token> parTokens)
        {
            for (var i = currentOperatorIndex - 1; i >= 0; i--)
            {
                if (TokenWorker.IsOperand(parTokens[i].TokenType))
                {
                    return parTokens[i];
                }
            }
            return null;
        }
        private Token ToRight(int currentOperatorIndex, List<Token> parTokens)
        {
            for (var i = currentOperatorIndex + 1; i < parTokens.Count; i++)
            {
                if (TokenWorker.IsOperand(parTokens[i].TokenType))
                {
                    return parTokens[i];
                }
            }
            return null;
        }
        public void Print(List<int> listPos, List<Token> parTokens)
        {
            ASTNode astNode;
            var leftTokenLenght = 0;
            for (var i = 0; i < listPos.Count - 1; i++)
            {
                leftTokenLenght = ToLeft(listPos[i], parTokens).TokenType.GetTokenNodeDescription(ToLeft(listPos[i], parTokens)).Length;
                if (i == 0)
                {
                    Console.WriteLine(GetWhitespace(((i + 1) * 3)+leftTokenLenght) + parTokens[listPos[i]].TokenType.GetTokenNodeDescription(parTokens[listPos[i]]));
                    
                }
                astNode = new ASTNode(parTokens[listPos[i]]);
                astNode.Left =new ASTNode(ToLeft(listPos[i], parTokens));
                astNode.Right = new ASTNode(parTokens[listPos[i + 1]]);
                _astNodes.Add(astNode);
                Console.WriteLine(GetWhitespace((i * 6)+leftTokenLenght) + "/---|---\\");
                Console.WriteLine(GetWhitespace((i * 6)-leftTokenLenght) + ToLeft(listPos[i], parTokens).TokenType.GetTokenNodeDescription(ToLeft(listPos[i], parTokens))
                    + GetWhitespace((7)) + parTokens[listPos[i + 1]].TokenType.GetTokenNodeDescription(parTokens[listPos[i + 1]]));
            }
            astNode = new ASTNode(parTokens[listPos[listPos.Count-1]]);
            astNode.Left = new ASTNode(ToLeft(listPos[listPos.Count - 1], parTokens));
            astNode.Right = new ASTNode(parTokens[listPos[listPos.Count - 1]]);
            _astNodes.Add(astNode);
            leftTokenLenght = ToLeft(listPos[listPos.Count - 1], parTokens).TokenType.GetTokenNodeDescription(ToLeft(listPos[listPos.Count - 1], parTokens)).Length;
            Console.WriteLine(GetWhitespace(((listPos.Count - 1) * 6)+leftTokenLenght) + "/---|---\\");
            Console.WriteLine(GetWhitespace(((listPos.Count - 1) * 6)-leftTokenLenght) + 
                ToLeft(listPos[listPos.Count - 1], parTokens).TokenType.GetTokenNodeDescription(ToLeft(listPos[listPos.Count - 1], parTokens)) +
                GetWhitespace(((listPos.Count - 1) * 3)) + 
                ToRight(listPos[listPos.Count - 1], parTokens).TokenType.GetTokenNodeDescription(ToRight(listPos[listPos.Count - 1], parTokens)));
        }
        private string GetWhitespace(int number)
        {
            string result = "";
            for (var i = 0; i <= number; i++)
            {
                result += " ";
            }
            return result;
        }
    }
}
