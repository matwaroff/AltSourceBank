using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AltSourceApp.Models
{
    public class LoginRequestModel
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    public class RegisterRequestModel
    {
        public string username { get; set; }
        public string password { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
    }

    public class DepositRequestModel
    {
        public decimal amount { get; set; }
        public string authToken { get; set; }
    }

    public class WithdrawlRequestModel
    {
        public decimal amount { get; set; }
        public string authToken { get; set; }
    }

    public class AuthOnlyRequestModel
    {
        public string authToken { get; set; }
    }
}
