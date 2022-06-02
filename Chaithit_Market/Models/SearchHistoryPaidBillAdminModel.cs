using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Chaithit_Market.Models
{
    public class SearchHistoryPaidBillAdminModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public Pagination<SearchHistoryPaidBillAdmin> historyPaidBill { get; set; }
    }

    public class SearchHistoryPaidBillAdmin
    {
        public int billID { set; get; } = 0;
        public string billNumber { set; get; } = "";
        public string unitName { set; get; } = ""; 
        public string startDate { set; get; } = "";
        public string endDate { set; get; } = "";
        public string payDate { set; get; } = "";
        public decimal rentAmount { set; get; } = 0;
        public decimal electricAmount { set; get; } = 0;
        public decimal waterAmount { set; get; } = 0;
        public decimal electricNightMarketAmount { set; get; } = 0;
        public decimal totalAmount { set; get; } = 0;
        public decimal discountAmount { set; get; } = 0;
        public decimal netAmount { set; get; } = 0;
        public decimal completeAmount { set; get; } = 0;
        public decimal balanceAmount { set; get; } = 0;
        public decimal overpayAmount { set; get; } = 0;
        public string fileCode { set; get; } = "";
        public string[] imageUrl { get; set; } = { "" };
        public int status { set; get; } = 0;
        public string zoneName { set; get; } = "";

        public void loadData(DataRow dr)
        {
            billID = int.Parse(dr["id"].ToString());
            billNumber = dr["bill_number"].ToString();
            unitName = dr["unit_name"].ToString();
            startDate =dr["start_date"].ToString();
            endDate = dr["end_date"].ToString();
            payDate = dr["pay_date"].ToString();
            rentAmount = decimal.Parse(dr["rent_amount"].ToString());
            electricAmount = decimal.Parse(dr["electric_amount"].ToString());
            waterAmount = decimal.Parse(dr["water_amount"].ToString());
            electricNightMarketAmount = decimal.Parse(dr["electric_night_market_amount"].ToString()); 
            totalAmount = decimal.Parse(dr["total_amount"].ToString());
            discountAmount = decimal.Parse(dr["discount_amount"].ToString());
            netAmount = decimal.Parse(dr["net_amount"].ToString());
            completeAmount = decimal.Parse(dr["complete_amount"].ToString());
            balanceAmount = decimal.Parse(dr["balance_amount"].ToString());
            overpayAmount = decimal.Parse(dr["overpay_amount"].ToString());
            fileCode = dr["file_code"].ToString();
            status = int.Parse(dr["status"].ToString());
            zoneName = dr["zone_name"].ToString();
        }
    }
}