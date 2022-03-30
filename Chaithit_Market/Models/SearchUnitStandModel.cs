using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Chaithit_Market.Models
{
    public class SearchUnitStandModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public Pagination<SearchUnitStand> data { get; set; }
    }

    public class SearchUnitStand
    {
        public int unitID { set; get; } = 0;
        public string unitCode { set; get; } = "";
        public string unitName { set; get; } = "";
        public string userName { set; get; } = "";
        public decimal rentAmount { set; get; } = 0;
        public int zoneID { set; get; } = 0;
        public int zoneSubID { set; get; } = 0;
        public string startDate { set; get; } = "";
        public string endDate { set; get; } = "";

        public void loadData(DataRow dr)
        {
            unitID = int.Parse(dr["id"].ToString());
            unitCode = dr["unit_code"].ToString();
            unitName = dr["unit_name"].ToString();
            userName = dr["name"].ToString();
            rentAmount = decimal.Parse(dr["rent_amount"].ToString());
            zoneID = int.Parse(dr["zone_id"].ToString());
            zoneSubID = int.Parse(dr["zone_sub_id"].ToString());
            startDate = DateTime.Parse(dr["start_date"].ToString()).ToString("yyyy-MM-dd");
            endDate = DateTime.Parse(dr["end_date"].ToString()).ToString("yyyy-MM-dd");
        }
    }
}