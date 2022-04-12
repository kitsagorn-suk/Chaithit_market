﻿using Chaithit_Market.Core;
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
                            _sql.InsertSystemLogChange(masterDataDTO.masterID, TableName, "name", masterDataDTO.name, userID);
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

        public GetAllDropdownModel GetDropdownMasterService(string authorization, string lang, string platform, int logID, int masterID, string TableName, string isAll)
        {
            if (_sql == null)
            {
                _sql = SQLManager.Instance;
            }

            GetAllDropdownModel value = new GetAllDropdownModel();
            try
            {
                value.data = new List<DropdownAllData>();

                ValidationModel validation = ValidationManager.CheckValidation(1, lang, platform);

                if (validation.Success == true)
                {
                    value.data = _sql.GetDropdownMasterData(masterID, TableName, isAll);
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
                LogManager.ServiceLog.WriteExceptionLog(ex, "GetDropdownMasterService:");
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
        
        public ReturnIdModel SaveZoneService(string authorization, string lang, string platform, int logID,
                   SaveZoneDTO saveZoneDTO, int userID)
        {
            if (_sql == null)
            {
                _sql = SQLManager.Instance;
            }

            ReturnIdModel value = new ReturnIdModel();
            try
            {
                value.data = new _ReturnIdModel();
                string TableName = "system_zone";
                ValidationModel validation = new ValidationModel();

                if (saveZoneDTO.mode.ToLower() == "insert")
                {
                    validation = ValidationManager.CheckValidationDupicateZone(lang, saveZoneDTO);
                    if (validation.Success == true)
                    {
                        value.data = _sql.InsertZone(saveZoneDTO, userID);
                    }
                    else
                    {
                        _sql.UpdateLogReceiveDataError(logID, validation.InvalidMessage);
                    }
                }
                else if (saveZoneDTO.mode.ToLower() == "update")
                {
                    validation = ValidationManager.CheckValidationDupicateZone(lang, saveZoneDTO);
                    if (validation.Success == true)
                    {
                        validation = ValidationManager.CheckValidationIDUpdate(saveZoneDTO.zoneID, TableName, lang);
                        if (validation.Success == true)
                        {
                            _sql.InsertSystemLogChange(saveZoneDTO.zoneID, TableName, "name", saveZoneDTO.name, userID);
                            value.data = _sql.UpdateZone(saveZoneDTO, userID);
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
                else if (saveZoneDTO.mode.ToLower() == "delete")
                {
                    validation = ValidationManager.CheckValidationIDUpdate(saveZoneDTO.zoneID, TableName, lang);
                    if (validation.Success == true)
                    {
                        value.data = _sql.DeleteZone(saveZoneDTO, userID);
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
                LogManager.ServiceLog.WriteExceptionLog(ex, "SaveZoneService:");
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

        public ReturnIdModel SaveZoneSubService(string authorization, string lang, string platform, int logID,
                   SaveZoneSubDTO saveZoneSubDTO, int userID)
        {
            if (_sql == null)
            {
                _sql = SQLManager.Instance;
            }

            ReturnIdModel value = new ReturnIdModel();
            try
            {
                value.data = new _ReturnIdModel();
                string TableName = "system_zone_sub";
                ValidationModel validation = new ValidationModel();

                if (saveZoneSubDTO.mode.ToLower() == "insert")
                {
                    validation = ValidationManager.CheckValidationDupicateZoneSub(lang, saveZoneSubDTO);
                    if (validation.Success == true)
                    {
                        value.data = _sql.InsertZoneSub(saveZoneSubDTO, userID);
                    }
                    else
                    {
                        _sql.UpdateLogReceiveDataError(logID, validation.InvalidMessage);
                    }
                }
                else if (saveZoneSubDTO.mode.ToLower() == "update")
                {
                    validation = ValidationManager.CheckValidationDupicateZoneSub(lang, saveZoneSubDTO);
                    if (validation.Success == true)
                    {
                        validation = ValidationManager.CheckValidationIDUpdate(saveZoneSubDTO.zoneSubID, TableName, lang);
                        if (validation.Success == true)
                        {
                            _sql.InsertSystemLogChange(saveZoneSubDTO.zoneSubID, TableName, "zone_id", saveZoneSubDTO.zoneID.ToString(), userID);
                            _sql.InsertSystemLogChange(saveZoneSubDTO.zoneSubID, TableName, "name", saveZoneSubDTO.name, userID);
                            value.data = _sql.UpdateZoneSub(saveZoneSubDTO, userID);
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
                else if (saveZoneSubDTO.mode.ToLower() == "delete")
                {
                    validation = ValidationManager.CheckValidationIDUpdate(saveZoneSubDTO.zoneSubID, TableName, lang);
                    if (validation.Success == true)
                    {
                        value.data = _sql.DeleteZoneSub(saveZoneSubDTO, userID);
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
                LogManager.ServiceLog.WriteExceptionLog(ex, "SaveZoneSubService:");
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

        public ReturnIdModel SaveUnitService(string authorization, string lang, string platform, int logID,
                   SaveUnitDTO saveUnitDTO, int userID)
        {
            if (_sql == null)
            {
                _sql = SQLManager.Instance;
            }

            ReturnIdModel value = new ReturnIdModel();
            try
            {
                value.data = new _ReturnIdModel();
                string TableName = "system_unit";
                ValidationModel validation = new ValidationModel();

                if (saveUnitDTO.mode.ToLower() == "insert")
                {
                    validation = ValidationManager.CheckValidationDupicateUnit(lang, saveUnitDTO);
                    if (validation.Success == true)
                    {
                        value.data = _sql.InsertUnit(saveUnitDTO, userID);
                    }
                    else
                    {
                        _sql.UpdateLogReceiveDataError(logID, validation.InvalidMessage);
                    }
                }
                else if (saveUnitDTO.mode.ToLower() == "update")
                {
                    validation = ValidationManager.CheckValidationDupicateUnit(lang, saveUnitDTO);
                    if (validation.Success == true)
                    {
                        validation = ValidationManager.CheckValidationIDUpdate(saveUnitDTO.unitID, TableName, lang);
                        if (validation.Success == true)
                        {
                            _sql.InsertSystemLogChange(saveUnitDTO.unitID, TableName, "zone_id", saveUnitDTO.zoneID.ToString(), userID);
                            _sql.InsertSystemLogChange(saveUnitDTO.unitID, TableName, "zone_sub_id", saveUnitDTO.zoneSubID.ToString(), userID);
                            _sql.InsertSystemLogChange(saveUnitDTO.unitID, TableName, "unit_code", saveUnitDTO.unitCode, userID);
                            _sql.InsertSystemLogChange(saveUnitDTO.unitID, TableName, "name", saveUnitDTO.name, userID);
                            _sql.InsertSystemLogChange(saveUnitDTO.unitID, TableName, "rate_id", saveUnitDTO.rateID.ToString(), userID);
                            value.data = _sql.UpdateUnit(saveUnitDTO, userID);
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
                else if (saveUnitDTO.mode.ToLower() == "delete")
                {
                    validation = ValidationManager.CheckValidationIDUpdate(saveUnitDTO.unitID, TableName, lang);
                    if (validation.Success == true)
                    {
                        value.data = _sql.DeleteUnit(saveUnitDTO, userID);
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
                LogManager.ServiceLog.WriteExceptionLog(ex, "SaveUnitService:");
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

        public ReturnIdModel SaveRateAmountService(string authorization, string lang, string platform, int logID,
                SaveRateAmountDTO saveRateAmountDTO, int userID)
        {
            if (_sql == null)
            {
                _sql = SQLManager.Instance;
            }

            ReturnIdModel value = new ReturnIdModel();
            try
            {
                value.data = new _ReturnIdModel();
                string TableName = "system_rate_amount";
                ValidationModel validation = new ValidationModel();

                if (saveRateAmountDTO.mode.ToLower() == "insert")
                {
                    validation = ValidationManager.CheckValidationDupicateRateAmount(lang, saveRateAmountDTO);
                    if (validation.Success == true)
                    {
                        value.data = _sql.InsertRateAmount(saveRateAmountDTO, userID);
                    }
                    else
                    {
                        _sql.UpdateLogReceiveDataError(logID, validation.InvalidMessage);
                    }
                }
                else if (saveRateAmountDTO.mode.ToLower() == "update")
                {
                    validation = ValidationManager.CheckValidationDupicateRateAmount(lang, saveRateAmountDTO);
                    if (validation.Success == true)
                    {
                        validation = ValidationManager.CheckValidationIDUpdate(saveRateAmountDTO.rateID, TableName, lang);
                        if (validation.Success == true)
                        {
                            _sql.InsertSystemLogChange(saveRateAmountDTO.rateID, TableName, "name", saveRateAmountDTO.name, userID);
                            _sql.InsertSystemLogChange(saveRateAmountDTO.rateID, TableName, "rent_amount_day", saveRateAmountDTO.rentAmountDay.ToString(), userID);
                            _sql.InsertSystemLogChange(saveRateAmountDTO.rateID, TableName, "rent_amount_month", saveRateAmountDTO.rentAmountMonth.ToString(), userID);
                            _sql.InsertSystemLogChange(saveRateAmountDTO.rateID, TableName, "electric_amount", saveRateAmountDTO.electricAmount.ToString(), userID);
                            _sql.InsertSystemLogChange(saveRateAmountDTO.rateID, TableName, "water_amount", saveRateAmountDTO.waterAmount.ToString(), userID);
                            _sql.InsertSystemLogChange(saveRateAmountDTO.rateID, TableName, "lamp_amount_per_one", saveRateAmountDTO.lampAmountPerOne.ToString(), userID);
                            _sql.InsertSystemLogChange(saveRateAmountDTO.rateID, TableName, "electric_equip_amount", saveRateAmountDTO.electricEquipAmount.ToString(), userID);
                            value.data = _sql.UpdateRateAmount(saveRateAmountDTO, userID);
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
                else if (saveRateAmountDTO.mode.ToLower() == "delete")
                {
                    validation = ValidationManager.CheckValidationIDUpdate(saveRateAmountDTO.rateID, TableName, lang);
                    if (validation.Success == true)
                    {
                        value.data = _sql.DeleteRateAmount(saveRateAmountDTO, userID);
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
                LogManager.ServiceLog.WriteExceptionLog(ex, "SaveRateAmountService:");
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

        public GetAllDropdownModel GetDropdownZoneService(string authorization, string lang, string platform, int logID, string isAll)
        {
            if (_sql == null)
            {
                _sql = SQLManager.Instance;
            }

            GetAllDropdownModel value = new GetAllDropdownModel();
            try
            {
                value.data = new List<DropdownAllData>();

                ValidationModel validation = ValidationManager.CheckValidation(1, lang, platform);

                if (validation.Success == true)
                {
                    value.data = _sql.GetDropdownZone(isAll);
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
                LogManager.ServiceLog.WriteExceptionLog(ex, "GetDropdownZoneService:");
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

        public GetAllDropdownModel GetDropdownZoneSubService(string authorization, string lang, string platform, int logID, int ZoneID, string isAll)
        {
            if (_sql == null)
            {
                _sql = SQLManager.Instance;
            }

            GetAllDropdownModel value = new GetAllDropdownModel();
            try
            {
                value.data = new List<DropdownAllData>();

                ValidationModel validation = ValidationManager.CheckValidation(1, lang, platform);

                if (validation.Success == true)
                {
                    value.data = _sql.GetDropdownZoneSub(ZoneID, isAll);
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
                LogManager.ServiceLog.WriteExceptionLog(ex, "GetDropdownZoneSubService:");
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

        public GetAllDropdownModel GetDropdownUnitService(string authorization, string lang, string platform, int logID, int ZoneID, int ZoneSubID, string isAll)
        {
            if (_sql == null)
            {
                _sql = SQLManager.Instance;
            }

            GetAllDropdownModel value = new GetAllDropdownModel();
            try
            {
                value.data = new List<DropdownAllData>();

                ValidationModel validation = ValidationManager.CheckValidation(1, lang, platform);

                if (validation.Success == true)
                {
                    value.data = _sql.GetDropdownUnit(ZoneID, ZoneSubID, isAll);
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
                LogManager.ServiceLog.WriteExceptionLog(ex, "GetDropdownZoneSubService:");
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

        public GetAllDropdownModel GetDropdownRateAmountService(string authorization, string lang, string platform, int logID, string isAll)
        {
            if (_sql == null)
            {
                _sql = SQLManager.Instance;
            }

            GetAllDropdownModel value = new GetAllDropdownModel();
            try
            {
                value.data = new List<DropdownAllData>();

                ValidationModel validation = ValidationManager.CheckValidation(1, lang, platform);

                if (validation.Success == true)
                {
                    value.data = _sql.GetDropdownRateAmount(isAll);
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
                LogManager.ServiceLog.WriteExceptionLog(ex, "GetDropdownRateAmountService:");
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
        
        public GetAllDropdownModel GetDropdownUnitStatusService(string authorization, string lang, string platform, int logID)
        {
            if (_sql == null)
            {
                _sql = SQLManager.Instance;
            }

            GetAllDropdownModel value = new GetAllDropdownModel();
            try
            {
                value.data = new List<DropdownAllData>();

                ValidationModel validation = ValidationManager.CheckValidation(1, lang, platform);

                if (validation.Success == true)
                {
                    value.data = _sql.GetDropdownUnitStatus();
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
                LogManager.ServiceLog.WriteExceptionLog(ex, "GetDropdownUnitStatusService:");
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

        public GetAllDropdownModel GetDropdownRentTypeService(string authorization, string lang, string platform, int logID)
        {
            if (_sql == null)
            {
                _sql = SQLManager.Instance;
            }

            GetAllDropdownModel value = new GetAllDropdownModel();
            try
            {
                value.data = new List<DropdownAllData>();

                ValidationModel validation = ValidationManager.CheckValidation(1, lang, platform);

                if (validation.Success == true)
                {
                    value.data = _sql.GetDropdownRentType();
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
                LogManager.ServiceLog.WriteExceptionLog(ex, "GetDropdownRentTypeService:");
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

        public SearchMasterDataModel SearchMasterDataService(string authorization, string lang, string platform, int logID, SearchNameCenterDTO searchNameCenterDTO, string TableName)
        {
            if (_sql == null)
            {
                _sql = SQLManager.Instance;
            }

            SearchMasterDataModel value = new SearchMasterDataModel();
            try
            {
                Pagination<SearchMasterData> data = new Pagination<SearchMasterData>();

                ValidationModel validation = ValidationManager.CheckValidation(1, lang, platform);

                if (validation.Success == true)
                {
                    data = _sql.SearchMasterData(searchNameCenterDTO, TableName);
                }
                else
                {
                    _sql.UpdateLogReceiveDataError(logID, validation.InvalidMessage);
                }

                value.success = validation.Success;
                value.data = data;
                value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, topic = validation.InvalidText };
            }
            catch (Exception ex)
            {
                LogManager.ServiceLog.WriteExceptionLog(ex, "SearchZoneService:");
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

        public SearchMasterZoneModel SearchZoneService(string authorization, string lang, string platform, int logID, SearchNameCenterDTO searchNameCenterDTO)
        {
            if (_sql == null)
            {
                _sql = SQLManager.Instance;
            }

            SearchMasterZoneModel value = new SearchMasterZoneModel();
            try
            {
                Pagination<SearchMasterZone> data = new Pagination<SearchMasterZone>();

                ValidationModel validation = ValidationManager.CheckValidation(1, lang, platform);

                if (validation.Success == true)
                {
                    data = _sql.SearchZone(searchNameCenterDTO);
                }
                else
                {
                    _sql.UpdateLogReceiveDataError(logID, validation.InvalidMessage);
                }

                value.success = validation.Success;
                value.data = data;
                value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, topic = validation.InvalidText };
            }
            catch (Exception ex)
            {
                LogManager.ServiceLog.WriteExceptionLog(ex, "SearchZoneService:");
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

        public SearchMasterZoneSubModel SearchZoneSubService(string authorization, string lang, string platform, int logID, SearchZoneSubDTO searchZoneSubDTO)
        {
            if (_sql == null)
            {
                _sql = SQLManager.Instance;
            }

            SearchMasterZoneSubModel value = new SearchMasterZoneSubModel();
            try
            {
                Pagination<SearchMasterZoneSub> data = new Pagination<SearchMasterZoneSub>();

                ValidationModel validation = ValidationManager.CheckValidation(1, lang, platform);

                if (validation.Success == true)
                {
                    data = _sql.SearchZoneSub(searchZoneSubDTO);
                }
                else
                {
                    _sql.UpdateLogReceiveDataError(logID, validation.InvalidMessage);
                }

                value.success = validation.Success;
                value.data = data;
                value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, topic = validation.InvalidText };
            }
            catch (Exception ex)
            {
                LogManager.ServiceLog.WriteExceptionLog(ex, "SearchZoneSubService:");
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

        public SearchMasterUnitModel SearchUnitService(string authorization, string lang, string platform, int logID, SearchUnitDTO searchUnitDTO)
        {
            if (_sql == null)
            {
                _sql = SQLManager.Instance;
            }

            SearchMasterUnitModel value = new SearchMasterUnitModel();
            try
            {
                Pagination<SearchMasterUnit> data = new Pagination<SearchMasterUnit>();

                ValidationModel validation = ValidationManager.CheckValidation(1, lang, platform);

                if (validation.Success == true)
                {
                    data = _sql.SearchUnit(searchUnitDTO);
                }
                else
                {
                    _sql.UpdateLogReceiveDataError(logID, validation.InvalidMessage);
                }

                value.success = validation.Success;
                value.data = data;
                value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, topic = validation.InvalidText };
            }
            catch (Exception ex)
            {
                LogManager.ServiceLog.WriteExceptionLog(ex, "SearchUnitService:");
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
        
        public SearchMasterRateAmountModel SearchRateAmountService(string authorization, string lang, string platform, int logID, SearchNameCenterDTO searchNameCenterDTO)
        {
            if (_sql == null)
            {
                _sql = SQLManager.Instance;
            }

            SearchMasterRateAmountModel value = new SearchMasterRateAmountModel();
            try
            {
                Pagination<SearchMasterRateAmount> data = new Pagination<SearchMasterRateAmount>();

                ValidationModel validation = ValidationManager.CheckValidation(1, lang, platform);

                if (validation.Success == true)
                {
                    data = _sql.SearchRateAmount(searchNameCenterDTO);
                }
                else
                {
                    _sql.UpdateLogReceiveDataError(logID, validation.InvalidMessage);
                }

                value.success = validation.Success;
                value.data = data;
                value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, topic = validation.InvalidText };
            }
            catch (Exception ex)
            {
                LogManager.ServiceLog.WriteExceptionLog(ex, "SearchRateAmountService:");
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