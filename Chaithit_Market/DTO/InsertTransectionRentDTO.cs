using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chaithit_Market.DTO
{
    public class InsertTransectionRentDTO
    {
        public int tranRentID { set; get; } = 0;
        public string mode { set; get; } = "";
        public string transCode { set; get; } = "";
        public int userID { set; get; } = 0;
        public int unitID { set; get; } = 0;
        public string startDate { set; get; } = "";
        public string endDate { set; get; } = "";
        public int rentType { set; get; } = 0;
        public int rentTypeAmount { set; get; } = 0;
        public int status { set; get; } = 0;
    }
}