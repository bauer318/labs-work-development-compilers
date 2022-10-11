using System;
using System.Collections.Generic;
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


        public Parser(List<Tokens> tokens)
        {
            this._tokens = tokens;
            // set the current token
            Get_Next();
        }

        private void Get_Next()
        {
            if (pos < this._tokens.Count)
            {
                curr_token = this._tokens[pos];
                pos++;
            }
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
                    result = new ASTPlus(result, rigthNode);
                }
                else if (curr_token._tokenType == TokenLab03.MINUS)
                {
                    Get_Next();
                    AST rigthNode = Factor();
                    //Ici on doit ajouter le noeud
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
                    factor = new ASTMultiply(factor, rigthNode);
                }
                else if (curr_token._tokenType == TokenLab03.DIVISION)
                {
                    Get_Next();
                    AST rigthNode = Term();
                    //Ici on doit ajouter le noeud
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
