using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chaithit_Market.DTO
{
    public class SaveUnitDTO
    {
        public string mode { set; get; } = "";
        public int unitID { set; get; } = 0;
        public int zoneID { set; get; } = 0;
        public int zoneSubID { set; get; } = 0;
        public string unitCode { set; get; } = "";
        public string name { set; get; } = "";
        public int rateID { set; get; } = 0;
        public string electricMeter { set; get; } = "";
        public int status { set; get; } = 0;
    }
}