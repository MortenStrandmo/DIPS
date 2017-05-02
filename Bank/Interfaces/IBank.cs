using System;
using System.Collections.Generic;
using System.Text;

namespace DIPS.Interface
{
    interface IBank
    {
        Account CreateAccount(Person customer, Money InitialDeposit);
        Account[] GetAccountsForCustomer(Person customer);
        void Deposit(Account to, Money amount);
        void Withdraw(Account from, Money amount);
        void Transfer(Account from, Account to, Money amount);
    }
}
