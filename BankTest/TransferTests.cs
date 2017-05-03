using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DIPS;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace BankTest
{
    [TestClass]
    public class TransferTests
    {
        /// <summary>
        /// Test the ArgumentNullException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transfer_NullException()
        {
            var bank = new Bank();
            bank.Transfer(null, null, null);
        }

        /// <summary>
        /// Test if you can transfer more money than you own.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Transfer_AccountBalance_OutOfRangeException()
        {
            var bank = new Bank();
            var customer1 = new Person("Tobias", "Nilsen", "1705196739843");
            var customer2 = new Person("Mia", "Østby", "2007198245863");
            var value = 1000;
            var money1 = new Money(value * 0.5M, customer1);
            var money2 = new Money(value, customer2);
            var money3 = new Money(value, customer1);
            var account1 = new Account(1, money1, customer1);
            var account2 = new Account(1, money2, customer2);
            bank.Transfer(account1, account2, money3);
        }

        /// <summary>
        /// Test if the transfer was successfull
        /// </summary>
        [TestMethod]
        public void Transfer_CorrectAccountBalance()
        {
            var bank = new Bank();
            var customer1 = new Person("Tobias", "Nil sen", "1705196739843");
            var customer2 = new Person("Mia", "Østby", "2007198245863");
            var value = 1000;
            var money1 = new Money(value, customer1);
            var money2 = new Money(value, customer2);
            var money3 = new Money(value, customer1);
            var account1 = new Account(1, money1, customer1);
            var account2 = new Account(1, money2, customer2);
            var expected1 = value * 0;
            var expected2 = value * 2;
            bank.Transfer(account1, account2, money3);
            var actual1 = account1.accountBalance.value;
            var actual2 = account2.accountBalance.value;
            Assert.AreEqual(expected1, actual1);
            Assert.AreEqual(expected2, actual2);
        }

        /// <summary>
        /// Test if negative money Transfer is possible.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Transfer_NegativeMoney()
        {
            var bank = new Bank();
            var customer1 = new Person("Tobias", "Nil sen", "1705196739843");
            var customer2 = new Person("Mia", "Østby", "2007198245863");
            var value = 1000;
            var money1 = new Money(value, customer1);
            var money2 = new Money(value, customer2);
            var money3 = new Money(-value, customer1);
            var account1 = new Account(1, money1, customer1);
            var account2 = new Account(1, money2, customer2);
            var expected1 = value * 0;
            var expected2 = value * 2;
            bank.Transfer(account1, account2, money3);
            var actual1 = account1.accountBalance.value;
            var actual2 = account2.accountBalance.value;
            Assert.AreEqual(expected1, actual1);
            Assert.AreEqual(expected2, actual2);
        }

        /// <summary>
        /// Test the concurrency of the transfer function.
        /// </summary>
        [TestMethod]
        public void Transfer_Concurrency()
        {
            var bank = new Bank();
            var iteration = 100;
            var customer1 = new Person("Tobias", "Nil sen", "1705196739843");
            var customer2 = new Person("Mia", "Østby", "2007198245863");
            var value = 10000;
            var money1 = new Money(value*2, customer1);
            var money2 = new Money(value, customer2);
            var money3 = new Money(value/100, customer1);
            var account1 = new Account(1, money1, customer1);
            var account2 = new Account(1, money2, customer2);
            var expected1 = account1.accountBalance.value - (iteration * money3.value);
            var expected2 = account2.accountBalance.value + (iteration * money3.value);


            List<Task> tasklist = new List<Task>();

            for (int i = 0; i < iteration; i++)
            {
                tasklist.Add(Task.Factory.StartNew(async () =>
                {
                    money3 = new Money(value / 100, customer1);
                    bank.Transfer(account1, account2, money3);
                }));
            }
            Task.WaitAll(tasklist.ToArray());
            var actual1 = account1.accountBalance.value;
            var actual2 = account2.accountBalance.value;

            Assert.AreEqual(expected2, actual2, nameof(account2));
            Assert.AreEqual(expected1, actual1, nameof(account1));
        }
    }
}
