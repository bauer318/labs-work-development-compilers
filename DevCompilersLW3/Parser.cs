using DevCompilersLW2;
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
        public List<AST> asts = new List<AST>();
        private List<Node<String>> nodes = new List<Node<String>>();
        private int it = 0;
        private Node<String> root = null;


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
                    asts.Add(new ASTPlus(result, rigthNode));
                    //Console.WriteLine("+ L: " + result + " R " + rigthNode);
                    nodes.Add(new Node<String>("+",result,rigthNode));
                    result = new ASTPlus(result, rigthNode);
                }
                else if (curr_token._tokenType == TokenLab03.MINUS)
                {
                    Get_Next();
                    AST rigthNode = Factor();
                    //Ici on doit ajouter le noeud
                    asts.Add(new ASTMinus(result, rigthNode));
                    //Console.WriteLine("- L: " + result + " R " + rigthNode);
                    nodes.Add(new Node<String>("-", result, rigthNode));
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
                    //Console.WriteLine("* L: " + factor + " R " + rigthNode);
                    nodes.Add(new Node<String>("*", factor, rigthNode));
                    factor = new ASTMultiply(factor, rigthNode);
                }
                else if (curr_token._tokenType == TokenLab03.DIVISION)
                {
                    Get_Next();
                    AST rigthNode = Term();
                    //Ici on doit ajouter le noeud
                    asts.Add(new ASTPlus(factor, rigthNode));
                    //Console.WriteLine("/ L: " + factor + " R " + rigthNode);
                    nodes.Add(new Node<String>("/", factor, rigthNode));
                    factor = new ASTDivide(factor, rigthNode);
                }
            }
            return factor;
        }
        public AST Term()
        {
            it++;
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
        public void Print()
        {
            for(var i = 0; i < nodes.Count; i++)
            {
                Console.WriteLine("Noeud "+i+" Operator "+nodes[i].Operator+ " Left "+nodes[i]._leftNode+" Right "+nodes[i]._rightNode);
                Console.WriteLine("Left is leaf : " + Node<String>.isLeaf(nodes[i]._leftNode)+ " Rigth is leaf : " + Node<String>.isLeaf(nodes[i]._rightNode));
            }
        }
        public void Print2()
        {
            root = Builde(nodes[nodes.Count - 1]);
            Console.WriteLine(root._rightNode._rightNode._rightNode._leftNode._rightNode._leftNode.Operator);
        }
        private Node<String> Builde(AST node)
        {
            if (Node<String>.isLeaf(node))
            {
                ASTLeaf l = (ASTLeaf)node;
                return l.last;
            }
            Node<String> next = new Node<string>(node.Operator);
            next._leftNode = Builde(node._leftNode);
            next._rightNode = Builde(node._rightNode);
            return next;
        }

    }

}
