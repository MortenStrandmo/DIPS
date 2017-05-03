using System;
using System.Collections.Generic;
using System.Text;

namespace DIPS
{
    public class Account
    {
        public Person customer { get; set; }
        public string accountName { get; set; }
        public Money accountBalance { get; set; }

        public Account(int serialNumber, Money initialDeposit, Person customer)
        {
            this.customer = customer;
            accountName = customer.firstName + customer.lastName + serialNumber;
            accountBalance = initialDeposit;
        }
    }
}
