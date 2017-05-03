using System;
using System.Collections.Generic;
using System.Text;

namespace DIPS
{
    public class Money
    {
        // Stored the balance of the account
        public decimal value;

        // Used to identify the owner of the Money
        public Person customer;

        public Money(decimal value, Person customer)
        {
            this.value = value;
            this.customer = customer;
        }
    }
}
