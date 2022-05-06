using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Chaithit_Market.Models
{
    public class GetDropdownMonthModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public List<DropdownMonth> data { get; set; }
    }

    public class DropdownMonth
    {
        public string id { get; set; } = "";
        public string name { get; set; } = "";

        public void loadData(DataRow dr)
        {
            id = dr["id"].ToString();
            name = dr["name"].ToString();
        }

    }
}