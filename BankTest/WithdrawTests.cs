using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DIPS;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace BankTest
{
    [TestClass]
    public class WithdrawTests
    {
        /// <summary>
        /// Test the ArgumentNullException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Withdraw_NullException()
        {
            var bank = new Bank();
            bank.Withdraw(null, null);
        }

        /// <summary>
        /// Test if it is possible to withdraw from an account you dont own.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Withdraw_Customer_OutOfRangeException()
        {
            var bank = new Bank();
            var customer1 = new Person("Tobias", "Nilsen", "1705196739843");
            var customer2 = new Person("Mia", "Østby", "2007198245863");
            var value1 = 1000;
            var customer1Money = new Money(value1, customer1);
            var customer2Money = new Money(value1, customer2);
            var accountCustomer1 = new Account(1, customer1Money, customer1);
            bank.Withdraw(accountCustomer1, customer2Money);
        }

        /// <summary>
        /// Test if you can withdraw more money then you own.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Withdraw_AccountBalance_OutOfRangeException()
        {
            var bank = new Bank();
            var customer = new Person("Tobias", "Nilsen", "1705196739843");
            var value = 1000;
            var money1 = new Money(value, customer);
            var money2 = new Money(value * 2, customer);
            var account = new Account(1, money1, customer);

            bank.Withdraw(account, money2);
        }

        /// <summary>
        /// Test if negative money is accepted.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Withdraw_NegativeMoney_OutOfRangeException()
        {
            var bank = new Bank();
            var customer = new Person("Tobias", "Nilsen", "1705196739843");
            var value = 1000;
            var money1 = new Money(value, customer);
            var money2 = new Money(value * -1, customer);
            var account = new Account(1, money1, customer);

            bank.Withdraw(account, money2);
        }

        /// <summary>
        /// It's hard to test if the the lock in Withdraw has any effect due to the random nature of concurrency.
        /// but this test should never fail since the lock is in place. 
        /// </summary>
        [TestMethod]
        public void Withdraw_CorrectAccountBalance()
        {
            var bank = new Bank();
            var customer = new Person("Tobias", "Nilsen", "1705196739843");
            var value = 1000;
            var money1 = new Money(value * 5, customer);
            var money2 = new Money(value * 4, customer);
            var account = new Account(1, money1, customer);
            var expected = money1.value - money2.value;
            bank.Withdraw(account, money2);
            var actual = account.accountBalance.value;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Concurrency is hard to test due to the random nature of thread locking, but over time 
        /// an error should occur if the lock in Widthdraw is removed.
        /// </summary>
        [TestMethod]
        public void Withdraw_Concurrency()
        {
            var bank = new Bank();
            var customer = new Person("Tobias", "Nilsen", "1705196739843");
            var value = 1E6M;
            var iteration = 100;
            var money = new Money(value, customer);
            var withdraw = new Money(value / 1E3M, customer);
            var account = new Account(1, money, customer);

            List<Task> tasklist = new List<Task>();

            for (int i = 0; i < iteration; i++)
            {
                tasklist.Add(Task.Factory.StartNew(async () =>
                {
                    bank.Withdraw(account, withdraw);
                }));
            }
            Task.WaitAll(tasklist.ToArray());

            var actual = account.accountBalance.value;
            var expected = value - iteration * withdraw.value;

            Assert.AreEqual(expected, actual);
        }
    }
}
