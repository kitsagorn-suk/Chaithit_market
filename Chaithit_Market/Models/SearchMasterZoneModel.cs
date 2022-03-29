using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Chaithit_Market.Models
{
    public class SearchMasterZoneModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public Pagination<SearchMasterZone> data { get; set; }
    }

    public class SearchMasterZone
    {
        public int zoneID { set; get; } = 0;
        public string name { set; get; } = "";
        public int status { set; get; } = 0;

        public void loadData(DataRow dr)
        {
            zoneID = int.Parse(dr["id"].ToString());
            name = dr["name"].ToString();
            status = int.Parse(dr["status"].ToString().ToLower() == "true" ? "1" : "0");
        }
    }
}