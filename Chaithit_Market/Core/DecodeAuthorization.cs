using Chaithit_Market.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chaithit_Market.Core
{
    public class DecodeAuthorization
    {
        public static AuthorizationModel AuthorizationDecode(string auth)
        {
            AuthorizationModel dataAuth = new AuthorizationModel();

            try
            {
                string[] data = auth.Split('.');
                if (data.Length != 3)
                {
                    throw new Exception();
                }
                string header = data[0];
                string payload = data[1];
                string signature = data[2];

                string decodeHeader = Utility.Base64ForUrlDecode(header);
                string decodePayload = Utility.Base64ForUrlDecode(payload);

                JToken tokenHeader = JObject.Parse(decodeHeader);
                JToken tokenPayload = JObject.Parse(decodePayload);

                dataAuth.user_id = int.Parse(tokenPayload["user_id"].ToString());
                dataAuth.create_date = Convert.ToDouble(tokenPayload["create_date"].ToString());
                dataAuth.expire_date = Convert.ToDouble(tokenPayload["expire_date"].ToString());
                dataAuth.username = tokenHeader["username"].ToString();
                dataAuth.password = tokenHeader["password"].ToString();
                dataAuth.type_login = tokenHeader["platform_login"].ToString();
                dataAuth.signal = tokenHeader["signal"].ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return dataAuth;
        }
    }
}