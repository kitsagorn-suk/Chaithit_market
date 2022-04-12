using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Chaithit_Market.Models
{
    public class SearchMasterPrefixUnitModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public Pagination<SearchMasterPrefixUnit> data { get; set; }
    }

    public class SearchMasterPrefixUnit
    {
        public int prefixUnitID { set; get; } = 0;
        public int zoneID { set; get; } = 0;
        public string prefixUnitName { set; get; } = "";
        public string zoneName { set; get; } = "";
        public int status { set; get; } = 0;

        public void loadData(DataRow dr)
        {
            prefixUnitID = int.Parse(dr["id"].ToString());
            prefixUnitName = dr["prefix_name"].ToString();
            zoneID = int.Parse(dr["zone_id"].ToString());
            zoneName = dr["zone_name"].ToString();
            status = int.Parse(dr["status"].ToString().ToLower() == "true" ? "1" : "0");
        }
    }
}