using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chaithit_Market.DTO
{
    public class GetDropdownUnitIsAllDTO
    {
        public int zoneID { set; get; } = 0;
        public int zoneSubID { set; get; } = 0;
        public string isAll { set; get; } = "true";
    }
}