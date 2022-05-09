using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chaithit_Market.Models
{
    public class ElectricModel
    {
        public string GroupName { set; get; } = "";
        public string DSN { set; get; } = "";
        public decimal Value { set; get; } = 0;
        public DateTime ValueTimeStamp { set; get; }
        public string StatusTextEN { set; get; } = "";
    }
}