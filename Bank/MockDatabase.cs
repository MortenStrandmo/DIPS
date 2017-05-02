using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace DIPS
{
    public class MockDatabase
    {
        /// <summary>
        /// Unique ID = socialSecurity
        /// </summary>
        public List<Person> personList;
        /// <summary>
        /// Unique ID = Person.SocialSecurity
        /// </summary>
        public List<Account> accountList;
        public MockDatabase()
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
                new Account("TobiasNilsen1"     , new Money(1000),  "1705196739843"),
                new Account("TobiasNilsen2"     , new Money(1000),  "1705196739843"),
                new Account("MiaØstby1"         , new Money(1000),  "2007198245863"),
                new Account("FridaGundersen1"   , new Money(1000),  "1403196948523"),
                new Account("LinneaJensen1"     , new Money(1000),  "1804198845128"),
                new Account("SunnivaOlstad1"    , new Money(1000),  "0401196058999"),
            };
        }
        public MockDatabase(Person customer, Account account)
        {
            personList = new List<Person> { customer };
            accountList = new List<Account> { account };

        }
        public void AddAccount(Account account)
        {
            accountList.Add(account);
        }
        public void AddPerson(Person person)
        {
            personList.Add(person);
        }
        public int CountAccounts(Person customer)
        {
            return accountList.Count(a => a.socialSecurity.Contains(customer.socialSecurity));
        }
    }
}
