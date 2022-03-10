using Chaithit_Market.Core;
using Chaithit_Market.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chaithit_Market.Services
{
    public class UploadService
    {
        private SQLManager _sql = SQLManager.Instance;

        public UploadModel UploadImageUserProfile(string file_path, string folderName, string lang)
        {
            UploadModel value = new UploadModel();
            try
            {
                string urlWeb = "https://dev-support.askmebet.co/ServiceChaithitMarket/UploadFileUser/";

                string folder = "/ImageFilePath/";

                value.data = new _ServiceUploadData();

                double timestampNow = Utility.DateTimeToUnixTimestamp(DateTime.Now);
                string receivedata = "file_path: " + file_path;

                ValidationModel validation = ValidationManager.CheckValidationUpload(file_path, lang);

                if (validation.Success == true)
                {
                    value.data.img_url = urlWeb + folderName + folder + file_path;
                }

                value.success = validation.Success;
                value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, topic = validation.InvalidText };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
            return value;
        }
    }
}