using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Configuration;

namespace Chaithit_Market.Models
{
    public class LogModel
    {
        public LogModel()
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US", false);

            string format = WebConfigurationManager.AppSettings["lorem_log_path"];

            string filename = DateTime.Today.ToString("ddMMyyyy");

            string path = string.Format(format, filename);

            if (!File.Exists(path))
            {
                FileStream stream = new FileStream(path, FileMode.Create);
                stream.Close();
            }
        }

        public void WriteExceptionLog(Exception ex, string servicename)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US", false);

            string format = WebConfigurationManager.AppSettings["lorem_log_path"];

            string filename = DateTime.Today.ToString("ddMMyyyy");

            string path = string.Format(format, filename);

            using (StreamWriter writer = new StreamWriter(path, true))
            {
                string description = ex.ToString();

                string[] split = description.Split('\\');

                string timestamp = DateTime.Now.ToLongTimeString();

                writer.WriteLine(string.Format("{0} --> {1} {2} {3} --> {4}", timestamp, split[split.Count() - 1], ex.TargetSite.Name, ex.Message, servicename));
            }
        }

        public void WriteCustomLog(string customString, string description)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US", false);

            string format = WebConfigurationManager.AppSettings["lorem_log_path"];

            string filename = DateTime.Today.ToString("ddMMyyyy");

            string path = string.Format(format, filename);

            using (StreamWriter writer = new StreamWriter(path, true))
            {
                string customString_ = customString.ToString();

                string[] split = description.Split('\\');

                string timestamp = DateTime.Now.ToLongTimeString();

                writer.WriteLine("\n" + string.Format("{0} --> {1} {2} {3} --> {4}", timestamp, split[split.Count() - 1], customString_, description, "!!!!!!"));
            }
        }
    }
}