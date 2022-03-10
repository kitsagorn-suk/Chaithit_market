using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Chaithit_Market.Models
{
    public class CheckUserByTokenModel
    {
        public int userID { set; get; } = 0;

        public void loadData(DataRow dr)
        {
            userID = int.Parse(dr["id"].ToString());
        }
    }
}