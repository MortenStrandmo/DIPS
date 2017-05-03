using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace DIPS
{
    /// <summary>
    /// This class is used to mock data for testing of the function <see cref="Bank.GetAccountsForCustomer(Person)"/>
    /// </summary>
    public class MockData
    {
        /// <summary>
        /// These lists are used to mock Data
        /// </summary>
        public List<Person> personList;
        public List<Account> accountList;
        public List<Money> moneyList;
        public MockData()
        {
            personList = new List<Person>
            {
                new Person("Tobias" , "Nilsen",     "1705196739843"),
                new Person("Mia"    , "Østby",      "2007198245863"),
                new Person("Frida"  , "Gundersen",  "1403196948523"),
                new Person("Linnea" , "Jensen",     "1804198845128"),
                new Person("Sunniva", "Olstad",     "0401196058999")
            };
            accountList = new List<Account>
            {
                new Account(1,new Money(1500, personList[0] ),personList[0]),
                new Account(2,new Money(2500, personList[0] ),personList[0]),
                new Account(1,new Money(1500, personList[2] ),personList[2]),
                new Account(1,new Money(1500, personList[3] ),personList[3]),
                new Account(1,new Money(1500, personList[3] ),personList[4]),
                new Account(2,new Money(1000000, personList[1] ),personList[1]),


            };
            moneyList = new List<Money>
            {
                new Money(500, personList[0]),
                new Money(1000, personList[1]),
                new Money(1500, personList[2]),
                new Money(2000, personList[3]),
                new Money(2500, personList[4]),
                new Money(-1000, personList[4]),
            };
        }

        public MockData(Person customer, Account account)
        {
            personList = new List<Person> { customer };
            accountList = new List<Account> { account };
        }

    }
}
