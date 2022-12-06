using DevCompilersLW2;
using DevCompilersLW3;
using DevCompilersLW5;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW6
{
    public class PortableCodeOptimizator
    {
        public List<PortableCode> PortableCodesOpt { get; set; }
        public SymbolTable SymbolTable { get; }

        public PortableCodeOptimizator(List<PortableCode> parPortableCodes, SymbolTable parSymbolTable)
        {
            SymbolTable = parSymbolTable;
            PortableCodesOpt = parPortableCodes;
        }

        private static TokenNode<Token> GetTokenOperatorResultType(TokenNode<Token> parLeftNode, TokenNode<Token> parRightNode)
        {
            TokenNode<Token> result = new TokenNode<Token>(new Token(TokenType.INVALID, "CONVERTED"));
            if (TokenNode<Token>.IsLeafToken(parLeftNode) && TokenNode<Token>.IsLeafToken(parRightNode))
            {
                if (!TokenWorker.IsTokenOperandEqualType(parLeftNode.Value.TokenType, parRightNode.Value.TokenType))
                {
                    result = new TokenNode<Token>(new Token(TokenType.INT_2_FLOAT, "CONVERTED"));
                }
                else
                {
                    result = parLeftNode;
                }
            }
            else
            {
                if (TokenNode<Token>.IsLeafToken(parLeftNode))
                {
                    if (parRightNode != null)
                        result = GetTokenOperatorResultType(parLeftNode, GetTokenOperatorResultType(parRightNode.LeftNode, parRightNode.RightNode));
                }
                else if (TokenNode<Token>.IsLeafToken(parRightNode))
                {
                    result = GetTokenOperatorResultType(GetTokenOperatorResultType(parLeftNode.LeftNode, parLeftNode.RightNode), parRightNode);

                }
                else
                {
                    if (parRightNode != null)
                        result = GetTokenOperatorResultType(GetTokenOperatorResultType(parLeftNode.LeftNode, parLeftNode.RightNode),
                            GetTokenOperatorResultType(parRightNode.LeftNode, parRightNode.RightNode));
                    else
                        result = GetTokenOperatorResultType(parLeftNode.LeftNode, parLeftNode.RightNode);
                }
            }

            return result;
        }

        private static TokenNode<Token> Get1(TokenNode<Token> tokenNode)
        {
            TokenNode<Token> result = null;
            if (tokenNode != null)
            {
                TokenNode<Token> leftNode = null;
                TokenNode<Token> rigthNode = null;
                if (tokenNode.LeftNode != null)
                {
                    leftNode = tokenNode.LeftNode;
                }
                if(tokenNode.RightNode != null)
                {
                    rigthNode = tokenNode.RightNode;
                }
                if(leftNode!=null)
                {
                    if (TokenNode<Token>.IsLeafToken(leftNode) && TokenNode<Token>.IsLeafToken(rigthNode))
                    {
                        switch (tokenNode.Value.Lexeme)
                        {
                            case "+":
                                return GetAdditionPortableCodeResult(leftNode, rigthNode);

                            case "-":
                                return GetSubstractionPortableCodeResult(leftNode, rigthNode);

                            case "/":
                                return GetDivisionPortableCodeResult(leftNode, rigthNode);

                            case "*":
                                return GetMultiplicationPortableCodeResult(leftNode, rigthNode);

                        }
                    }
                    else
                        
                    {
                        result = new TokenNode<Token>(tokenNode.Value);
                        if (TokenNode<Token>.IsLeafToken(leftNode))
                        {
                            result.LeftNode = leftNode;
                            if (rigthNode != null)
                            {
                                switch (tokenNode.Value.Lexeme)
                                {
                                    case "+":
                                        result.RightNode = GetAdditionPortableCodeResult(leftNode, Get1(rigthNode));
                                        break;
                                    case "-":
                                        result.RightNode = GetSubstractionPortableCodeResult(leftNode, Get1(rigthNode));
                                        break;
                                    case "/":
                                        result.RightNode = GetDivisionPortableCodeResult(leftNode, Get1(rigthNode));
                                        break;
                                    case "*":
                                        result.RightNode = GetMultiplicationPortableCodeResult(leftNode, Get1(rigthNode));
                                        break;
                                }
                            }
                        }
                        else if (TokenNode<Token>.IsLeafToken(rigthNode))
                        {
                            result.RightNode = rigthNode;
                            switch (tokenNode.Value.Lexeme)
                            {
                                case "+":
                                    result.LeftNode = GetAdditionPortableCodeResult(Get1(leftNode), rigthNode);
                                    break;
                                case "-":
                                    result.LeftNode = GetSubstractionPortableCodeResult(Get1(leftNode), rigthNode);
                                    break;
                                case "/":
                                    result.LeftNode = GetDivisionPortableCodeResult(Get1(leftNode), rigthNode);
                                    break;
                                case "*":
                                    result.LeftNode = GetMultiplicationPortableCodeResult(Get1(leftNode), rigthNode);
                                    break;
                            }
                        }
                    }
                }
            }
            return result;
        }

        public static TokenNode<Token> GetAdditionPortableCodeResult(TokenNode<Token> parLeftNode, TokenNode<Token> parRightNode)
        {
            TokenNode<Token> result = null;
            if(TokenWorker.IsConstantType(parLeftNode.Value.TokenType) && TokenWorker.IsConstantType(parRightNode.Value.TokenType))
            {
                if (TokenWorker.IsTokenOperandDecimalType(parLeftNode.Value.TokenType) ||
                TokenWorker.IsTokenOperandDecimalType(parRightNode.Value.TokenType))
                {
                    double resultValue = Convert.ToDouble(parLeftNode.Value.Lexeme.Replace('.', ','))
                                + Convert.ToDouble(parRightNode.Value.Lexeme.Replace('.', ','));
                    string lexeme = resultValue.ToString().Replace(',', '.');
                    result = new TokenNode<Token>(new Token(TokenType.CORRECT_DECIMAL_CONSTANT, lexeme));
                }
                else
                {
                    int resultValue = Convert.ToInt32(parLeftNode.Value.Lexeme) + Convert.ToInt32(parRightNode.Value.Lexeme);
                    string lexeme = resultValue.ToString();
                    result = new TokenNode<Token>(new Token(TokenType.INTEGER_CONSTANT, lexeme));
                }
            }
            else
            {
                if (TokenWorker.IsConstantType(parLeftNode.Value.TokenType))
                {
                    result = GetAdditionPortableCodeResult(parLeftNode, GetAdditionPortableCodeResult(parRightNode.LeftNode, parRightNode.RightNode));
                }//else if(TokenWorker.IsConstantType(parLeftNode.Value.TokenType))
            }
            return result;
        }
        public static TokenNode<Token> GetSubstractionPortableCodeResult(TokenNode<Token> parLeftNode, TokenNode<Token> parRightNode)
        {
            TokenNode<Token> result;
            if (TokenWorker.IsTokenOperandDecimalType(parLeftNode.Value.TokenType) ||
                TokenWorker.IsTokenOperandDecimalType(parRightNode.Value.TokenType))
            {
                double resultValue = Convert.ToDouble(parLeftNode.Value.Lexeme.Replace('.', ','))
                            - Convert.ToDouble(parRightNode.Value.Lexeme.Replace('.', ','));
                string lexeme = resultValue.ToString().Replace(',', '.');
                result = new TokenNode<Token>(new Token(TokenType.CORRECT_DECIMAL_CONSTANT, lexeme));
            }
            else
            {
                int resultValue = Convert.ToInt32(parLeftNode.Value.Lexeme) - Convert.ToInt32(parRightNode.Value.Lexeme);
                string lexeme = resultValue.ToString();
                result = new TokenNode<Token>(new Token(TokenType.INTEGER_CONSTANT, lexeme));
            }
            return result;
        }
        public static TokenNode<Token> GetMultiplicationPortableCodeResult(TokenNode<Token> parLeftNode, TokenNode<Token> parRightNode)
        {
            TokenNode<Token> result;
            if (TokenWorker.IsTokenOperandDecimalType(parLeftNode.Value.TokenType) ||
                TokenWorker.IsTokenOperandDecimalType(parRightNode.Value.TokenType))
            {
                double resultValue = Convert.ToDouble(parLeftNode.Value.Lexeme.Replace('.', ','))
                            * Convert.ToDouble(parRightNode.Value.Lexeme.Replace('.', ','));
                string lexeme = resultValue.ToString().Replace(',', '.');
                result = new TokenNode<Token>(new Token(TokenType.CORRECT_DECIMAL_CONSTANT, lexeme));
            }
            else
            {
                int resultValue = Convert.ToInt32(parLeftNode.Value.Lexeme) * Convert.ToInt32(parRightNode.Value.Lexeme);
                string lexeme = resultValue.ToString();
                result = new TokenNode<Token>(new Token(TokenType.INTEGER_CONSTANT, lexeme));
            }
            return result;
        }
        public static TokenNode<Token> GetDivisionPortableCodeResult(TokenNode<Token> parLeftNode, TokenNode<Token> parRightNode)
        {
            TokenNode<Token> result;
            if (TokenWorker.IsTokenOperandDecimalType(parLeftNode.Value.TokenType) ||
                TokenWorker.IsTokenOperandDecimalType(parRightNode.Value.TokenType))
            {
                double resultValue = Convert.ToDouble(parLeftNode.Value.Lexeme.Replace('.', ','))
                            / Convert.ToDouble(parRightNode.Value.Lexeme.Replace('.', ','));
                string lexeme = resultValue.ToString().Replace(',', '.');
                result = new TokenNode<Token>(new Token(TokenType.CORRECT_DECIMAL_CONSTANT, lexeme));
            }
            else
            {
                int resultValue = Convert.ToInt32(parLeftNode.Value.Lexeme) / Convert.ToInt32(parRightNode.Value.Lexeme);
                string lexeme = resultValue.ToString();
                result = new TokenNode<Token>(new Token(TokenType.INTEGER_CONSTANT, lexeme));
            }
            return result;
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
                    if (parSemanticTree.LeftNode != null)
                    {
                        operands.Add(parSemanticTree.LeftNode.Value);
                        CreatePortableCodeList(parSemanticTree.LeftNode);
                    }
                    if (parSemanticTree.RightNode != null)
                    {
                        operands.Add(parSemanticTree.RightNode.Value);
                        CreatePortableCodeList(parSemanticTree.RightNode);
                    }
                }
            }
        }
        private static int id = 0;
        public static TokenNode<Token> GetTokenResult(TokenNode<Token> tokenNode)
        {
            TokenNode<Token> result = tokenNode;
            id++;
            if (tokenNode != null)
            {
                if (IsFirstOperation(tokenNode))
                {
                    Console.WriteLine("Last after ");
                    Console.WriteLine(tokenNode.LeftNode.Value.Lexeme + " and ");
                }
                else
                {
                    GetTokenResult(tokenNode.LeftNode);
                    GetTokenResult(tokenNode.RightNode);
                }
                /*TokenNode<Token> left = null;
                TokenNode<Token> right = null;
                if (tokenNode.LeftNode != null)
                {
                    left = tokenNode.LeftNode;
                }
                if (tokenNode.RightNode != null)
                {
                    right = tokenNode.RightNode;
                }
                if(left != null)
                {
                    if (!TokenNode<Token>.IsLeafToken(left))
                    {
                        Console.WriteLine("Not leaf Left " + left.Value.Lexeme);
                        left = GetTokenResult(left);
                    }
                    Console.WriteLine("leaf Left " + left.Value.Lexeme);
                }
                if(right != null)
                {
                    if (!TokenNode<Token>.IsLeafToken(right))
                    {
                        Console.WriteLine("Not leaf right " + right.Value.Lexeme);
                        right = GetTokenResult(right);
                    }
                    Console.WriteLine("Not leaf right " + right.Value.Lexeme);
                }*/

            }
            return result;
        }
        private static bool IsFirstOperation(TokenNode<Token> tokenNode)
        {
            if (tokenNode.RightNode == null)
            {
                return TokenNode<Token>.IsLeafToken(tokenNode.LeftNode);
            }
            return tokenNode != null && TokenNode<Token>.IsLeafToken(tokenNode.LeftNode) && 
                TokenNode<Token>.IsLeafToken(tokenNode.RightNode);
        }

        public List<PortableCode> Go()
        {
            List<PortableCode> result = new List<PortableCode>();
            foreach(PortableCode portableCode in PortableCodesOpt)
            {
                if (PortableCodeWorker.Can(portableCode))
                {
                    switch (portableCode.OperationCode)
                    {
                        case "add":
                            result.Add(PortableCodeWorker.RealizeAdd(portableCode));
                            break;
                        case "div":
                            result.Add(PortableCodeWorker.RealizeDiv(portableCode));
                            break;
                        case "sub":
                            result.Add(PortableCodeWorker.RealizeSub(portableCode));
                            break;
                        case "mul":
                            result.Add(PortableCodeWorker.RealizeMul(portableCode));
                            break;
                        /*case "i2f":
                            result.Add(PortableCodeWorker.RealizeInt2Float(portableCode));
                            break;*/
                    }
                }
                else
                {
                    result.Add(portableCode);
                }
            }
            return result;
        }

    }
}
