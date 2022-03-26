using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Chaithit_Market.Models
{
    public class SearchHistoryPaidBillModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public Pagination<SearchHistoryPaidBill> data { get; set; }
    }

    public class SearchHistoryPaidBill
    {
        public int billID { set; get; } = 0;
        public string billCode { set; get; } = "";
        public string startDate { set; get; } = "";
        public string endDate { set; get; } = "";
        public decimal rentalAmount { set; get; } = 0;
        public decimal electricAmount { set; get; } = 0;
        public decimal waterAmount { set; get; } = 0;
        public decimal totalAmount { set; get; } = 0;

        public void loadData(DataRow dr)
        {
            billID = int.Parse(dr["id"].ToString());
            billCode = dr["bill_code"].ToString();
            startDate = DateTime.Parse(dr["start_date"].ToString()).ToString("yyyy-MM-dd");
            endDate = DateTime.Parse(dr["end_date"].ToString()).ToString("yyyy-MM-dd");
            rentalAmount = decimal.Parse(dr["rental_amount"].ToString());
            electricAmount = decimal.Parse(dr["electric_amount"].ToString());
            waterAmount = decimal.Parse(dr["water_amount"].ToString());
            totalAmount = decimal.Parse(dr["total_amount"].ToString());
        }
    }
}