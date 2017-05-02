using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DIPS;
using System.Linq;
using System.Collections.Generic;

namespace BankTest
{
    [TestClass]
    public class BankTests
    {
        [TestMethod]
        public void CreateAccount_Value()
        {
            Bank bank = new Bank();
            Money money = new Money(1000);
            Person person = new Person("Tobias", "Nilsen", "1705196739843");
            bank.database.AddAccount(bank.CreateAccount(person, money));
            string expected = "TobiasNilsen3";
            string actual = bank.database.accountList.Last().accountName;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CreateAccount_NoMoney()
        {
            Bank bank = new Bank();
            Money money = new Money(500);
            Person person = new Person("Tobias", "Nilsen", "1705196739843");
            var x = bank.CreateAccount(person, money);
            bank.CreateAccount(person, money);
        }

        [TestMethod]
        public void GetAccountsForCustomer_Value()
        {
            Bank bank = new Bank();
            Money money = new Money(1000);
            Person person = new Person("Tobias", "Nilsen", "1705196739843");
            var actual = bank.GetAccountsForCustomer(person);
            Account[] expected = new[] {
                new Account("TobiasNilsen1", new Money(1000), "1705196739843"),
                new Account("TobiasNilsen2", new Money(1000), "1705196739843"),
                };

            // Check 
            Assert.AreEqual(actual.Count(), expected.Count());
            for (int i = 0; i < actual.Count(); i++)
            {
                Assert.AreEqual(actual[i].accountName, expected[i].accountName);
                Assert.AreEqual(actual[i].accountBalance.value, expected[i].accountBalance.value);
                Assert.AreEqual(actual[i].socialSecurity, expected[i].socialSecurity);
            }
        }
    }
}
