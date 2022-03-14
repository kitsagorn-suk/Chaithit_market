using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Chaithit_Market.Models
{
    public class SearchRentalModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public Pagination<SearchRental> data { get; set; }
    }

    public class SearchRental
    {
        public int rentID { set; get; } = 0;
        public string name { set; get; } = "";
        public decimal rentAmount { set; get; } = 0;
        public int placeID { set; get; } = 0;
        public int isUsed { set; get; } = 0;

        public void loadData(DataRow dr)
        {
            rentID = int.Parse(dr["id"].ToString());
            name = dr["name"].ToString();
            rentAmount = decimal.Parse(dr["rent_amount"].ToString());
            placeID = int.Parse(dr["place_id"].ToString());
            isUsed = int.Parse(dr["is_used"].ToString().ToLower() == "true" ? "1" : "0");
        }
    }
}