﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chaithit_Market.DTO
{
    public class GetHistoryUserBillDTO
    {
        public int userID { set; get; } = 0;
        public string startDate { set; get; } = "";
        public string endDate { set; get; } = "";
     }
}