using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Chaithit_Market.Models
{
    public class GetHistoryPaidBillUserNoPageModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public List<HistoryPaidBillUserNoPageModel> data { get; set; }
    }

    public class HistoryPaidBillUserNoPageModel
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
        public int discountPercent { set; get; } = 0;
        public decimal discountAmount { set; get; } = 0;
        public int isComplete { set; get; } = 0;
        public string unitName { set; get; } = "";
        public string zoneName { set; get; } = "";

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
            discountPercent = int.Parse(dr["discount_percent"].ToString());
            discountAmount = decimal.Parse(dr["discount_amount"].ToString());
            isComplete = int.Parse(dr["is_complete"].ToString());
            unitName = dr["unit_name"].ToString();
            zoneName = dr["zone_name"].ToString();
        }
    }
}