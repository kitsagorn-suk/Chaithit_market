using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chaithit_Market.DTO
{
    public class SaveTranPayDTO
    {
        public string billID { set; get; } = "";
        public decimal payAmount { set; get; } = 0;
        public string comment { set; get; } = "";
    }
}