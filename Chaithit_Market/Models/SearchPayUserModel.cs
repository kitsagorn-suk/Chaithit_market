using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Chaithit_Market.Models
{
    public class SearchPayUserModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public Pagination<SearchPayUser> searchPayUser { get; set; }
    }

    public class SearchPayUser
    {
        public int payID { set; get; } = 0;
        public string billNumber { set; get; } = "";
        public decimal payAmount { set; get; } = 0;
        public string[] fileCodeUrl { set; get; } = { };
        public string comment { set; get; } = "";
        public string createDate { set; get; } = "";

        public void loadData(DataRow dr)
        {
            payID = int.Parse(dr["pay_id"].ToString());
            billNumber = dr["bill_code"].ToString();
            payAmount = decimal.Parse(dr["pay_amount"].ToString());
            if (!string.IsNullOrEmpty(dr["file_code_url"].ToString()))
            {
                string url = dr["file_code_url"].ToString();
                fileCodeUrl = url.Split(',');
            }
            comment = dr["comment"].ToString();
            createDate = dr["create_date"].ToString();
        }
    }
}