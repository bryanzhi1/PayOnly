using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayOnlyWebApp.Models;
using PayOnlyWebApp.DAL;

namespace PayOnlyWebApp.Controllers
{
    [Route("api")]
    [ApiController]
    public class APIController : ControllerBase
    {
        private ApiDAL ApiContext = new ApiDAL();

        [Route("GetMerchant/{MerchantId}")]
        [HttpGet]
        public Merchant GetMerchant(int MerchantId)
        {
            Merchant merchant = ApiContext.GetMerchant(MerchantId);
            return merchant;
        }

        [Route("GetUserById/{UserId}")]
        [HttpGet]
        public User GetUserById(int UserId)
        {
            User user = ApiContext.GetUserById(UserId);
            return user;
        }

        [Route("GetUserByEmail/{email}")]
        [HttpGet]
        public User GetUserByEmail(string email)
        {
            User user = ApiContext.GetUserByEmail(email);
            return user;
        }

        [Route("PostTransaction")]
        [HttpPost]
        public object PostTransaction([FromBody] TransactionPost transaction)
        {
            bool otherstatus = ApiContext.DeductUser(transaction.UserID, transaction.TransactionAmount);
            if (otherstatus == true)
            {
                bool status = ApiContext.PostTxn(transaction);
                bool otherotherstatus = ApiContext.CreditMerchant(transaction.MerchantID, transaction.TransactionAmount);
                return Ok();
            }

            else
            {
                return Unauthorized();
            }
        }

        [Route("TopUp")]
        [HttpPost]
        public object TopUpUser([FromBody] TopUpModel topup)
        {
            bool status = ApiContext.TopUpUser(topup);
            return Ok();
        }

        //Get Transactions by User
        [Route("GetTxnByUser/{UserId}")]
        [HttpGet]
        public List<Transaction> GetTxnByUser(int UserId)
        {
            List<Transaction> transactionlist = ApiContext.GetTxnByUser(UserId);
            return transactionlist;
        }

        // PUT: api/API/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
