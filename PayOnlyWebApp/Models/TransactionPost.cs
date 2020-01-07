using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayOnlyWebApp.Models
{
    public class TransactionPost
    {
        public double TransactionAmount { get; set; }
        public int UserID { get; set; }
        public int MerchantID { get; set; }
    }
}
