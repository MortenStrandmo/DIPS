using System;
using DIPS.Interface;
using System.Linq;

namespace DIPS
{
    public class Bank : IBank
    {
        // Mock database for querying.
        public MockData db = new MockData();

        /// <summary>
        /// Returns an account if the <paramref name="customer"/>s <paramref name="InitialDeposit"/> is greater than 1000.
        /// </summary>
        /// <remarks>Known exceptions that can occur <see cref="ArgumentNullException"/> <see cref="ArgumentOutOfRangeException" /></remarks>
        public Account CreateAccount(Person customer, Money InitialDeposit)
        {
            // Threshold for opening an account.
            int threshold = 1000;
            if (customer == null || InitialDeposit == null)
            {
                throw new ArgumentNullException(nameof(customer) + nameof(InitialDeposit));
            }
            // Check if the customer has enough money to open the account.
            else if (InitialDeposit.value >= threshold)
            {
                return new Account(customer.accountSerialNumber++, InitialDeposit, customer);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(InitialDeposit.value));
            }
        }

        /// <summary>
        /// Returns all the accounts that the customer owns. 
        /// <para>If non is found the function will return an empty array of accounts.</para>  
        /// </summary>
        /// <remarks>Known exceptions that can occur<see cref="ArgumentNullException"/></remarks>
        public Account[] GetAccountsForCustomer(Person customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer));
            }
            // Check if the person excist in the database of customers
            else if (db.personList.Exists(a => a.socialSecurity == customer.socialSecurity))
            {
                var result = (from a in db.accountList
                              where a.customer.socialSecurity == customer.socialSecurity
                              select a).ToArray();
                return result;
            }
            else
            {
                return new Account[0];
            }
        }

        /// <summary>
        /// Deposit money into an account.
        /// <para>The <see cref="Account"/> and the <see cref="Money"/> has to originate from the same <see cref="Person"/> and the <see cref="amount"/> has to be greater than 0</para>
        /// </summary>
        /// <remarks>Known exceptions that can occur<see cref="ArgumentNullException"/>, <see cref="ArgumentOutOfRangeException" /></remarks>
        public void Deposit(Account to, Money amount)
        {
            if (to == null || amount == null)
            {
                throw new ArgumentNullException(nameof(to) + nameof(amount) + nameof(amount.value));
            }

            var result = true;
            // Check if the owner of the account is the owner of the money.
            result &= to.customer.socialSecurity == amount.customer.socialSecurity;
            // Check if the number is a positiver number
            result &= amount.value > 0;

            if (result == true)
            {
                lock (to)
                {
                    // Lock to avoid concurrency / race conditions
                    to.accountBalance.value += amount.value;
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(to.customer.socialSecurity)
                    + nameof(amount.customer.socialSecurity)
                    + nameof(amount.value));
            }
        }

        /// <summary>
        /// Withdraw money from an account.
        /// <para>The <see cref="Account"/> and the <see cref="Money"/> has to originate from the same <see cref="Person"/> and the <see cref="account"/> balance has to be greater than the amount drawn</para>
        /// </summary>
        /// <remarks>Known exceptions that can occur<see cref="ArgumentNullException"/>, <see cref="ArgumentOutOfRangeException" /></remarks>
        public void Withdraw(Account from, Money amount)
        {
            if (from == null || amount == null)
            {
                throw new ArgumentNullException(nameof(from) + nameof(amount) + nameof(amount));
            }
            var result = true;
            // Check if the owner of the account is the owner of the money.
            result &= from.customer.socialSecurity == amount.customer.socialSecurity;
            // Check if the accountbalance is greater then the widthdraw amount.
            result &= from.accountBalance.value >= amount.value;
            // Check if the number is a positiver number
            result &= amount.value > 0;

            if (result == true)
            {
                lock (from)
                {
                    // Lock to avoid concurrency / race conditions
                    from.accountBalance.value -= amount.value;
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(from.customer.socialSecurity)
                    + nameof(amount.customer.socialSecurity)
                    + nameof(amount.value));
            }
        }

        /// <summary>
        /// Transfer founds from one account to another.
        /// </summary>
        /// <remarks>Known exceptions that can occur<see cref="ArgumentNullException"/>, <see cref="ArgumentOutOfRangeException" /></remarks>
        public void Transfer(Account from, Account to, Money amount)
        {
            if (from == null || to == null || amount == null)
            {
                throw new ArgumentNullException(nameof(from) + nameof(to) + nameof(amount));
            }
            else if (from.accountBalance.value >= amount.value)
            {
                    Withdraw(from, amount);
                    // Change ownership of the money
                    amount.customer = to.customer;
                    Deposit(to, amount);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(from.accountBalance.value));
            }
        }

    }
}
