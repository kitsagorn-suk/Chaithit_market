using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chaithit_Market.Models
{
    public class UploadModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public _ServiceUploadData data { get; set; }
    }

    public class _ServiceUploadData
    {
        public string img_url { get; set; } = "";
        public string file_size { get; set; } = "";
    }
}