using System;
using System.Collections.Generic;
using System.Text;

namespace DIPS
{
    public class Account
    {
        public string socialSecurity { get; set; }
        public string accountName { get; set; }
        public Money accountBalance { get; set; }
        public Account(string accountName, Money initialDeposit, string socialSecurity)
        {
            this.socialSecurity = socialSecurity;
            this.accountName = accountName;
            accountBalance = initialDeposit;
        }
        public Account()
        {
            accountName = string.Empty;
            accountName = null;
        }
    }
}
