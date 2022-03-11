using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Chaithit_Market.Models
{
    public class ReturnIdModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public _ReturnIdModel data { get; set; }
    }

    public class _ReturnIdModel
    {
        public int id { get; set; } = 0;

        public void loadData(DataRow dr)
        {
            id = int.Parse(dr["id"].ToString());
        }

    }
}