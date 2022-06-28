using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chaithit_Market.DTO
{
    public class SaveUserProfileDTO
    {
        public string mode { set; get; } = "";
        public int userProfileID { set; get; } = 0;
        public string userName { set; get; } = "";
        public string password { set; get; } = "";
        public string firstName { set; get; } = "";
        public string lastName { set; get; } = "";
        public string mobile { set; get; } = "";
        public string position { set; get; } = "";
        public string startDate { set; get; } = "";
        public string endDate{ set; get; } = "";
        public int statusEmp { set; get; } = 0;
        public int empType { set; get; } = 0;
        public string recommender { set; get; } = "";
        public string address { set; get; } = "";
        public string taxNo { set; get; } = "";
        public int officeID { set; get; } = 0;
        public int branchNo { set; get; } = 0;
    }
}