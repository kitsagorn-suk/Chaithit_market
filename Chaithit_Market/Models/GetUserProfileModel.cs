using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Chaithit_Market.Models
{
    public class GetUserProfileModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public UserProfileModel data { get; set; }
    }

    public class UserProfileModel
    {
        public int userProfileID { set; get; } = 0;
        public string userName { set; get; } = "";
        public string firstName { set; get; } = "";
        public string lastName { set; get; } = "";
        public string mobile { set; get; } = "";
        public string position { set; get; } = "";
        public string startDate { set; get; } = "";
        public string endDate { set; get; } = "";
        public int statusEmp { set; get; } = 0;
        public string fileCode { set; get; } = "";
        public string[] imageUrl { get; set; } = { "" };

        public void loadData(DataRow dr)
        {
            userProfileID = int.Parse(dr["id"].ToString());
            userName = dr["username"].ToString();
            firstName = dr["firstname"].ToString();
            lastName = dr["lastname"].ToString();
            mobile = dr["mobile"].ToString();
            position = dr["position"].ToString();
            startDate = DateTime.Parse(dr["start_date"].ToString()).ToString("yyyy-MM-dd");
            endDate = DateTime.Parse(dr["end_date"].ToString()).ToString("yyyy-MM-dd");
            statusEmp = int.Parse(dr["status_emp"].ToString());
            fileCode = dr["file_code"].ToString();
            //dnsMeter = dr["dns_meter"].ToString();
        }

        public List<MarketDetail> dataMarket { get; set; }
    }

    public class MarketDetail
    {
        public string rentName { set; get; } = "";
        public string startDate { set; get; } = "";
        public string endDate { set; get; } = "";

        public void loadData(DataRow dr)
        {
            rentName = dr["rent_name"].ToString();
            startDate = DateTime.Parse(dr["start_date"].ToString()).ToString("yyyy-MM-dd");
            endDate = DateTime.Parse(dr["end_date"].ToString()).ToString("yyyy-MM-dd");
        }
    }
}