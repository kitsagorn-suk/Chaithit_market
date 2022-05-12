using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Chaithit_Market.Models
{
    public class GetTranPayImageModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public TranPayImageModel data { get; set; }
    }

    public class TranPayImageModel
    {
        public string fileCode { set; get; } = "";
        public string[] imageUrl { get; set; } = { "" };

        public void loadData(DataRow dr)
        {
            fileCode = dr["file_code"].ToString();
        }
    }
}