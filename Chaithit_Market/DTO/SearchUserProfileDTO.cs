using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chaithit_Market.DTO
{
    public class SearchUserProfileDTO
    {
        public string userName { set; get; } = "";
        public string firstName { set; get; } = "";
        public string lastName { set; get; } = "";
        public string mobile { set; get; } = "";
        public string position { set; get; } = "";
        public int perPage { set; get; } = 0;
        public int pageInt { set; get; } = 0;
        public int sortField { set; get; } = 0;
        public string sortType { set; get; } = "";
    }
}