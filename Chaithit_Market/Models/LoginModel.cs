using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Chaithit_Market.Models
{
    public class LoginModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public LoginData data { get; set; }
    }

    public class LoginData
    {
        public int id { get; set; } = 0;
        public string username { get; set; } = "";
        public string employeeName { get; set; } = "";
        public string imageUrl { get; set; } = "";
        public string token { get; set; } = "";
        public string platform { get; set; } = "";

        public void loadData(DataRow dr)
        {
            id = int.Parse(dr["id"].ToString());
            username = dr["username"].ToString();
            employeeName = dr["firstname"].ToString() + " " + dr["lastname"].ToString();
            imageUrl = dr["image_name"].ToString();
        }
    }
}