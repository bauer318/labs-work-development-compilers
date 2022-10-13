using DevCompilersLW2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevCompilersLW3
{
    public class Parser
    {
        private List<TokenLab03> TermItems = new List<TokenLab03>() { TokenLab03.ADD, TokenLab03.MINUS };
        private List<TokenLab03> FactorItems = new List<TokenLab03>() { TokenLab03.MULTIPLY, TokenLab03.DIVISION };
        private readonly List<Tokens> _tokens;
        private int pos = 0;
        private Tokens curr_token = null;
        public List<AST> asts = new List<AST>();


        public Parser(List<Tokens> tokens)
        {
            this._tokens = tokens;
            // set the current token
            Get_Next();
        }
        public Parser()
        {

        }
        public void PrintTree(List<AST> parAST)
        {
            for(var i = 0; i < parAST.Count; i++)
            {
                Console.WriteLine(parAST[i].ToString());
                
            }

        }
        public void T(StringBuilder sb, AST ast)
        {
            if (ast != null)
            {
                sb.Append(ast.GetNodeHead());
                sb.Append("\n");
                T(sb, ast.GetLeftNode());
                T(sb, ast.GetRightNode());
            }
        }

        private void Get_Next()
        {
            if (pos < this._tokens.Count)
            {
                curr_token = this._tokens[pos];
                pos++;
            }
        }
        public List<int> GetOperatorsPosition(List<Token> parTokens)
        {
            List<int> result = new List<int>();
            for(var i=0; i < parTokens.Count; i++)
            {
                if(TokenWorker.IsTokenTypeOperator(parTokens[i]) || parTokens[i].TokenType==TokenType.EQUAL_SIGN)
                {
                    result.Add(i);
                }
            }

            return result;
        }
        private Token ToLeft(int currentOperatorIndex, List<Token> parTokens)
        {
            for(var i = currentOperatorIndex - 1; i >= 0; i--)
            {
                if(TokenWorker.IsOperand(parTokens[i].TokenType))
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
            for(var i = 0; i<listPos.Count-1; i++)
            {
                if (i == 0)
                {
                    Console.WriteLine(GetWhitespace((i + 1) * 3) +"<"+parTokens[listPos[i]].Lexeme+">");
                }
                Console.WriteLine(GetWhitespace(i * 6)+"/---|---\\");
                Console.WriteLine(GetWhitespace(i*6)+ToLeft(listPos[i],parTokens).Lexeme+GetWhitespace((7))+"<"+parTokens[listPos[i+1]].Lexeme+">");
            }
            Console.WriteLine(GetWhitespace((listPos.Count-1) * 6) + "/---|---\\");
            Console.WriteLine(GetWhitespace((listPos.Count - 1) * 6) + ToLeft(listPos[listPos.Count - 1], parTokens).Lexeme+ 
                GetWhitespace(((listPos.Count - 1) * 3)) + ToRight(listPos[listPos.Count - 1], parTokens).Lexeme);
        }
        private string GetWhitespace(int number)
        {
            string result = "";
            for(var i=0; i <= number; i++)
            {
                result += " ";
            }
            return result;
        }
        public AST ParseExp()
        {
            AST result = Factor();
            while (curr_token._tokenType != TokenLab03.EOF && result != null && TermItems.Contains(curr_token._tokenType))
            {
                if (curr_token._tokenType == TokenLab03.ADD)
                {
                    Get_Next();
                    AST rigthNode = Factor();
                    //Ici on doit ajouter le noeud
                    asts.Add(new ASTPlus(result, rigthNode));
                    result = new ASTPlus(result, rigthNode);
                }
                else if (curr_token._tokenType == TokenLab03.MINUS)
                {
                    Get_Next();
                    AST rigthNode = Factor();
                    //Ici on doit ajouter le noeud
                    asts.Add(new ASTMinus(result, rigthNode));
                    result = new ASTMinus(result, rigthNode);
                }
            }

            return result;
        }
        public AST Factor()
        {
            AST factor = Term();
            while (curr_token._tokenType != TokenLab03.EOF && factor != null && FactorItems.Contains(curr_token._tokenType))
            {
                if (curr_token._tokenType == TokenLab03.MULTIPLY)
                {
                    Get_Next();
                    AST rigthNode = Term();
                    //Ici on doit ajouter le noeud
                    asts.Add(new ASTMultiply(factor, rigthNode));
                    factor = new ASTMultiply(factor, rigthNode);
                }
                else if (curr_token._tokenType == TokenLab03.DIVISION)
                {
                    Get_Next();
                    AST rigthNode = Term();
                    //Ici on doit ajouter le noeud
                    asts.Add(new ASTPlus(factor, rigthNode));
                    factor = new ASTDivide(factor, rigthNode);
                }
            }
            return factor;
        }
        public AST Term()
        {
            AST term = null;

            if (curr_token._tokenType == TokenLab03.LBRACE)
            {
                Get_Next();
                term = ParseExp();
                if (curr_token._tokenType != TokenLab03.RBRACE)
                {
                    Console.WriteLine("Missing )");
                }
            }
            else if (curr_token._tokenType == TokenLab03.NUMBER)
            {
                term = new ASTLeaf((decimal)curr_token._value);
            }

            Get_Next();
            return term;
        }
    }
}
