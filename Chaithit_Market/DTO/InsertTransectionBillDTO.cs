using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chaithit_Market.DTO
{
    public class InsertTransectionBillDTO
    {
        public int tranBillID { set; get; } = 0;
        public string mode { set; get; } = "";
        public int zoneID { set; get; } = 0;
        public int tranRentID { set; get; } = 0;
        public string billCode { set; get; } = "";
        public string startDate { set; get; } = "";
        public string endDate { set; get; } = "";
        public decimal rentAmount { set; get; } = 0;
        public int electricUnit { set; get; } = 0;
        public decimal electricAmount { set; get; } = 0;
        public int waterUnit { set; get; } = 0;
        public decimal waterAmount { set; get; } = 0;
        public int lampUnit { set; get; } = 0;
        public int electricEquipUnit { set; get; } = 0;
        public decimal electricNightMarketAmount { set; get; } = 0;
        public decimal totalAmount { set; get; } = 0;
        public int discountPercent { set; get; } = 0;
        public decimal discountAmount { set; get; } = 0;
        public decimal netAmount { set; get; } = 0;
        public string payDate { set; get; } = "";
        public int completeAmount { set; get; } = 0;
        public int rentHaveVat { set; get; } = 0;
        public int electricHaveVat { set; get; } = 0;
        public int waterHaveVat { set; get; } = 0;
    }
}