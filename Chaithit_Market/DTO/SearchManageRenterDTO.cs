using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chaithit_Market.DTO
{
    public class SearchManageRenterDTO
    {
        private string _nameOrMobile;
        public string nameOrMobile {
            get { return _nameOrMobile ?? String.Empty; }
            set { _nameOrMobile = value; }
        } 
        public int empType { set; get; } = 0;
        public string unitNo { set; get; } = "";
        public string startDate { set; get; } = "";
        public string endDate { set; get; } = "";
        public string type { set; get; } = "";
        public int perPage { set; get; } = 0;
        public int pageInt { set; get; } = 0;
        public int sortField { set; get; } = 0;
        public string sortType { set; get; } = "";
    }
}