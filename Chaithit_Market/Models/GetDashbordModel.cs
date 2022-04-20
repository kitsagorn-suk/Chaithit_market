using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Chaithit_Market.Models
{
    public class GetDashbordModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public DashbordModel data { get; set; }
    }

    public class DashbordModel
    {
        public decimal totalAmt { set; get; } = 0;
        public decimal collectAmt { set; get; } = 0;
        public decimal notComplete { set; get; } = 0;
        public decimal overPay { set; get; } = 0;
        public int totalUnit { set; get; } = 0;
        public int isUse { set; get; } = 0;
        public int notUse { set; get; } = 0;

        public void loadData(DataRow dr)
        {
            totalAmt = decimal.Parse(dr["Totalamt"].ToString());
            collectAmt = decimal.Parse(dr["Collectamt"].ToString());
            notComplete = decimal.Parse(dr["Notcomplete"].ToString());
            overPay = decimal.Parse(dr["Overpay"].ToString());
            totalUnit = int.Parse(dr["Totalunit"].ToString());
            isUse = int.Parse(dr["Isuse"].ToString());
            notUse = int.Parse(dr["Notuse"].ToString());
        }
    }
}