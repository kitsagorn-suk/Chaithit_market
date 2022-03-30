using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chaithit_Market.DTO
{
    public class SearchUnitstandDTO
    {
        public string unitCode { set; get; } = "";
        public string unitName { set; get; } = "";
        public int perPage { set; get; } = 0;
        public int pageInt { set; get; } = 0;
        public int sortField { set; get; } = 0;
        public string sortType { set; get; } = "";
    }
}