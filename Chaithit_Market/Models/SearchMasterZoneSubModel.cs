using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Chaithit_Market.Models
{
    public class SearchMasterZoneSubModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public Pagination<SearchMasterZoneSub> data { get; set; }
    }

    public class SearchMasterZoneSub
    {
        public int zoneSubID { set; get; } = 0;
        public int zoneID { set; get; } = 0;
        public string zoneSubName { set; get; } = "";
        public string zoneName { set; get; } = "";
        public int status { set; get; } = 0;

        public void loadData(DataRow dr)
        {
            zoneSubID = int.Parse(dr["id"].ToString());
            zoneSubName = dr["zoneSub_name"].ToString();
            zoneID = int.Parse(dr["zone_id"].ToString());
            zoneName = dr["zone_name"].ToString();
            status = int.Parse(dr["status"].ToString().ToLower() == "true" ? "1" : "0");
        }
    }
}