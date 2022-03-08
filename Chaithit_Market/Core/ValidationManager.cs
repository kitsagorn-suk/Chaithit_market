using Chaithit_Market.DTO;
using Chaithit_Market.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Chaithit_Market.Core
{
    public class ValidationManager
    {
        private static SQLManager _sql = SQLManager.Instance;

        public static ValidationModel CheckValidation(int chkID, string lang, string platform)
        {
            ValidationModel value = new ValidationModel();
            try
            {
                ValidationModel.InvalidState state;

                #region E300001
                state = ValidationModel.InvalidState.E300001; //Error Platform
                if (platform != "web" || platform == null || platform == "")
                {
                    GetMessageTopicDTO getMessage = ValidationModel.GetInvalidMessage(state, lang);
                    return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = getMessage.message, InvalidText = getMessage.topic };
                }
                #endregion

                #region E302001
                state = ValidationModel.InvalidState.E302001; //Data not found
                if (chkID == 0)
                {
                    GetMessageTopicDTO getMessage = ValidationModel.GetInvalidMessage(state, lang);
                    return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = getMessage.message, InvalidText = getMessage.topic };
                }
                #endregion

                GetMessageTopicDTO getMessageSuccess = ValidationModel.GetInvalidMessage(ValidationModel.InvalidState.S201001, lang);
                value.Success = true;
                value.InvalidCode = ValidationModel.GetInvalidCode(ValidationModel.InvalidState.S201001);
                value.InvalidMessage = getMessageSuccess.message;
                value.InvalidText = getMessageSuccess.topic;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public static ValidationModel CheckValidationLogin(string username, string password, string lang, int dataID)
        {
            ValidationModel value = new ValidationModel();
            try
            {
                GetMessageTopicDTO getMessage = new GetMessageTopicDTO();
                ValidationModel.InvalidState state;

                #region E301001
                state = ValidationModel.InvalidState.E301001; //ชื่อผู้ใช้หรือรหัสผ่านไม่ถูกต้อง
                int user_id = _sql.CheckUserPassword(username, password);
                if (user_id == 0)
                {
                    GetMessageTopicDTO getMessage301001 = ValidationModel.GetInvalidMessage(state, lang);
                    return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = getMessage301001.message, InvalidText = getMessage301001.topic };
                }
                #endregion

                getMessage = ValidationModel.GetInvalidMessage(ValidationModel.InvalidState.S201001, lang);
                value.Success = true;
                value.InvalidCode = ValidationModel.GetInvalidCode(ValidationModel.InvalidState.S201001);
                value.InvalidMessage = getMessage.message;
                value.InvalidText = getMessage.topic;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public static ValidationModel CheckValidationUpdate(int ID, string Type, string lang, List<string> listobjectID, int roleID)
        {
            ValidationModel value = new ValidationModel();
            //try
            //{
            //    GetMessageTopicDTO getMessage = new GetMessageTopicDTO();
            //    ValidationModel.InvalidState state;

            //    DataTable dt = _sql.CheckValidationUpdateByID(ID, Type);

            //    string result = "";
            //    bool haveAuthorization = false;
            //    foreach (var pObjectID in listobjectID)
            //    {
            //        DataTable dtIsActive = _sql.CheckValidationRoleID(pObjectID, roleID);
            //        if (dtIsActive.Rows.Count > 0)
            //        {
            //            if (dtIsActive.Rows[0]["is_active"].ToString().Equals("1"))
            //            {
            //                result += "1";
            //            }
            //            else
            //            {
            //                result += "0";
            //            }
            //        }
            //        else
            //        {
            //            result += "0";
            //        }
            //    }

            //    if (!string.IsNullOrEmpty(result))
            //    {
            //        if (!result.Contains("0"))
            //        {
            //            haveAuthorization = true;
            //        }
            //    }

            //    if (dt.Rows.Count > 0)
            //    {
            //        if (!haveAuthorization)
            //        {
            //            state = ValidationModel.InvalidState.E301007; //คุณไม่มีสิทธิ์แก้ไข
            //            getMessage = ValidationModel.GetInvalidMessage(state, lang);
            //            return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = getMessage.message, InvalidText = getMessage.topic };
            //        }
            //    }
            //    else
            //    {
            //        state = ValidationModel.InvalidState.E302001; //Data not found
            //        getMessage = ValidationModel.GetInvalidMessage(state, lang);
            //        return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = getMessage.message, InvalidText = getMessage.topic };
            //    }

            //    getMessage = ValidationModel.GetInvalidMessage(ValidationModel.InvalidState.S201001, lang);
            //    value.Success = true;
            //    value.InvalidCode = ValidationModel.GetInvalidCode(ValidationModel.InvalidState.S201001);
            //    value.InvalidMessage = getMessage.message;
            //    value.InvalidText = getMessage.topic;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            return value;
        }
    }
}