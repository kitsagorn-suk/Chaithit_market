using Chaithit_Market.Core;
using Chaithit_Market.DTO;
using Chaithit_Market.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;

namespace Chaithit_Market.Controllers
{
    public class AuthenticationController : ApiController
    {
        private static AuthenticationController instance = null;
        private SQLManager _sql = SQLManager.Instance;
        private AuthenticationController()
        {
            if (_sql == null)
            {
                _sql = SQLManager.Instance;
            }
        }
        public static AuthenticationController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AuthenticationController();
                }
                return instance;
            }
        }

        public AuthorizationModel ValidateHeader(string authorization, string lang, bool check_user)
        {
            AuthorizationModel data = new AuthorizationModel();

            #region check auth null
            if (string.IsNullOrEmpty(authorization) || authorization.ToString().Trim().ToLower() == "null")
            {
                ValidationModel value = new ValidationModel();
                ValidationModel.InvalidState state;
                state = ValidationModel.InvalidState.E501;
                GetMessageTopicDTO getMessage = ValidationModel.GetInvalidMessage(state, lang);
                ValidationModel value_return = new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = getMessage.message, InvalidText = getMessage.topic };

                var response = new BasicResponse
                {
                    success = false,
                    msg = new MsgModel(value_return.InvalidMessage)
                };
                var error = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                error.Content = new ObjectContent<BasicResponse>(response, new JsonMediaTypeFormatter(), "application/json");
                throw new HttpResponseException(error);
            }

            #endregion

            try
            {
                data = DecodeAuthorization.AuthorizationDecode(authorization);
            }
            catch (Exception)
            {
                ValidationModel value = new ValidationModel();
                ValidationModel.InvalidState state;
                state = ValidationModel.InvalidState.E501;
                GetMessageTopicDTO getMessage = ValidationModel.GetInvalidMessage(state, lang);
                ValidationModel value_return = new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = getMessage.message, InvalidText = getMessage.topic };

                var ex = new Exception(string.Format("{0} - {1}", getMessage.message, HttpStatusCode.Unauthorized));
                ex.Data.Add(HttpStatusCode.Unauthorized, HttpStatusCode.Unauthorized);  // store "3" and "Invalid Parameters"
                throw ex;
            }

            #region checkhasauthorization
            bool success2 = _sql.CheckToken(authorization);
            if (!success2)
            {
                var response = new BasicResponse
                {
                    success = false,
                    msg = new MsgModel("มีการเข้าสู่ระบบจากเครื่องอื่นกรุณา login ใหม่")
                };
                var error = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                error.Content = new ObjectContent<BasicResponse>(response, new JsonMediaTypeFormatter(), "application/json");
                throw new HttpResponseException(error);
            }
            #endregion

            #region checkexpiretoken
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dateTime = dateTime.AddSeconds(data.expire_date).ToLocalTime();

            bool status_expire = (DateTime.Now > dateTime) ? true : false;
            if (status_expire)
            {
                ValidationModel value = new ValidationModel();
                ValidationModel.InvalidState state;
                state = ValidationModel.InvalidState.E502;
                GetMessageTopicDTO getMessage = ValidationModel.GetInvalidMessage(state, lang);
                ValidationModel value_return = new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = getMessage.message, InvalidText = getMessage.topic };

                var response = new BasicResponse
                {
                    success = false,
                    msg = new MsgModel(value_return.InvalidMessage)
                };
                var error = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                error.Content = new ObjectContent<BasicResponse>(response, new JsonMediaTypeFormatter(), "application/json");
                throw new HttpResponseException(error);
            }
            #endregion

            #region checkversion 
            //string platfrom = (data.version_android != "0") ? "ANDROID" : "IOS";
            //string version_android = string.Empty;
            //string version_ios = string.Empty;

            ////0 = not force, 1 = force, 2 = ผ่านเลย
            //int status_force_update = _sql.CheckVersionForceUpdate(data.version_android, data.version_ios, data.user_id);
            //if (status_force_update == 1)
            //{
            //    var response = new CheckUpdateResponse
            //    {
            //        status = false,
            //        error = MessageControler.Instance.GetMessageLang(lang, 399006),
            //        is_force = true,
            //        version = _sql.CheckLastVersion(platfrom),
            //        store_link = (platfrom == "ANDROID") ? WebConfigurationManager.AppSettings["url_android"] : WebConfigurationManager.AppSettings["url_ios"]
            //    };
            //    throw new WebFaultException<CheckUpdateResponse>(response, HttpStatusCode.UpgradeRequired);//426
            //}
            //else if ((status_force_update == 2) && (data.user_id != 0))
            //{
            //    _sql.UpdateStatusCheckUpdate(data.user_id, platfrom);
            //    var response = new CheckUpdateResponse
            //    {
            //        status = false,
            //        error = MessageControler.Instance.GetMessageLang(lang, 399006),
            //        is_force = false,
            //        version = _sql.CheckLastVersion(platfrom),
            //        store_link = (platfrom == "ANDROID") ? WebConfigurationManager.AppSettings["url_android"] : WebConfigurationManager.AppSettings["url_ios"]
            //    };
            //    throw new WebFaultException<CheckUpdateResponse>(response, HttpStatusCode.UpgradeRequired);//426
            //}

            #endregion

            #region checkmaintenance
            //DataTable dtMaintenance = _sql.CheckMaintenance();
            //DataRow drMaintenance = dtMaintenance.Rows[0];
            //string message = drMaintenance["description"].ToString();
            //bool status_maintenance = Convert.ToBoolean(int.Parse(drMaintenance["maintenance_flag"].ToString()));
            //if (status_expire)
            //{
            //    var response = new BasicResponse
            //    {
            //        status = false,
            //        error = message
            //    };
            //    throw new WebFaultException<BasicResponse>(response, HttpStatusCode.ServiceUnavailable);//503
            //}
            #endregion

            return data;
        }
    }
}