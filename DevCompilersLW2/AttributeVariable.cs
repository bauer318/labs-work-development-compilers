using System;
using System.Collections.Generic;
using System.Text;

namespace DevCompilersLW2
{
    public class AttributeVariable
    {
        private int _id;
        private string _name;
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        public AttributeVariable(int parId, string parName)
        {
            _id = parId;
            _name = parName;
        }
    }
}
