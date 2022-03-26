using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chaithit_Market.DTO
{
    public class MasterPlaceSubDTO
    {
        public string mode { set; get; } = "";
        public int placeSubID { set; get; } = 0;
        public int placeID { set; get; } = 0;
        public string nameEN { set; get; } = "";
        public string nameTH { set; get; } = "";
        public decimal amountRentDay { set; get; } = 0;
        public decimal amountRentMonth { set; get; } = 0;
        public decimal amountRentSpecial { set; get; } = 0;
        public string specialExpireDate { set; get; } = "";
        public decimal amountWater { set; get; } = 0;
        public decimal amountElectricity { set; get; } = 0;
    }
}