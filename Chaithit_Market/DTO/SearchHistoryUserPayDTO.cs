using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chaithit_Market.DTO
{
    public class SearchHistoryUserPayDTO
    {
        public int userID { set; get; } = 0;
        public int perPage { set; get; } = 0;
        public int pageInt { set; get; } = 0;
        public int sortField { set; get; } = 0;
        public string sortType { set; get; } = "";
    }
}