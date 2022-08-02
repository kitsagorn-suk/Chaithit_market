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
        public string firstName { set; get; } = "";
        public string lastName { set; get; } = "";
        public string detail { set; get; } = "";
        public int statusEmp { set; get; } = 0;
        public string userName { set; get; } = "";
        public string fileCode { set; get; } = "";
        public string[] imageUrl { get; set; } = { "" };
        public string recommender { set; get; } = "";
        public string userStartDate { set; get; } = "";
        public string taxNo { set; get; } = "";
        public string address { set; get; } = "";
        public int officeID { set; get; } = 0;
        public int branchNo { set; get; } = 0;

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
            firstName = dr["fisrt_name"].ToString();
            lastName = dr["last_name"].ToString();
            detail = dr["detail"].ToString();
            statusEmp = int.Parse(dr["status_emp"].ToString());
            userName = dr["user_name"].ToString();
            fileCode = dr["file_code"].ToString();
            recommender = dr["recommender"].ToString();
            userStartDate = dr["user_start_date"].ToString();
            taxNo = dr["tax_no"].ToString();
            address = dr["address"].ToString();
            officeID = int.Parse(dr["office_id"].ToString());
            branchNo = int.Parse(dr["branch_no"].ToString());
        }
    }
}