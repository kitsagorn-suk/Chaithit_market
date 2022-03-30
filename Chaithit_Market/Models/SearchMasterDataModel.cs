using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Chaithit_Market.Models
{
    public class SearchMasterDataModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public Pagination<SearchMasterData> data { get; set; }
    }

    public class SearchMasterData
    {
        public int id { set; get; } = 0;
        public string name { set; get; } = "";
        public int status { set; get; } = 0;

        public void loadData(DataRow dr)
        {
            id = int.Parse(dr["id"].ToString());
            name = dr["name"].ToString();
            status = int.Parse(dr["status"].ToString().ToLower() == "true" ? "1" : "0");
        }
    }
}