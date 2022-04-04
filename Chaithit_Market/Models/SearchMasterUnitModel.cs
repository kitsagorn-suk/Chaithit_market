using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Chaithit_Market.Models
{
    public class SearchMasterUnitModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public Pagination<SearchMasterUnit> data { get; set; }
    }

    public class SearchMasterUnit
    {
        public int unitID { set; get; } = 0;
        public string unitCode { set; get; } = "";
        public string unitName { set; get; } = "";
        public int zoneID { set; get; } = 0;
        public string zoneName { set; get; } = "";
        public int zoneSubID { set; get; } = 0;
        public string zoneSubName { set; get; } = "";
        public int rateID { set; get; } = 0;
        public decimal rentAmountDay { set; get; } = 0;
        public decimal rentAmountMonth { set; get; } = 0;
        public decimal electricAmount { set; get; } = 0;
        public decimal waterAmount { set; get; } = 0;
        public decimal lampAmountPerOne { set; get; } = 0;
        public decimal electricEquipAmount { set; get; } = 0;
        public int status { set; get; } = 0;

        public void loadData(DataRow dr)
        {
            unitID = int.Parse(dr["id"].ToString());
            unitCode = dr["unit_code"].ToString();
            unitName = dr["name"].ToString();
            zoneID = int.Parse(dr["zone_id"].ToString());
            zoneName = dr["zone_name"].ToString();
            zoneSubID = int.Parse(dr["zone_sub_id"].ToString());
            zoneSubName = dr["zone_sub_name"].ToString();
            rateID = int.Parse(dr["rate_id"].ToString());
            rentAmountDay = decimal.Parse(dr["rent_amount_day"].ToString());
            rentAmountMonth = decimal.Parse(dr["rent_amount_month"].ToString());
            electricAmount = decimal.Parse(dr["electric_amount"].ToString());
            waterAmount = decimal.Parse(dr["water_amount"].ToString());
            lampAmountPerOne = decimal.Parse(dr["lamp_amount_per_one"].ToString());
            electricEquipAmount = decimal.Parse(dr["electric_equip_amount"].ToString());
            status = int.Parse(dr["status"].ToString().ToLower() == "true" ? "1" : "0");
        }
    }
}