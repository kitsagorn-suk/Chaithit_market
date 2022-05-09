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
                            _sql.InsertSystemLogChange(saveUserProfileDTO.userProfileID, TableName, "password", saveUserProfileDTO.endDate, userID);
                            _sql.InsertSystemLogChange(saveUserProfileDTO.userProfileID, TableName, "firstname", saveUserProfileDTO.firstName, userID);
                            _sql.InsertSystemLogChange(saveUserProfileDTO.userProfileID, TableName, "lastname", saveUserProfileDTO.lastName, userID);
                            _sql.InsertSystemLogChange(saveUserProfileDTO.userProfileID, TableName, "mobile", saveUserProfileDTO.mobile, userID);
                            _sql.InsertSystemLogChange(saveUserProfileDTO.userProfileID, TableName, "position", saveUserProfileDTO.position, userID);
                            _sql.InsertSystemLogChange(saveUserProfileDTO.userProfileID, TableName, "start_date", saveUserProfileDTO.startDate, userID);
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
                string TableName = "transaction_rent";
                ValidationModel validation = new ValidationModel();
                
                if (insertTransectionRentDTO.mode.ToLower() == "insert")
                {
                    validation = ValidationManager.CheckValidationDupicateTransectionRent(lang, insertTransectionRentDTO);
                    if (validation.Success == true)
                    {
                        value.data = _sql.InsertTransectionRent(insertTransectionRentDTO, userID);
                    }
                    else
                    {
                        _sql.UpdateLogReceiveDataError(logID, validation.InvalidMessage);
                    }
                }
                else if (insertTransectionRentDTO.mode.ToLower() == "update")
                {
                    validation = ValidationManager.CheckValidationDupicateTransectionRent(lang, insertTransectionRentDTO);
                    if (validation.Success == true)
                    {
                        validation = ValidationManager.CheckValidationIDUpdate(insertTransectionRentDTO.tranRentID, TableName, lang);
                        if (validation.Success == true)
                        {
                            _sql.InsertSystemLogChange(insertTransectionRentDTO.tranRentID, TableName, "trans_code", insertTransectionRentDTO.transCode, userID);
                            _sql.InsertSystemLogChange(insertTransectionRentDTO.tranRentID, TableName, "user_id", insertTransectionRentDTO.userID.ToString(), userID);
                            _sql.InsertSystemLogChange(insertTransectionRentDTO.tranRentID, TableName, "unit_id", insertTransectionRentDTO.unitID.ToString(), userID);
                            _sql.InsertSystemLogChange(insertTransectionRentDTO.tranRentID, TableName, "start_date", insertTransectionRentDTO.startDate, userID);
                            _sql.InsertSystemLogChange(insertTransectionRentDTO.tranRentID, TableName, "end_date", insertTransectionRentDTO.endDate, userID);
                            _sql.InsertSystemLogChange(insertTransectionRentDTO.tranRentID, TableName, "rent_type", insertTransectionRentDTO.rentType.ToString(), userID);
                            _sql.InsertSystemLogChange(insertTransectionRentDTO.tranRentID, TableName, "pay_date", insertTransectionRentDTO.payDate.ToString(), userID);
                            value.data = _sql.UpdateTransectionRent(insertTransectionRentDTO, userID);
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
                string TableName = "transaction_bill";
                ValidationModel validation = new ValidationModel();


                if (insertTransectionBillDTO.mode.ToLower() == "insert")
                {
                    validation = ValidationManager.CheckValidationDupicateTransectionBill(lang, insertTransectionBillDTO);
                    if (validation.Success == true)
                    {
                        value.data = _sql.InsertTransectionBill(insertTransectionBillDTO, userID);
                    }
                    else
                    {
                        _sql.UpdateLogReceiveDataError(logID, validation.InvalidMessage);
                    }
                }
                else if (insertTransectionBillDTO.mode.ToLower() == "update")
                {
                    validation = ValidationManager.CheckValidationDupicateTransectionBill(lang, insertTransectionBillDTO);
                    if (validation.Success == true)
                    {
                        validation = ValidationManager.CheckValidationIDUpdate(insertTransectionBillDTO.tranBillID, TableName, lang);
                        if (validation.Success == true)
                        {
                            _sql.InsertSystemLogChange(insertTransectionBillDTO.tranBillID, TableName, "tran_rent_id", insertTransectionBillDTO.tranRentID.ToString(), userID);
                            _sql.InsertSystemLogChange(insertTransectionBillDTO.tranBillID, TableName, "bill_code", insertTransectionBillDTO.billCode.ToString(), userID);
                            _sql.InsertSystemLogChange(insertTransectionBillDTO.tranBillID, TableName, "start_date", insertTransectionBillDTO.startDate, userID);
                            _sql.InsertSystemLogChange(insertTransectionBillDTO.tranBillID, TableName, "end_date", insertTransectionBillDTO.endDate, userID);
                            _sql.InsertSystemLogChange(insertTransectionBillDTO.tranBillID, TableName, "rent_amount", insertTransectionBillDTO.rentAmount.ToString(), userID);
                            _sql.InsertSystemLogChange(insertTransectionBillDTO.tranBillID, TableName, "electric_unit", insertTransectionBillDTO.electricUnit.ToString(), userID);
                            _sql.InsertSystemLogChange(insertTransectionBillDTO.tranBillID, TableName, "electric_amount", insertTransectionBillDTO.electricAmount.ToString(), userID);
                            _sql.InsertSystemLogChange(insertTransectionBillDTO.tranBillID, TableName, "water_unit", insertTransectionBillDTO.waterUnit.ToString(), userID);
                            _sql.InsertSystemLogChange(insertTransectionBillDTO.tranBillID, TableName, "water_amount", insertTransectionBillDTO.waterAmount.ToString(), userID);
                            _sql.InsertSystemLogChange(insertTransectionBillDTO.tranBillID, TableName, "lamp_unit", insertTransectionBillDTO.lampUnit.ToString(), userID);
                            _sql.InsertSystemLogChange(insertTransectionBillDTO.tranBillID, TableName, "electric_equip_unit", insertTransectionBillDTO.electricEquipUnit.ToString(), userID);
                            _sql.InsertSystemLogChange(insertTransectionBillDTO.tranBillID, TableName, "electric_night_market_amount", insertTransectionBillDTO.electricNightMarketAmount.ToString(), userID);
                            _sql.InsertSystemLogChange(insertTransectionBillDTO.tranBillID, TableName, "total_amount", insertTransectionBillDTO.totalAmount.ToString(), userID);
                            _sql.InsertSystemLogChange(insertTransectionBillDTO.tranBillID, TableName, "discount_percent", insertTransectionBillDTO.discountPercent.ToString(), userID);
                            _sql.InsertSystemLogChange(insertTransectionBillDTO.tranBillID, TableName, "discount_amount", insertTransectionBillDTO.discountAmount.ToString(), userID);
                            _sql.InsertSystemLogChange(insertTransectionBillDTO.tranBillID, TableName, "net_amount", insertTransectionBillDTO.netAmount.ToString(), userID);
                            value.data = _sql.UpdateTransectionBill(insertTransectionBillDTO, userID);
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

        public ReturnIdModel SaveTranPayService(string authorization, string lang, string platform, int logID,
            SaveTranPayDTO saveTranPayDTO, int userID)
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
                
                int billID = 0;
                //int.TryParse(p, out billID);
                validation = ValidationManager.CheckValidationDupicateTranPay(lang, billID);
                if (validation.Success == true)
                {
                    value.data = _sql.InsertTranPay(saveTranPayDTO, userID);
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
                LogManager.ServiceLog.WriteExceptionLog(ex, "SaveTranPayService:");
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