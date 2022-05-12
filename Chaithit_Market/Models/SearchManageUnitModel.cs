using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Chaithit_Market.Models
{
    public class SearchManageUnitModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public Pagination<SearchManageUnit> data { get; set; }
    }

    public class SearchManageUnit
    {
        public int unitID { set; get; } = 0;
        public string zoneName { set; get; } = "";
        public string zoneSubName { set; get; } = "";
        public string unitNo { set; get; } = "";
        public decimal electricAmount { set; get; } = 0;
        public decimal waterAmount { set; get; } = 0;
        public decimal lampAmountPerOne { set; get; } = 0;
        public decimal electricEquipAmount { set; get; } = 0;
        public int isUsed { set; get; } = 0;
        public int userID { set; get; } = 0;
        public string startDate { set; get; } = "";
        public string endDate { set; get; } = "";
        public int rentType { set; get; } = 0;
        public string payDate { set; get; } = "";
        public int tranRentID { set; get; } = 0;

        public void loadData(DataRow dr)
        {
            unitID = int.Parse(dr["id"].ToString());
            zoneName = dr["zone_name"].ToString();
            zoneSubName = dr["zone_sub_name"].ToString();
            unitNo = dr["unit_no"].ToString();
            electricAmount = decimal.Parse(dr["electric_amount"].ToString());
            waterAmount = decimal.Parse(dr["water_amount"].ToString());
            lampAmountPerOne = decimal.Parse(dr["lamp_amount_per_one"].ToString());
            electricEquipAmount = decimal.Parse(dr["electric_equip_amount"].ToString());
            isUsed = int.Parse(dr["is_used"].ToString());
            userID = int.Parse(dr["user_id"].ToString());
            startDate = dr["start_date"].ToString();
            endDate = dr["end_date"].ToString();
            rentType = int.Parse(dr["rent_type"].ToString());
            payDate = dr["pay_date"].ToString();
            tranRentID = int.Parse(dr["tran_rent_id"].ToString());
        }
    }
}