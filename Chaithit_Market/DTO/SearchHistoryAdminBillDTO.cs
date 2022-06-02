using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chaithit_Market.DTO
{
    public class SearchHistoryAdminBillDTO
    {
        private string _billNumber;
        public string billNumber
        {
            get { return _billNumber ?? String.Empty; }
            set { _billNumber = value; }
        }
        public string startDate { set; get; } = "";
        public string endDate { set; get; } = "";
        public int isComplete { set; get; } = 0;
        public int perPage { set; get; } = 0;
        public int pageInt { set; get; } = 0;
        public int sortField { set; get; } = 0;
        public string sortType { set; get; } = "";
    }
}