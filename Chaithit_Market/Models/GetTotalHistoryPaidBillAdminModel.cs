using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Chaithit_Market.Models
{
    public class GetTotalHistoryPaidBillAdminModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public TotalHistoryPaidBillAdminModel data { get; set; }
    }

    public class TotalHistoryPaidBillAdminModel
    {
        public decimal total { set; get; } = 0;

        public void loadData(DataRow dr)
        {
            total = decimal.Parse(dr["Total"].ToString());
        }
    }
}