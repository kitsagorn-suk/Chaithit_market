using Chaithit_Market.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Chaithit_Market.Core
{
    public class GenAuthorization
    {
        public static string GetAuthorization(string username, string password, string signal, string platform)
        {
            SQLManager _sql = SQLManager.Instance;
            int expire_token = int.Parse(WebConfigurationManager.AppSettings["expire_token"]);

            double timestampNow = Utility.DateTimeToUnixTimestamp(DateTime.Now);
            double timestampExpire = Utility.DateTimeToUnixTimestamp(DateTime.Now.AddDays(expire_token));
            CheckUserByTokenModel dataFormToken = new CheckUserByTokenModel();
            dataFormToken = _sql.CheckUserID(username, password);

            string header = "";
            header = "{";
            header += " \"username\":\"" + username + "\",";
            header += " \"password\":\"" + password + "\",";
            header += " \"platform_login\":\"" + platform + "\",";
            header += " \"signal\":\"" + signal + "\"";
            header += " }";

            string payload = "";
            payload = "{";
            payload += " \"expire_date\":" + timestampExpire + ",";
            payload += " \"create_date\":" + timestampNow + ",";
            payload += " \"user_id\":" + dataFormToken.userID + ",";
            payload += " \"role_id\":" + dataFormToken.roleID + ",";
            payload += " }";

            string encryptedHeader = Utility.Base64UrlEncode(header);
            string encryptedPayload = Utility.Base64UrlEncode(payload);
            string encryptedSignature = Utility.sha256(encryptedHeader + "." + encryptedPayload + signal);
            string result = encryptedHeader + "." + encryptedPayload + "." + encryptedSignature;

            return result;
        }
    }
}