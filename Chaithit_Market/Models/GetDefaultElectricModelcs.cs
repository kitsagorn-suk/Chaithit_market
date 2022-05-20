using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Chaithit_Market.Models
{
    public class GetDefaultElectricModelcs
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public DefaultElectricModelc data { get; set; }
    }

    public class DefaultElectricModelc
    {
        public decimal electricUnit { set; get; } = 0;

        public void loadData(DataRow dr)
        {
            electricUnit = decimal.Parse(dr["electric_unit"].ToString());
        }
    }
}