using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chaithit_Market.DTO
{
    public class SaveRateAmountDTO
    {
        public string mode { set; get; } = "";
        public int rateID { set; get; } = 0;
        public string name { set; get; } = "";
        public decimal rentAmountDay { set; get; } = 0;
        public decimal rentAmountMonth { set; get; } = 0;
        public decimal electricAmount { set; get; } = 0;
        public decimal waterAmount { set; get; } = 0;
        public decimal lampAmountPerOne { set; get; } = 0;
        public decimal electricEquipAmount { set; get; } = 0;
        public int status { set; get; } = 0;
    }
}