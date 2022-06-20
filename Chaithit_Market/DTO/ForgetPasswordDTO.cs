using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chaithit_Market.DTO
{
    public class ForgetPasswordDTO
    {
        public int userID { set; get; } = 0;
        public string passwordNew { set; get; } = "";
    }
}