using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Chaithit_Market.Models
{
    public class SearchRentalStandModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public Pagination<SearchRentalStand> data { get; set; }
    }

    public class SearchRentalStand
    {
        public int rentID { set; get; } = 0;
        public string rentCode { set; get; } = "";
        public string rentName { set; get; } = "";
        public string userName { set; get; } = "";
        public decimal rentAmount { set; get; } = 0;
        public int placeID { set; get; } = 0;
        public string startDate { set; get; } = "";
        public string endDate { set; get; } = "";

        public void loadData(DataRow dr)
        {
            rentID = int.Parse(dr["id"].ToString());
            rentCode = dr["rent_code"].ToString();
            rentName = dr["rent_name"].ToString();
            userName = dr["name"].ToString();
            rentAmount = decimal.Parse(dr["rent_amount"].ToString());
            placeID = int.Parse(dr["place_id"].ToString());
            startDate = dr["start_date"].ToString();
            endDate = dr["end_date"].ToString();
        }
    }
}