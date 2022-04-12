using Chaithit_Market.Core;
using Chaithit_Market.DTO;
using Chaithit_Market.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chaithit_Market.Services
{
    public class GetService
    {
        private SQLManager _sql = SQLManager.Instance;

        public SearchUserProfileModel SearchUserProfileService(string authorization, string lang, string platform, int logID, SearchUserProfileDTO searchUserProfileDTO)
        {
            if (_sql == null)
            {
                _sql = SQLManager.Instance;
            }

            SearchUserProfileModel value = new SearchUserProfileModel();
            try
            {
                Pagination<SearchUserProfile> data = new Pagination<SearchUserProfile>();

                ValidationModel validation = ValidationManager.CheckValidation(1, lang, platform);

                if (validation.Success == true)
                {
                    data = _sql.SearchUserProfile(searchUserProfileDTO);
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
                LogManager.ServiceLog.WriteExceptionLog(ex, "SearchUserProfileService:");
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

        public SearchUnitStandModel SearchUnitStandService(string authorization, string lang, string platform, int logID, SearchUnitstandDTO searchUnitstandDTO)
        {
            if (_sql == null)
            {
                _sql = SQLManager.Instance;
            }

            SearchUnitStandModel value = new SearchUnitStandModel();
            try
            {
                Pagination<SearchUnitStand> data = new Pagination<SearchUnitStand>();

                ValidationModel validation = ValidationManager.CheckValidation(1, lang, platform);

                if (validation.Success == true)
                {
                    data = _sql.SearchUnitStand(searchUnitstandDTO);
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
                LogManager.ServiceLog.WriteExceptionLog(ex, "SearchUnitStandService:");
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

        public SearchHistoryPaidBillModel SearchHistoryPaidBillService(string authorization, string lang, string platform, int logID, SearchBillDTO searchBillDTO)
        {
            if (_sql == null)
            {
                _sql = SQLManager.Instance;
            }

            SearchHistoryPaidBillModel value = new SearchHistoryPaidBillModel();
            try
            {
                Pagination<SearchHistoryPaidBill> data = new Pagination<SearchHistoryPaidBill>();

                ValidationModel validation = ValidationManager.CheckValidation(1, lang, platform);

                if (validation.Success == true)
                {
                    data = _sql.SearchHistoryPaidBill(searchBillDTO);
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
                LogManager.ServiceLog.WriteExceptionLog(ex, "SearchHistoryPaidBillService:");
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

        public GetUserProfileModel GetUserProfileService(string authorization, string lang, string platform, int logID, int userProfileID)
        {
            if (_sql == null)
            {
                _sql = SQLManager.Instance;
            }

            GetUserProfileModel value = new GetUserProfileModel();
            try
            {
                value.data = new UserProfileModel();
                value.data.dataMarket = new List<MarketDetail>();

                ValidationModel validation = ValidationManager.CheckValidation(1, lang, platform);

                if (validation.Success == true)
                {
                    value.data = _sql.GetUserProfile(userProfileID);
                    value.data.dataMarket = _sql.GetUserMarket(userProfileID);
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
                LogManager.ServiceLog.WriteExceptionLog(ex, "GetUserProfileService:");
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

        public GetUserProfileModel GetUserRentalService(string authorization, string lang, string platform, int logID, int userProfileID)
        {
            if (_sql == null)
            {
                _sql = SQLManager.Instance;
            }

            GetUserProfileModel value = new GetUserProfileModel();
            try
            {
                value.data = new UserProfileModel();
                value.data.dataMarket = new List<MarketDetail>();

                ValidationModel validation = ValidationManager.CheckValidation(1, lang, platform);

                if (validation.Success == true)
                {
                    value.data = _sql.GetUserRental(userProfileID);
                    value.data.dataMarket = _sql.GetUserMarket(userProfileID);
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
                LogManager.ServiceLog.WriteExceptionLog(ex, "GetUserProfileService:");
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

        public SearchManageRenterModel SearchManageRenterService(string authorization, string lang, string platform, int logID, SearchManageRenterDTO searchManageRenterDTO)
        {
            if (_sql == null)
            {
                _sql = SQLManager.Instance;
            }

            SearchManageRenterModel value = new SearchManageRenterModel();
            try
            {
                Pagination<SearchManageRenter> data = new Pagination<SearchManageRenter>();

                ValidationModel validation = ValidationManager.CheckValidation(1, lang, platform);

                if (validation.Success == true)
                {
                    data = _sql.SearchManageRenter(searchManageRenterDTO);
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
                LogManager.ServiceLog.WriteExceptionLog(ex, "SearchManageRenterService:");
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

        public SearchManageUnitModel SearchManageUnitService(string authorization, string lang, string platform, int logID, SearchManageUnitDTO searchManageUnitDTO)
        {
            if (_sql == null)
            {
                _sql = SQLManager.Instance;
            }

            SearchManageUnitModel value = new SearchManageUnitModel();
            try
            {
                Pagination<SearchManageUnit> data = new Pagination<SearchManageUnit>();

                ValidationModel validation = ValidationManager.CheckValidation(1, lang, platform);

                if (validation.Success == true)
                {
                    data = _sql.SearchManageUnit(searchManageUnitDTO);
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
                LogManager.ServiceLog.WriteExceptionLog(ex, "SearchManageUnitService:");
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

        public GetAllDropdownModel GetDropdownUserService(string authorization, string lang, string platform, int logID, string isAll)
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
                    value.data = _sql.GetDropdownUser(isAll);
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
                LogManager.ServiceLog.WriteExceptionLog(ex, "GetDropdownUserService:");
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