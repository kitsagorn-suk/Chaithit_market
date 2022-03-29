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

        public static ValidationModel CheckValidationLogin(string username, string password , int type, string lang, int dataID)
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

                #region E301010
                state = ValidationModel.InvalidState.E301010; //ชื่อผู้ใช้หรือรหัสผ่านไม่ถูกต้อง
                int status_emp = _sql.CheckUserType(username);
                if (status_emp != type)
                {
                    GetMessageTopicDTO getMessage301010 = ValidationModel.GetInvalidMessage(state, lang);
                    return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = getMessage301010.message, InvalidText = getMessage301010.topic };
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

        public static ValidationModel CheckValidationIDUpdate(int ID, string Tablename, string lang)
        {
            ValidationModel value = new ValidationModel();
            try
            {
                GetMessageTopicDTO getMessage = new GetMessageTopicDTO();
                ValidationModel.InvalidState state;

                DataTable dt = _sql.CheckValidationUpdateByID(ID, Tablename);

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["count"].ToString() == "0")
                    {
                        state = ValidationModel.InvalidState.E302001; //Data not found
                        getMessage = ValidationModel.GetInvalidMessage(state, lang);
                        return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = getMessage.message, InvalidText = getMessage.topic };
                    }
                }

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

        public static ValidationModel CheckValidationDupicateMasterData(string lang, string TableName, MasterDataDTO masterDataDTO)
        {
            if (_sql == null)
            {
                _sql = SQLManager.Instance;
            }
            ValidationModel value = new ValidationModel();
            try
            {
                GetMessageTopicDTO getMessage = new GetMessageTopicDTO();
                ValidationModel.InvalidState state;

                DataTable dt = _sql.CheckDuplicateMaster(TableName,masterDataDTO);

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["status_name_en"].ToString() != "0")
                    {
                        state = ValidationModel.InvalidState.E301008;
                        getMessage = ValidationModel.GetInvalidMessage(state, lang);
                        return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = getMessage.message, InvalidText = getMessage.topic };
                    }
                    if (dt.Rows[0]["status_name_th"].ToString() != "0")
                    {
                        state = ValidationModel.InvalidState.E301009;
                        getMessage = ValidationModel.GetInvalidMessage(state, lang);
                        return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = getMessage.message, InvalidText = getMessage.topic };
                    }
                }

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

        public static ValidationModel CheckValidationDupicateUser(string lang, SaveUserProfileDTO saveUserProfileDTO)
        {
            ValidationModel value = new ValidationModel();
            try
            {
                GetMessageTopicDTO getMessage = new GetMessageTopicDTO();
                ValidationModel.InvalidState state;

                DataTable dt = _sql.CheckDupicateUser(saveUserProfileDTO, 0);

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Name"].ToString() != "0")
                    {
                        state = ValidationModel.InvalidState.E301002;
                        getMessage = ValidationModel.GetInvalidMessage(state, lang);
                        return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = getMessage.message, InvalidText = getMessage.topic };
                    }
                    if (dt.Rows[0]["UserName"].ToString() != "0")
                    {
                        state = ValidationModel.InvalidState.E301004;
                        getMessage = ValidationModel.GetInvalidMessage(state, lang);
                        return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = getMessage.message, InvalidText = getMessage.topic };
                    }
                    if (dt.Rows[0]["Mobile"].ToString() != "0")
                    {
                        state = ValidationModel.InvalidState.E301003;
                        getMessage = ValidationModel.GetInvalidMessage(state, lang);
                        return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = getMessage.message, InvalidText = getMessage.topic };
                    }
                }

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

        public static ValidationModel CheckValidationDupicateRental(string lang, SaveRentalDTO saveRentalDTO)
        {
            ValidationModel value = new ValidationModel();
            try
            {
                GetMessageTopicDTO getMessage = new GetMessageTopicDTO();
                ValidationModel.InvalidState state;

                DataTable dt = _sql.CheckDupicateRental(saveRentalDTO, 0);

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["RentCode"].ToString() != "0")
                    {
                        state = ValidationModel.InvalidState.E301005;
                        getMessage = ValidationModel.GetInvalidMessage(state, lang);
                        return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = getMessage.message, InvalidText = getMessage.topic };
                    }
                    if (dt.Rows[0]["Name"].ToString() != "0")
                    {
                        state = ValidationModel.InvalidState.E301006;
                        getMessage = ValidationModel.GetInvalidMessage(state, lang);
                        return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = getMessage.message, InvalidText = getMessage.topic };
                    }
                }

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

        public static ValidationModel CheckValidationDupicateTransectionRent(string lang, InsertTransectionRentDTO insertTransectionRentDTO)
        {
            ValidationModel value = new ValidationModel();
            try
            {
                GetMessageTopicDTO getMessage = new GetMessageTopicDTO();
                ValidationModel.InvalidState state;

                DataTable dt = _sql.CheckDupicateTransectionRent(insertTransectionRentDTO, 0);

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["TransCode"].ToString() != "0")
                    {
                        state = ValidationModel.InvalidState.E301007;
                        getMessage = ValidationModel.GetInvalidMessage(state, lang);
                        return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = getMessage.message, InvalidText = getMessage.topic };
                    }
                    if (dt.Rows[0]["CheckLent"].ToString() == "0")
                    {
                        state = ValidationModel.InvalidState.E301011;
                        getMessage = ValidationModel.GetInvalidMessage(state, lang);
                        return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = getMessage.message, InvalidText = getMessage.topic };
                    }
                    if (dt.Rows[0]["HaveLent"].ToString() != "0")
                    {
                        state = ValidationModel.InvalidState.E301012;
                        getMessage = ValidationModel.GetInvalidMessage(state, lang);
                        return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = getMessage.message, InvalidText = getMessage.topic };
                    }
                }

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

        public static ValidationModel CheckValidationDupicateTransectionBill(string lang, InsertTransectionBillDTO insertTransectionBillDTO)
        {
            ValidationModel value = new ValidationModel();
            try
            {
                GetMessageTopicDTO getMessage = new GetMessageTopicDTO();
                ValidationModel.InvalidState state;

                DataTable dt = _sql.CheckDupicateTransectionBill(insertTransectionBillDTO, 0);

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["BillCode"].ToString() != "0")
                    {
                        state = ValidationModel.InvalidState.E301008;
                        getMessage = ValidationModel.GetInvalidMessage(state, lang);
                        return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = getMessage.message, InvalidText = getMessage.topic };
                    }
                }

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

        public static ValidationModel CheckValidationUpload(string urlPath, string lang)
        {
            ValidationModel value = new ValidationModel();
            try
            {
                GetMessageTopicDTO getMessage = new GetMessageTopicDTO();
                ValidationModel.InvalidState state;

                #region E302001
                state = ValidationModel.InvalidState.E300002; //No urlPath
                if (urlPath == "" || urlPath == null)
                {
                    getMessage = ValidationModel.GetInvalidMessage(state, lang);
                    return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = getMessage.message, InvalidText = getMessage.topic };
                }
                #endregion

                getMessage = ValidationModel.GetInvalidMessage(ValidationModel.InvalidState.S200002, lang);
                value.Success = true;
                value.InvalidCode = ValidationModel.GetInvalidCode(ValidationModel.InvalidState.S200002);
                value.InvalidMessage = getMessage.message;
                value.InvalidText = getMessage.topic;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public static ValidationModel CheckValidationDupicateMasterPlaceSub(string lang, string TableName, MasterPlaceSubDTO masterPlaceSubDTO)
        {
            if (_sql == null)
            {
                _sql = SQLManager.Instance;
            }
            ValidationModel value = new ValidationModel();
            try
            {
                GetMessageTopicDTO getMessage = new GetMessageTopicDTO();
                ValidationModel.InvalidState state;

                DataTable dt = _sql.CheckDuplicatePlaceSub(masterPlaceSubDTO);

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["status_name_en"].ToString() != "0")
                    {
                        state = ValidationModel.InvalidState.E301008;
                        getMessage = ValidationModel.GetInvalidMessage(state, lang);
                        return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = getMessage.message, InvalidText = getMessage.topic };
                    }
                    if (dt.Rows[0]["status_name_th"].ToString() != "0")
                    {
                        state = ValidationModel.InvalidState.E301009;
                        getMessage = ValidationModel.GetInvalidMessage(state, lang);
                        return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = getMessage.message, InvalidText = getMessage.topic };
                    }
                }

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

        public static ValidationModel CheckValidationDupicateZone(string lang, SaveZoneDTO saveZoneDTO)
        {
            ValidationModel value = new ValidationModel();
            try
            {
                GetMessageTopicDTO getMessage = new GetMessageTopicDTO();
                ValidationModel.InvalidState state;

                DataTable dt = _sql.CheckDupicateZone(saveZoneDTO, 0);

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Name"].ToString() != "0")
                    {
                        state = ValidationModel.InvalidState.E301006;
                        getMessage = ValidationModel.GetInvalidMessage(state, lang);
                        return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = getMessage.message, InvalidText = getMessage.topic };
                    }
                }

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

        public static ValidationModel CheckValidationDupicateZoneSub(string lang, SaveZoneSubDTO saveZoneSubDTO)
        {
            ValidationModel value = new ValidationModel();
            try
            {
                GetMessageTopicDTO getMessage = new GetMessageTopicDTO();
                ValidationModel.InvalidState state;

                DataTable dt = _sql.CheckDupicateZoneSub(saveZoneSubDTO, 0);

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Name"].ToString() != "0")
                    {
                        state = ValidationModel.InvalidState.E301006;
                        getMessage = ValidationModel.GetInvalidMessage(state, lang);
                        return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = getMessage.message, InvalidText = getMessage.topic };
                    }
                    if (dt.Rows[0]["HaveZone"].ToString() == "0")
                    {
                        state = ValidationModel.InvalidState.E301013;
                        getMessage = ValidationModel.GetInvalidMessage(state, lang);
                        return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = getMessage.message, InvalidText = getMessage.topic };
                    }
                }

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

        public static ValidationModel CheckValidationDupicateUnit(string lang, SaveUnitDTO saveUnitDTO)
        {
            ValidationModel value = new ValidationModel();
            try
            {
                GetMessageTopicDTO getMessage = new GetMessageTopicDTO();
                ValidationModel.InvalidState state;

                DataTable dt = _sql.CheckDupicateUnit(saveUnitDTO, 0);

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Name"].ToString() != "0")
                    {
                        state = ValidationModel.InvalidState.E301006;
                        getMessage = ValidationModel.GetInvalidMessage(state, lang);
                        return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = getMessage.message, InvalidText = getMessage.topic };
                    }
                    if (dt.Rows[0]["UnitCode"].ToString() != "0")
                    {
                        state = ValidationModel.InvalidState.E301014;
                        getMessage = ValidationModel.GetInvalidMessage(state, lang);
                        return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = getMessage.message, InvalidText = getMessage.topic };
                    }
                    if (dt.Rows[0]["HaveZone"].ToString() == "0")
                    {
                        state = ValidationModel.InvalidState.E301013;
                        getMessage = ValidationModel.GetInvalidMessage(state, lang);
                        return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = getMessage.message, InvalidText = getMessage.topic };
                    }
                    if (dt.Rows[0]["HaveZoneSub"].ToString() == "0")
                    {
                        if (saveUnitDTO.zoneSubID != 0)
                        {
                            state = ValidationModel.InvalidState.E301015;
                            getMessage = ValidationModel.GetInvalidMessage(state, lang);
                            return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = getMessage.message, InvalidText = getMessage.topic };
                        }
                    }
                    if (dt.Rows[0]["HaveRate"].ToString() == "0")
                    {
                        if (saveUnitDTO.rateID != 0)
                        {
                            state = ValidationModel.InvalidState.E301016;
                            getMessage = ValidationModel.GetInvalidMessage(state, lang);
                            return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = getMessage.message, InvalidText = getMessage.topic };
                        }
                    }
                }

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
        
        public static ValidationModel CheckValidationDupicateRateAmount(string lang, SaveRateAmountDTO saveRateAmountDTO)
        {
            ValidationModel value = new ValidationModel();
            try
            {
                GetMessageTopicDTO getMessage = new GetMessageTopicDTO();
                ValidationModel.InvalidState state;

                DataTable dt = _sql.CheckDupicateRateAmount(saveRateAmountDTO, 0);

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Name"].ToString() != "0")
                    {
                        state = ValidationModel.InvalidState.E301006;
                        getMessage = ValidationModel.GetInvalidMessage(state, lang);
                        return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = getMessage.message, InvalidText = getMessage.topic };
                    }
                    if (dt.Rows[0]["Rate"].ToString() != "0")
                    {
                        state = ValidationModel.InvalidState.E301017;
                        getMessage = ValidationModel.GetInvalidMessage(state, lang);
                        return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = getMessage.message, InvalidText = getMessage.topic };
                    }
                }

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
    }
}