using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Chaithit_Market.Models
{
    public class SearchManageRenterModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public Pagination<SearchManageRenter> data { get; set; }
    }

    public class SearchManageRenter
    {
        public int userProfileID { set; get; } = 0;
        public string name { set; get; } = "";
        public string mobile { set; get; } = "";
        public int empType { set; get; } = 0;
        public string empTypeName { set; get; } = "";
        public string startDate { set; get; } = "";
        public string endDate { set; get; } = "";
        public string unitNo { set; get; } = "";

        public void loadData(DataRow dr)
        {
            userProfileID = int.Parse(dr["user_id"].ToString());
            name = dr["name"].ToString();
            mobile = dr["mobile"].ToString();
            empType = int.Parse(dr["emp_type"].ToString());
            empTypeName = dr["emp_type_name"].ToString();
            startDate = dr["start_date"].ToString();
            endDate = dr["end_date"].ToString();
            unitNo = dr["unit_no"].ToString();
        }
    }
}