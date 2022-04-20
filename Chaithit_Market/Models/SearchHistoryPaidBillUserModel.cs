using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Chaithit_Market.Models
{
    public class SearchHistoryPaidBillUserModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public Pagination<SearchHistoryPaidBillUser> data { get; set; }
    }

    public class SearchHistoryPaidBillUser
    {
        public int billID { set; get; } = 0;
        public string billCode { set; get; } = "";
        public string payDate { set; get; } = "";
        public string startDate { set; get; } = "";
        public string endDate { set; get; } = "";
        public decimal rentAmount { set; get; } = 0;
        public decimal electricAmount { set; get; } = 0;
        public decimal waterAmount { set; get; } = 0;
        public decimal electricNightMarketAmount { set; get; } = 0;
        public decimal netAmount { set; get; } = 0;
        public decimal balanceAmount { set; get; } = 0;

        public void loadData(DataRow dr)
        {
            billID = int.Parse(dr["id"].ToString());
            billCode = dr["bill_code"].ToString();
            payDate = dr["pay_date"].ToString();
            startDate = dr["start_date"].ToString();
            endDate = dr["end_date"].ToString();
            rentAmount = decimal.Parse(dr["rent_amount"].ToString());
            electricAmount = decimal.Parse(dr["electric_amount"].ToString());
            waterAmount = decimal.Parse(dr["water_amount"].ToString());
            electricNightMarketAmount = decimal.Parse(dr["electric_night_market_amount"].ToString());
            netAmount = decimal.Parse(dr["net_amount"].ToString());
            balanceAmount = decimal.Parse(dr["balance_amount"].ToString());
        }
    }
}