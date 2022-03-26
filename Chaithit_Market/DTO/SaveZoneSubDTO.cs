using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chaithit_Market.DTO
{
    public class SaveZoneSubDTO
    {
        public string mode { set; get; } = "";
        public int zoneSubID { set; get; } = 0;
        public int zoneID { set; get; } = 0;
        public string name { set; get; } = "";
    }
}