using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chaithit_Market.DTO
{
    public class ChangePasswordDTO
    {
        public int userID { set; get; } = 0;
        public string passwordOld { set; get; } = "";
        public string passwordNew { set; get; } = "";
        public string passwordNewConfirm { set; get; } = "";
    }
}