using System;
using System.Collections.Generic;
using System.Text;

namespace DIPS
{
    public class Person
    {
        public string socialSecurity { get; set; } = string.Empty;
        public string firstName { get; set; } = string.Empty;
        public string lastName { get; set; } = string.Empty;



        public Person(string firstName, string lastName, string socialSecurity)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.socialSecurity = socialSecurity;
        }
        public string SocialSecurity() => socialSecurity;
    }
}
