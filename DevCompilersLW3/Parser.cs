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
        private List<Node<String>> nodes = new List<Node<String>>();
        private int white_space_count = 0;
        private Node<String> root = null;

        public Parser(List<Tokens> tokens)
        {
            this._tokens = tokens;
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
        public TokenNode ParseExp()
        {
            TokenNode result = Factor();
            while (curr_token._tokenType != TokenLab03.EOF && result != null && TermItems.Contains(curr_token._tokenType))
            {
                if (curr_token._tokenType == TokenLab03.ADD)
                {
                    Get_Next();
                    TokenNode rigthNode = Factor();
                    nodes.Add(new Node<String>("+",result,rigthNode));
                    result = new NodeAll("+",result, rigthNode); ;
                }
                else if (curr_token._tokenType == TokenLab03.MINUS)
                {
                    Get_Next();
                    TokenNode rigthNode = Factor();
                    nodes.Add(new Node<String>("-", result, rigthNode));
                    result = new NodeAll("-",result, rigthNode);
                }
            }

            return result;
        }
        public TokenNode Factor()
        {
            TokenNode factor = Term();
            while (curr_token._tokenType != TokenLab03.EOF && factor != null && FactorItems.Contains(curr_token._tokenType))
            {
                if (curr_token._tokenType == TokenLab03.MULTIPLY)
                {
                    
                    Get_Next();
                    TokenNode rigthNode = Term();
                    nodes.Add(new Node<String>("*", factor, rigthNode));
                    factor = new NodeAll("*",factor, rigthNode);
                }
                else if (curr_token._tokenType == TokenLab03.DIVISION)
                {
                    Get_Next();
                    TokenNode rigthNode = Term();
                    nodes.Add(new Node<String>("/", factor, rigthNode));
                    factor = new NodeAll("/",factor, rigthNode);
                }
            }
            return factor;
        }
        public TokenNode Term()
        {
            TokenNode term = null;
            if (curr_token._tokenType == TokenLab03.LBRACE)
            {
                Get_Next();
                term = ParseExp();
            }
            else if (curr_token._tokenType == TokenLab03.NUMBER)
            {
                term = new ASTLeaf(curr_token._value.ToString());   
            }
            Get_Next();
            return term;
        }
        public void Print()
        {
            for(var i = 0; i < nodes.Count; i++)
            {
                Console.WriteLine("Noeud "+i+" value "+nodes[i].value+ " Left "+nodes[i]._leftNode+" Right "+nodes[i]._rightNode);
                Console.WriteLine("Left is leaf : " + Node<String>.isLeaf(nodes[i]._leftNode)+ " Rigth is leaf : " + Node<String>.isLeaf(nodes[i]._rightNode));
            }
        }
        public void Print2()
        {
            root = Builde(nodes[nodes.Count - 1]);
            StringBuilder sb = new StringBuilder();
            TraverserPreOrder(sb,"","" ,root);
            Console.WriteLine(sb);
        }
        private Node<String> Builde(TokenNode node)
        {
            if (Node<String>.isLeaf(node))
            {
                ASTLeaf l = (ASTLeaf)node;
                return l.last;
            }
            Node<String> next = new Node<string>(node.value);
            next._leftNode = Builde(node._leftNode);
            next._rightNode = Builde(node._rightNode);
            return next;
        }
        
        private void TraverserPreOrder(StringBuilder parSb, string padding, string pointer, TokenNode parTree)
        {
            if(parTree != null)
            {
                parSb.Append(padding);
                parSb.Append(pointer);
                parSb.Append(parTree.value);
                parSb.Append("\n");
                StringBuilder paddingBuilder = new StringBuilder(padding);
                if (white_space_count > 0)
                {
                    paddingBuilder.Append("    ");
                }
                white_space_count++;
                String paddingForBoth = paddingBuilder.ToString();
                String pointerForRight = "|---";
                String pointerForLeft = "|---";
                TraverserPreOrder(parSb,paddingForBoth,pointerForLeft, parTree._leftNode);
                TraverserPreOrder(parSb, paddingForBoth, pointerForRight, parTree._rightNode); ;
            }
        }

    }

}
