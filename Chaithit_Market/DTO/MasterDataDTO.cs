using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chaithit_Market.DTO
{
    public class MasterDataDTO
    {
        public int masterID { set; get; } = 0;
        public string mode { set; get; } = "";
        public string nameEN { set; get; } = "";
        public string nameTH { set; get; } = "";
    }
}