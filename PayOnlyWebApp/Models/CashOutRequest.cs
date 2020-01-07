using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayOnlyWebApp.Models
{
    public class CashOutRequest
    {
        public int CashOutID { get; set; }
        public int MerchantID { get; set; }
        public string BankName { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public double Amount { get; set; }
        public string Status { get; set; }
    }
}
