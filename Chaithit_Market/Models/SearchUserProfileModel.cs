using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Chaithit_Market.Models
{
    public class SearchUserProfileModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public Pagination<SearchUserProfile> data { get; set; }
    }

    public class SearchUserProfile
    {
        public int userProfileID { set; get; } = 0;
        public string userName { set; get; } = "";
        public string firstName { set; get; } = "";
        public string lastName { set; get; } = "";
        public string mobile { set; get; } = "";
        public string position { set; get; } = "";
        public int empType { set; get; } = 0;

        public void loadData(DataRow dr)
        {
            userProfileID = int.Parse(dr["id"].ToString());
            userName = dr["username"].ToString();
            firstName = dr["firstname"].ToString();
            lastName = dr["lastname"].ToString();
            mobile = dr["mobile"].ToString();
            position = dr["position"].ToString();
            empType = int.Parse(dr["emp_type"].ToString());
        }
    }
}