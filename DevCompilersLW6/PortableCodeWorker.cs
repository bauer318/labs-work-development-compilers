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
        public static bool CanOperate(PortableCode parPortableCode)
        {
            foreach(Token operand in parPortableCode.OperandList)
            {
                if (!TokenWorker.IsTokenConstantType(operand))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
