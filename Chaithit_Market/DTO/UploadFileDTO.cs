using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chaithit_Market.DTO
{
    public class UploadFileDTO
    {
        public int fileDetailID { set; get; } = 0; 
        public int actionID { set; get; } = 0;
        public string actionName { set; get; } = "";
        public string fileCode { set; get; } = "";
        public string fileExtension { set; get; } = "";
        public string name { set; get; } = "";
        public string url { set; get; } = "";
    }
}