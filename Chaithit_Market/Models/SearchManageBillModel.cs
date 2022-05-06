using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Chaithit_Market.Models
{
    public class SearchManageBillModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public Pagination<SearchManageBill> data { get; set; }
    }

    public class SearchManageBill
    {
        public int tranBillID { set; get; } = 0;
        public int tranRentID { set; get; } = 0;
        public string billCode { set; get; } = "";
        public string unitName { set; get; } = "";
        public string startDate { set; get; } = "";
        public string endDate { set; get; } = "";
        public string payDate { set; get; } = "";
        public decimal rentAmount { set; get; } = 0;
        public int electricUnit { set; get; } = 0;
        public decimal electricAmount { set; get; } = 0;
        public int waterUnit { set; get; } = 0;
        public decimal waterAmount { set; get; } = 0;
        public int lampUnit { set; get; } = 0;
        public int electricEquipUnit { set; get; } = 0;
        public decimal electricNightMarketAmount { set; get; } = 0;
        public decimal totalAmount { set; get; } = 0;
        public int discountPercent { set; get; } = 0;
        public decimal discountAmount { set; get; } = 0;
        public decimal netAmount { set; get; } = 0;
        public decimal completeAmount { set; get; } = 0;
        public decimal balanceAmount { set; get; } = 0;
        public decimal overpayAmount { set; get; } = 0;
        public int isComplete { set; get; } = 0;
        public string fileCode { set; get; } = "";
        public string[] imageUrl { get; set; } = { "" };

        public void loadData(DataRow dr)
        {
            tranBillID = int.Parse(dr["id"].ToString());
            tranRentID = int.Parse(dr["tran_rent_id"].ToString());
            billCode = dr["bill_code"].ToString();
            unitName = dr["unit_name"].ToString();
            startDate = dr["start_date"].ToString();
            endDate = dr["end_date"].ToString();
            payDate = dr["pay_date"].ToString();
            rentAmount = decimal.Parse(dr["rent_amount"].ToString());
            electricUnit = int.Parse(dr["electric_unit"].ToString());
            electricAmount = decimal.Parse(dr["electric_amount"].ToString());
            waterUnit = int.Parse(dr["water_unit"].ToString());
            waterAmount = decimal.Parse(dr["water_amount"].ToString());
            lampUnit = int.Parse(dr["lamp_unit"].ToString());
            electricEquipUnit = int.Parse(dr["electric_equip_unit"].ToString());
            electricNightMarketAmount = decimal.Parse(dr["electric_night_market_amount"].ToString());
            totalAmount = decimal.Parse(dr["total_amount"].ToString());
            discountPercent = int.Parse(dr["discount_percent"].ToString());
            discountAmount = decimal.Parse(dr["discount_amount"].ToString());
            netAmount = decimal.Parse(dr["net_amount"].ToString());
            completeAmount = decimal.Parse(dr["complete_amount"].ToString());
            balanceAmount = decimal.Parse(dr["balance_amount"].ToString());
            overpayAmount = decimal.Parse(dr["overpay_amount"].ToString());
            isComplete = int.Parse(dr["is_complete"].ToString());
            fileCode = dr["file_code"].ToString();
        }
    }
}