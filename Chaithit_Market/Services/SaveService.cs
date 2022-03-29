using Chaithit_Market.Core;
using Chaithit_Market.DTO;
using Chaithit_Market.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chaithit_Market.Services
{
    public class SaveService
    {
        private SQLManager _sql = SQLManager.Instance;

        public ReturnIdModel SaveUserProfileService(string authorization, string lang, string platform, int logID,
            SaveUserProfileDTO saveUserProfileDTO, int userID)
        {
            if (_sql == null)
            {
                _sql = SQLManager.Instance;
            }

            ReturnIdModel value = new ReturnIdModel();
            try
            {
                value.data = new _ReturnIdModel();
                string TableName = "user_profile";
                ValidationModel validation = new ValidationModel();
                
                if (saveUserProfileDTO.mode.ToLower() == "insert")
                {
                    validation = ValidationManager.CheckValidationDupicateUser(lang, saveUserProfileDTO);
                    if (validation.Success == true)
                    {
                        value.data = _sql.InsertUserProfile(saveUserProfileDTO, userID);
                    }
                    else
                    {
                        _sql.UpdateLogReceiveDataError(logID, validation.InvalidMessage);
                    }
                }
                else if (saveUserProfileDTO.mode.ToLower() == "update")
                {
                    validation = ValidationManager.CheckValidationDupicateUser(lang, saveUserProfileDTO);
                    if (validation.Success == true)
                    {
                        validation = ValidationManager.CheckValidationIDUpdate(saveUserProfileDTO.userProfileID, TableName, lang);
                        if (validation.Success == true)
                        {
                            _sql.InsertSystemLogChange(saveUserProfileDTO.userProfileID, TableName, "firstname", saveUserProfileDTO.firstName, userID);
                            _sql.InsertSystemLogChange(saveUserProfileDTO.userProfileID, TableName, "lastname", saveUserProfileDTO.lastName, userID);
                            _sql.InsertSystemLogChange(saveUserProfileDTO.userProfileID, TableName, "mobile", saveUserProfileDTO.mobile, userID);
                            _sql.InsertSystemLogChange(saveUserProfileDTO.userProfileID, TableName, "position", saveUserProfileDTO.position, userID);
                            _sql.InsertSystemLogChange(saveUserProfileDTO.userProfileID, TableName, "start_date", saveUserProfileDTO.startDate, userID);
                            _sql.InsertSystemLogChange(saveUserProfileDTO.userProfileID, TableName, "end_date", saveUserProfileDTO.endDate, userID);
                            _sql.InsertSystemLogChange(saveUserProfileDTO.userProfileID, TableName, "status_emp", saveUserProfileDTO.statusEmp.ToString(), userID);
                            _sql.InsertSystemLogChange(saveUserProfileDTO.userProfileID, TableName, "emp_type", saveUserProfileDTO.empType.ToString(), userID);
                            value.data = _sql.UpdateUserProfile(saveUserProfileDTO, userID);
                        }
                        else
                        {
                            _sql.UpdateLogReceiveDataError(logID, validation.InvalidMessage);
                        }
                    }
                    else
                    {
                        _sql.UpdateLogReceiveDataError(logID, validation.InvalidMessage);
                    }
                }
                else if (saveUserProfileDTO.mode.ToLower() == "delete")
                {
                    validation = ValidationManager.CheckValidationIDUpdate(saveUserProfileDTO.userProfileID, TableName, lang);
                    if (validation.Success == true)
                    {
                        value.data = _sql.DeleteUserProfile(saveUserProfileDTO, userID);
                    }
                    else
                    {
                        _sql.UpdateLogReceiveDataError(logID, validation.InvalidMessage);
                    }
                }

                value.success = validation.Success;
                value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, topic = validation.InvalidText };
            }
            catch (Exception ex)
            {
                LogManager.ServiceLog.WriteExceptionLog(ex, "SaveUserProfileService:");
                if (logID > 0)
                {
                    _sql.UpdateLogReceiveDataError(logID, ex.ToString());
                }
                throw ex;
            }
            finally
            {
                _sql.UpdateStatusLog(logID, 1);
            }
            return value;
        }

        public ReturnIdModel SaveRentalService(string authorization, string lang, string platform, int logID,
                    SaveRentalDTO saveRentalDTO, int userID)
        {
            if (_sql == null)
            {
                _sql = SQLManager.Instance;
            }

            ReturnIdModel value = new ReturnIdModel();
            try
            {
                value.data = new _ReturnIdModel();
                string TableName = "rental";
                ValidationModel validation = new ValidationModel();

                if (saveRentalDTO.mode.ToLower() == "insert")
                {
                    validation = ValidationManager.CheckValidationDupicateRental(lang, saveRentalDTO);
                    if (validation.Success == true)
                    {
                        value.data = _sql.InsertRental(saveRentalDTO, userID);
                    }
                    else
                    {
                        _sql.UpdateLogReceiveDataError(logID, validation.InvalidMessage);
                    }
                }
                else if (saveRentalDTO.mode.ToLower() == "update")
                {
                    validation = ValidationManager.CheckValidationDupicateRental(lang, saveRentalDTO);
                    if (validation.Success == true)
                    {
                        validation = ValidationManager.CheckValidationIDUpdate(saveRentalDTO.rentalID, TableName, lang);
                        if (validation.Success == true)
                        {
                            _sql.InsertSystemLogChange(saveRentalDTO.rentalID, TableName, "rent_code", saveRentalDTO.rentCode, userID);
                            _sql.InsertSystemLogChange(saveRentalDTO.rentalID, TableName, "name", saveRentalDTO.name, userID);
                            _sql.InsertSystemLogChange(saveRentalDTO.rentalID, TableName, "place_sub_id", saveRentalDTO.placeSubID.ToString(), userID);
                            _sql.InsertSystemLogChange(saveRentalDTO.rentalID, TableName, "is_used", saveRentalDTO.isUsed.ToString(), userID);
                            value.data = _sql.UpdateRental(saveRentalDTO, userID);
                        }
                        else
                        {
                            _sql.UpdateLogReceiveDataError(logID, validation.InvalidMessage);
                        }
                    }
                    else
                    {
                        _sql.UpdateLogReceiveDataError(logID, validation.InvalidMessage);
                    }
                }
                else if (saveRentalDTO.mode.ToLower() == "delete")
                {
                    validation = ValidationManager.CheckValidationIDUpdate(saveRentalDTO.rentalID, TableName, lang);
                    if (validation.Success == true)
                    {
                        value.data = _sql.DeleteRental(saveRentalDTO, userID);
                    }
                    else
                    {
                        _sql.UpdateLogReceiveDataError(logID, validation.InvalidMessage);
                    }
                }

                value.success = validation.Success;
                value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, topic = validation.InvalidText };
            }
            catch (Exception ex)
            {
                LogManager.ServiceLog.WriteExceptionLog(ex, "SaveRentalService:");
                if (logID > 0)
                {
                    _sql.UpdateLogReceiveDataError(logID, ex.ToString());
                }
                throw ex;
            }
            finally
            {
                _sql.UpdateStatusLog(logID, 1);
            }
            return value;
        }

        public ReturnIdModel InsertTransectionRentService(string authorization, string lang, string platform, int logID,
                   InsertTransectionRentDTO insertTransectionRentDTO, int userID)
        {
            if (_sql == null)
            {
                _sql = SQLManager.Instance;
            }

            ReturnIdModel value = new ReturnIdModel();
            try
            {
                value.data = new _ReturnIdModel();
                ValidationModel validation = new ValidationModel();
                
                validation = ValidationManager.CheckValidationDupicateTransectionRent(lang, insertTransectionRentDTO);
                if (validation.Success == true)
                {
                    value.data = _sql.InsertTransectionRent(insertTransectionRentDTO, userID);
                }
                else
                {
                    _sql.UpdateLogReceiveDataError(logID, validation.InvalidMessage);
                }

                value.success = validation.Success;
                value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, topic = validation.InvalidText };
            }
            catch (Exception ex)
            {
                LogManager.ServiceLog.WriteExceptionLog(ex, "InsertTransectionRentService:");
                if (logID > 0)
                {
                    _sql.UpdateLogReceiveDataError(logID, ex.ToString());
                }
                throw ex;
            }
            finally
            {
                _sql.UpdateStatusLog(logID, 1);
            }
            return value;
        }

        public ReturnIdModel InsertTransectionBillService(string authorization, string lang, string platform, int logID,
                   InsertTransectionBillDTO insertTransectionBillDTO, int userID)
        {
            if (_sql == null)

            {
                _sql = SQLManager.Instance;
            }

            ReturnIdModel value = new ReturnIdModel();
            try
            {
                value.data = new _ReturnIdModel();
                ValidationModel validation = new ValidationModel();

                validation = ValidationManager.CheckValidationDupicateTransectionBill(lang, insertTransectionBillDTO);
                if (validation.Success == true)
                {
                    value.data = _sql.InsertTransectionBill(insertTransectionBillDTO, userID);
                }
                else
                {
                    _sql.UpdateLogReceiveDataError(logID, validation.InvalidMessage);
                }

                value.success = validation.Success;
                value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, topic = validation.InvalidText };
            }
            catch (Exception ex)
            {
                LogManager.ServiceLog.WriteExceptionLog(ex, "InsertTransectionBillService:");
                if (logID > 0)
                {
                    _sql.UpdateLogReceiveDataError(logID, ex.ToString());
                }
                throw ex;
            }
            finally
            {
                _sql.UpdateStatusLog(logID, 1);
            }
            return value;
        }
    }
}