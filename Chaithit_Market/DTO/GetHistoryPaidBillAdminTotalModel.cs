using Chaithit_Market.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Chaithit_Market.DTO
{
    public class GetHistoryPaidBillAdminTotalModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public HistoryPaidBillAdminTotalModel data { get; set; }
    }

    public class HistoryPaidBillAdminTotalModel
    {
        public decimal netAmount { set; get; } = 0;
        public decimal rentAmount { set; get; } = 0;
        public decimal electricAmount { set; get; } = 0;
        public decimal waterAmount { set; get; } = 0;
        public decimal electricNightMarketAmount { set; get; } = 0;

        public void loadData(DataRow dr)
        {
            netAmount = decimal.Parse(dr["net_amount"].ToString());
            rentAmount = decimal.Parse(dr["rent_amount"].ToString());
            electricAmount = decimal.Parse(dr["electric_amount"].ToString());
            waterAmount = decimal.Parse(dr["water_amount"].ToString());
            electricNightMarketAmount = decimal.Parse(dr["electric_night_market_amount"].ToString());
        }
    }
}