﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chaithit_Market.DTO
{
    public class SaveRentalDTO
    {
        public string mode { set; get; } = "";
        public int rentalID { set; get; } = 0;
        public int placeSubID { set; get; } = 0;
        public string rentCode { set; get; } = "";
        public string name { set; get; } = "";
        public int isUsed { set; get; } = 0;
    }
}