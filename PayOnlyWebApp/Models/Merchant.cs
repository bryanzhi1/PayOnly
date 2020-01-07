using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayOnlyWebApp.Models
{
    public class Merchant
    {
        public int MerchantId { get; set; }

        public string MerchantName { get; set; }
        public string InChargeName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public double Balance { get; set; }
    }
}
