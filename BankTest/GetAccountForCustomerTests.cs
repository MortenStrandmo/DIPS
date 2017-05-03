using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DIPS;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankTest
{
    [TestClass]
    public class GetAccountForCustomerTests
    {
        /// <summary>
        /// Test that the function throws an ArgumentNullException when the argument to the function is null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetAccountsForCustomer_NullException()
        {
            var bank = new Bank();
            bank.GetAccountsForCustomer(null);
        }

        /// <summary>
        /// Test the ReturnValue
        /// </summary>
        [TestMethod]
        public void GetAccountsForCustomer_ReturnValueWhenManyAccounts()
        {
            var bank = new Bank();

            // Use mockData to find test the function.
            var md = new MockData();
            var person = md.personList.First();
            var actual = bank.GetAccountsForCustomer(person);
            var expected = new[] {
            new Account(1, new Money(1500, person), person),
            new Account(2, new Money(2500, person), person),
            };

            for (int i = 0; i < expected.Count(); i++)
            {
                Assert.AreEqual(actual[i].accountBalance.value,
                    expected[i].accountBalance.value);
                Assert.AreEqual(actual[i].accountName, expected[i].accountName);
                Assert.AreEqual(actual[i].customer.accountSerialNumber,
                    expected[i].customer.accountSerialNumber);
            }
        }
        [TestMethod]
        public void GetAccountsForCustomer_ReturnValueWhenNoAccounts()
        {
            var bank = new Bank();

            // Use mockData to find test the function.
            var md = new MockData();
            var person = new Person("Harald", "V", "2102193715254");
            var expected = new Account[0];
            var actual = bank.GetAccountsForCustomer(person);

            for (int i = 0; i < expected.Count(); i++)
            {
                Assert.AreEqual(actual[i].accountBalance.value,
                    expected[i].accountBalance.value);
                Assert.AreEqual(actual[i].accountName, expected[i].accountName);
                Assert.AreEqual(actual[i].customer.accountSerialNumber,
                    expected[i].customer.accountSerialNumber);
            }

        }
    }
}
