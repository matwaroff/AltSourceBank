using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AltSourceApp.Helpers;
using AltSourceApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace AltSourceApp.Controllers
{
    [Route("accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        [HttpPut]
        [Route("login")]
        public string Login([FromBody]JObject body)
        {
            LoginRequestModel request = body.ToObject<LoginRequestModel>();
            Account acct = Account.Login(request.username, request.password);
            if(acct != null)
            {
                JObject account = JObject.FromObject(acct);
                account["passwordHash"] = null;
                return account.ToString();
            }
            else
            {
                return new ErrorHelper("Login invalid. Please try again...").ToJSON();
            }
        }

        [HttpPut]
        [Route("update")]
        public string Update([FromBody]JObject body)
        {
            AuthOnlyRequestModel request = body.ToObject<AuthOnlyRequestModel>();
            Account acct = Account.FromAuthToken(request.authToken);

            if(acct != null)
            {
                acct.passwordHash = null;
                return JObject.FromObject(acct).ToString();
            }
            else
            {
                return new ErrorHelper("Authentication Failed. Please log in again...").ToJSON();
            }
        }
        [HttpPost]
        [Route("register")]
        public string Register([FromBody]JObject body)
        {
            RegisterRequestModel request = body.ToObject<RegisterRequestModel>();
            Account newAcct = new Account(request.username, Account.HashPassword(request.password), request.firstName, request.lastName);

            return newAcct.Save()
                ? JObject.FromObject(newAcct).ToString()
                : new ErrorHelper("Registration failed. Please try again.").ToJSON();
                
        }

        [HttpPost]
        [Route("deposit")]
        public string Deposit([FromBody]JObject body)
        {
            DepositRequestModel request = body.ToObject<DepositRequestModel>();
            Account acct = Account.FromAuthToken(request.authToken);
            if(acct != null)
            {
                acct.Deposit(request.amount);
                return acct.balance.ToString();
            }
            else
            {
                return new ErrorHelper("Authentication failed. Please login again.").ToJSON();
            }
        }

        [HttpPost]
        [Route("withdrawl")]
        public string Withdrawl([FromBody] JObject body)
        {
            WithdrawlRequestModel request = body.ToObject<WithdrawlRequestModel>();
            Account acct = Account.FromAuthToken(request.authToken);
            if(acct != null)
            {
                if (!acct.Withdrawl(request.amount))
                    return new ErrorHelper("Insufficient Funds. Please deposit some money first.").ToJSON();
                else return acct.balance.ToString();
            }
            else
            {
                return new ErrorHelper("Authentication failed. Please login again.").ToJSON();
            }
        }

        [HttpPut]
        [Route("balance")]
        public string CheckBalance([FromBody] JObject body)
        {
            AuthOnlyRequestModel request = body.ToObject<AuthOnlyRequestModel>();
            Account acct = Account.FromAuthToken(request.authToken);
            if(acct != null)
            {
                return acct.balance.ToString();
            }
            else
            {
                return new ErrorHelper("Authentication failed. Please login again.").ToJSON();
            }

        }

        [HttpPut]
        [Route("transactions")]
        public string TransactionHistory([FromBody] JObject body)
        {
            AuthOnlyRequestModel request = body.ToObject<AuthOnlyRequestModel>();
            Account acct = Account.FromAuthToken(request.authToken);
            if(acct != null)
            {
                return JArray.FromObject(acct.transactionHistory).ToString();
            }
            else
            {
                return new ErrorHelper("Authentication failed. Please login again.").ToJSON();
            }
        }
    }
}