using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Chaithit_Market.Models
{
    public class GetAllDropdownModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public List<DropdownAllData> data { get; set; }
    }

    public class DropdownAllData
    {
        public int id { get; set; } = 0;
        public string name { get; set; } = "";

        public void loadData(DataRow dr)
        {
            id = int.Parse(dr["id"].ToString());
            name = dr["name"].ToString();
        }

    }
}