using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chaithit_Market.DTO
{
    public class SavePrefixUnitDTO
    {
        public string mode { set; get; } = "";
        public int prefixUnitID { set; get; } = 0;
        public int zoneID { set; get; } = 0;
        public string prefixName { set; get; } = "";
    }
}