using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Chaithit_Market.Models
{
    public class GetRenterByUserIDModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public RenterByUserIDModel data { get; set; }
    }

    public class RenterByUserIDModel
    {
        public string name { set; get; } = "";

        public void loadData(DataRow dr)
        {
            name = dr["name"].ToString();
        }

        public List<RentDetailModel> dataRent { get; set; }
        public List<RentDetailModel> dataRentNight { get; set; }
    }

    public class RentDetailModel
    {
        public int rentID { set; get; } = 0;
        public int zoneID { set; get; } = 0;
        public string zoneName { set; get; } = "";
        public string unitName { set; get; } = "";
        public decimal rentAmount { set; get; } = 0;
        public decimal electricPerUnit { set; get; } = 0;
        public decimal waterPerUnit { set; get; } = 0;
        public decimal lampAmountPerOne { set; get; } = 0;
        public decimal electricEquipPerUnit { set; get; } = 0;
        public decimal electricUnit { set; get; } = 0;
        public decimal waterUnit { set; get; } = 0;
        public decimal lampUnit { set; get; } = 0;
        public decimal electricEquipUnit { set; get; } = 0;
        public string rentType { set; get; } = "";
        public string statusPay { set; get; } = "";
        public string statusElectric { set; get; } = "";

        public void loadData(DataRow dr)
        {
            rentID = int.Parse(dr["id"].ToString());
            zoneID = int.Parse(dr["zone_id"].ToString());
            zoneName = dr["zone_name"].ToString();
            unitName = dr["unit_name"].ToString();
            rentAmount = decimal.Parse(dr["rent_amount"].ToString());
            electricPerUnit = decimal.Parse(dr["electric_per_unit"].ToString());
            waterPerUnit = decimal.Parse(dr["water_per_unit"].ToString());
            lampAmountPerOne = decimal.Parse(dr["lamp_amount_per_one"].ToString());
            electricEquipPerUnit = decimal.Parse(dr["electric_equip_per_unit"].ToString());
            electricUnit = decimal.Parse(dr["electric_unit"].ToString());
            waterUnit = decimal.Parse(dr["water_unit"].ToString());
            lampUnit = decimal.Parse(dr["lamp_unit"].ToString());
            electricEquipUnit = decimal.Parse(dr["electric_equip_unit"].ToString());
            rentType = dr["rent_type"].ToString();
            statusPay = dr["status_pay"].ToString();
            statusElectric = dr["status_electric"].ToString();
        }
    }
}