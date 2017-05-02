using System;
using DIPS.Interface;
using System.Linq;

namespace DIPS
{
    public class Bank : IBank
    {
        public MockDatabase database = new MockDatabase();

        public decimal Threshold { get; private set; } = 1000;

        public Account CreateAccount(Person customer, Money InitialDeposit)
        {
            string accountName = string.Empty;
            // Check if the customer has enough money to open the account
            if (InitialDeposit.value >= Threshold)
            {
                // Check if the person excist in the database of customers
                if (database.personList.Exists(a => a.socialSecurity == customer.socialSecurity))
                {
                    // Check how many accounts the person has
                    var serialNumber = database.CountAccounts(customer);

                    // Concatenate a string based on firstname, lastname and amount of accounts
                    accountName = customer.firstName + customer.lastName + (serialNumber + 1).ToString();
                }
                return new Account(accountName, InitialDeposit, customer.socialSecurity);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(InitialDeposit), InitialDeposit.value, $"The Threshold to create an account was: {Threshold}");
            }
        }

        public void Deposit(Account to, Money amount)
        {

        }

        public Account[] GetAccountsForCustomer(Person customer)
        {
            Account[] account = null;
            // Check if the person excist in the database of customers
            if (database.personList.Exists(a => a.socialSecurity == customer.socialSecurity))
            {
                var accountList = from a in database.accountList
                                  where a.socialSecurity == customer.socialSecurity
                                  select a;
                account = accountList.ToArray();
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(customer), "Customer not in database");
            }
            return account;
        }

        public void Transfer(Account from, Account to, Money amount)
        {
            throw new NotImplementedException();
        }

        public void Withdraw(Account from, Money amount)
        {
            throw new NotImplementedException();
        }
    }
}
