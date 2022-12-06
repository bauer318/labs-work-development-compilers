using DevCompilersLW2;
using DevCompilersLW3;
using DevCompilersLW5;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW6
{
    public static class PortableCodeWorker
    {
        public static List<PortableCode> PortableCodesOpt { get; private set; } = new List<PortableCode>();

        public static List<PortableCode> PortableCodes { get; set; } = new List<PortableCode>();
        public static bool CanOperate(PortableCode parPortableCode)
        {
            //Console.WriteLine("Enter " + parPortableCode.toString());
            foreach(Token operand in parPortableCode.OperandList)
            {
                //Console.Write(" Type " + operand.TokenType);
                if (!TokenWorker.IsTokenConstantType(operand))
                {
                    //Console.WriteLine(" Out with false");
                    return false;
                }
                //Console.WriteLine();
            }
            //Console.WriteLine(" Out with true");
            return true;
        }
        public static bool Can(PortableCode parPortableCode)
        {
            double numberD;
            int numberInt;
            foreach(Token operand in parPortableCode.OperandList)
            {
                if (TokenWorker.IsTokenOperandDecimalType(operand.TokenType))
                {
                    if (Double.TryParse(operand.Lexeme.Replace('.', ','),out numberD))
                    {
                        ;
                    }
                    else
                    {
                        PortableCodesOpt.Add(parPortableCode);
                        return false;
                    }
                }
                else if(TokenWorker.IsTokenOperandIntegerType(operand.TokenType))
                {
                    if(Int32.TryParse(operand.Lexeme,out numberInt))
                    {
                        ;
                    }
                    else
                    {
                        PortableCodesOpt.Add(parPortableCode);
                        return false;
                    }
                }
            }
            return true;
        }
        public static bool OneOfOperandIsConstant(PortableCode parPortableCode)
        {
            double numberD;
            int numberInt;
            foreach (Token operand in parPortableCode.OperandList)
            {
                if (TokenWorker.IsTokenOperandDecimalType(operand.TokenType))
                {
                    if (Double.TryParse(operand.Lexeme.Replace('.', ','), out numberD))
                    {
                        return true;
                    }
                }
                else if (TokenWorker.IsTokenOperandIntegerType(operand.TokenType))
                {
                    if (Int32.TryParse(operand.Lexeme, out numberInt))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static List<PortableCode> GetLastList(List<PortableCode> portableCodes)
        {
            List<PortableCode> result = new List<PortableCode>();
            for(var i= portableCodes.Count-1; i >= 0; i--)
            {
                if (OneOfOperandIsConstant(portableCodes[i]))
                { 
                    PortableCode nextPortableCode = PortableCode.GetPortableCodeResultById(portableCodes[i].Id - 1, portableCodes);
                    {
                        foreach (Token operand in portableCodes[i].OperandList)
                        {
                            if (TokenWorker.IsTokenTypeOperator(operand))
                            {
                                if (TokenWorker.GetCodeOperationByTokenLexeme(operand.Lexeme).Equals(nextPortableCode.OperationCode))
                                {
                                    result.Add(RealizeOperation(portableCodes[i], nextPortableCode));

                                }
                            }
                        }
                        i--;
                    }
                }
                else
                {
                    result.Add(portableCodes[i]);
                }
            }

            return InverserList(result);

        }
        private static List<PortableCode> InverserList(List<PortableCode> portableCodes)
        {
            List<PortableCode> result = new List<PortableCode>();
            for(var i = portableCodes.Count-1; i>=0; i--)
            {
                result.Add(portableCodes[i]);
            }

            return result;
        }

        public static PortableCode RealizeOperation(PortableCode currentPortableCode, PortableCode nextPortableCode)
        {
            PortableCode portableCode = currentPortableCode;
            foreach(Token operand in currentPortableCode.OperandList)
            {
                if (!TokenWorker.IsTokenTypeOperator(operand))
                {
                    List<Token> operands = new List<Token>();
                    operands.Add(operand);
                    operands.Add(nextPortableCode.Result);
                    String operationCodeCurrent = portableCode.OperationCode;
                    PortableCode temp = new PortableCode(nextPortableCode.OperationCode, portableCode.Result, operands,portableCode.Id);
                    switch (portableCode.OperationCode)
                    {
                        case "add":
                            portableCode = PortableCodeWorker.RealizeAdd(temp);
                            break;
                        case "div":
                            portableCode = PortableCodeWorker.RealizeDiv(temp);
                            break;
                        case "sub":
                            portableCode = PortableCodeWorker.RealizeSub(temp);
                            break;
                        case "mul":
                            portableCode = PortableCodeWorker.RealizeMul(temp);
                            break;
                    }
                    portableCode.OperationCode = operationCodeCurrent;
                }
            }
            return portableCode;
        }
        public static bool IsResultValueToken(PortableCode parPortableCode)
        {
            return parPortableCode.OperationCode.Equals(String.Empty) &&
                parPortableCode.OperandList.Count == 0;
        }
        public static bool IsConstantInt2Float(PortableCode parPortableCode)
        {
            return parPortableCode.OperationCode.Equals("i2f") &&
                TokenWorker.IsTokenConstantType(parPortableCode.OperandList[0]);
        }
        public static PortableCode RealizeAdd(PortableCode parPortableCode)
        {
            Console.WriteLine("Add " + parPortableCode.OperandList[0].Lexeme + " and " + parPortableCode.OperandList[1].Lexeme);
            Token result = parPortableCode.OperandList[0];
            var id = parPortableCode.Id;
            if (TokenWorker.IsTokenOperandDecimalType(parPortableCode.OperandList[0].TokenType)
                || TokenWorker.IsTokenOperandDecimalType(parPortableCode.OperandList[1].TokenType))
            {
                result.TokenType = TokenType.CORRECT_DECIMAL_CONSTANT;
                double resultValue = Convert.ToDouble(parPortableCode.OperandList[0].Lexeme.Replace('.', ','))
                    + Convert.ToDouble(parPortableCode.OperandList[1].Lexeme.Replace('.', ','));
                result.Lexeme = resultValue.ToString().Replace(',', '.');
            }
            else
            {
                result.TokenType = TokenType.INTEGER_CONSTANT;
                int resultValue = Convert.ToInt32(parPortableCode.OperandList[0].Lexeme) +
                    Convert.ToInt32(parPortableCode.OperandList[1].Lexeme);
                result.Lexeme = resultValue.ToString();
            }
            return new PortableCode(parPortableCode.OperationCode,result,id);
        }

        public static PortableCode RealizeSub(PortableCode parPortableCode)
        {
            Console.WriteLine("Sub " + parPortableCode.OperandList[0].Lexeme + " and " + parPortableCode.OperandList[1].Lexeme);
            Token result = parPortableCode.OperandList[0];
            var id = parPortableCode.Id;
            if (TokenWorker.IsTokenOperandDecimalType(parPortableCode.OperandList[0].TokenType)
                || TokenWorker.IsTokenOperandDecimalType(parPortableCode.OperandList[1].TokenType))
            {
                result.TokenType = TokenType.CORRECT_DECIMAL_CONSTANT;
                double resultValue = Convert.ToDouble(parPortableCode.OperandList[0].Lexeme.Replace('.', ','))
                    - Convert.ToDouble(parPortableCode.OperandList[1].Lexeme.Replace('.', ','));
                result.Lexeme = resultValue.ToString().Replace(',', '.');
            }
            else
            {
                result.TokenType = TokenType.INTEGER_CONSTANT;
                int resultValue = Convert.ToInt32(parPortableCode.OperandList[0].Lexeme) -
                    Convert.ToInt32(parPortableCode.OperandList[1].Lexeme);
                result.Lexeme = resultValue.ToString();
            }
            return new PortableCode(parPortableCode.OperationCode, result,id);
        }

        public static PortableCode GetPortableCodeByIdResult(List<PortableCode> portableCodes, int parResultId)
        {
            foreach(PortableCode portableCode in portableCodes)
            {
                if (parResultId == portableCode.Result.AttributeValue)
                {
                    return portableCode;
                }
            }
            return null;
        }
        public static bool IsPortableCodeResult(List<PortableCode> portableCodes, int parResultId)
        {
            foreach (PortableCode portableCode in portableCodes)
            {
                if (parResultId == portableCode.Result.AttributeValue)
                {
                    return true;
                }
            }
            return false;
        }

        public static PortableCode GetAdditionPortableCodeResult(PortableCode portableCode)
        {
            PortableCode result = null;
            Token leftOperand = portableCode.OperandList[0];
            Token rightOperand = portableCode.OperandList[1];
            if(!(TokenWorker.IsTokenConstantType(leftOperand) && TokenWorker.IsTokenConstantType(rightOperand)))
            {
                result = portableCode;
            }
            else
            {
                Token resultToken;
                if (TokenWorker.IsTokenOperandDecimalType(leftOperand.TokenType)
                || TokenWorker.IsTokenOperandDecimalType(rightOperand.TokenType))
                {

                    double resultValue = Convert.ToDouble(leftOperand.Lexeme.Replace('.', ','))
                        + Convert.ToDouble(rightOperand.Lexeme.Replace('.', ','));
                    string lexeme = resultValue.ToString().Replace(',', '.');
                    resultToken = new Token(TokenType.CORRECT_DECIMAL_CONSTANT, lexeme, portableCode.Result.AttributeValue);
                }
                else
                {
                    int resultValue = Convert.ToInt32(leftOperand.Lexeme) + Convert.ToInt32(rightOperand.Lexeme);
                    string lexeme = resultValue.ToString();
                    resultToken = new Token(TokenType.INTEGER_CONSTANT, lexeme, portableCode.Result.AttributeValue);
                }
                result = new PortableCode(resultToken);
            }

            return result;
        }
        public static PortableCode GetSubstractionPortableCodeResult(PortableCode portableCode)
        {
            PortableCode result = null;
            Token leftOperand = portableCode.OperandList[0];
            Token rightOperand = portableCode.OperandList[1];
            if (!(TokenWorker.IsTokenConstantType(leftOperand) && TokenWorker.IsTokenConstantType(rightOperand)))
            {
                result = portableCode;
            }
            else
            {
                Token resultToken;
                if (TokenWorker.IsTokenOperandDecimalType(leftOperand.TokenType)
                || TokenWorker.IsTokenOperandDecimalType(rightOperand.TokenType))
                {

                    double resultValue = Convert.ToDouble(leftOperand.Lexeme.Replace('.', ','))
                        - Convert.ToDouble(rightOperand.Lexeme.Replace('.', ','));
                    string lexeme = resultValue.ToString().Replace(',', '.');
                    resultToken = new Token(TokenType.CORRECT_DECIMAL_CONSTANT, lexeme, portableCode.Result.AttributeValue);
                }
                else
                {
                    int resultValue = Convert.ToInt32(leftOperand.Lexeme) - Convert.ToInt32(rightOperand.Lexeme);
                    string lexeme = resultValue.ToString();
                    resultToken = new Token(TokenType.INTEGER_CONSTANT, lexeme, portableCode.Result.AttributeValue);
                }
                result = new PortableCode(resultToken);
            }

            return result;
        }
        public static PortableCode GetMultiplicationPortableCodeResult(PortableCode portableCode)
        {
            PortableCode result = null;
            Token leftOperand = portableCode.OperandList[0];
            Token rightOperand = portableCode.OperandList[1];
            if (!(TokenWorker.IsTokenConstantType(leftOperand) && TokenWorker.IsTokenConstantType(rightOperand)))
            {
                result = portableCode;
            }
            else
            {
                Token resultToken;
                if (TokenWorker.IsTokenOperandDecimalType(leftOperand.TokenType)
                || TokenWorker.IsTokenOperandDecimalType(rightOperand.TokenType))
                {

                    double resultValue = Convert.ToDouble(leftOperand.Lexeme.Replace('.', ','))
                        * Convert.ToDouble(rightOperand.Lexeme.Replace('.', ','));
                    string lexeme = resultValue.ToString().Replace(',', '.');
                    resultToken = new Token(TokenType.CORRECT_DECIMAL_CONSTANT, lexeme, portableCode.Result.AttributeValue);
                }
                else
                {
                    int resultValue = Convert.ToInt32(leftOperand.Lexeme) * Convert.ToInt32(rightOperand.Lexeme);
                    string lexeme = resultValue.ToString();
                    resultToken = new Token(TokenType.INTEGER_CONSTANT, lexeme, portableCode.Result.AttributeValue);
                }
                result = new PortableCode(resultToken);
            }

            return result;
        }
        public static PortableCode GetDivisionPortableCodeResult(PortableCode portableCode)
        {
            PortableCode result = null;
            Token leftOperand = portableCode.OperandList[0];
            Token rightOperand = portableCode.OperandList[1];
            if (!(TokenWorker.IsTokenConstantType(leftOperand) && TokenWorker.IsTokenConstantType(rightOperand)))
            {
                result = portableCode;
            }
            else
            {
                if(!(rightOperand.Lexeme.Contains("0.0") || rightOperand.Lexeme.Equals("0")))
                {
                    Token resultToken;
                    if (TokenWorker.IsTokenOperandDecimalType(leftOperand.TokenType)
                    || TokenWorker.IsTokenOperandDecimalType(rightOperand.TokenType))
                    {

                        double resultValue = Convert.ToDouble(leftOperand.Lexeme.Replace('.', ','))
                            / Convert.ToDouble(rightOperand.Lexeme.Replace('.', ','));
                        string lexeme = resultValue.ToString().Replace(',', '.');
                        resultToken = new Token(TokenType.CORRECT_DECIMAL_CONSTANT, lexeme, portableCode.Result.AttributeValue);
                    }
                    else
                    {
                        int resultValue = Convert.ToInt32(leftOperand.Lexeme) / Convert.ToInt32(rightOperand.Lexeme);
                        string lexeme = resultValue.ToString();
                        resultToken = new Token(TokenType.INTEGER_CONSTANT, lexeme, portableCode.Result.AttributeValue);
                    }
                    result = new PortableCode(resultToken);
                }
                else
                {
                    Console.WriteLine("divide by zero");
                }
            }

            return result;
        }
        public static PortableCode GetOp(PortableCode portableCode)
        {
            PortableCode result = null;
            if (portableCode.OperationCode.Equals("i2f"))
            {
                result = new PortableCode(portableCode.Result);
            }
            else
            {
                result = GetOp2(portableCode);
            }

            return result;
        }
        public static PortableCode GetOp2(PortableCode portableCode)
        {
            PortableCode result = null;
            if (portableCode != null)
            {
                Token leftOperand = portableCode.OperandList[0];
                Token rightOperand = portableCode.OperandList[1];
                string operationCode = portableCode.OperationCode;
                if (TokenWorker.IsTokenConstantType(leftOperand) && TokenWorker.IsTokenConstantType(rightOperand))
                {
                    switch (operationCode)
                    {
                        case "add":
                            result = GetAdditionPortableCodeResult(portableCode);
                            break;
                        case "div":
                            result = GetDivisionPortableCodeResult(portableCode);
                            break;
                        case "mul":
                            result = GetMultiplicationPortableCodeResult(portableCode);
                            break;
                        case "sub":
                            result = GetSubstractionPortableCodeResult(portableCode);
                            break;
                    }
                }
                else
                {
                    if (TokenWorker.IsTokenConstantType(leftOperand) && 
                        IsPortableCodeResult(PortableCodes, rightOperand.AttributeValue))
                    {
                        List<Token> tokensOperands = new List<Token>();
                        tokensOperands.Add(leftOperand);
                        tokensOperands.Add(GetOp2(GetPortableCodeByIdResult(PortableCodes, rightOperand.AttributeValue)).Result);
                        PortableCode tempPortableCode =
                            new PortableCode(portableCode.OperationCode, portableCode.Result, tokensOperands, portableCode.Result.AttributeValue);
                        switch (operationCode)
                        {
                            case "add":
                                result = GetAdditionPortableCodeResult(tempPortableCode);
                                break;
                            case "div":
                                result = GetDivisionPortableCodeResult(tempPortableCode);
                                break;
                            case "mul":
                                result = GetMultiplicationPortableCodeResult(tempPortableCode);
                                break;
                            case "sub":
                                result = GetSubstractionPortableCodeResult(tempPortableCode);
                                break;
                        }
                    }else if(TokenWorker.IsTokenConstantType(rightOperand) &&
                        IsPortableCodeResult(PortableCodes, leftOperand.AttributeValue))
                    {
                        List<Token> tokensOperands = new List<Token>();
                        tokensOperands.Add(rightOperand);
                        tokensOperands.Add(GetOp2(GetPortableCodeByIdResult(PortableCodes, leftOperand.AttributeValue)).Result);
                        PortableCode tempPortableCode =
                            new PortableCode(portableCode.OperationCode, portableCode.Result, tokensOperands, portableCode.Result.AttributeValue);
                        switch (operationCode)
                        {
                            case "add":
                                result = GetAdditionPortableCodeResult(tempPortableCode);
                                break;
                            case "div":
                                result = GetDivisionPortableCodeResult(tempPortableCode);
                                break;
                            case "mul":
                                result = GetMultiplicationPortableCodeResult(tempPortableCode);
                                break;
                            case "sub":
                                result = GetSubstractionPortableCodeResult(tempPortableCode);
                                break;
                        }
                    }else if(IsPortableCodeResult(PortableCodes, leftOperand.AttributeValue) 
                        && IsPortableCodeResult(PortableCodes, rightOperand.AttributeValue))
                    {
                        List<Token> tokensOperands = new List<Token>();
                        tokensOperands.Add(GetOp2(GetPortableCodeByIdResult(PortableCodes, rightOperand.AttributeValue)).Result);
                        tokensOperands.Add(GetOp2(GetPortableCodeByIdResult(PortableCodes, leftOperand.AttributeValue)).Result);
                        PortableCode tempPortableCode =
                            new PortableCode(portableCode.OperationCode, portableCode.Result, tokensOperands, portableCode.Result.AttributeValue);
                        switch (operationCode)
                        {
                            case "add":
                                result = GetAdditionPortableCodeResult(tempPortableCode);
                                break;
                            case "div":
                                result = GetDivisionPortableCodeResult(tempPortableCode);
                                break;
                            case "mul":
                                result = GetMultiplicationPortableCodeResult(tempPortableCode);
                                break;
                            case "sub":
                                result = GetSubstractionPortableCodeResult(tempPortableCode);
                                break;
                        }
                    }else
                    {
                        result = portableCode;
                    }
                }
            }
            return result;
        }
        public static PortableCode RealizeMul(PortableCode parPortableCode)
        {
            Console.WriteLine("Mul " + parPortableCode.OperandList[0].Lexeme + " and " + parPortableCode.OperandList[1].Lexeme);
            Token result = parPortableCode.OperandList[0];
            var id = parPortableCode.Id;
            if (TokenWorker.IsTokenOperandDecimalType(parPortableCode.OperandList[0].TokenType)
                || TokenWorker.IsTokenOperandDecimalType(parPortableCode.OperandList[1].TokenType))
            {
                result.TokenType = TokenType.CORRECT_DECIMAL_CONSTANT;
                double resultValue = Convert.ToDouble(parPortableCode.OperandList[0].Lexeme.Replace('.', ','))
                    * Convert.ToDouble(parPortableCode.OperandList[1].Lexeme.Replace('.', ','));
                result.Lexeme = resultValue.ToString().Replace(',', '.');
            }
            else
            {
                result.TokenType = TokenType.INTEGER_CONSTANT;
                int resultValue = Convert.ToInt32(parPortableCode.OperandList[0].Lexeme) *
                    Convert.ToInt32(parPortableCode.OperandList[1].Lexeme);
                result.Lexeme = resultValue.ToString();
            }
            return new PortableCode(parPortableCode.OperationCode, result,id);
        }

        public static PortableCode RealizeDiv(PortableCode parPortableCode)
        {
            Console.WriteLine("Div " + parPortableCode.OperandList[0].Lexeme + " and " + parPortableCode.OperandList[1].Lexeme);
            Token result = parPortableCode.OperandList[0];
            var id = parPortableCode.Id;

            if (TokenWorker.IsTokenOperandDecimalType(parPortableCode.OperandList[0].TokenType)
                || TokenWorker.IsTokenOperandDecimalType(parPortableCode.OperandList[1].TokenType))
            {
                try
                {
                    result.TokenType = TokenType.CORRECT_DECIMAL_CONSTANT;
                    double resultValue = Convert.ToDouble(parPortableCode.OperandList[0].Lexeme.Replace('.', ','))
                        / Convert.ToDouble(parPortableCode.OperandList[1].Lexeme.Replace('.', ','));
                    result.Lexeme = resultValue.ToString().Replace(',', '.');
                }catch(ArithmeticException a)
                {
                    throw new ArithmeticException("Divide by zero", a);
                }
               
            }
            else
            {
                Console.WriteLine("On div avec Int ");
                try { 
                result.TokenType = TokenType.INTEGER_CONSTANT;
                int resultValue = Convert.ToInt32(parPortableCode.OperandList[0].Lexeme) /
                    Convert.ToInt32(parPortableCode.OperandList[1].Lexeme);
                result.Lexeme = resultValue.ToString();
                }
                catch (ArithmeticException a)
                {
                    throw new ArithmeticException("Divide by zero", a);
                }
            }
            return new PortableCode(parPortableCode.OperationCode, result,id);
        }
        /*public static PortableCode RealizeInt2Float(PortableCode parPortableCode)
        {
            Token result = parPortableCode.OperandList[0];
            result.TokenType = TokenType.CORRECT_DECIMAL_CONSTANT;
            result.lexeme +=  ".0";
            return new PortableCode(parPortableCode.OperationCode, result);
        }*/

    }
}
