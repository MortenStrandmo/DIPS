using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DIPS;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankTest
{
    [TestClass]
    public class DepositTests
    {
        /// <summary>
        /// Test the ArgumentNullException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Deposit_NullException()
        {
            var bank = new Bank();
            bank.Deposit(null, null);
        }

        /// <summary>
        /// Test if it is possible to Deposit from an account you dont own.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Deposit_Customer_OutOfRangeException()
        {
            var bank = new Bank();
            var customer1 = new Person("Tobias", "Nilsen", "1705196739843");
            var customer2 = new Person("Mia", "Østby", "2007198245863");
            var value1 = 1000;
            var customer1Money = new Money(value1, customer1);
            var customer2Money = new Money(value1, customer2);
            var accountCustomer1 = new Account(1, customer1Money, customer1);
            bank.Deposit(accountCustomer1, customer2Money);
        }

        /// <summary>
        /// Test if negative money widthdrawal is possible.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Deposit_NegativeMoney_OutOfRangeException()
        {
            var bank = new Bank();
            var customer = new Person("Tobias", "Nilsen", "1705196739843");
            var value1 = 10000;
            var value2 = -1000;
            var amount1 = new Money(value1, customer);
            var amount2 = new Money(value2, customer);
            var account = new Account(1, amount1, customer);
            bank.Deposit(account, amount2);
        }

        /// <summary>
        /// Test that the balance increases.
        /// </summary>
        [TestMethod]
        public void Deposit_CorrectAccountBalance()
        {
            var bank = new Bank();
            var customer = new Person("Tobias", "Nilsen", "1705196739843");
            var value = 1000;
            var money = new Money(value, customer);
            var deposit = new Money(value, customer);
            var account = new Account(1, money, customer);

            var expected = money.value + deposit.value;
            bank.Deposit(account, deposit);
            var actual = account.accountBalance.value;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// It's hard to test if the the lock in Withdraw has any effect due to the random nature of concurrency.
        /// but this test should never fail since the lock is in place. 
        /// </summary>
        [TestMethod]
        public void Deposit_Concurrency()
        {
            var bank = new Bank();
            var customer = new Person("Tobias", "Nilsen", "1705196739843");
            var value = 1000;
            var iteration = 100;
            var money = new Money(value, customer);
            var deposit = new Money(value, customer);
            var account = new Account(1, money, customer);

            List<Task> tasklist = new List<Task>();

            for (int i = 0; i < iteration; i++)
            {
                tasklist.Add(Task.Factory.StartNew(async () =>
                {
                    bank.Deposit(account, deposit);
                }));
            }
            Task.WaitAll(tasklist.ToArray());

            var actual = account.accountBalance.value;
            var expected = value + iteration * deposit.value;

            Assert.AreEqual(expected, actual);
        }
    }
}
