using Chaithit_Market.Core;
using Chaithit_Market.DTO;
using Chaithit_Market.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chaithit_Market.Services
{
    public class MasterDataService
    {
        private SQLManager _sql = SQLManager.Instance;

        public ReturnIdModel SaveMasterService(string authorization, string lang, string platform, int logID,
            MasterDataDTO masterDataDTO, string TableName, int userID)
        {
            if (_sql == null)
            {
                _sql = SQLManager.Instance;
            }

            ReturnIdModel value = new ReturnIdModel();
            try
            {
                _ReturnIdModel data = new _ReturnIdModel();

                ValidationModel validation = ValidationManager.CheckValidationDupicateMasterData(lang, TableName, masterDataDTO);
                if (validation.Success == true)
                {
                    if (masterDataDTO.mode.ToLower() == "insert")
                    {
                        value.data = _sql.InsertMasterData(masterDataDTO, TableName, userID);
                    }
                    else if (masterDataDTO.mode.ToLower() == "update")
                    {
                        validation = ValidationManager.CheckValidationIDUpdate(masterDataDTO.masterID, TableName, lang);
                        if (validation.Success == true)
                        {
                            _sql.InsertSystemLogChange(masterDataDTO.masterID, TableName, "name_en", masterDataDTO.nameEN, userID);
                            _sql.InsertSystemLogChange(masterDataDTO.masterID, TableName, "name_th", masterDataDTO.nameTH, userID);
                            value.data = _sql.UpdateMasterData(masterDataDTO, TableName, userID);
                        }
                        else
                        {
                            _sql.UpdateLogReceiveDataError(logID, validation.InvalidMessage);
                        }
                    }
                    else if (masterDataDTO.mode.ToLower() == "delete")
                    {
                        validation = ValidationManager.CheckValidationIDUpdate(masterDataDTO.masterID, TableName, lang);
                        if (validation.Success == true)
                        {
                            value.data = _sql.DeleteMasterData(masterDataDTO, TableName, userID);
                        }
                        else
                        {
                            _sql.UpdateLogReceiveDataError(logID, validation.InvalidMessage);
                        }
                    }
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
                LogManager.ServiceLog.WriteExceptionLog(ex, "SaveMasterService:");
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

        public GetMasterDataModel GetMasterService(string authorization, string lang, string platform, int logID, int masterID, string TableName)
        {
            if (_sql == null)
            {
                _sql = SQLManager.Instance;
            }

            GetMasterDataModel value = new GetMasterDataModel();
            try
            {
                MasterData data = new MasterData();

                ValidationModel validation = ValidationManager.CheckValidation(1, lang, platform);

                if (validation.Success == true)
                {
                    data = _sql.GetMasterData(masterID, TableName);
                    value.data = data;
                    value.success = validation.Success;
                }
                else
                {
                    _sql.UpdateLogReceiveDataError(logID, validation.InvalidMessage);
                }

                value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, topic = validation.InvalidText };
            }
            catch (Exception ex)
            {
                LogManager.ServiceLog.WriteExceptionLog(ex, "GetMasterService:");
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

        public ReturnIdModel SaveMasterPlaceSubService(string authorization, string lang, string platform, int logID,
            MasterPlaceSubDTO masterPlaceSubDTO, string TableName, int userID)
        {
            if (_sql == null)
            {
                _sql = SQLManager.Instance;
            }

            ReturnIdModel value = new ReturnIdModel();
            try
            {
                _ReturnIdModel data = new _ReturnIdModel();

                //ValidationModel validation = ValidationManager.CheckValidationDupicateMasterData(lang, TableName, masterPlaceSubDTO);
                //if (validation.Success == true)
                //{
                //    if (masterPlaceSubDTO.mode.ToLower() == "insert")
                //    {
                //        value.data = _sql.InsertMasterPlaceSubData(masterPlaceSubDTO, userID);
                //    }
                //    else if (masterPlaceSubDTO.mode.ToLower() == "update")
                //    {
                //        validation = ValidationManager.CheckValidationIDUpdate(masterPlaceSubDTO.masterID, TableName, lang);
                //        if (validation.Success == true)
                //        {
                //            _sql.InsertSystemLogChange(masterPlaceSubDTO.masterID, TableName, "name_en", masterPlaceSubDTO.nameEN, userID);
                //            _sql.InsertSystemLogChange(masterPlaceSubDTO.masterID, TableName, "name_th", masterPlaceSubDTO.nameTH, userID);
                //            value.data = _sql.UpdateMasterData(masterPlaceSubDTO, TableName, userID);
                //        }
                //        else
                //        {
                //            _sql.UpdateLogReceiveDataError(logID, validation.InvalidMessage);
                //        }
                //    }
                //    else if (masterPlaceSubDTO.mode.ToLower() == "delete")
                //    {
                //        validation = ValidationManager.CheckValidationIDUpdate(masterPlaceSubDTO.masterID, TableName, lang);
                //        if (validation.Success == true)
                //        {
                //            value.data = _sql.DeleteMasterData(masterPlaceSubDTO, TableName, userID);
                //        }
                //        else
                //        {
                //            _sql.UpdateLogReceiveDataError(logID, validation.InvalidMessage);
                //        }
                //    }
                //}
                //else
                //{
                //    _sql.UpdateLogReceiveDataError(logID, validation.InvalidMessage);
                //}

                ValidationModel validation = ValidationManager.CheckValidation(1,lang, platform);

                value.success = validation.Success;
                value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, topic = validation.InvalidText };
            }
            catch (Exception ex)
            {
                LogManager.ServiceLog.WriteExceptionLog(ex, "SaveMasterPlaceSubService:");
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