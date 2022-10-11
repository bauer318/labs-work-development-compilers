using System;
using System.Linq;

namespace DevCompilersLW3
{
    public class Class1
    {
        char[] digits = { '0', '1', '2',
            '3', '4', '5', '6', '7', '8', '9' };
        char[] letters = {'_','A','B','C','D','E','F','G','H', 'I', 'J', 'K', 'L'
        ,'M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z','a','b','c','d','e','f'
        ,'g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z'};
        char[] op = { '+','-','*','/'};
        string s;
        int i, k, e;
        int stateFrom = 0;
        
        int state;
        public Class1(char[] parOp, char[] parLetters, char[] parDigits)
        {
            s = "=+)=";
            op = parOp;
            letters = parLetters;
            digits = parDigits;
        }
        private void Error()
        {
            Console.WriteLine("Error");
        }
        private void NextToken()
        {
            i++;
        }
        private bool InLetters(char parElement)
        {
            for(var i = 0; i < letters.Length; i++)
            {
                if (letters[i] == parElement)
                {
                    return true;
                }
            }
            return false;
        }
        private bool InDigits(char parElement)
        {
            for (var i = 0; i < digits.Length; i++)
            {
                if (digits[i] == parElement)
                {
                    return true;
                }
            }
            return false;
        }
        private bool InOperation(char parElement)
        {
            for (var i = 0; i < op.Length; i++)
            {
                if (op[i] == parElement)
                {
                    return true;
                }
            }
            return false;
        }
        private void Identifier()
        {

            while (i < s.Length && (InLetters(s.ElementAt(i+1)) || InDigits(s.ElementAt(i + 1))))
            {
                NextToken();
            }
        }
        private void Number()
        {
            while(i<s.Length && InDigits(s.ElementAt(i)))
            {
                NextToken();
            }
        }
        private bool IsOperationCharacterLeft(int parCurrentIndex)
        {
            bool result = false;
            if (parCurrentIndex - 1 >= 0)
            {
                return InOperation(s.ElementAt(parCurrentIndex - 1)) || s.ElementAt(i)=='=';
            }
            return result;
        }
        private bool IsOpenedBraceLeft(int parCurrentIndex)
        {
            bool result = false;
            if (parCurrentIndex - 1 >= 0)
            {
                return s.ElementAt(parCurrentIndex - 1)=='(';
            }
            return result;
        }
        private bool HasIdentificatorForEqualSymbol(int parCurrentIndex)
        {

            return parCurrentIndex - 1 == 0 && InLetters(s.ElementAt(parCurrentIndex - 1));
           
        }
        
        private int CountCLosedOpenedBrace()
        {
            var result = 0;
            for(var i=0; i < s.Length; i++)
            {
                if (s.ElementAt(i) == '(')
                {
                    result++;
                }
                else if (s.ElementAt(i) == ')')
                {
                    result--;
                }
            }

            return result;
        }
        //For k>0
        private void PrintErrorNotClosedBrace()
        {
            var count = 0;
            for(var j= s.Length - 1; j >= 0; j--)
            {
                if (s.ElementAt(j)==')')
                {
                    count++;
                }
                else if(s.ElementAt(j)=='(')
                {
                    count--;
                    if (count < 0)
                    {
                        Console.WriteLine("Not closed brace for " + s.ElementAt(j)+" at "+j);
                        count = 0;
                    }
                }
            }  
        }
        //For k<0
        private void PrintErrorNotOpenedBrace()
        {
            var count = 0;
            for(var j = 0; j < s.Length; j++)
            {
                if (s.ElementAt(j) == '(')
                {
                    count++;
                }
                else if(s.ElementAt(j)==')')
                {
                    count--;
                    if (count < 0)
                    {
                        Console.WriteLine("Not opened brace for " + s.ElementAt(j) + " at " + j);
                        count = 0;
                    }
                }
            }
            
        }
        private int GetPositionLastOpenedBrace()
        {
            for(var i = s.Length - 1; i >= 0; i--)
            {
                if (s.ElementAt(i) == '(')
                {
                    return i;
                }
            }
            return -1;
        }
        private bool IsCorrectPositionEgalSign(int i, int e)
        {
            return e == 1 && i == 1;
        }

        //private int CaseZeroRealize(int k, int i,string current,)

        public void MainMethode()
        {
            i = 0;
            k = 0;
            state = 0;
            e = 0;
            Console.WriteLine("Expression " + s);
            while (state != 3)
            {
                switch (state)
                {
                    case 0:
                        if (i < s.Length-1)
                        {
                            if (s.ElementAt(i) == '(')
                            {
                                k++;
                                NextToken();
                            }
                            else if(!(InDigits(s.ElementAt(i)) || InLetters(s.ElementAt(i))))
                            {
                                if (InOperation(s.ElementAt(i)))
                                {
                                    if (IsOperationCharacterLeft(i))
                                    {
                                        Console.WriteLine("Case 0 -- Successive operation at "+i);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Case 0 -- Not operand for " + s.ElementAt(i)+ " at "+i);
                                    }
                                }
                                else if(s.ElementAt(i)=='=')
                                {
                                    e++;
                                    if (e > 1)
                                    {
                                        Console.WriteLine("Many sign = at " + i);
                                    }
                                    else if(IsCorrectPositionEgalSign(i,e))
                                    {
                                        Console.WriteLine("Not identificator for = at " + i);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Wrong position for " + s.ElementAt(i) + " at " + i +
                                           "The egal sign must be after identificator and this identificator must be the first in expression");
                                    }
                                    
                                }
                                else if (s.ElementAt(i) == ')')
                                {
                                    k--;
                                    if (k < 0)
                                    {
                                        Console.Write("Case 0 -- ");
                                        PrintErrorNotOpenedBrace();
                                    }
                                    else if (k == 0 || IsOpenedBraceLeft(i))
                                    {
                                        Console.WriteLine("Case 0 -- Not need empty brace " + i);
                                    }
                                    if (IsOperationCharacterLeft(i))
                                    {
                                        Console.WriteLine("Case 0 -- Not operand for " + s.ElementAt(i - 1)+" at "+i);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Case 0 -- Not operation for  " + s.ElementAt(i+1)+" at "+1);
                                    }
                                   state = 1;
                                }
                                NextToken();
                            }
                            else
                            {
                                if (InLetters(s.ElementAt(i)))
                                {
                                    Console.WriteLine("Ident ");
                                    Identifier();
                                }
                                else if(InDigits(s.ElementAt(i)))
                                {
                                    Console.WriteLine("Const ");
                                    Number();
                                }
                                state = 1;
                            }
                        }
                        else
                        {
                            state = 2;
                            stateFrom = 0;
                        }
                        break;
                    case 1:
                        if (i < s.Length-1)
                        {
                           
                            if (s.ElementAt(i) == ')')
                            {
                                k--;
                                if (k != 0)
                                {
                                    if (k > 0)
                                    {
                                        Console.Write("Case 1 -- ");
                                        PrintErrorNotClosedBrace();
                                    }
                                    else
                                    {
                                        Console.Write("Case 1 -- ");
                                        PrintErrorNotOpenedBrace();
                                    }
                                    k = 0;
                                }
                            } 
                            else if (s.ElementAt(i) == '(')
                            {
                                k++;
                                Console.WriteLine("Case 1 -- Not operation for this opened brace " + s.ElementAt(i) + " at " + i);
                            }
                            else
                            {
                                if(s.ElementAt(i)=='=')
                                {
                                    e++;
                                    if (IsCorrectPositionEgalSign(i,e))
                                    {
                                        if (!InLetters(s.ElementAt(i - 1)))
                                        {
                                            Console.WriteLine("Not identificator for " + s.ElementAt(i) + " at " + i);
                                        }
                                    }
                                    else if (e > 1)
                                    {
                                        Console.WriteLine("Many sign 1" + s.ElementAt(i) + " at " + i);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Wrong position for " +s.ElementAt(i)+" at "+i+
                                            "The egal sign must be after identificator and this identificator must be the first in expression");
                                    }
                                }
                                state = 0;
                            }
                            NextToken();
                        }
                        else
                        {
                            Console.WriteLine("Operation ");
                            state = 2;
                            stateFrom = 1;
                        }
                        break;
                    case 2:
                        if (s.ElementAt(i) == ')')
                        {
                            k--;
                            if (k == 0)
                            {
                                Console.WriteLine("OKAY From state 1");
                            }
                            else 
                            {
                                k = 0;
                                Console.WriteLine("Case 2 -- Not opened brace for current " + s.ElementAt(i)+" at "+i);
                            }
                        }
                        else if (s.ElementAt(i) == '(')
                        {
                            Console.WriteLine("Case 2 -- Not closed brase for current " + s.ElementAt(i));
                        }
                        else if (s.ElementAt(i) == '=')
                        {
                            e++;
                            if (IsCorrectPositionEgalSign(i,e))
                            {
                                if (InLetters(s.ElementAt(i - 1)))
                                {
                                    Console.WriteLine("Case 2 -- Not definition for " + s.ElementAt(i) + " at " + i);
                                }
                                else
                                {
                                    Console.WriteLine("Case 2 -- Not identificator for "+ s.ElementAt(i) + " at " + i);
                                }
                            }
                            else if(e>1)
                            {
                                Console.WriteLine("Case 2 Many sign ");
                            }
                            else
                            {
                                Console.WriteLine("Wrong position for " + s.ElementAt(i) + " at " + i +
                                           "The egal sign must be after identificator and this identificator must be the first in expression");
                            }
                            
                        }
                        else if (k != 0)
                        {
                            Console.WriteLine("Case 2 -- Not closed braced for ) at "+GetPositionLastOpenedBrace());
                        }
                        if (stateFrom == 1)
                        {
                            if(InOperation(s.ElementAt(i)) && !(s.ElementAt(i)=='='))
                            {
                                Console.WriteLine("Case 2 -- Not operand for current "+s.ElementAt(i));
                            }
                           
                        }
                        else if(stateFrom==0)
                        {
                            if (InDigits(s.ElementAt(i)))
                            {
                                Console.WriteLine("OKAY From state 0");
                            }
                            else if(InOperation(s.ElementAt(i)))
                            {
                                Console.WriteLine("Case 2 -- Successive operation");
                            }
                            
                        }
                        state = 3;
                        break;
                }
            }
        }
    }
}
