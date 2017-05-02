using System;
using System.Collections.Generic;
using System.Text;

namespace DIPS
{
    public class Money
    {
        private decimal _value;

        public decimal value
        {
            get { return _value; }
            set { _value = value; }
        }

        public Money(decimal value)
        {
            _value = value;
        }
    }
}
