using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DIPS;
namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var bank = new Bank();
            var iteration = 100;
            var customer1 = new Person("Tobias", "Nil sen", "1705196739843");
            var customer2 = new Person("Mia", "Østby", "2007198245863");
            var value = 10000;
            var money1 = new Money(value * 2, customer1);
            var money2 = new Money(value, customer2);
            var money3 = new Money(value / 100, customer1);
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
            Console.WriteLine(actual1);
            Console.WriteLine(actual2);
            Console.Read();
        }
    }
}
