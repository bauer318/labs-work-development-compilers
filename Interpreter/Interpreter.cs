using DevCompilersLW2;
using DevCompilersLW3;
using DevCompilersLW5;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter
{
    public class Interpreter
    {

        private readonly List<PortableCode> _portableCodes;
        private readonly SymbolTable _symbolTable;
       
        public Interpreter(List<PortableCode> parPortableCode, SymbolTable parSymbolTable)
        {
            _portableCodes = parPortableCode;
            _symbolTable = parSymbolTable;
            SymbolTableWorker.SymbolTable = _symbolTable;
        }


        public void InputValues()
        {
            _symbolTable.AttributeVariables.ForEach(attributeVariable => {
                if (!attributeVariable.IsTempVariable)
                {
                    while (true)
                    {
                        Console.Write($"Input [{InterpreterWorker.GetVariableTypeDescription(attributeVariable.TokenType)}] {attributeVariable.Name} = ");
                        var input = Console.ReadLine();
                        if (attributeVariable.TokenType == TokenType.CORRECT_INTEGER_IDENTIFICATOR)
                        {
                            if (int.TryParse(input, out var _))
                            {
                                attributeVariable.Value = new Token(TokenType.INTEGER_CONSTANT, input.ToString());
                                break;
                            }
                            else
                            {
                                Console.WriteLine("ERROR! Expected an integer's type variable ");
                            }
                        }
                        if (attributeVariable.TokenType == TokenType.CORRECT_DECIMAL_IDENTIFICATOR)
                        {
                            if (input.Contains(".") && double.TryParse(input.ToString().Replace(".", ","), out var _))
                            {
                                attributeVariable.Value = new Token(TokenType.CORRECT_DECIMAL_CONSTANT, input.ToString().Replace(",", "."));
                                break;
                            }
                            else
                            {
                                Console.WriteLine("ERROR! Expected an double's type variable ");
                            }
                        }
                    }
                }

            });
        }
        public void ProcessCalcul()
        {
            try
            {
                _portableCodes.ForEach(portableCode =>
                {
                    string operationCode = portableCode.OperationCode;
                    Token result = null;
                    switch (operationCode)
                    {
                        case "i2f":
                            result = CastVariable(portableCode);
                            break;
                        case "add":
                            result = GetPortableCodeResultTokenAddition(portableCode);
                            break;
                        case "div":
                            result = GetPortableCodeResultTokenDivision(portableCode);
                            break;
                        case "mul":
                            result = GetPortableCodeResultTokenMultiplication(portableCode);
                            break;
                        case "sub":
                            result = GetPortableCodeResultTokenSubtraction(portableCode);
                            break;
                    }
                    if (result != null)
                    {
                        SetTempVariableNumerciValue(portableCode, result);
                    }
                });

            }
            catch(DivideByZeroException e)
            {
                throw new DivByZeroCustomException("Div by zero");
            }
           
        }
  
        private AttributeVariable GetAttributeVariableById(int parId)
        {
            foreach(AttributeVariable a in _symbolTable.AttributeVariables)
            {
                if(a.Id == parId)
                {
                    return a;
                }
            }
            return null;
        }
        private Token CastVariable(PortableCode portableCode)
        {
            Token token = portableCode.OperandList[0];
            AttributeVariable attributeVariable = GetAttributeVariableById(token.AttributeValue);
            if (attributeVariable != null)
                token.Lexeme = attributeVariable.Value.Lexeme + ".0";
            else
                token.Lexeme += ".0";
            return new Token(TokenType.CORRECT_DECIMAL_CONSTANT, token.Lexeme, token.AttributeValue);
        }
        private Token GetPortableCodeResultTokenAddition(PortableCode portableCode)
        {
            Token firstOperand = portableCode.OperandList[0];
            Token secondOperand = portableCode.OperandList[1];
            Token result;
            if (IsVariable(firstOperand))
            {
                firstOperand = GetAttributeVariableById(firstOperand.AttributeValue).Value;
            }
            if (IsVariable(secondOperand))
            {
                secondOperand = GetAttributeVariableById(secondOperand.AttributeValue).Value;
            }
            if (firstOperand.TokenType == TokenType.CORRECT_DECIMAL_CONSTANT ||
                secondOperand.TokenType == TokenType.CORRECT_DECIMAL_CONSTANT)
            {
                double resultValue = Convert.ToDouble(firstOperand.Lexeme.Replace(".", ",")) +
                    Convert.ToDouble(secondOperand.Lexeme.Replace(".", ","));
                string lexeme = resultValue.ToString().Replace(",", ".");
                result = new Token(TokenType.CORRECT_DECIMAL_CONSTANT, lexeme,portableCode.Result.AttributeValue);
            }
            else
            {
                int resultValue = Convert.ToInt32(firstOperand.Lexeme) +
                    Convert.ToInt32(secondOperand.Lexeme);
                string lexeme = resultValue.ToString();
                result = new Token(TokenType.INTEGER_CONSTANT, lexeme, portableCode.Result.AttributeValue);
            }
            return result;
        }

        private bool IsVariable(Token parToken)
        {
            AttributeVariable attributeVariable = GetAttributeVariableById(parToken.AttributeValue);
            return attributeVariable != null && parToken.AttributeValue != 0;
        }

        private Token GetPortableCodeResultTokenSubtraction(PortableCode portableCode)
        {
            Token firstOperand = portableCode.OperandList[0];
            Token secondOperand = portableCode.OperandList[1];
            Token result;
            if (IsVariable(firstOperand))
            {
                firstOperand = GetAttributeVariableById(firstOperand.AttributeValue).Value;
            }
            if (IsVariable(secondOperand))
            {
                secondOperand = GetAttributeVariableById(secondOperand.AttributeValue).Value;
            }
            if (firstOperand.TokenType == TokenType.CORRECT_DECIMAL_CONSTANT ||
                secondOperand.TokenType == TokenType.CORRECT_DECIMAL_CONSTANT)
            {
                double resultValue = Convert.ToDouble(firstOperand.Lexeme.Replace(".", ",")) -
                    Convert.ToDouble(secondOperand.Lexeme.Replace(".", ","));
                string lexeme = resultValue.ToString().Replace(",", ".");
                result = new Token(TokenType.CORRECT_DECIMAL_CONSTANT, lexeme, portableCode.Result.AttributeValue);
            }
            else
            {
                int resultValue = Convert.ToInt32(firstOperand.Lexeme) -
                    Convert.ToInt32(secondOperand.Lexeme);
                string lexeme = resultValue.ToString();
                result = new Token(TokenType.INTEGER_CONSTANT, lexeme, portableCode.Result.AttributeValue);
            }
            return result;
        }

        private Token GetPortableCodeResultTokenMultiplication(PortableCode portableCode)
        {
            Token firstOperand = portableCode.OperandList[0];
            Token secondOperand = portableCode.OperandList[1];
            Token result;
            if (IsVariable(firstOperand))
            {
                firstOperand = SymbolTableWorker.GetAttributeVariableById(firstOperand.AttributeValue).Value;
            }
            if (IsVariable(secondOperand))
            {
                secondOperand = SymbolTableWorker.GetAttributeVariableById(secondOperand.AttributeValue).Value;
            }
            if (firstOperand.TokenType == TokenType.CORRECT_DECIMAL_CONSTANT ||
                secondOperand.TokenType == TokenType.CORRECT_DECIMAL_CONSTANT)
            {
                double resultValue = Convert.ToDouble(firstOperand.Lexeme.Replace(".", ",")) *
                    Convert.ToDouble(secondOperand.Lexeme.Replace(".", ","));
                string lexeme = resultValue.ToString().Replace(",", ".");
                result = new Token(TokenType.CORRECT_DECIMAL_CONSTANT, lexeme, portableCode.Result.AttributeValue);
            }
            else
            {
                int resultValue = Convert.ToInt32(firstOperand.Lexeme) *
                    Convert.ToInt32(secondOperand.Lexeme);
                string lexeme = resultValue.ToString();
                result = new Token(TokenType.INTEGER_CONSTANT, lexeme, portableCode.Result.AttributeValue);
            }
            return result;
        }

        private Token GetPortableCodeResultTokenDivision(PortableCode portableCode)
        {
            Token firstOperand = portableCode.OperandList[0];
            Token secondOperand = portableCode.OperandList[1];
            Token result;
            if (IsVariable(firstOperand))
            {
                firstOperand = GetAttributeVariableById(firstOperand.AttributeValue).Value;
            }
            if (IsVariable(secondOperand))
            {
                secondOperand = GetAttributeVariableById(secondOperand.AttributeValue).Value;
            }
            if (secondOperand.Lexeme.Equals("0") || secondOperand.Lexeme.Equals("0.0"))
            {
                throw new DivideByZeroException();
            }
            else
            {
                if (firstOperand.TokenType == TokenType.CORRECT_DECIMAL_CONSTANT ||
                secondOperand.TokenType == TokenType.CORRECT_DECIMAL_CONSTANT)
                {
                    double resultValue = Convert.ToDouble(firstOperand.Lexeme.Replace(".", ",")) /
                        Convert.ToDouble(secondOperand.Lexeme.Replace(".", ","));
                    string lexeme = resultValue.ToString().Replace(",", ".");
                    result = new Token(TokenType.CORRECT_DECIMAL_CONSTANT, lexeme, portableCode.Result.AttributeValue);
                }
                else
                {
                    int resultValue = Convert.ToInt32(firstOperand.Lexeme) /
                        Convert.ToInt32(secondOperand.Lexeme);
                    string lexeme = resultValue.ToString();
                    result = new Token(TokenType.INTEGER_CONSTANT, lexeme, portableCode.Result.AttributeValue);
                }
            }
            return result;
        }
    
        public void SetTempVariableNumerciValue(PortableCode parPortableCode, Token parValue)
        {
            AttributeVariable attributeVariable = GetAttributeVariableById(parPortableCode.Result.AttributeValue);
            if(attributeVariable != null)
            {
                attributeVariable.Value = parValue;
                attributeVariable.TokenType = parValue.TokenType;
            }
        }

        public void PrintResult()
        {
            PortableCode resultPortableCode = _portableCodes[_portableCodes.Count - 1];
            AttributeVariable attributeVariable = GetAttributeVariableById(resultPortableCode.Result.AttributeValue);
            string result = attributeVariable.Value.Lexeme;
            Console.WriteLine("Result " + result);
        }

        

    }
}
