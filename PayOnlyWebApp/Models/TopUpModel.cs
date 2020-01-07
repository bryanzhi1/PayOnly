using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayOnlyWebApp.Models
{
    public class TopUpModel
    {
        public int UserID { get; set; }
        public double topUpAmount { get; set; }
    }
}
