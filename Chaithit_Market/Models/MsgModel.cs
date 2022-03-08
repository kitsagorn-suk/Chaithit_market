using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chaithit_Market.Models
{
    public class MsgModel
    {
        public MsgModel() { }
        public MsgModel(string text)
        {
            this.text = text;
        }
        public int code { get; set; } = 0;
        public string text { get; set; } = "";
        public string topic { get; set; } = "";
    }
}