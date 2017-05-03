using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DIPS;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace BankTest
{
    [TestClass]
    public class CreateAccountTests
    {
        /// <summary>
        /// Test the ArgumentNullException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateAccount_NullException()
        {
            var bank = new Bank();
            var test = bank.CreateAccount(null, null);
        }

        /// <summary>
        /// Test if a person with not enough money can start an account.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CreateAccount_ArgumentOutOfRangeException()
        {
            var bank = new Bank();
            var customer = new Person("Tobias", "Nilsen", "1705196739843");
            var value = 500;
            var amount = new Money(value, customer);
            var account = new Account(1, amount, customer);

            var test = bank.CreateAccount(customer, amount);
        }

        /// <summary>
        /// Test the ReturnValue
        /// </summary>
        [TestMethod]
        public void CreateAccount_ReturnValue()
        {
            var value = 2000;
            var bank = new Bank();
            var person = new Person("Morten", "Bertheussen", "17018845456");
            var money = new Money(value, person);
            var actual = new Account[] {
                bank.CreateAccount(person, money),
                bank.CreateAccount(person, money),
                bank.CreateAccount(person, money)
            };

            var expected = new[] {
            new Account(1, new Money(value, person), person),
            new Account(2, new Money(value, person), person),
            new Account(3, new Money(value, person), person),
            };

            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(expected[i].accountName, actual[i].accountName);
                Assert.AreEqual(expected[i].accountBalance.value, actual[i].accountBalance.value);
                Assert.AreEqual(expected[i].customer, actual[i].customer);
            }
        }
    }
}
