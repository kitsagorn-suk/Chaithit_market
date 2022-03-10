using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chaithit_Market.DTO
{
    public class InsertTransectionBillDTO
    {
        public string billCode { set; get; } = "";
        public int tranRentID { set; get; } = 0;
        public string startDate { set; get; } = "";
        public string endDate { set; get; } = "";
        public decimal rentalAmount { set; get; } = 0;
        public decimal electricAmount { set; get; } = 0;
        public decimal waterAmount { set; get; } = 0;
        public decimal totalAmount { set; get; } = 0;
    }
}