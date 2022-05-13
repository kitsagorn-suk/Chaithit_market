﻿using Chaithit_Market.DTO;
using Chaithit_Market.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Configuration;

namespace Chaithit_Market.Core
{
    public class SQLManager
    {
        string userAgent = HttpContext.Current.Request.UserAgent.ToLower();
        string ipAddress = "";

        public static string GetIP()
        {

            var context = System.Web.HttpContext.Current;
            string ip = String.Empty;

            if (context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                ip = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            else if (!String.IsNullOrWhiteSpace(context.Request.UserHostAddress))
                ip = context.Request.UserHostAddress;

            if (ip == "::1")
                ip = "127.0.0.1";

            ip = ip.Split(':')[0];

            return ip;
        }

        [ThreadStatic]
        private static SQLManager instance = null;

        private SQLManager()
        {

        }

        public static SQLManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SQLManager();
                }
                return instance;
            }
        }

        public int InsertLogReceiveData(string pServiceName, string pReceiveData, string pTimeStampNow, string pAuthorization, int pUserID, string pType)
        {
            int id = 0;

            ipAddress = GetIP();

            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec insert_log_receive_data @pServiceName, @pReceiveData, @pTimeStampNow, @pAuthorization, @pUserID, " +
                "@pType, @pDeviceInfo");

            SqlParameter paramServiceName = new SqlParameter(@"pServiceName", SqlDbType.VarChar, 50);
            paramServiceName.Direction = ParameterDirection.Input;
            paramServiceName.Value = pServiceName;

            SqlParameter paramReceiveData = new SqlParameter(@"pReceiveData", SqlDbType.Text);
            paramReceiveData.Direction = ParameterDirection.Input;
            paramReceiveData.Value = pReceiveData;

            SqlParameter paramTimeStampNow = new SqlParameter(@"pTimeStampNow", SqlDbType.VarChar, 100);
            paramTimeStampNow.Direction = ParameterDirection.Input;
            paramTimeStampNow.Value = pTimeStampNow;

            SqlParameter paramAuthorization = new SqlParameter(@"pAuthorization", SqlDbType.Text);
            paramAuthorization.Direction = ParameterDirection.Input;
            paramAuthorization.Value = pAuthorization == null ? "" : pAuthorization;

            SqlParameter paramUserID = new SqlParameter(@"pUserID", SqlDbType.Int);
            paramUserID.Direction = ParameterDirection.Input;
            paramUserID.Value = pUserID;

            SqlParameter paramType = new SqlParameter(@"pType", SqlDbType.VarChar, 100);
            paramType.Direction = ParameterDirection.Input;
            paramType.Value = pType;

            SqlParameter paramDeviceInfo = new SqlParameter(@"pDeviceInfo", SqlDbType.Text);
            paramDeviceInfo.Direction = ParameterDirection.Input;
            paramDeviceInfo.Value = userAgent;

            sql.Parameters.Add(paramServiceName);
            sql.Parameters.Add(paramReceiveData);
            sql.Parameters.Add(paramTimeStampNow);
            sql.Parameters.Add(paramAuthorization);
            sql.Parameters.Add(paramUserID);
            sql.Parameters.Add(paramType);
            sql.Parameters.Add(paramDeviceInfo);

            table = sql.executeQueryWithReturnTable();

            if (table != null && table.Rows.Count > 0)
            {
                DataRow dr = table.Rows[0];
                id = int.Parse(dr["id"].ToString());
            }
            return id;
        }

        public int UpdateStatusLog(int pLogID, int pStatus)
        {
            int id = 0;

            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec update_status_log_receive_data @pLogID, @pStatus");

            SqlParameter paramLogID = new SqlParameter(@"pLogID", SqlDbType.Int);
            paramLogID.Direction = ParameterDirection.Input;
            paramLogID.Value = pLogID;

            SqlParameter paramStatus = new SqlParameter(@"pStatus", SqlDbType.Int);
            paramStatus.Direction = ParameterDirection.Input;
            paramStatus.Value = pStatus;

            sql.Parameters.Add(paramLogID);
            sql.Parameters.Add(paramStatus);

            table = sql.executeQueryWithReturnTable();

            if (table != null && table.Rows.Count > 0)
            {
                DataRow dr = table.Rows[0];
                id = int.Parse(dr["id"].ToString());
            }
            return id;
        }

        public GetMessageTopicDTO GetMessageLang(string pLang, int pMsgCode)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec get_message_lang @pLang, @pMsgCode");

            SqlParameter paramLang = new SqlParameter(@"pLang", SqlDbType.VarChar, 5);
            paramLang.Direction = ParameterDirection.Input;
            paramLang.Value = pLang;

            SqlParameter paramMsgCode = new SqlParameter(@"pMsgCode", SqlDbType.Int);
            paramMsgCode.Direction = ParameterDirection.Input;
            paramMsgCode.Value = pMsgCode;

            sql.Parameters.Add(paramLang);
            sql.Parameters.Add(paramMsgCode);

            table = sql.executeQueryWithReturnTable();

            GetMessageTopicDTO result = new GetMessageTopicDTO();

            if (table != null && table.Rows.Count > 0)
            {
                DataRow dr = table.Rows[0];
                result.message = dr["text"].ToString();
                result.topic = dr["topic"].ToString();
            }
            return result;
        }

        public CheckUserByTokenModel CheckUserID(string pUserName, string pPassword)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec check_user_id @pUserName, @pPassword");

            SqlParameter paramUserName = new SqlParameter(@"pUserName", SqlDbType.VarChar, 150);
            paramUserName.Direction = ParameterDirection.Input;
            paramUserName.Value = pUserName;

            SqlParameter paramPassword = new SqlParameter(@"pPassword", SqlDbType.VarChar, 250);
            paramPassword.Direction = ParameterDirection.Input;
            paramPassword.Value = pPassword;

            sql.Parameters.Add(paramUserName);
            sql.Parameters.Add(paramPassword);

            table = sql.executeQueryWithReturnTable();

            CheckUserByTokenModel data = new CheckUserByTokenModel();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    data.loadData(row);
                }
            }

            return data;
        }

        public LoginData Login(string UserName, string Password, string TokenID, string DeviceType, string Lang)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec login @pUserName, @pPassword, @pTokenID, @pDeviceType, @pLang");

            SqlParameter pUserName = new SqlParameter(@"pUserName", SqlDbType.VarChar, 100);
            pUserName.Direction = ParameterDirection.Input;
            pUserName.Value = UserName;

            SqlParameter pPassword = new SqlParameter(@"pPassword", SqlDbType.VarChar, 200);
            pPassword.Direction = ParameterDirection.Input;
            pPassword.Value = Password;

            SqlParameter pTokenID = new SqlParameter(@"pTokenID", SqlDbType.VarChar);
            pTokenID.Direction = ParameterDirection.Input;
            pTokenID.Value = TokenID;
            
            SqlParameter pDeviceType = new SqlParameter(@"pDeviceType", SqlDbType.VarChar, 100);
            pDeviceType.Direction = ParameterDirection.Input;
            pDeviceType.Value = DeviceType;

            SqlParameter pLang = new SqlParameter(@"pLang", SqlDbType.VarChar, 5);
            pLang.Direction = ParameterDirection.Input;
            pLang.Value = Lang;

            sql.Parameters.Add(pUserName);
            sql.Parameters.Add(pPassword);
            sql.Parameters.Add(pTokenID);
            sql.Parameters.Add(pDeviceType);
            sql.Parameters.Add(pLang);

            table = sql.executeQueryWithReturnTable();

            LoginData data = new LoginData();

            if (table != null && table.Rows.Count > 0)
            {
                data.loadData(table.Rows[0]);
            }

            return data;
        }

        public int UpdateLogReceiveDataError(int pLogID, string pErrorText)
        {
            int id = 0;

            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec update_log_receive_data_error @pLogID, @pErrorText");

            SqlParameter paramLogID = new SqlParameter(@"pLogID", SqlDbType.Int);
            paramLogID.Direction = ParameterDirection.Input;
            paramLogID.Value = pLogID;

            SqlParameter paramErrorText = new SqlParameter(@"pErrorText", SqlDbType.Text);
            paramErrorText.Direction = ParameterDirection.Input;
            paramErrorText.Value = pErrorText;

            sql.Parameters.Add(paramLogID);
            sql.Parameters.Add(paramErrorText);

            table = sql.executeQueryWithReturnTable();

            if (table != null && table.Rows.Count > 0)
            {
                DataRow dr = table.Rows[0];
                id = int.Parse(dr["id"].ToString());
            }
            return id;
        }

        public int CheckUserPassword(string pUserName, string pPassword)
        {
            int total = 0;

            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec check_user_password @pUserName, @pPassword");

            SqlParameter paramUserName = new SqlParameter(@"pUserName", SqlDbType.VarChar, 100);
            paramUserName.Direction = ParameterDirection.Input;
            paramUserName.Value = pUserName;

            SqlParameter paramPassword = new SqlParameter(@"pPassword", SqlDbType.VarChar, 200);
            paramPassword.Direction = ParameterDirection.Input;
            paramPassword.Value = pPassword;

            sql.Parameters.Add(paramUserName);
            sql.Parameters.Add(paramPassword);

            table = sql.executeQueryWithReturnTable();

            if (table != null && table.Rows.Count > 0)
            {
                DataRow dr = table.Rows[0];
                total = int.Parse(dr["total"].ToString());
            }
            return total;
        }

        public int CheckUserType(string pUserName)
        {
            int statusEmp = 0;

            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec check_user_type @pUserName");

            SqlParameter paramUserName = new SqlParameter(@"pUserName", SqlDbType.VarChar, 100);
            paramUserName.Direction = ParameterDirection.Input;
            paramUserName.Value = pUserName;

            sql.Parameters.Add(paramUserName);

            table = sql.executeQueryWithReturnTable();

            if (table != null && table.Rows.Count > 0)
            {
                DataRow dr = table.Rows[0];
                statusEmp = int.Parse(dr["status_emp"].ToString());
            }
            return statusEmp;
        }

        public bool CheckToken(string pToken)
        {
            bool success = false;

            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec check_token_id @pToken");

            SqlParameter paramToken = new SqlParameter(@"pToken", SqlDbType.VarChar);
            paramToken.Direction = ParameterDirection.Input;
            paramToken.Value = pToken;

            sql.Parameters.Add(paramToken);

            table = sql.executeQueryWithReturnTable();

            if (table != null && table.Rows.Count > 0)
            {
                success = true;
            }
            return success;
        }

        public DataTable CheckDuplicateMaster(string TableName,MasterDataDTO masterDataDTO)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("");

            sql = new SQLCustomExecute("exec check_duplicate_master @pMasterID,@pTableName,@pName");
            
            SqlParameter pMasterID = new SqlParameter(@"pMasterID", SqlDbType.Int);
            pMasterID.Direction = ParameterDirection.Input;
            pMasterID.Value = masterDataDTO.masterID;
            sql.Parameters.Add(pMasterID);

            SqlParameter pTableName = new SqlParameter(@"pTableName", SqlDbType.VarChar);
            pTableName.Direction = ParameterDirection.Input;
            pTableName.Value = TableName;
            sql.Parameters.Add(pTableName);

            SqlParameter pName = new SqlParameter(@"pName", SqlDbType.VarChar);
            pName.Direction = ParameterDirection.Input;
            pName.Value = masterDataDTO.name;
            sql.Parameters.Add(pName);

            table = sql.executeQueryWithReturnTable();

            return table;
        }

        public _ReturnIdModel InsertMasterData(MasterDataDTO masterDataDTO, string TableName, int userID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec insert_master_data " +
                "@pTableName," +
                "@pName," +
                "@pUserID ");

            SqlParameter pTableName = new SqlParameter(@"pTableName", SqlDbType.VarChar);
            pTableName.Direction = ParameterDirection.Input;
            pTableName.Value = TableName;
            sql.Parameters.Add(pTableName);

            SqlParameter pName = new SqlParameter(@"pName", SqlDbType.VarChar);
            pName.Direction = ParameterDirection.Input;
            pName.Value = masterDataDTO.name;
            sql.Parameters.Add(pName);

            SqlParameter pUserID = new SqlParameter(@"pUserID", SqlDbType.Int);
            pUserID.Direction = ParameterDirection.Input;
            pUserID.Value = userID;
            sql.Parameters.Add(pUserID);

            table = sql.executeQueryWithReturnTable();

            _ReturnIdModel data = new _ReturnIdModel();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    data.loadData(row);
                }
            }

            return data;
        }

        public _ReturnIdModel UpdateMasterData(MasterDataDTO masterDataDTO, string TableName, int userID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec update_master_data " +
                "@pMasterID," +
                "@pTableName," +
                "@pName," +
                "@pUserID ");

            SqlParameter pMasterID = new SqlParameter(@"pMasterID", SqlDbType.Int);
            pMasterID.Direction = ParameterDirection.Input;
            pMasterID.Value = masterDataDTO.masterID;
            sql.Parameters.Add(pMasterID);

            SqlParameter pTableName = new SqlParameter(@"pTableName", SqlDbType.VarChar);
            pTableName.Direction = ParameterDirection.Input;
            pTableName.Value = TableName;
            sql.Parameters.Add(pTableName);

            SqlParameter pName = new SqlParameter(@"pName", SqlDbType.VarChar);
            pName.Direction = ParameterDirection.Input;
            pName.Value = masterDataDTO.name;
            sql.Parameters.Add(pName);

            SqlParameter pUserID = new SqlParameter(@"pUserID", SqlDbType.Int);
            pUserID.Direction = ParameterDirection.Input;
            pUserID.Value = userID;
            sql.Parameters.Add(pUserID);

            table = sql.executeQueryWithReturnTable();

            _ReturnIdModel data = new _ReturnIdModel();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    data.loadData(row);
                }
            }

            return data;
        }

        public _ReturnIdModel DeleteMasterData(MasterDataDTO masterDataDTO, string TableName, int userID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec delete_master_data " +
                "@pMasterID," +
                "@pTableName," +
                "@pUserID ");

            SqlParameter pMasterID = new SqlParameter(@"pMasterID", SqlDbType.Int);
            pMasterID.Direction = ParameterDirection.Input;
            pMasterID.Value = masterDataDTO.masterID;
            sql.Parameters.Add(pMasterID);

            SqlParameter pTableName = new SqlParameter(@"pTableName", SqlDbType.VarChar);
            pTableName.Direction = ParameterDirection.Input;
            pTableName.Value = TableName;
            sql.Parameters.Add(pTableName);

            SqlParameter pUserID = new SqlParameter(@"pUserID", SqlDbType.Int);
            pUserID.Direction = ParameterDirection.Input;
            pUserID.Value = userID;
            sql.Parameters.Add(pUserID);

            table = sql.executeQueryWithReturnTable();

            _ReturnIdModel data = new _ReturnIdModel();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    data.loadData(row);
                }
            }

            return data;
        }

        public List<DropdownAllData> GetDropdownMasterData(int id, string TableName, string isAll)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec get_dropdown_master_data " +
                "@pMasterID," +
                "@pTableName," +
                "@pIsAll");

            SqlParameter pMasterID = new SqlParameter(@"pMasterID", SqlDbType.Int);
            pMasterID.Direction = ParameterDirection.Input;
            pMasterID.Value = id;
            sql.Parameters.Add(pMasterID);

            SqlParameter pTableName = new SqlParameter(@"pTableName", SqlDbType.VarChar);
            pTableName.Direction = ParameterDirection.Input;
            pTableName.Value = TableName;
            sql.Parameters.Add(pTableName);

            SqlParameter pIsAll = new SqlParameter(@"pIsAll", SqlDbType.VarChar);
            pIsAll.Direction = ParameterDirection.Input;
            pIsAll.Value = isAll;
            sql.Parameters.Add(pIsAll);

            table = sql.executeQueryWithReturnTable();

            List<DropdownAllData> listData = new List<DropdownAllData>();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    DropdownAllData data = new DropdownAllData();
                    data.loadData(row);
                    listData.Add(data);
                }
            }

            return listData;
        }

        public DataTable CheckValidationUpdateByID(int ID, string TableName)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec check_validation_update " +
                "@pID, " +
                "@pTableName");

            SqlParameter pID = new SqlParameter(@"pID", SqlDbType.Int);
            pID.Direction = ParameterDirection.Input;
            pID.Value = ID;
            sql.Parameters.Add(pID);

            SqlParameter pTableName = new SqlParameter(@"pTableName", SqlDbType.VarChar);
            pTableName.Direction = ParameterDirection.Input;
            pTableName.Value = TableName;
            sql.Parameters.Add(pTableName);

            table = sql.executeQueryWithReturnTable();

            return table;
        }

        public _ReturnIdModel InsertSystemLogChange(int actionID, string tableName, string fieldName, string newData, int userID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec insert_system_log_change " +
                "@pActionID," +
                "@pTableName," +
                "@pFieldName," +
                "@pNewData," +
                "@pUserID ");

            SqlParameter pActionID = new SqlParameter(@"pActionID", SqlDbType.Int);
            pActionID.Direction = ParameterDirection.Input;
            pActionID.Value = actionID;
            sql.Parameters.Add(pActionID);

            SqlParameter pTableName = new SqlParameter(@"pTableName", SqlDbType.VarChar, 100);
            pTableName.Direction = ParameterDirection.Input;
            pTableName.Value = tableName;
            sql.Parameters.Add(pTableName);

            SqlParameter pFieldName = new SqlParameter(@"pFieldName", SqlDbType.VarChar, 100);
            pFieldName.Direction = ParameterDirection.Input;
            pFieldName.Value = fieldName;
            sql.Parameters.Add(pFieldName);

            SqlParameter pNewData = new SqlParameter(@"pNewData", SqlDbType.VarChar, 4000);
            pNewData.Direction = ParameterDirection.Input;
            pNewData.Value = newData;
            sql.Parameters.Add(pNewData);

            SqlParameter pUserID = new SqlParameter(@"pUserID", SqlDbType.Int);
            pUserID.Direction = ParameterDirection.Input;
            pUserID.Value = userID;
            sql.Parameters.Add(pUserID);

            table = sql.executeQueryWithReturnTable();

            _ReturnIdModel data = new _ReturnIdModel();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    data.loadData(row);
                }
            }

            return data;
        }

        public DataTable CheckDupicateUser(SaveUserProfileDTO saveUserProfileDTO, int pUserID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec check_dupicate_user " +
                "@pUserProfileID, " + 
                "@pFirstName, " +
                "@pLastName, " +
                "@pUserName, " +
                "@pMobile ");

            SqlParameter pUserProfileID = new SqlParameter(@"pUserProfileID", SqlDbType.Int);
            pUserProfileID.Direction = ParameterDirection.Input;
            pUserProfileID.Value = saveUserProfileDTO.userProfileID;
            sql.Parameters.Add(pUserProfileID);

            SqlParameter pFirstName = new SqlParameter(@"pFirstName", SqlDbType.VarChar, 150);
            pFirstName.Direction = ParameterDirection.Input;
            pFirstName.Value = saveUserProfileDTO.firstName;
            sql.Parameters.Add(pFirstName);

            SqlParameter pLastName = new SqlParameter(@"pLastName", SqlDbType.VarChar, 150);
            pLastName.Direction = ParameterDirection.Input;
            pLastName.Value = saveUserProfileDTO.lastName;
            sql.Parameters.Add(pLastName);

            SqlParameter pUserName = new SqlParameter(@"pUserName", SqlDbType.VarChar, 200);
            pUserName.Direction = ParameterDirection.Input;
            pUserName.Value = saveUserProfileDTO.userName;
            sql.Parameters.Add(pUserName);

            SqlParameter pMobile = new SqlParameter(@"pMobile", SqlDbType.VarChar, 15);
            pMobile.Direction = ParameterDirection.Input;
            pMobile.Value = saveUserProfileDTO.mobile;
            sql.Parameters.Add(pMobile);

            table = sql.executeQueryWithReturnTable();

            return table;
        }

        public DataTable CheckDupicateTransectionRent(InsertTransectionRentDTO insertTransectionRentDTO, int pUserID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec check_dupicate_transection_rent " +
                "@pTranRentID, " +
                "@pTransCode, " +
                "@pUnitID");

            SqlParameter pTranRentID = new SqlParameter(@"pTranRentID", SqlDbType.Int);
            pTranRentID.Direction = ParameterDirection.Input;
            pTranRentID.Value = insertTransectionRentDTO.tranRentID;
            sql.Parameters.Add(pTranRentID);

            SqlParameter pTransCode = new SqlParameter(@"pTransCode", SqlDbType.VarChar, 10);
            pTransCode.Direction = ParameterDirection.Input;
            pTransCode.Value = insertTransectionRentDTO.transCode;
            sql.Parameters.Add(pTransCode);

            SqlParameter pUnitID = new SqlParameter(@"pUnitID", SqlDbType.Int);
            pUnitID.Direction = ParameterDirection.Input;
            pUnitID.Value = insertTransectionRentDTO.unitID;
            sql.Parameters.Add(pUnitID);

            table = sql.executeQueryWithReturnTable();

            return table;
        }

        public DataTable CheckDupicateTransectionBill(InsertTransectionBillDTO insertTransectionBillDTO, int pUserID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec check_dupicate_transection_bill " +
                "@pTranBillID, " +
                "@pBillCode, " +
                "@pTranRentID ");

            SqlParameter pTranBillID = new SqlParameter(@"pTranBillID", SqlDbType.Int);
            pTranBillID.Direction = ParameterDirection.Input;
            pTranBillID.Value = insertTransectionBillDTO.tranBillID;
            sql.Parameters.Add(pTranBillID);

            SqlParameter pBillCode = new SqlParameter(@"pBillCode", SqlDbType.VarChar, 150);
            pBillCode.Direction = ParameterDirection.Input;
            pBillCode.Value = insertTransectionBillDTO.billCode;
            sql.Parameters.Add(pBillCode);

            SqlParameter pTranRentID = new SqlParameter(@"pTranRentID", SqlDbType.Int);
            pTranRentID.Direction = ParameterDirection.Input;
            pTranRentID.Value = insertTransectionBillDTO.tranRentID;
            sql.Parameters.Add(pTranRentID);

            table = sql.executeQueryWithReturnTable();

            return table;
        }

        public _ReturnIdModel InsertUserProfile(SaveUserProfileDTO saveUserProfileDTO, int userID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec insert_user_profile " +
                "@pUserName," +
                "@pPassWord," +
                "@pFirstname," +
                "@pLastname," +
                "@pMoblie," +
                "@pPosition," +
                "@pStartDate," +
                "@pEndDate," +
                "@pStatusEmp," +
                "@pEmpType," + 
                "@pCreateBy ");

            SqlParameter pUserName = new SqlParameter(@"pUserName", SqlDbType.VarChar);
            pUserName.Direction = ParameterDirection.Input;
            pUserName.Value = saveUserProfileDTO.userName;
            sql.Parameters.Add(pUserName);

            SqlParameter pPassWord = new SqlParameter(@"pPassWord", SqlDbType.VarChar);
            pPassWord.Direction = ParameterDirection.Input;
            pPassWord.Value = saveUserProfileDTO.password;
            sql.Parameters.Add(pPassWord);

            SqlParameter pFirstname = new SqlParameter(@"pFirstname", SqlDbType.VarChar);
            pFirstname.Direction = ParameterDirection.Input;
            pFirstname.Value = saveUserProfileDTO.firstName;
            sql.Parameters.Add(pFirstname);

            SqlParameter pLastname = new SqlParameter(@"pLastname", SqlDbType.VarChar);
            pLastname.Direction = ParameterDirection.Input;
            pLastname.Value = saveUserProfileDTO.lastName;
            sql.Parameters.Add(pLastname);

            SqlParameter pMoblie = new SqlParameter(@"pMoblie", SqlDbType.VarChar);
            pMoblie.Direction = ParameterDirection.Input;
            pMoblie.Value = saveUserProfileDTO.mobile;
            sql.Parameters.Add(pMoblie);

            SqlParameter pPosition = new SqlParameter(@"pPosition", SqlDbType.VarChar);
            pPosition.Direction = ParameterDirection.Input;
            pPosition.Value = saveUserProfileDTO.position;
            sql.Parameters.Add(pPosition);

            SqlParameter pStartDate = new SqlParameter(@"pStartDate", SqlDbType.VarChar);
            pStartDate.Direction = ParameterDirection.Input;
            pStartDate.Value = saveUserProfileDTO.startDate;
            sql.Parameters.Add(pStartDate);

            SqlParameter pEndDate = new SqlParameter(@"pEndDate", SqlDbType.VarChar);
            pEndDate.Direction = ParameterDirection.Input;
            pEndDate.Value = saveUserProfileDTO.endDate;
            sql.Parameters.Add(pEndDate);

            SqlParameter pStatusEmp = new SqlParameter(@"pStatusEmp", SqlDbType.Int);
            pStatusEmp.Direction = ParameterDirection.Input;
            pStatusEmp.Value = saveUserProfileDTO.statusEmp;
            sql.Parameters.Add(pStatusEmp);

            SqlParameter pEmpType = new SqlParameter(@"pEmpType", SqlDbType.Int);
            pEmpType.Direction = ParameterDirection.Input;
            pEmpType.Value = saveUserProfileDTO.empType;
            sql.Parameters.Add(pEmpType);

            SqlParameter pCreateBy = new SqlParameter(@"pCreateBy", SqlDbType.Int);
            pCreateBy.Direction = ParameterDirection.Input;
            pCreateBy.Value = userID;
            sql.Parameters.Add(pCreateBy);

            table = sql.executeQueryWithReturnTable();

            _ReturnIdModel data = new _ReturnIdModel();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    data.loadData(row);
                }
            }

            return data;
        }

        public _ReturnIdModel UpdateUserProfile(SaveUserProfileDTO saveUserProfileDTO, int userID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec update_user_profile " +
                "@pUserProfileID," +
                "@pFirstname," +
                "@pLastname," +
                "@pMoblie," +
                "@pPosition," +
                "@pStartDate," +
                "@pStatusEmp," +
                "@pEmpType," +
                "@pUpdateBy, " +
                "@pPassword ");

            SqlParameter pUserProfileID = new SqlParameter(@"pUserProfileID", SqlDbType.Int);
            pUserProfileID.Direction = ParameterDirection.Input;
            pUserProfileID.Value = saveUserProfileDTO.userProfileID;
            sql.Parameters.Add(pUserProfileID);

            SqlParameter pFirstname = new SqlParameter(@"pFirstname", SqlDbType.VarChar);
            pFirstname.Direction = ParameterDirection.Input;
            pFirstname.Value = saveUserProfileDTO.firstName;
            sql.Parameters.Add(pFirstname);

            SqlParameter pLastname = new SqlParameter(@"pLastname", SqlDbType.VarChar);
            pLastname.Direction = ParameterDirection.Input;
            pLastname.Value = saveUserProfileDTO.lastName;
            sql.Parameters.Add(pLastname);

            SqlParameter pMoblie = new SqlParameter(@"pMoblie", SqlDbType.VarChar);
            pMoblie.Direction = ParameterDirection.Input;
            pMoblie.Value = saveUserProfileDTO.mobile;
            sql.Parameters.Add(pMoblie);

            SqlParameter pPosition = new SqlParameter(@"pPosition", SqlDbType.VarChar);
            pPosition.Direction = ParameterDirection.Input;
            pPosition.Value = saveUserProfileDTO.position;
            sql.Parameters.Add(pPosition);

            SqlParameter pStartDate = new SqlParameter(@"pStartDate", SqlDbType.VarChar);
            pStartDate.Direction = ParameterDirection.Input;
            pStartDate.Value = saveUserProfileDTO.startDate;
            sql.Parameters.Add(pStartDate);

            SqlParameter pStatusEmp = new SqlParameter(@"pStatusEmp", SqlDbType.Int);
            pStatusEmp.Direction = ParameterDirection.Input;
            pStatusEmp.Value = saveUserProfileDTO.statusEmp;
            sql.Parameters.Add(pStatusEmp);

            SqlParameter pEmpType = new SqlParameter(@"pEmpType", SqlDbType.Int);
            pEmpType.Direction = ParameterDirection.Input;
            pEmpType.Value = saveUserProfileDTO.empType;
            sql.Parameters.Add(pEmpType);

            SqlParameter pUserID = new SqlParameter(@"pUpdateBy", SqlDbType.Int);
            pUserID.Direction = ParameterDirection.Input;
            pUserID.Value = userID;
            sql.Parameters.Add(pUserID);

            SqlParameter pPassword = new SqlParameter(@"pPassword", SqlDbType.VarChar);
            pPassword.Direction = ParameterDirection.Input;
            pPassword.Value = saveUserProfileDTO.password;
            sql.Parameters.Add(pPassword);

            table = sql.executeQueryWithReturnTable();

            _ReturnIdModel data = new _ReturnIdModel();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    data.loadData(row);
                }
            }

            return data;
        }

        public _ReturnIdModel DeleteUserProfile(SaveUserProfileDTO saveUserProfileDTO, int userID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec delete_user_profile " +
                "@pUserProfileID," +
                "@pUpdateBy ");

            SqlParameter pUserProfileID = new SqlParameter(@"pUserProfileID", SqlDbType.Int);
            pUserProfileID.Direction = ParameterDirection.Input;
            pUserProfileID.Value = saveUserProfileDTO.userProfileID;
            sql.Parameters.Add(pUserProfileID);

            SqlParameter pUserID = new SqlParameter(@"pUpdateBy", SqlDbType.Int);
            pUserID.Direction = ParameterDirection.Input;
            pUserID.Value = userID;
            sql.Parameters.Add(pUserID);

            table = sql.executeQueryWithReturnTable();

            _ReturnIdModel data = new _ReturnIdModel();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    data.loadData(row);
                }
            }

            return data;
        }

        public _ReturnIdModel InsertTransectionRent(InsertTransectionRentDTO insertTransectionRentDTO, int userID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec insert_transection_rent " +
                "@pTransCode," +
                "@pUserID," +
                "@pUnitID," +
                "@pStartDate," +
                "@pEndDate," +
                "@pRentType," +
                "@pCreateBy");

            SqlParameter pTransCode = new SqlParameter(@"pTransCode", SqlDbType.VarChar);
            pTransCode.Direction = ParameterDirection.Input;
            pTransCode.Value = insertTransectionRentDTO.transCode;
            sql.Parameters.Add(pTransCode);

            SqlParameter pUserID = new SqlParameter(@"pUserID", SqlDbType.Int);
            pUserID.Direction = ParameterDirection.Input;
            pUserID.Value = insertTransectionRentDTO.userID;
            sql.Parameters.Add(pUserID);

            SqlParameter pUnitID = new SqlParameter(@"pUnitID", SqlDbType.Int);
            pUnitID.Direction = ParameterDirection.Input;
            pUnitID.Value = insertTransectionRentDTO.unitID;
            sql.Parameters.Add(pUnitID);

            SqlParameter pStartDate = new SqlParameter(@"pStartDate", SqlDbType.VarChar);
            pStartDate.Direction = ParameterDirection.Input;
            pStartDate.Value = insertTransectionRentDTO.startDate;
            sql.Parameters.Add(pStartDate);

            SqlParameter pEndDate = new SqlParameter(@"pEndDate", SqlDbType.VarChar);
            pEndDate.Direction = ParameterDirection.Input;
            pEndDate.Value = insertTransectionRentDTO.endDate;
            sql.Parameters.Add(pEndDate);

            SqlParameter pRentType = new SqlParameter(@"pRentType", SqlDbType.Int);
            pRentType.Direction = ParameterDirection.Input;
            pRentType.Value = insertTransectionRentDTO.rentType;
            sql.Parameters.Add(pRentType);

            SqlParameter pCreateBy = new SqlParameter(@"pCreateBy", SqlDbType.Int);
            pCreateBy.Direction = ParameterDirection.Input;
            pCreateBy.Value = userID;
            sql.Parameters.Add(pCreateBy);

            table = sql.executeQueryWithReturnTable();

            _ReturnIdModel data = new _ReturnIdModel();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    data.loadData(row);
                }
            }

            return data;
        }

        public _ReturnIdModel UpdateTransectionRent(InsertTransectionRentDTO insertTransectionRentDTO, int userID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec update_transection_rent " +
                "@pTransRentID," +
                "@pTransCode," +
                "@pUserID," +
                "@pUnitID," +
                "@pStartDate," +
                "@pEndDate," +
                "@pRentType," +
                "@pCreateBy");

            SqlParameter pTransRentID = new SqlParameter(@"pTransRentID", SqlDbType.Int);
            pTransRentID.Direction = ParameterDirection.Input;
            pTransRentID.Value = insertTransectionRentDTO.tranRentID;
            sql.Parameters.Add(pTransRentID);

            SqlParameter pTransCode = new SqlParameter(@"pTransCode", SqlDbType.VarChar);
            pTransCode.Direction = ParameterDirection.Input;
            pTransCode.Value = insertTransectionRentDTO.transCode;
            sql.Parameters.Add(pTransCode);

            SqlParameter pUserID = new SqlParameter(@"pUserID", SqlDbType.Int);
            pUserID.Direction = ParameterDirection.Input;
            pUserID.Value = insertTransectionRentDTO.userID;
            sql.Parameters.Add(pUserID);

            SqlParameter pUnitID = new SqlParameter(@"pUnitID", SqlDbType.Int);
            pUnitID.Direction = ParameterDirection.Input;
            pUnitID.Value = insertTransectionRentDTO.unitID;
            sql.Parameters.Add(pUnitID);

            SqlParameter pStartDate = new SqlParameter(@"pStartDate", SqlDbType.VarChar);
            pStartDate.Direction = ParameterDirection.Input;
            pStartDate.Value = insertTransectionRentDTO.startDate;
            sql.Parameters.Add(pStartDate);

            SqlParameter pEndDate = new SqlParameter(@"pEndDate", SqlDbType.VarChar);
            pEndDate.Direction = ParameterDirection.Input;
            pEndDate.Value = insertTransectionRentDTO.endDate;
            sql.Parameters.Add(pEndDate);

            SqlParameter pRentType = new SqlParameter(@"pRentType", SqlDbType.Int);
            pRentType.Direction = ParameterDirection.Input;
            pRentType.Value = insertTransectionRentDTO.rentType;
            sql.Parameters.Add(pRentType);

            SqlParameter pCreateBy = new SqlParameter(@"pCreateBy", SqlDbType.Int);
            pCreateBy.Direction = ParameterDirection.Input;
            pCreateBy.Value = userID;
            sql.Parameters.Add(pCreateBy);

            table = sql.executeQueryWithReturnTable();

            _ReturnIdModel data = new _ReturnIdModel();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    data.loadData(row);
                }
            }

            return data;
        }

        public _ReturnIdModel InsertTransectionBill(InsertTransectionBillDTO insertTransectionBillDTO, int userID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec insert_transection_bill " +
                "@pTranRentID," +
                "@pBillCode," +
                "@pStartDate," +
                "@pEndDate," +
                "@pRentAmount," +
                "@pWaterUnit," +
                "@pLampUnit," +
                "@pElectricEquipUnit," +
                "@pDiscountPercent," +
                "@pDiscountAmount," +
                "@pPayDate," +
                "@pCreateBy");

            SqlParameter pTranRentID = new SqlParameter(@"pTranRentID", SqlDbType.Int);
            pTranRentID.Direction = ParameterDirection.Input;
            pTranRentID.Value = insertTransectionBillDTO.tranRentID;
            sql.Parameters.Add(pTranRentID);
            
            SqlParameter pBillCode = new SqlParameter(@"pBillCode", SqlDbType.VarChar);
            pBillCode.Direction = ParameterDirection.Input;
            pBillCode.Value = insertTransectionBillDTO.billCode;
            sql.Parameters.Add(pBillCode);
            
            SqlParameter pStartDate = new SqlParameter(@"pStartDate", SqlDbType.VarChar);
            pStartDate.Direction = ParameterDirection.Input;
            pStartDate.Value = insertTransectionBillDTO.startDate;
            sql.Parameters.Add(pStartDate);

            SqlParameter pEndDate = new SqlParameter(@"pEndDate", SqlDbType.VarChar);
            pEndDate.Direction = ParameterDirection.Input;
            pEndDate.Value = insertTransectionBillDTO.endDate;
            sql.Parameters.Add(pEndDate);

            SqlParameter pRentAmount = new SqlParameter(@"pRentAmount", SqlDbType.Decimal);
            pRentAmount.Direction = ParameterDirection.Input;
            pRentAmount.Value = insertTransectionBillDTO.rentAmount;
            sql.Parameters.Add(pRentAmount);

            SqlParameter pWaterUnit = new SqlParameter(@"pWaterUnit", SqlDbType.Int);
            pWaterUnit.Direction = ParameterDirection.Input;
            pWaterUnit.Value = insertTransectionBillDTO.waterUnit;
            sql.Parameters.Add(pWaterUnit);

            SqlParameter pLampUnit = new SqlParameter(@"pLampUnit", SqlDbType.Int);
            pLampUnit.Direction = ParameterDirection.Input;
            pLampUnit.Value = insertTransectionBillDTO.lampUnit;
            sql.Parameters.Add(pLampUnit);

            SqlParameter pElectricEquipUnit = new SqlParameter(@"pElectricEquipUnit", SqlDbType.Int);
            pElectricEquipUnit.Direction = ParameterDirection.Input;
            pElectricEquipUnit.Value = insertTransectionBillDTO.electricEquipUnit;
            sql.Parameters.Add(pElectricEquipUnit);

            SqlParameter pDiscountPercent = new SqlParameter(@"pDiscountPercent", SqlDbType.Int);
            pDiscountPercent.Direction = ParameterDirection.Input;
            pDiscountPercent.Value = insertTransectionBillDTO.discountPercent;
            sql.Parameters.Add(pDiscountPercent);

            SqlParameter pDiscountAmount = new SqlParameter(@"pDiscountAmount", SqlDbType.Decimal);
            pDiscountAmount.Direction = ParameterDirection.Input;
            pDiscountAmount.Value = insertTransectionBillDTO.discountAmount;
            sql.Parameters.Add(pDiscountAmount);

            SqlParameter pPayDate = new SqlParameter(@"pPayDate", SqlDbType.VarChar);
            pPayDate.Direction = ParameterDirection.Input;
            pPayDate.Value = insertTransectionBillDTO.endDate;
            sql.Parameters.Add(pPayDate);

            SqlParameter pCreateBy = new SqlParameter(@"pCreateBy", SqlDbType.Int);
            pCreateBy.Direction = ParameterDirection.Input;
            pCreateBy.Value = userID;
            sql.Parameters.Add(pCreateBy);

            table = sql.executeQueryWithReturnTable();

            _ReturnIdModel data = new _ReturnIdModel();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    data.loadData(row);
                }
            }

            return data;
        }

        public _ReturnIdModel UpdateTransectionBill(InsertTransectionBillDTO insertTransectionBillDTO, int userID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec update_transection_bill " +
                "@pTranBillID," +
                "@pTranRentID," +
                "@pBillCode," +
                "@pStartDate," +
                "@pEndDate," +
                "@pRentAmount," +
                "@pElectricUnit," +
                "@pElectricAmount," +
                "@pWaterUnit," +
                "@pWaterAmount," +
                "@pLampUnit," +
                "@pElectricEquipUnit," +
                "@pElectricNightMarketAmount," +
                "@pTotalAmount," +
                "@pDiscountPercent," +
                "@pDiscountAmount," +
                "@pNetAmount," +
                "@pPayDate," +
                "@pCreateBy");

            SqlParameter pTransBillID = new SqlParameter(@"pTranBillID", SqlDbType.Int);
            pTransBillID.Direction = ParameterDirection.Input;
            pTransBillID.Value = insertTransectionBillDTO.tranBillID;
            sql.Parameters.Add(pTransBillID);

            SqlParameter pTranRentID = new SqlParameter(@"pTranRentID", SqlDbType.Int);
            pTranRentID.Direction = ParameterDirection.Input;
            pTranRentID.Value = insertTransectionBillDTO.tranRentID;
            sql.Parameters.Add(pTranRentID);

            SqlParameter pBillCode = new SqlParameter(@"pBillCode", SqlDbType.VarChar);
            pBillCode.Direction = ParameterDirection.Input;
            pBillCode.Value = insertTransectionBillDTO.billCode;
            sql.Parameters.Add(pBillCode);

            SqlParameter pStartDate = new SqlParameter(@"pStartDate", SqlDbType.VarChar);
            pStartDate.Direction = ParameterDirection.Input;
            pStartDate.Value = insertTransectionBillDTO.startDate;
            sql.Parameters.Add(pStartDate);

            SqlParameter pEndDate = new SqlParameter(@"pEndDate", SqlDbType.VarChar);
            pEndDate.Direction = ParameterDirection.Input;
            pEndDate.Value = insertTransectionBillDTO.endDate;
            sql.Parameters.Add(pEndDate);

            SqlParameter pRentAmount = new SqlParameter(@"pRentAmount", SqlDbType.Decimal);
            pRentAmount.Direction = ParameterDirection.Input;
            pRentAmount.Value = insertTransectionBillDTO.rentAmount;
            sql.Parameters.Add(pRentAmount);

            SqlParameter pElectricUnit = new SqlParameter(@"pElectricUnit", SqlDbType.Int);
            pElectricUnit.Direction = ParameterDirection.Input;
            pElectricUnit.Value = insertTransectionBillDTO.electricUnit;
            sql.Parameters.Add(pElectricUnit);

            SqlParameter pElectricAmount = new SqlParameter(@"pElectricAmount", SqlDbType.Decimal);
            pElectricAmount.Direction = ParameterDirection.Input;
            pElectricAmount.Value = insertTransectionBillDTO.electricAmount;
            sql.Parameters.Add(pElectricAmount);

            SqlParameter pWaterUnit = new SqlParameter(@"pWaterUnit", SqlDbType.Int);
            pWaterUnit.Direction = ParameterDirection.Input;
            pWaterUnit.Value = insertTransectionBillDTO.waterUnit;
            sql.Parameters.Add(pWaterUnit);

            SqlParameter pWaterAmount = new SqlParameter(@"pWaterAmount", SqlDbType.Decimal);
            pWaterAmount.Direction = ParameterDirection.Input;
            pWaterAmount.Value = insertTransectionBillDTO.waterAmount;
            sql.Parameters.Add(pWaterAmount);

            SqlParameter pLampUnit = new SqlParameter(@"pLampUnit", SqlDbType.Int);
            pLampUnit.Direction = ParameterDirection.Input;
            pLampUnit.Value = insertTransectionBillDTO.lampUnit;
            sql.Parameters.Add(pLampUnit);

            SqlParameter pElectricEquipUnit = new SqlParameter(@"pElectricEquipUnit", SqlDbType.Int);
            pElectricEquipUnit.Direction = ParameterDirection.Input;
            pElectricEquipUnit.Value = insertTransectionBillDTO.electricEquipUnit;
            sql.Parameters.Add(pElectricEquipUnit);

            SqlParameter pElectricNightMarketAmount = new SqlParameter(@"pElectricNightMarketAmount", SqlDbType.Decimal);
            pElectricNightMarketAmount.Direction = ParameterDirection.Input;
            pElectricNightMarketAmount.Value = insertTransectionBillDTO.electricNightMarketAmount;
            sql.Parameters.Add(pElectricNightMarketAmount);

            SqlParameter pTotalAmount = new SqlParameter(@"pTotalAmount", SqlDbType.Decimal);
            pTotalAmount.Direction = ParameterDirection.Input;
            pTotalAmount.Value = insertTransectionBillDTO.totalAmount;
            sql.Parameters.Add(pTotalAmount);

            SqlParameter pDiscountPercent = new SqlParameter(@"pDiscountPercent", SqlDbType.Int);
            pDiscountPercent.Direction = ParameterDirection.Input;
            pDiscountPercent.Value = insertTransectionBillDTO.discountPercent;
            sql.Parameters.Add(pDiscountPercent);

            SqlParameter pDiscountAmount = new SqlParameter(@"pDiscountAmount", SqlDbType.Decimal);
            pDiscountAmount.Direction = ParameterDirection.Input;
            pDiscountAmount.Value = insertTransectionBillDTO.discountAmount;
            sql.Parameters.Add(pDiscountAmount);

            SqlParameter pNetAmount = new SqlParameter(@"pNetAmount", SqlDbType.Decimal);
            pNetAmount.Direction = ParameterDirection.Input;
            pNetAmount.Value = insertTransectionBillDTO.netAmount;
            sql.Parameters.Add(pNetAmount);

            SqlParameter pPayDate = new SqlParameter(@"pPayDate", SqlDbType.VarChar);
            pPayDate.Direction = ParameterDirection.Input;
            pPayDate.Value = insertTransectionBillDTO.endDate;
            sql.Parameters.Add(pPayDate);

            SqlParameter pCreateBy = new SqlParameter(@"pCreateBy", SqlDbType.Int);
            pCreateBy.Direction = ParameterDirection.Input;
            pCreateBy.Value = userID;
            sql.Parameters.Add(pCreateBy);

            table = sql.executeQueryWithReturnTable();

            _ReturnIdModel data = new _ReturnIdModel();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    data.loadData(row);
                }
            }

            return data;
        }

        public Pagination<SearchUserProfile> SearchUserProfile(SearchUserProfileDTO searchUserProfileDTO)
        {
            DataTable table = new DataTable();

            SQLCustomExecute sql = new SQLCustomExecute("exec get_search_all_user_profile_page " +
                "@pUserProfileID, " +
                "@pUserName, " +
                "@pFirstName, " +
                "@pLastName, " +
                "@pMobile, " +
                "@pPosition, " +
                "@pPage, " +
                "@pPerPage, " +
                "@pSortField, " +
                "@pSortType");

            SqlParameter pUserProfileID = new SqlParameter(@"pUserProfileID", SqlDbType.Int);
            pUserProfileID.Direction = ParameterDirection.Input;
            pUserProfileID.Value = searchUserProfileDTO.userProfileID;
            sql.Parameters.Add(pUserProfileID);

            SqlParameter pUserName = new SqlParameter(@"pUserName", SqlDbType.VarChar, 255);
            pUserName.Direction = ParameterDirection.Input;
            pUserName.Value = searchUserProfileDTO.userName;
            sql.Parameters.Add(pUserName);

            SqlParameter pFirstName = new SqlParameter(@"pFirstName", SqlDbType.VarChar, 255);
            pFirstName.Direction = ParameterDirection.Input;
            pFirstName.Value = searchUserProfileDTO.firstName;
            sql.Parameters.Add(pFirstName);

            SqlParameter pLastName = new SqlParameter(@"pLastName", SqlDbType.VarChar, 255);
            pLastName.Direction = ParameterDirection.Input;
            pLastName.Value = searchUserProfileDTO.lastName;
            sql.Parameters.Add(pLastName);

            SqlParameter pMobile = new SqlParameter(@"pMobile", SqlDbType.VarChar, 255);
            pMobile.Direction = ParameterDirection.Input;
            pMobile.Value = searchUserProfileDTO.mobile;
            sql.Parameters.Add(pMobile);

            SqlParameter pPosition = new SqlParameter(@"pPosition", SqlDbType.VarChar, 255);
            pPosition.Direction = ParameterDirection.Input;
            pPosition.Value = searchUserProfileDTO.position;
            sql.Parameters.Add(pPosition);

            SqlParameter paramPage = new SqlParameter(@"pPage", SqlDbType.Int);
            paramPage.Direction = ParameterDirection.Input;
            paramPage.Value = searchUserProfileDTO.pageInt;
            sql.Parameters.Add(paramPage);

            SqlParameter paramPerPage = new SqlParameter(@"pPerPage", SqlDbType.Int);
            paramPerPage.Direction = ParameterDirection.Input;
            paramPerPage.Value = searchUserProfileDTO.perPage;
            sql.Parameters.Add(paramPerPage);

            SqlParameter pSortField = new SqlParameter(@"pSortField", SqlDbType.Int);
            pSortField.Direction = ParameterDirection.Input;
            pSortField.Value = searchUserProfileDTO.sortField;
            sql.Parameters.Add(pSortField);

            SqlParameter pSortType = new SqlParameter(@"pSortType", SqlDbType.VarChar, 1);
            pSortType.Direction = ParameterDirection.Input;
            pSortType.Value = searchUserProfileDTO.sortType;
            sql.Parameters.Add(pSortType);

            table = sql.executeQueryWithReturnTable();

            Pagination<SearchUserProfile> pagination = new Pagination<SearchUserProfile>();


            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    SearchUserProfile data = new SearchUserProfile();
                    data.loadData(row);
                    pagination.data.Add(data);
                }
            }

            int total = GetTotalSearchUserProfile(searchUserProfileDTO);

            pagination.SetPagination(total, searchUserProfileDTO.perPage, searchUserProfileDTO.pageInt);

            return pagination;
        }

        public int GetTotalSearchUserProfile(SearchUserProfileDTO searchUserProfileDTO)
        {
            int total = 0;

            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec get_search_all_user_profile_total " +
                "@pUserProfileID, " +
                "@pUserName, " +
                "@pFirstName, " +
                "@pLastName, " +
                "@pMobile, " +
                "@pPosition ");

            SqlParameter pUserProfileID = new SqlParameter(@"pUserProfileID", SqlDbType.Int);
            pUserProfileID.Direction = ParameterDirection.Input;
            pUserProfileID.Value = searchUserProfileDTO.userProfileID;
            sql.Parameters.Add(pUserProfileID);

            SqlParameter pUserName = new SqlParameter(@"pUserName", SqlDbType.VarChar, 255);
            pUserName.Direction = ParameterDirection.Input;
            pUserName.Value = searchUserProfileDTO.userName;
            sql.Parameters.Add(pUserName);

            SqlParameter pFirstName = new SqlParameter(@"pFirstName", SqlDbType.VarChar, 255);
            pFirstName.Direction = ParameterDirection.Input;
            pFirstName.Value = searchUserProfileDTO.firstName;
            sql.Parameters.Add(pFirstName);

            SqlParameter pLastName = new SqlParameter(@"pLastName", SqlDbType.VarChar, 255);
            pLastName.Direction = ParameterDirection.Input;
            pLastName.Value = searchUserProfileDTO.lastName;
            sql.Parameters.Add(pLastName);

            SqlParameter pMobile = new SqlParameter(@"pMobile", SqlDbType.VarChar, 255);
            pMobile.Direction = ParameterDirection.Input;
            pMobile.Value = searchUserProfileDTO.mobile;
            sql.Parameters.Add(pMobile);

            SqlParameter pPosition = new SqlParameter(@"pPosition", SqlDbType.VarChar, 255);
            pPosition.Direction = ParameterDirection.Input;
            pPosition.Value = searchUserProfileDTO.position;
            sql.Parameters.Add(pPosition);

            table = sql.executeQueryWithReturnTable();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    DataRow dr = table.Rows[0];
                    total = int.Parse(dr["total"].ToString());
                }
            }

            return total;
        }

        public Pagination<SearchUnitStand> SearchUnitStand(SearchUnitstandDTO searchUnitstandDTO)
        {
            DataTable table = new DataTable();

            SQLCustomExecute sql = new SQLCustomExecute("exec get_search_all_unit_stand_page " +
                "@pUnitCode, " +
                "@pUnitName, " +
                "@pPage, " +
                "@pPerPage, " +
                "@pSortField, " +
                "@pSortType");

            SqlParameter pUnitCode = new SqlParameter(@"pUnitCode", SqlDbType.VarChar, 255);
            pUnitCode.Direction = ParameterDirection.Input;
            pUnitCode.Value = searchUnitstandDTO.unitCode;
            sql.Parameters.Add(pUnitCode);

            SqlParameter pUnitName = new SqlParameter(@"pUnitName", SqlDbType.VarChar, 255);
            pUnitName.Direction = ParameterDirection.Input;
            pUnitName.Value = searchUnitstandDTO.unitName;
            sql.Parameters.Add(pUnitName);

            SqlParameter pPage = new SqlParameter(@"pPage", SqlDbType.Int);
            pPage.Direction = ParameterDirection.Input;
            pPage.Value = searchUnitstandDTO.pageInt;
            sql.Parameters.Add(pPage);

            SqlParameter pPerPage = new SqlParameter(@"pPerPage", SqlDbType.Int);
            pPerPage.Direction = ParameterDirection.Input;
            pPerPage.Value = searchUnitstandDTO.perPage;
            sql.Parameters.Add(pPerPage);

            SqlParameter pSortField = new SqlParameter(@"pSortField", SqlDbType.Int);
            pSortField.Direction = ParameterDirection.Input;
            pSortField.Value = searchUnitstandDTO.sortField;
            sql.Parameters.Add(pSortField);

            SqlParameter pSortType = new SqlParameter(@"pSortType", SqlDbType.VarChar, 1);
            pSortType.Direction = ParameterDirection.Input;
            pSortType.Value = searchUnitstandDTO.sortType;
            sql.Parameters.Add(pSortType);

            table = sql.executeQueryWithReturnTable();

            Pagination<SearchUnitStand> pagination = new Pagination<SearchUnitStand>();


            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    SearchUnitStand data = new SearchUnitStand();
                    data.loadData(row);
                    pagination.data.Add(data);
                }
            }

            int total = GetTotalSearchUnitStand(searchUnitstandDTO);

            pagination.SetPagination(total, searchUnitstandDTO.perPage, searchUnitstandDTO.pageInt);

            return pagination;
        }

        public int GetTotalSearchUnitStand(SearchUnitstandDTO searchUnitstandDTO)
        {
            int total = 0;

            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec get_search_all_unit_stand_total " +
                "@pUnitCode, " +
                "@pUnitName ");

            SqlParameter pUnitCode = new SqlParameter(@"pUnitCode", SqlDbType.VarChar, 255);
            pUnitCode.Direction = ParameterDirection.Input;
            pUnitCode.Value = searchUnitstandDTO.unitCode;
            sql.Parameters.Add(pUnitCode);

            SqlParameter pUnitName = new SqlParameter(@"pUnitName", SqlDbType.VarChar, 255);
            pUnitName.Direction = ParameterDirection.Input;
            pUnitName.Value = searchUnitstandDTO.unitName;
            sql.Parameters.Add(pUnitName);

            table = sql.executeQueryWithReturnTable();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    DataRow dr = table.Rows[0];
                    total = int.Parse(dr["total"].ToString());
                }
            }

            return total;
        }

        public _ReturnIdModel InsertUploadFile(UploadFileDTO uploadFileDTO, int userID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec insert_file_detail " +
                "@pActionID," +
                "@pActionName," +
                "@pFileExtension," +
                "@pName," +
                "@pUrl," +
                "@pCreateBy");

            SqlParameter pActionID = new SqlParameter(@"pActionID", SqlDbType.Int);
            pActionID.Direction = ParameterDirection.Input;
            pActionID.Value = uploadFileDTO.actionID;
            sql.Parameters.Add(pActionID);

            SqlParameter pActionName = new SqlParameter(@"pActionName", SqlDbType.VarChar);
            pActionName.Direction = ParameterDirection.Input;
            pActionName.Value = uploadFileDTO.actionName;
            sql.Parameters.Add(pActionName);

            SqlParameter pFileExtension = new SqlParameter(@"pFileExtension", SqlDbType.VarChar);
            pFileExtension.Direction = ParameterDirection.Input;
            pFileExtension.Value = uploadFileDTO.fileExtension;
            sql.Parameters.Add(pFileExtension);

            SqlParameter pName = new SqlParameter(@"pName", SqlDbType.VarChar);
            pName.Direction = ParameterDirection.Input;
            pName.Value = uploadFileDTO.name;
            sql.Parameters.Add(pName);

            SqlParameter pUrl = new SqlParameter(@"pUrl", SqlDbType.VarChar);
            pUrl.Direction = ParameterDirection.Input;
            pUrl.Value = uploadFileDTO.url;
            sql.Parameters.Add(pUrl);

            SqlParameter pCreateBy = new SqlParameter(@"pCreateBy", SqlDbType.Int);
            pCreateBy.Direction = ParameterDirection.Input;
            pCreateBy.Value = userID;
            sql.Parameters.Add(pCreateBy);

            table = sql.executeQueryWithReturnTable();

            _ReturnIdModel data = new _ReturnIdModel();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    data.loadData(row);
                }
            }

            return data;
        }

        public _ReturnIdModel DeleteUploadFile(UploadFileDTO uploadFileDTO, int userID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec delete_file_detail " +
                "@pFileDetailID," +
                "@pUpdateBy ");

            SqlParameter pFileDetailID = new SqlParameter(@"pFileDetailID", SqlDbType.Int);
            pFileDetailID.Direction = ParameterDirection.Input;
            pFileDetailID.Value = uploadFileDTO.fileDetailID;
            sql.Parameters.Add(pFileDetailID);

            SqlParameter pUserID = new SqlParameter(@"pUpdateBy", SqlDbType.Int);
            pUserID.Direction = ParameterDirection.Input;
            pUserID.Value = userID;
            sql.Parameters.Add(pUserID);

            table = sql.executeQueryWithReturnTable();

            _ReturnIdModel data = new _ReturnIdModel();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    data.loadData(row);
                }
            }

            return data;
        }

        public string[] GetImageUrl(string FileCode, string ActionName)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec get_image_url " +
                "@pFileCode, " +
                "@pActionName");

            SqlParameter pFileCode = new SqlParameter(@"pFileCode", SqlDbType.VarChar);
            pFileCode.Direction = ParameterDirection.Input;
            pFileCode.Value = FileCode;
            sql.Parameters.Add(pFileCode);

            SqlParameter pActionName = new SqlParameter(@"pActionName", SqlDbType.VarChar);
            pActionName.Direction = ParameterDirection.Input;
            pActionName.Value = ActionName;
            sql.Parameters.Add(pActionName);

            table = sql.executeQueryWithReturnTable();
            
            List<string> list = new List<string>();
            foreach (DataRow dr in table.Rows)
            {
                list.Add(dr["url"].ToString());
            }
            if (list.Count == 0)
            {
                list.Add("");
            }
            String[] strArry = list.ToArray();

            return strArry;
        }

        public UserProfileModel GetUserProfile(int userProfileID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec get_user_profile " +
                "@pUserProfileID");

            SqlParameter pUserProfileID = new SqlParameter(@"pUserProfileID", SqlDbType.Int);
            pUserProfileID.Direction = ParameterDirection.Input;
            pUserProfileID.Value = userProfileID;
            sql.Parameters.Add(pUserProfileID);

            table = sql.executeQueryWithReturnTable();

            UserProfileModel data = new UserProfileModel();
            
            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    data.loadData(row);
                }
            }

            data.imageUrl = GetImageUrl(data.fileCode, "user_profile");

            return data;
        }

        public List<MarketDetail> GetUserMarket(int userProfileID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec get_user_market " +
                "@pUserProfileID");

            SqlParameter pUserProfileID = new SqlParameter(@"pUserProfileID", SqlDbType.Int);
            pUserProfileID.Direction = ParameterDirection.Input;
            pUserProfileID.Value = userProfileID;
            sql.Parameters.Add(pUserProfileID);

            table = sql.executeQueryWithReturnTable();

            List<MarketDetail> list = new List<MarketDetail>();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    MarketDetail data = new MarketDetail();
                    data.loadData(row);
                    list.Add(data);
                }
            }
            else
            {
                MarketDetail data = new MarketDetail();
                list.Add(data);
            }

            return list;
        }
        
        public Pagination<SearchHistoryPaidBillAdmin> SearchHistoryPaidBillAdmin(SearchHistoryAdminBillDTO searchHistoryAdminBillDTO)
        {
            DataTable table = new DataTable();

            SQLCustomExecute sql = new SQLCustomExecute("exec get_search_all_ncbill_admin_page " +
                "@pStartDate, " +
                "@pEndDate, " +
                "@pIsComplete, " +
                "@pPage, " +
                "@pPerPage, " +
                "@pSortField, " +
                "@pSortType");

            SqlParameter pStartDate = new SqlParameter(@"pStartDate", SqlDbType.VarChar, 255);
            pStartDate.Direction = ParameterDirection.Input;
            pStartDate.Value = searchHistoryAdminBillDTO.startDate;
            sql.Parameters.Add(pStartDate);

            SqlParameter pEndDate = new SqlParameter(@"pEndDate", SqlDbType.VarChar, 255);
            pEndDate.Direction = ParameterDirection.Input;
            pEndDate.Value = searchHistoryAdminBillDTO.endDate;
            sql.Parameters.Add(pEndDate);

            SqlParameter pIsComplete = new SqlParameter(@"pIsComplete", SqlDbType.Int);
            pIsComplete.Direction = ParameterDirection.Input;
            pIsComplete.Value = searchHistoryAdminBillDTO.isComplete;
            sql.Parameters.Add(pIsComplete);

            SqlParameter pPage = new SqlParameter(@"pPage", SqlDbType.Int);
            pPage.Direction = ParameterDirection.Input;
            pPage.Value = searchHistoryAdminBillDTO.pageInt;
            sql.Parameters.Add(pPage);

            SqlParameter pPerPage = new SqlParameter(@"pPerPage", SqlDbType.Int);
            pPerPage.Direction = ParameterDirection.Input;
            pPerPage.Value = searchHistoryAdminBillDTO.perPage;
            sql.Parameters.Add(pPerPage);

            SqlParameter pSortField = new SqlParameter(@"pSortField", SqlDbType.Int);
            pSortField.Direction = ParameterDirection.Input;
            pSortField.Value = searchHistoryAdminBillDTO.sortField;
            sql.Parameters.Add(pSortField);

            SqlParameter pSortType = new SqlParameter(@"pSortType", SqlDbType.VarChar, 1);
            pSortType.Direction = ParameterDirection.Input;
            pSortType.Value = searchHistoryAdminBillDTO.sortType;
            sql.Parameters.Add(pSortType);

            table = sql.executeQueryWithReturnTable();

            Pagination<SearchHistoryPaidBillAdmin> pagination = new Pagination<SearchHistoryPaidBillAdmin>();


            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    SearchHistoryPaidBillAdmin data = new SearchHistoryPaidBillAdmin();
                    data.loadData(row);
                    pagination.data.Add(data);
                }
            }

            int total = GetTotalSearchHistoryPaidBillAdmin(searchHistoryAdminBillDTO);

            pagination.SetPagination(total, searchHistoryAdminBillDTO.perPage, searchHistoryAdminBillDTO.pageInt);

            return pagination;
        }

        public int GetTotalSearchHistoryPaidBillAdmin(SearchHistoryAdminBillDTO searchHistoryAdminBillDTO)
        {
            int total = 0;

            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec get_search_all_ncbill_admin_total " +
                "@pStartDate, " +
                "@pEndDate, " +
                "@pIsComplete ");

            SqlParameter pStartDate = new SqlParameter(@"pStartDate", SqlDbType.VarChar, 255);
            pStartDate.Direction = ParameterDirection.Input;
            pStartDate.Value = searchHistoryAdminBillDTO.startDate;
            sql.Parameters.Add(pStartDate);

            SqlParameter pEndDate = new SqlParameter(@"pEndDate", SqlDbType.VarChar, 255);
            pEndDate.Direction = ParameterDirection.Input;
            pEndDate.Value = searchHistoryAdminBillDTO.endDate;
            sql.Parameters.Add(pEndDate);

            SqlParameter pIsComplete = new SqlParameter(@"pIsComplete", SqlDbType.Int);
            pIsComplete.Direction = ParameterDirection.Input;
            pIsComplete.Value = searchHistoryAdminBillDTO.isComplete;
            sql.Parameters.Add(pIsComplete);

            table = sql.executeQueryWithReturnTable();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    DataRow dr = table.Rows[0];
                    total = int.Parse(dr["total"].ToString());
                }
            }

            return total;
        }

        public Pagination<SearchHistoryPaidBillUser> SearchHistoryPaidBillUser(SearchHistoryUserBillDTO searchHistoryUserBillDTO)
        {
            DataTable table = new DataTable();

            SQLCustomExecute sql = new SQLCustomExecute("exec get_search_all_ncbill_user_page " +
                "@pUserID, " +
                "@pStartDate, " +
                "@pEndDate, " +
                "@pPage, " +
                "@pPerPage, " +
                "@pSortField, " +
                "@pSortType");

            SqlParameter pUserID = new SqlParameter(@"pUserID", SqlDbType.Int);
            pUserID.Direction = ParameterDirection.Input;
            pUserID.Value = searchHistoryUserBillDTO.userID;
            sql.Parameters.Add(pUserID);

            SqlParameter pStartDate = new SqlParameter(@"pStartDate", SqlDbType.VarChar, 255);
            pStartDate.Direction = ParameterDirection.Input;
            pStartDate.Value = searchHistoryUserBillDTO.startDate;
            sql.Parameters.Add(pStartDate);

            SqlParameter pEndDate = new SqlParameter(@"pEndDate", SqlDbType.VarChar, 255);
            pEndDate.Direction = ParameterDirection.Input;
            pEndDate.Value = searchHistoryUserBillDTO.endDate;
            sql.Parameters.Add(pEndDate);

            SqlParameter pPage = new SqlParameter(@"pPage", SqlDbType.Int);
            pPage.Direction = ParameterDirection.Input;
            pPage.Value = searchHistoryUserBillDTO.pageInt;
            sql.Parameters.Add(pPage);

            SqlParameter pPerPage = new SqlParameter(@"pPerPage", SqlDbType.Int);
            pPerPage.Direction = ParameterDirection.Input;
            pPerPage.Value = searchHistoryUserBillDTO.perPage;
            sql.Parameters.Add(pPerPage);

            SqlParameter pSortField = new SqlParameter(@"pSortField", SqlDbType.Int);
            pSortField.Direction = ParameterDirection.Input;
            pSortField.Value = searchHistoryUserBillDTO.sortField;
            sql.Parameters.Add(pSortField);

            SqlParameter pSortType = new SqlParameter(@"pSortType", SqlDbType.VarChar, 1);
            pSortType.Direction = ParameterDirection.Input;
            pSortType.Value = searchHistoryUserBillDTO.sortType;
            sql.Parameters.Add(pSortType);

            table = sql.executeQueryWithReturnTable();

            Pagination<SearchHistoryPaidBillUser> pagination = new Pagination<SearchHistoryPaidBillUser>();


            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    SearchHistoryPaidBillUser data = new SearchHistoryPaidBillUser();
                    data.loadData(row);
                    pagination.data.Add(data);
                }
            }

            int total = GetTotalSearchHistoryPaidBillUser(searchHistoryUserBillDTO);

            pagination.SetPagination(total, searchHistoryUserBillDTO.perPage, searchHistoryUserBillDTO.pageInt);

            return pagination;
        }

        public int GetTotalSearchHistoryPaidBillUser(SearchHistoryUserBillDTO searchHistoryUserBillDTO)
        {
            int total = 0;

            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec get_search_all_ncbill_user_total " +
                "@pUserID, " +
                "@pStartDate, " +
                "@pEndDate ");

            SqlParameter pUserID = new SqlParameter(@"pUserID", SqlDbType.Int);
            pUserID.Direction = ParameterDirection.Input;
            pUserID.Value = searchHistoryUserBillDTO.userID;
            sql.Parameters.Add(pUserID);

            SqlParameter pStartDate = new SqlParameter(@"pStartDate", SqlDbType.VarChar, 255);
            pStartDate.Direction = ParameterDirection.Input;
            pStartDate.Value = searchHistoryUserBillDTO.startDate;
            sql.Parameters.Add(pStartDate);

            SqlParameter pEndDate = new SqlParameter(@"pEndDate", SqlDbType.VarChar, 255);
            pEndDate.Direction = ParameterDirection.Input;
            pEndDate.Value = searchHistoryUserBillDTO.endDate;
            sql.Parameters.Add(pEndDate);

            table = sql.executeQueryWithReturnTable();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    DataRow dr = table.Rows[0];
                    total = int.Parse(dr["total"].ToString());
                }
            }

            return total;
        }

        public Pagination<SearchHistoryPaidBillAdmin> SearchOutStandingBillUser(SearchHistoryUserBillDTO searchHistoryUserBillDTO)
        {
            DataTable table = new DataTable();

            SQLCustomExecute sql = new SQLCustomExecute("exec get_search_all_outstanding_bill_user_page " +
                "@pUserID, " +
                "@pStartDate, " +
                "@pEndDate, " +
                "@pPage, " +
                "@pPerPage, " +
                "@pSortField, " +
                "@pSortType");

            SqlParameter pUserID = new SqlParameter(@"pUserID", SqlDbType.Int);
            pUserID.Direction = ParameterDirection.Input;
            pUserID.Value = searchHistoryUserBillDTO.userID;
            sql.Parameters.Add(pUserID);

            SqlParameter pStartDate = new SqlParameter(@"pStartDate", SqlDbType.VarChar, 255);
            pStartDate.Direction = ParameterDirection.Input;
            pStartDate.Value = searchHistoryUserBillDTO.startDate;
            sql.Parameters.Add(pStartDate);

            SqlParameter pEndDate = new SqlParameter(@"pEndDate", SqlDbType.VarChar, 255);
            pEndDate.Direction = ParameterDirection.Input;
            pEndDate.Value = searchHistoryUserBillDTO.endDate;
            sql.Parameters.Add(pEndDate);

            SqlParameter pPage = new SqlParameter(@"pPage", SqlDbType.Int);
            pPage.Direction = ParameterDirection.Input;
            pPage.Value = searchHistoryUserBillDTO.pageInt;
            sql.Parameters.Add(pPage);

            SqlParameter pPerPage = new SqlParameter(@"pPerPage", SqlDbType.Int);
            pPerPage.Direction = ParameterDirection.Input;
            pPerPage.Value = searchHistoryUserBillDTO.perPage;
            sql.Parameters.Add(pPerPage);

            SqlParameter pSortField = new SqlParameter(@"pSortField", SqlDbType.Int);
            pSortField.Direction = ParameterDirection.Input;
            pSortField.Value = searchHistoryUserBillDTO.sortField;
            sql.Parameters.Add(pSortField);

            SqlParameter pSortType = new SqlParameter(@"pSortType", SqlDbType.VarChar, 1);
            pSortType.Direction = ParameterDirection.Input;
            pSortType.Value = searchHistoryUserBillDTO.sortType;
            sql.Parameters.Add(pSortType);

            table = sql.executeQueryWithReturnTable();

            Pagination<SearchHistoryPaidBillAdmin> pagination = new Pagination<SearchHistoryPaidBillAdmin>();


            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    SearchHistoryPaidBillAdmin data = new SearchHistoryPaidBillAdmin();
                    data.loadData(row);
                    pagination.data.Add(data);
                }
            }

            int total = GetTotalSearchOutStandingBillUser(searchHistoryUserBillDTO);

            pagination.SetPagination(total, searchHistoryUserBillDTO.perPage, searchHistoryUserBillDTO.pageInt);

            return pagination;
        }

        public int GetTotalSearchOutStandingBillUser(SearchHistoryUserBillDTO searchHistoryUserBillDTO)
        {
            int total = 0;

            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec get_search_all_outstanding_bill_user_total " +
                "@pUserID, " +
                "@pStartDate, " +
                "@pEndDate ");

            SqlParameter pUserID = new SqlParameter(@"pUserID", SqlDbType.Int);
            pUserID.Direction = ParameterDirection.Input;
            pUserID.Value = searchHistoryUserBillDTO.userID;
            sql.Parameters.Add(pUserID);

            SqlParameter pStartDate = new SqlParameter(@"pStartDate", SqlDbType.VarChar, 255);
            pStartDate.Direction = ParameterDirection.Input;
            pStartDate.Value = searchHistoryUserBillDTO.startDate;
            sql.Parameters.Add(pStartDate);

            SqlParameter pEndDate = new SqlParameter(@"pEndDate", SqlDbType.VarChar, 255);
            pEndDate.Direction = ParameterDirection.Input;
            pEndDate.Value = searchHistoryUserBillDTO.endDate;
            sql.Parameters.Add(pEndDate);

            table = sql.executeQueryWithReturnTable();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    DataRow dr = table.Rows[0];
                    total = int.Parse(dr["total"].ToString());
                }
            }

            return total;
        }

        public UserProfileModel GetUserRental(int userProfileID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec get_user_profile " +
                "@pUserProfileID");

            SqlParameter pUserProfileID = new SqlParameter(@"pUserProfileID", SqlDbType.Int);
            pUserProfileID.Direction = ParameterDirection.Input;
            pUserProfileID.Value = userProfileID;
            sql.Parameters.Add(pUserProfileID);

            table = sql.executeQueryWithReturnTable();

            UserProfileModel data = new UserProfileModel();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    data.loadData(row);
                }
            }

            data.imageUrl = GetImageUrl("", "user_profile");

            return data;
        }

        public DataTable CheckDuplicatePlaceSub(MasterPlaceSubDTO masterPlaceSubDTO)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("");

            sql = new SQLCustomExecute("exec check_duplicate_place_sub @pMasterID,@pTableName,@pNameEN, @pNameTH");

            SqlParameter pMasterID = new SqlParameter(@"pMasterID", SqlDbType.Int);
            pMasterID.Direction = ParameterDirection.Input;
            pMasterID.Value = masterPlaceSubDTO.placeSubID;
            sql.Parameters.Add(pMasterID);

            SqlParameter pNameEN = new SqlParameter(@"pNameEN", SqlDbType.VarChar);
            pNameEN.Direction = ParameterDirection.Input;
            pNameEN.Value = masterPlaceSubDTO.nameEN;
            sql.Parameters.Add(pNameEN);

            SqlParameter pNameTH = new SqlParameter(@"pNameTH", SqlDbType.VarChar);
            pNameTH.Direction = ParameterDirection.Input;
            pNameTH.Value = masterPlaceSubDTO.nameTH;
            sql.Parameters.Add(pNameTH);

            table = sql.executeQueryWithReturnTable();

            return table;
        }

        public _ReturnIdModel InsertMasterPlaceSubData(MasterPlaceSubDTO masterPlaceSubDTO, int userID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec insert_master_place_sub " +
                "@pPlaceID," +
                "@pNameEN," +
                "@pNameTH," +
                "@pAmountRentDay," +
                "@pAmountRentMonth," +
                "@pAmountRentSpecial," +
                "@pSpecialExpireDate," +
                "@pAmountWater," +
                "@pAmountElectricity," +
                "@pUserID ");

            SqlParameter pPlaceID = new SqlParameter(@"pPlaceID", SqlDbType.Int);
            pPlaceID.Direction = ParameterDirection.Input;
            pPlaceID.Value = masterPlaceSubDTO.placeSubID;
            sql.Parameters.Add(pPlaceID);

            SqlParameter pNameEN = new SqlParameter(@"pNameEN", SqlDbType.VarChar);
            pNameEN.Direction = ParameterDirection.Input;
            pNameEN.Value = masterPlaceSubDTO.nameEN;
            sql.Parameters.Add(pNameEN);

            SqlParameter pNameTH = new SqlParameter(@"pNameTH", SqlDbType.VarChar);
            pNameTH.Direction = ParameterDirection.Input;
            pNameTH.Value = masterPlaceSubDTO.nameTH;
            sql.Parameters.Add(pNameTH);

            SqlParameter pAmountRentDay = new SqlParameter(@"pAmountRentDay", SqlDbType.Decimal);
            pAmountRentDay.Direction = ParameterDirection.Input;
            pAmountRentDay.Value = masterPlaceSubDTO.amountRentDay;
            sql.Parameters.Add(pAmountRentDay);

            SqlParameter pAmountRentMonth = new SqlParameter(@"pAmountRentMonth", SqlDbType.Decimal);
            pAmountRentMonth.Direction = ParameterDirection.Input;
            pAmountRentMonth.Value = masterPlaceSubDTO.amountRentMonth;
            sql.Parameters.Add(pAmountRentMonth);

            SqlParameter pAmountRentSpecial = new SqlParameter(@"pAmountRentSpecial", SqlDbType.Decimal);
            pAmountRentSpecial.Direction = ParameterDirection.Input;
            pAmountRentSpecial.Value = masterPlaceSubDTO.amountRentSpecial;
            sql.Parameters.Add(pAmountRentSpecial);

            SqlParameter pSpecialExpireDate = new SqlParameter(@"pSpecialExpireDate", SqlDbType.VarChar);
            pSpecialExpireDate.Direction = ParameterDirection.Input;
            pSpecialExpireDate.Value = masterPlaceSubDTO.specialExpireDate;
            sql.Parameters.Add(pNameEN);

            SqlParameter pAmountWater = new SqlParameter(@"pAmountWater", SqlDbType.Decimal);
            pAmountWater.Direction = ParameterDirection.Input;
            pAmountWater.Value = masterPlaceSubDTO.amountRentSpecial;
            sql.Parameters.Add(pAmountWater);

            SqlParameter pAmountElectricity = new SqlParameter(@"pAmountElectricity", SqlDbType.Decimal);
            pAmountElectricity.Direction = ParameterDirection.Input;
            pAmountElectricity.Value = masterPlaceSubDTO.amountElectricity;
            sql.Parameters.Add(pAmountElectricity);

            SqlParameter pUserID = new SqlParameter(@"pUserID", SqlDbType.Int);
            pUserID.Direction = ParameterDirection.Input;
            pUserID.Value = userID;
            sql.Parameters.Add(pUserID);

            table = sql.executeQueryWithReturnTable();

            _ReturnIdModel data = new _ReturnIdModel();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    data.loadData(row);
                }
            }

            return data;
        }

        public DataTable CheckDupicateZone(SaveZoneDTO saveZoneDTO, int pUserID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec check_dupicate_zone " +
                "@pZoneID, " +
                "@pName ");

            SqlParameter pZoneID = new SqlParameter(@"pZoneID", SqlDbType.Int);
            pZoneID.Direction = ParameterDirection.Input;
            pZoneID.Value = saveZoneDTO.zoneID;
            sql.Parameters.Add(pZoneID);

            SqlParameter pName = new SqlParameter(@"pName", SqlDbType.VarChar, 150);
            pName.Direction = ParameterDirection.Input;
            pName.Value = saveZoneDTO.name;
            sql.Parameters.Add(pName);

            table = sql.executeQueryWithReturnTable();

            return table;
        }

        public _ReturnIdModel InsertZone(SaveZoneDTO saveZoneDTO, int userID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec insert_zone " +
                "@pName," +
                "@pCreateBy");
            
            SqlParameter pName = new SqlParameter(@"pName", SqlDbType.VarChar);
            pName.Direction = ParameterDirection.Input;
            pName.Value = saveZoneDTO.name;
            sql.Parameters.Add(pName);

            SqlParameter pCreateBy = new SqlParameter(@"pCreateBy", SqlDbType.Int);
            pCreateBy.Direction = ParameterDirection.Input;
            pCreateBy.Value = userID;
            sql.Parameters.Add(pCreateBy);

            table = sql.executeQueryWithReturnTable();

            _ReturnIdModel data = new _ReturnIdModel();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    data.loadData(row);
                }
            }

            return data;
        }

        public _ReturnIdModel UpdateZone(SaveZoneDTO saveZoneDTO, int userID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec update_zone " +
                "@pZoneID," +
                "@pName," +
                "@pUpdateBy ");

            SqlParameter pZoneID = new SqlParameter(@"pZoneID", SqlDbType.Int);
            pZoneID.Direction = ParameterDirection.Input;
            pZoneID.Value = saveZoneDTO.zoneID;
            sql.Parameters.Add(pZoneID);

            SqlParameter pName = new SqlParameter(@"pName", SqlDbType.VarChar);
            pName.Direction = ParameterDirection.Input;
            pName.Value = saveZoneDTO.name;
            sql.Parameters.Add(pName);

            SqlParameter pUserID = new SqlParameter(@"pUpdateBy", SqlDbType.Int);
            pUserID.Direction = ParameterDirection.Input;
            pUserID.Value = userID;
            sql.Parameters.Add(pUserID);

            table = sql.executeQueryWithReturnTable();

            _ReturnIdModel data = new _ReturnIdModel();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    data.loadData(row);
                }
            }

            return data;
        }

        public _ReturnIdModel DeleteZone(SaveZoneDTO saveZoneDTO, int userID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec delete_zone " +
                "@pZoneID," +
                "@pUpdateBy ");

            SqlParameter pZoneID = new SqlParameter(@"pZoneID", SqlDbType.Int);
            pZoneID.Direction = ParameterDirection.Input;
            pZoneID.Value = saveZoneDTO.zoneID;
            sql.Parameters.Add(pZoneID);

            SqlParameter pUserID = new SqlParameter(@"pUpdateBy", SqlDbType.Int);
            pUserID.Direction = ParameterDirection.Input;
            pUserID.Value = userID;
            sql.Parameters.Add(pUserID);

            table = sql.executeQueryWithReturnTable();

            _ReturnIdModel data = new _ReturnIdModel();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    data.loadData(row);
                }
            }

            return data;
        }

        public DataTable CheckDupicateZoneSub(SaveZoneSubDTO saveZoneSubDTO, int pUserID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec check_dupicate_zone_sub " +
                "@pZoneSubID, " +
                "@pZoneID, " +
                "@pName ");

            SqlParameter pZoneSubID = new SqlParameter(@"pZoneSubID", SqlDbType.Int);
            pZoneSubID.Direction = ParameterDirection.Input;
            pZoneSubID.Value = saveZoneSubDTO.zoneSubID;
            sql.Parameters.Add(pZoneSubID);

            SqlParameter pZoneID = new SqlParameter(@"pZoneID", SqlDbType.Int);
            pZoneID.Direction = ParameterDirection.Input;
            pZoneID.Value = saveZoneSubDTO.zoneID;
            sql.Parameters.Add(pZoneID);

            SqlParameter pName = new SqlParameter(@"pName", SqlDbType.VarChar, 150);
            pName.Direction = ParameterDirection.Input;
            pName.Value = saveZoneSubDTO.name;
            sql.Parameters.Add(pName);

            table = sql.executeQueryWithReturnTable();

            return table;
        }

        public _ReturnIdModel InsertZoneSub(SaveZoneSubDTO saveZoneSubDTO, int userID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec insert_zone_sub " +
                "@pZoneID," +
                "@pName," +
                "@pCreateBy");

            SqlParameter pZoneID = new SqlParameter(@"pZoneID", SqlDbType.Int);
            pZoneID.Direction = ParameterDirection.Input;
            pZoneID.Value = saveZoneSubDTO.zoneID;
            sql.Parameters.Add(pZoneID);

            SqlParameter pName = new SqlParameter(@"pName", SqlDbType.VarChar);
            pName.Direction = ParameterDirection.Input;
            pName.Value = saveZoneSubDTO.name;
            sql.Parameters.Add(pName);

            SqlParameter pCreateBy = new SqlParameter(@"pCreateBy", SqlDbType.Int);
            pCreateBy.Direction = ParameterDirection.Input;
            pCreateBy.Value = userID;
            sql.Parameters.Add(pCreateBy);

            table = sql.executeQueryWithReturnTable();

            _ReturnIdModel data = new _ReturnIdModel();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    data.loadData(row);
                }
            }

            return data;
        }

        public _ReturnIdModel UpdateZoneSub(SaveZoneSubDTO saveZoneSubDTO, int userID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec update_zone_sub " +
                "@pZoneSubID," +
                "@pZoneID," +
                "@pName," +
                "@pUpdateBy ");

            SqlParameter pZoneSubID = new SqlParameter(@"pZoneSubID", SqlDbType.Int);
            pZoneSubID.Direction = ParameterDirection.Input;
            pZoneSubID.Value = saveZoneSubDTO.zoneSubID;
            sql.Parameters.Add(pZoneSubID);

            SqlParameter pZoneID = new SqlParameter(@"pZoneID", SqlDbType.Int);
            pZoneID.Direction = ParameterDirection.Input;
            pZoneID.Value = saveZoneSubDTO.zoneID;
            sql.Parameters.Add(pZoneID);

            SqlParameter pName = new SqlParameter(@"pName", SqlDbType.VarChar);
            pName.Direction = ParameterDirection.Input;
            pName.Value = saveZoneSubDTO.name;
            sql.Parameters.Add(pName);

            SqlParameter pUserID = new SqlParameter(@"pUpdateBy", SqlDbType.Int);
            pUserID.Direction = ParameterDirection.Input;
            pUserID.Value = userID;
            sql.Parameters.Add(pUserID);

            table = sql.executeQueryWithReturnTable();

            _ReturnIdModel data = new _ReturnIdModel();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    data.loadData(row);
                }
            }

            return data;
        }

        public _ReturnIdModel DeleteZoneSub(SaveZoneSubDTO saveZoneSubDTO, int userID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec delete_zone_sub " +
                "@pZoneSubID," +
                "@pUpdateBy ");

            SqlParameter pZoneSubID = new SqlParameter(@"pZoneSubID", SqlDbType.Int);
            pZoneSubID.Direction = ParameterDirection.Input;
            pZoneSubID.Value = saveZoneSubDTO.zoneSubID;
            sql.Parameters.Add(pZoneSubID);

            SqlParameter pUserID = new SqlParameter(@"pUpdateBy", SqlDbType.Int);
            pUserID.Direction = ParameterDirection.Input;
            pUserID.Value = userID;
            sql.Parameters.Add(pUserID);

            table = sql.executeQueryWithReturnTable();

            _ReturnIdModel data = new _ReturnIdModel();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    data.loadData(row);
                }
            }

            return data;
        }

        public DataTable CheckDupicateUnit(SaveUnitDTO saveUnitDTO, int pUserID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec check_dupicate_unit " +
                "@pUnitID, " +
                "@pZoneSubID, " +
                "@pZoneID, " +
                "@pName, " +
                "@pUnitCode, " +
                "@pRateID ");

            SqlParameter pUnitID = new SqlParameter(@"pUnitID", SqlDbType.Int);
            pUnitID.Direction = ParameterDirection.Input;
            pUnitID.Value = saveUnitDTO.unitID;
            sql.Parameters.Add(pUnitID);

            SqlParameter pZoneSubID = new SqlParameter(@"pZoneSubID", SqlDbType.Int);
            pZoneSubID.Direction = ParameterDirection.Input;
            pZoneSubID.Value = saveUnitDTO.zoneSubID;
            sql.Parameters.Add(pZoneSubID);

            SqlParameter pZoneID = new SqlParameter(@"pZoneID", SqlDbType.Int);
            pZoneID.Direction = ParameterDirection.Input;
            pZoneID.Value = saveUnitDTO.zoneID;
            sql.Parameters.Add(pZoneID);

            SqlParameter pName = new SqlParameter(@"pName", SqlDbType.VarChar, 250);
            pName.Direction = ParameterDirection.Input;
            pName.Value = saveUnitDTO.name;
            sql.Parameters.Add(pName);

            SqlParameter pUnitCode = new SqlParameter(@"pUnitCode", SqlDbType.VarChar, 10);
            pUnitCode.Direction = ParameterDirection.Input;
            pUnitCode.Value = saveUnitDTO.unitCode;
            sql.Parameters.Add(pUnitCode);

            SqlParameter pRateID = new SqlParameter(@"pRateID", SqlDbType.Int);
            pRateID.Direction = ParameterDirection.Input;
            pRateID.Value = saveUnitDTO.rateID;
            sql.Parameters.Add(pRateID);

            table = sql.executeQueryWithReturnTable();

            return table;
        }

        public _ReturnIdModel InsertUnit(SaveUnitDTO saveUnitDTO, int userID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec insert_unit " +
                "@pZoneID," +
                "@pZoneSubID," +
                "@pUnitCode," +
                "@pName," +
                "@pRateID," +
                "@pDnsMeter," +
                "@pCreateBy");

            SqlParameter pZoneID = new SqlParameter(@"pZoneID", SqlDbType.Int);
            pZoneID.Direction = ParameterDirection.Input;
            pZoneID.Value = saveUnitDTO.zoneID;
            sql.Parameters.Add(pZoneID);

            SqlParameter pZoneSubID = new SqlParameter(@"pZoneSubID", SqlDbType.Int);
            pZoneSubID.Direction = ParameterDirection.Input;
            pZoneSubID.Value = saveUnitDTO.zoneSubID;
            sql.Parameters.Add(pZoneSubID);

            SqlParameter pUnitCode = new SqlParameter(@"pUnitCode", SqlDbType.VarChar);
            pUnitCode.Direction = ParameterDirection.Input;
            pUnitCode.Value = saveUnitDTO.unitCode;
            sql.Parameters.Add(pUnitCode);

            SqlParameter pName = new SqlParameter(@"pName", SqlDbType.VarChar);
            pName.Direction = ParameterDirection.Input;
            pName.Value = saveUnitDTO.name;
            sql.Parameters.Add(pName);

            SqlParameter pRateID = new SqlParameter(@"pRateID", SqlDbType.Int);
            pRateID.Direction = ParameterDirection.Input;
            pRateID.Value = saveUnitDTO.rateID;
            sql.Parameters.Add(pRateID);

            SqlParameter pDnsMeter = new SqlParameter(@"pDnsMeter", SqlDbType.VarChar);
            pDnsMeter.Direction = ParameterDirection.Input;
            pDnsMeter.Value = saveUnitDTO.electricMeter;
            sql.Parameters.Add(pDnsMeter);

            SqlParameter pCreateBy = new SqlParameter(@"pCreateBy", SqlDbType.Int);
            pCreateBy.Direction = ParameterDirection.Input;
            pCreateBy.Value = userID;
            sql.Parameters.Add(pCreateBy);

            table = sql.executeQueryWithReturnTable();

            _ReturnIdModel data = new _ReturnIdModel();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    data.loadData(row);
                }
            }

            return data;
        }

        public _ReturnIdModel UpdateUnit(SaveUnitDTO saveUnitDTO, int userID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec update_unit " +
                "@pUnitID," +
                "@pZoneID," +
                "@pZoneSubID," +
                "@pUnitCode," +
                "@pName," +
                "@pRateID," +
                "@pDnsMeter," +
                "@pUpdateBy ");

            SqlParameter pUnitID = new SqlParameter(@"pUnitID", SqlDbType.Int);
            pUnitID.Direction = ParameterDirection.Input;
            pUnitID.Value = saveUnitDTO.unitID;
            sql.Parameters.Add(pUnitID);

            SqlParameter pZoneID = new SqlParameter(@"pZoneID", SqlDbType.Int);
            pZoneID.Direction = ParameterDirection.Input;
            pZoneID.Value = saveUnitDTO.zoneID;
            sql.Parameters.Add(pZoneID);

            SqlParameter pZoneSubID = new SqlParameter(@"pZoneSubID", SqlDbType.Int);
            pZoneSubID.Direction = ParameterDirection.Input;
            pZoneSubID.Value = saveUnitDTO.zoneSubID;
            sql.Parameters.Add(pZoneSubID);

            SqlParameter pUnitCode = new SqlParameter(@"pUnitCode", SqlDbType.VarChar);
            pUnitCode.Direction = ParameterDirection.Input;
            pUnitCode.Value = saveUnitDTO.unitCode;
            sql.Parameters.Add(pUnitCode);

            SqlParameter pName = new SqlParameter(@"pName", SqlDbType.VarChar);
            pName.Direction = ParameterDirection.Input;
            pName.Value = saveUnitDTO.name;
            sql.Parameters.Add(pName);

            SqlParameter pRateID = new SqlParameter(@"pRateID", SqlDbType.Int);
            pRateID.Direction = ParameterDirection.Input;
            pRateID.Value = saveUnitDTO.rateID;
            sql.Parameters.Add(pRateID);

            SqlParameter pDnsMeter = new SqlParameter(@"pDnsMeter", SqlDbType.VarChar);
            pDnsMeter.Direction = ParameterDirection.Input;
            pDnsMeter.Value = saveUnitDTO.electricMeter;
            sql.Parameters.Add(pDnsMeter);

            SqlParameter pUserID = new SqlParameter(@"pUpdateBy", SqlDbType.Int);
            pUserID.Direction = ParameterDirection.Input;
            pUserID.Value = userID;
            sql.Parameters.Add(pUserID);

            table = sql.executeQueryWithReturnTable();

            _ReturnIdModel data = new _ReturnIdModel();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    data.loadData(row);
                }
            }

            return data;
        }

        public _ReturnIdModel DeleteUnit(SaveUnitDTO saveUnitDTO, int userID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec delete_unit " +
                "@pUnitID," +
                "@pUpdateBy ");

            SqlParameter pUnitID = new SqlParameter(@"pUnitID", SqlDbType.Int);
            pUnitID.Direction = ParameterDirection.Input;
            pUnitID.Value = saveUnitDTO.unitID;
            sql.Parameters.Add(pUnitID);

            SqlParameter pUserID = new SqlParameter(@"pUpdateBy", SqlDbType.Int);
            pUserID.Direction = ParameterDirection.Input;
            pUserID.Value = userID;
            sql.Parameters.Add(pUserID);

            table = sql.executeQueryWithReturnTable();

            _ReturnIdModel data = new _ReturnIdModel();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    data.loadData(row);
                }
            }

            return data;
        }
        
        public DataTable CheckDupicateRateAmount(SaveRateAmountDTO saveRateAmountDTO, int pUserID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec check_dupicate_rate_amount " +
                "@pRateID, " +
                "@pName," +
                "@pRentAmountDay, " +
                "@pRentAmountMonth, " +
                "@pElectricAmount, " +
                "@pWaterAmount, " +
                "@pLampAmountPerOne, " +
                "@pElectricEquipAmount ");

            SqlParameter pRateID = new SqlParameter(@"pRateID", SqlDbType.Int);
            pRateID.Direction = ParameterDirection.Input;
            pRateID.Value = saveRateAmountDTO.rateID;
            sql.Parameters.Add(pRateID);

            SqlParameter pName = new SqlParameter(@"pName", SqlDbType.VarChar);
            pName.Direction = ParameterDirection.Input;
            pName.Value = saveRateAmountDTO.name;
            sql.Parameters.Add(pName);

            SqlParameter pRentAmountDay = new SqlParameter(@"pRentAmountDay", SqlDbType.Decimal);
            pRentAmountDay.Direction = ParameterDirection.Input;
            pRentAmountDay.Value = saveRateAmountDTO.rentAmountDay;
            sql.Parameters.Add(pRentAmountDay);

            SqlParameter pRentAmountMonth = new SqlParameter(@"pRentAmountMonth", SqlDbType.Decimal);
            pRentAmountMonth.Direction = ParameterDirection.Input;
            pRentAmountMonth.Value = saveRateAmountDTO.rentAmountMonth;
            sql.Parameters.Add(pRentAmountMonth);

            SqlParameter pElectricAmount = new SqlParameter(@"pElectricAmount", SqlDbType.Decimal);
            pElectricAmount.Direction = ParameterDirection.Input;
            pElectricAmount.Value = saveRateAmountDTO.electricAmount;
            sql.Parameters.Add(pElectricAmount);

            SqlParameter pWaterAmount = new SqlParameter(@"pWaterAmount", SqlDbType.Decimal);
            pWaterAmount.Direction = ParameterDirection.Input;
            pWaterAmount.Value = saveRateAmountDTO.waterAmount;
            sql.Parameters.Add(pWaterAmount);

            SqlParameter pLampAmountPerOne = new SqlParameter(@"pLampAmountPerOne", SqlDbType.Decimal);
            pLampAmountPerOne.Direction = ParameterDirection.Input;
            pLampAmountPerOne.Value = saveRateAmountDTO.lampAmountPerOne;
            sql.Parameters.Add(pLampAmountPerOne);

            SqlParameter pElectricEquipAmount = new SqlParameter(@"pElectricEquipAmount", SqlDbType.Decimal);
            pElectricEquipAmount.Direction = ParameterDirection.Input;
            pElectricEquipAmount.Value = saveRateAmountDTO.electricEquipAmount;
            sql.Parameters.Add(pElectricEquipAmount);

            table = sql.executeQueryWithReturnTable();

            return table;
        }

        public _ReturnIdModel InsertRateAmount(SaveRateAmountDTO saveRateAmountDTO, int userID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec insert_rate_amount " +
                "@pName," +
                "@pRentAmountDay," +
                "@pRentAmountMonth," +
                "@pElectricAmount," +
                "@pWaterAmount," +
                "@pLampAmountPerOne," +
                "@pElectricEquipAmount," +
                "@pCreateBy");

            SqlParameter pName = new SqlParameter(@"pName", SqlDbType.VarChar);
            pName.Direction = ParameterDirection.Input;
            pName.Value = saveRateAmountDTO.name;
            sql.Parameters.Add(pName);

            SqlParameter pRentAmountDay = new SqlParameter(@"pRentAmountDay", SqlDbType.Decimal);
            pRentAmountDay.Direction = ParameterDirection.Input;
            pRentAmountDay.Value = saveRateAmountDTO.rentAmountDay;
            sql.Parameters.Add(pRentAmountDay);

            SqlParameter pRentAmountMonth = new SqlParameter(@"pRentAmountMonth", SqlDbType.Decimal);
            pRentAmountMonth.Direction = ParameterDirection.Input;
            pRentAmountMonth.Value = saveRateAmountDTO.rentAmountMonth;
            sql.Parameters.Add(pRentAmountMonth);

            SqlParameter pElectricAmount = new SqlParameter(@"pElectricAmount", SqlDbType.Decimal);
            pElectricAmount.Direction = ParameterDirection.Input;
            pElectricAmount.Value = saveRateAmountDTO.electricAmount;
            sql.Parameters.Add(pElectricAmount);

            SqlParameter pWaterAmount = new SqlParameter(@"pWaterAmount", SqlDbType.Decimal);
            pWaterAmount.Direction = ParameterDirection.Input;
            pWaterAmount.Value = saveRateAmountDTO.waterAmount;
            sql.Parameters.Add(pWaterAmount);

            SqlParameter pLampAmountPerOne = new SqlParameter(@"pLampAmountPerOne", SqlDbType.Decimal);
            pLampAmountPerOne.Direction = ParameterDirection.Input;
            pLampAmountPerOne.Value = saveRateAmountDTO.lampAmountPerOne;
            sql.Parameters.Add(pLampAmountPerOne);

            SqlParameter pElectricEquipAmount = new SqlParameter(@"pElectricEquipAmount", SqlDbType.Decimal);
            pElectricEquipAmount.Direction = ParameterDirection.Input;
            pElectricEquipAmount.Value = saveRateAmountDTO.electricEquipAmount;
            sql.Parameters.Add(pElectricEquipAmount);

            SqlParameter pCreateBy = new SqlParameter(@"pCreateBy", SqlDbType.Int);
            pCreateBy.Direction = ParameterDirection.Input;
            pCreateBy.Value = userID;
            sql.Parameters.Add(pCreateBy);

            table = sql.executeQueryWithReturnTable();

            _ReturnIdModel data = new _ReturnIdModel();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    data.loadData(row);
                }
            }

            return data;
        }

        public _ReturnIdModel UpdateRateAmount(SaveRateAmountDTO saveRateAmountDTO, int userID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec update_rate_amount " +
                "@pRateID," +
                "@pName," +
                "@pRentAmountDay," +
                "@pRentAmountMonth," +
                "@pElectricAmount," +
                "@pWaterAmount," +
                "@pLampAmountPerOne," +
                "@pElectricEquipAmount," +
                "@pUpdateBy ");

            SqlParameter pRateID = new SqlParameter(@"pRateID", SqlDbType.Int);
            pRateID.Direction = ParameterDirection.Input;
            pRateID.Value = saveRateAmountDTO.rateID;
            sql.Parameters.Add(pRateID);

            SqlParameter pName = new SqlParameter(@"pName", SqlDbType.VarChar);
            pName.Direction = ParameterDirection.Input;
            pName.Value = saveRateAmountDTO.name;
            sql.Parameters.Add(pName);

            SqlParameter pRentAmountDay = new SqlParameter(@"pRentAmountDay", SqlDbType.Decimal);
            pRentAmountDay.Direction = ParameterDirection.Input;
            pRentAmountDay.Value = saveRateAmountDTO.rentAmountDay;
            sql.Parameters.Add(pRentAmountDay);

            SqlParameter pRentAmountMonth = new SqlParameter(@"pRentAmountMonth", SqlDbType.Decimal);
            pRentAmountMonth.Direction = ParameterDirection.Input;
            pRentAmountMonth.Value = saveRateAmountDTO.rentAmountMonth;
            sql.Parameters.Add(pRentAmountMonth);

            SqlParameter pElectricAmount = new SqlParameter(@"pElectricAmount", SqlDbType.Decimal);
            pElectricAmount.Direction = ParameterDirection.Input;
            pElectricAmount.Value = saveRateAmountDTO.electricAmount;
            sql.Parameters.Add(pElectricAmount);

            SqlParameter pWaterAmount = new SqlParameter(@"pWaterAmount", SqlDbType.Decimal);
            pWaterAmount.Direction = ParameterDirection.Input;
            pWaterAmount.Value = saveRateAmountDTO.waterAmount;
            sql.Parameters.Add(pWaterAmount);

            SqlParameter pLampAmountPerOne = new SqlParameter(@"pLampAmountPerOne", SqlDbType.Decimal);
            pLampAmountPerOne.Direction = ParameterDirection.Input;
            pLampAmountPerOne.Value = saveRateAmountDTO.lampAmountPerOne;
            sql.Parameters.Add(pLampAmountPerOne);

            SqlParameter pElectricEquipAmount = new SqlParameter(@"pElectricEquipAmount", SqlDbType.Decimal);
            pElectricEquipAmount.Direction = ParameterDirection.Input;
            pElectricEquipAmount.Value = saveRateAmountDTO.electricEquipAmount;
            sql.Parameters.Add(pElectricEquipAmount);

            SqlParameter pUserID = new SqlParameter(@"pUpdateBy", SqlDbType.Int);
            pUserID.Direction = ParameterDirection.Input;
            pUserID.Value = userID;
            sql.Parameters.Add(pUserID);

            table = sql.executeQueryWithReturnTable();

            _ReturnIdModel data = new _ReturnIdModel();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    data.loadData(row);
                }
            }

            return data;
        }

        public _ReturnIdModel DeleteRateAmount(SaveRateAmountDTO saveRateAmountDTO, int userID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec delete_rate_amount " +
                "@pRateID," +
                "@pUpdateBy ");

            SqlParameter pRateID = new SqlParameter(@"pRateID", SqlDbType.Int);
            pRateID.Direction = ParameterDirection.Input;
            pRateID.Value = saveRateAmountDTO.rateID;
            sql.Parameters.Add(pRateID);

            SqlParameter pUserID = new SqlParameter(@"pUpdateBy", SqlDbType.Int);
            pUserID.Direction = ParameterDirection.Input;
            pUserID.Value = userID;
            sql.Parameters.Add(pUserID);

            table = sql.executeQueryWithReturnTable();

            _ReturnIdModel data = new _ReturnIdModel();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    data.loadData(row);
                }
            }

            return data;
        }

        public List<DropdownAllData> GetDropdownZone(string isAll)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec get_dropdown_zone " +
                "@pIsAll");

            SqlParameter pIsAll = new SqlParameter(@"pIsAll", SqlDbType.VarChar);
            pIsAll.Direction = ParameterDirection.Input;
            pIsAll.Value = isAll;
            sql.Parameters.Add(pIsAll);

            table = sql.executeQueryWithReturnTable();

            List<DropdownAllData> listData = new List<DropdownAllData>();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    DropdownAllData data = new DropdownAllData();
                    data.loadData(row);
                    listData.Add(data);
                }
            }

            return listData;
        }

        public List<DropdownAllData> GetDropdownZoneSub(int ZoneID, string isAll)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec get_dropdown_zone_sub " +
                 "@pZoneID, " +
                 "@pIsAll");

            SqlParameter pZoneID = new SqlParameter(@"pZoneID", SqlDbType.Int);
            pZoneID.Direction = ParameterDirection.Input;
            pZoneID.Value = ZoneID;
            sql.Parameters.Add(pZoneID);

            SqlParameter pIsAll = new SqlParameter(@"pIsAll", SqlDbType.VarChar);
            pIsAll.Direction = ParameterDirection.Input;
            pIsAll.Value = isAll;
            sql.Parameters.Add(pIsAll);

            table = sql.executeQueryWithReturnTable();

            List<DropdownAllData> listData = new List<DropdownAllData>();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    DropdownAllData data = new DropdownAllData();
                    data.loadData(row);
                    listData.Add(data);
                }
            }

            return listData;
        }

        public List<DropdownAllData> GetDropdownUnit(int ZoneID, int ZoneSubID, string isAll)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec get_dropdown_unit " +
                 "@pZoneID, " +
                 "@pZoneSubID, " +
                 "@pIsAll");

            SqlParameter pZoneID = new SqlParameter(@"pZoneID", SqlDbType.Int);
            pZoneID.Direction = ParameterDirection.Input;
            pZoneID.Value = ZoneID;
            sql.Parameters.Add(pZoneID);

            SqlParameter pZoneSubID = new SqlParameter(@"pZoneSubID", SqlDbType.Int);
            pZoneSubID.Direction = ParameterDirection.Input;
            pZoneSubID.Value = ZoneSubID;
            sql.Parameters.Add(pZoneSubID);

            SqlParameter pIsAll = new SqlParameter(@"pIsAll", SqlDbType.VarChar);
            pIsAll.Direction = ParameterDirection.Input;
            pIsAll.Value = isAll;
            sql.Parameters.Add(pIsAll);

            table = sql.executeQueryWithReturnTable();

            List<DropdownAllData> listData = new List<DropdownAllData>();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    DropdownAllData data = new DropdownAllData();
                    data.loadData(row);
                    listData.Add(data);
                }
            }

            return listData;
        }

        public List<DropdownAllData> GetDropdownRateAmount(string isAll)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec get_dropdown_rate_amount " +
                "@pIsAll");

            SqlParameter pIsAll = new SqlParameter(@"pIsAll", SqlDbType.VarChar);
            pIsAll.Direction = ParameterDirection.Input;
            pIsAll.Value = isAll;
            sql.Parameters.Add(pIsAll);

            table = sql.executeQueryWithReturnTable();

            List<DropdownAllData> listData = new List<DropdownAllData>();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    DropdownAllData data = new DropdownAllData();
                    data.loadData(row);
                    listData.Add(data);
                }
            }

            return listData;
        }

        public List<DropdownAllData> GetDropdownUnitStatus()
        {
            DataTable table = new DataTable();
            table.Columns.Add("id");
            table.Columns.Add("name");

            DataRow dr = table.NewRow();
            dr["id"] = 1;
            dr["name"] = "ว่าง";
            table.Rows.Add(dr);

            DataRow dr1 = table.NewRow();
            dr1["id"] = 2;
            dr1["name"] = "ไม่ว่าง";
            table.Rows.Add(dr1);

            List<DropdownAllData> listData = new List<DropdownAllData>();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    DropdownAllData data = new DropdownAllData();
                    data.loadData(row);
                    listData.Add(data);
                }
            }

            return listData;
        }
        
        public List<DropdownMonth> GetDropdownSixMonthAgo()
        {
            DataTable table = new DataTable();
            table.Columns.Add("id");
            table.Columns.Add("name");

            DateTime curdate = DateTime.Now;
            string[] newDateTime1 = curdate.AddMonths(0).ToString("yyyy-MM-dd").Split('-');
            string[] newDateTime2 = curdate.AddMonths(-1).ToString("yyyy-MM-dd").Split('-');
            string[] newDateTime3 = curdate.AddMonths(-2).ToString("yyyy-MM-dd").Split('-');
            string[] newDateTime4 = curdate.AddMonths(-3).ToString("yyyy-MM-dd").Split('-');
            string[] newDateTime5 = curdate.AddMonths(-4).ToString("yyyy-MM-dd").Split('-');
            string[] newDateTime6 = curdate.AddMonths(-5).ToString("yyyy-MM-dd").Split('-');

            DataRow dr = table.NewRow();
            dr["id"] = newDateTime1[0] + newDateTime1[1];
            dr["name"] = getFullMonthName(newDateTime1[1]) + " " + newDateTime1[0];
            table.Rows.Add(dr);

            DataRow dr1 = table.NewRow();
            dr1["id"] = newDateTime2[0] + newDateTime2[1];
            dr1["name"] = getFullMonthName(newDateTime2[1]) + " " + newDateTime2[0];
            table.Rows.Add(dr1);

            DataRow dr2 = table.NewRow();
            dr2["id"] = newDateTime3[0] + newDateTime3[1];
            dr2["name"] = getFullMonthName(newDateTime3[1]) + " " + newDateTime3[0];
            table.Rows.Add(dr2);

            DataRow dr3 = table.NewRow();
            dr3["id"] = newDateTime4[0] + newDateTime4[1];
            dr3["name"] = getFullMonthName(newDateTime4[1]) + " " + newDateTime4[0];
            table.Rows.Add(dr3);

            DataRow dr4 = table.NewRow();
            dr4["id"] = newDateTime5[0] + newDateTime5[1];
            dr4["name"] = getFullMonthName(newDateTime5[1]) + " " + newDateTime5[0];
            table.Rows.Add(dr4);

            DataRow dr5 = table.NewRow();
            dr5["id"] = newDateTime6[0] + newDateTime6[1];
            dr5["name"] = getFullMonthName(newDateTime6[1]) + " " + newDateTime6[0];
            table.Rows.Add(dr5);

            List<DropdownMonth> listData = new List<DropdownMonth>();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    DropdownMonth data = new DropdownMonth();
                    data.loadData(row);
                    listData.Add(data);
                }
            }

            return listData;
        }

        public string getFullMonthName(string monthNo)
        {
            string fullmonth = "";
            if (monthNo == "01")
            {
                fullmonth = "มกราคม";
            }
            else if (monthNo == "02")
            {
                fullmonth = "กุมภาพันธ์";
            }
            else if (monthNo == "03")
            {
                fullmonth = "มีนาคม";
            }
            else if (monthNo == "04")
            {
                fullmonth = "เมษายน";
            }
            else if (monthNo == "05")
            {
                fullmonth = "พฤษภาคม";
            }
            else if (monthNo == "06")
            {
                fullmonth = "มิถุนายน";
            }
            else if (monthNo == "07")
            {
                fullmonth = "กรกฎาคม";
            }
            else if (monthNo == "08")
            {
                fullmonth = "สิงหาคม";
            }
            else if (monthNo == "09")
            {
                fullmonth = "กันยายน";
            }
            else if (monthNo == "10")
            {
                fullmonth = "ตุลาคม";
            }
            else if (monthNo == "11")
            {
                fullmonth = "พฤศจิกายน";
            }
            else if (monthNo == "12")
            {
                fullmonth = "ธันวาคม";
            }

            return fullmonth;
        }

        public List<DropdownAllData> GetDropdownRentType()
        {
            DataTable table = new DataTable();
            table.Columns.Add("id");
            table.Columns.Add("name");

            DataRow dr = table.NewRow();
            dr["id"] = 1;
            dr["name"] = "Day";
            table.Rows.Add(dr);

            DataRow dr1 = table.NewRow();
            dr1["id"] = 2;
            dr1["name"] = "Month";
            table.Rows.Add(dr1);

            List<DropdownAllData> listData = new List<DropdownAllData>();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    DropdownAllData data = new DropdownAllData();
                    data.loadData(row);
                    listData.Add(data);
                }
            }

            return listData;
        }

        public List<DropdownAllData> GetDropdownPrefixUnit(int ZoneID, string isAll)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec get_dropdown_prefix_unit " +
                 "@pZoneID, " +
                 "@pIsAll");

            SqlParameter pZoneID = new SqlParameter(@"pZoneID", SqlDbType.Int);
            pZoneID.Direction = ParameterDirection.Input;
            pZoneID.Value = ZoneID;
            sql.Parameters.Add(pZoneID);

            SqlParameter pIsAll = new SqlParameter(@"pIsAll", SqlDbType.VarChar);
            pIsAll.Direction = ParameterDirection.Input;
            pIsAll.Value = isAll;
            sql.Parameters.Add(pIsAll);

            table = sql.executeQueryWithReturnTable();

            List<DropdownAllData> listData = new List<DropdownAllData>();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    DropdownAllData data = new DropdownAllData();
                    data.loadData(row);
                    listData.Add(data);
                }
            }

            return listData;
        }

        public Pagination<SearchMasterData> SearchMasterData(SearchNameCenterDTO searchNameCenterDTO, string TableName)
        {
            DataTable table = new DataTable();

            SQLCustomExecute sql = new SQLCustomExecute("exec get_search_all_master_data_page " +
                "@pTableName, " +
                "@pName, " +
                "@pPage, " +
                "@pPerPage, " +
                "@pSortField, " +
                "@pSortType");

            SqlParameter pTableName = new SqlParameter(@"pTableName", SqlDbType.VarChar, 250);
            pTableName.Direction = ParameterDirection.Input;
            pTableName.Value = TableName;
            sql.Parameters.Add(pTableName);

            SqlParameter pName = new SqlParameter(@"pName", SqlDbType.VarChar, 250);
            pName.Direction = ParameterDirection.Input;
            pName.Value = searchNameCenterDTO.name;
            sql.Parameters.Add(pName);

            SqlParameter pPage = new SqlParameter(@"pPage", SqlDbType.Int);
            pPage.Direction = ParameterDirection.Input;
            pPage.Value = searchNameCenterDTO.pageInt;
            sql.Parameters.Add(pPage);

            SqlParameter pPerPage = new SqlParameter(@"pPerPage", SqlDbType.Int);
            pPerPage.Direction = ParameterDirection.Input;
            pPerPage.Value = searchNameCenterDTO.perPage;
            sql.Parameters.Add(pPerPage);

            SqlParameter pSortField = new SqlParameter(@"pSortField", SqlDbType.Int);
            pSortField.Direction = ParameterDirection.Input;
            pSortField.Value = searchNameCenterDTO.sortField;
            sql.Parameters.Add(pSortField);

            SqlParameter pSortType = new SqlParameter(@"pSortType", SqlDbType.VarChar, 1);
            pSortType.Direction = ParameterDirection.Input;
            pSortType.Value = searchNameCenterDTO.sortType;
            sql.Parameters.Add(pSortType);

            table = sql.executeQueryWithReturnTable();

            Pagination<SearchMasterData> pagination = new Pagination<SearchMasterData>();


            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    SearchMasterData data = new SearchMasterData();
                    data.loadData(row);
                    pagination.data.Add(data);
                }
            }

            int total = GetTotalSearchMasterData(searchNameCenterDTO, TableName);

            pagination.SetPagination(total, searchNameCenterDTO.perPage, searchNameCenterDTO.pageInt);

            return pagination;
        }

        public int GetTotalSearchMasterData(SearchNameCenterDTO searchNameCenterDTO, string TableName)
        {
            int total = 0;

            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec get_search_all_master_data_total " +
                "@pTableName, " +
                "@pName ");

            SqlParameter pTableName = new SqlParameter(@"pTableName", SqlDbType.VarChar, 250);
            pTableName.Direction = ParameterDirection.Input;
            pTableName.Value = TableName;
            sql.Parameters.Add(pTableName);

            SqlParameter pName = new SqlParameter(@"pName", SqlDbType.VarChar, 250);
            pName.Direction = ParameterDirection.Input;
            pName.Value = searchNameCenterDTO.name;
            sql.Parameters.Add(pName);

            table = sql.executeQueryWithReturnTable();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    DataRow dr = table.Rows[0];
                    total = int.Parse(dr["total"].ToString());
                }
            }

            return total;
        }

        public Pagination<SearchMasterZone> SearchZone(SearchNameCenterDTO searchNameCenterDTO)
        {
            DataTable table = new DataTable();

            SQLCustomExecute sql = new SQLCustomExecute("exec get_search_all_zone_page " +
                "@pName, " +
                "@pPage, " +
                "@pPerPage, " +
                "@pSortField, " +
                "@pSortType");

            SqlParameter pName = new SqlParameter(@"pName", SqlDbType.VarChar, 250);
            pName.Direction = ParameterDirection.Input;
            pName.Value = searchNameCenterDTO.name;
            sql.Parameters.Add(pName);

            SqlParameter pPage = new SqlParameter(@"pPage", SqlDbType.Int);
            pPage.Direction = ParameterDirection.Input;
            pPage.Value = searchNameCenterDTO.pageInt;
            sql.Parameters.Add(pPage);

            SqlParameter pPerPage = new SqlParameter(@"pPerPage", SqlDbType.Int);
            pPerPage.Direction = ParameterDirection.Input;
            pPerPage.Value = searchNameCenterDTO.perPage;
            sql.Parameters.Add(pPerPage);

            SqlParameter pSortField = new SqlParameter(@"pSortField", SqlDbType.Int);
            pSortField.Direction = ParameterDirection.Input;
            pSortField.Value = searchNameCenterDTO.sortField;
            sql.Parameters.Add(pSortField);

            SqlParameter pSortType = new SqlParameter(@"pSortType", SqlDbType.VarChar, 1);
            pSortType.Direction = ParameterDirection.Input;
            pSortType.Value = searchNameCenterDTO.sortType;
            sql.Parameters.Add(pSortType);

            table = sql.executeQueryWithReturnTable();

            Pagination<SearchMasterZone> pagination = new Pagination<SearchMasterZone>();


            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    SearchMasterZone data = new SearchMasterZone();
                    data.loadData(row);
                    pagination.data.Add(data);
                }
            }

            int total = GetTotalSearchZone(searchNameCenterDTO);

            pagination.SetPagination(total, searchNameCenterDTO.perPage, searchNameCenterDTO.pageInt);

            return pagination;
        }

        public int GetTotalSearchZone(SearchNameCenterDTO searchNameCenterDTO)
        {
            int total = 0;

            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec get_search_all_zone_total " +
                "@pName ");

            SqlParameter pName = new SqlParameter(@"pName", SqlDbType.VarChar, 250);
            pName.Direction = ParameterDirection.Input;
            pName.Value = searchNameCenterDTO.name;
            sql.Parameters.Add(pName);

            table = sql.executeQueryWithReturnTable();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    DataRow dr = table.Rows[0];
                    total = int.Parse(dr["total"].ToString());
                }
            }

            return total;
        }

        public Pagination<SearchMasterUnit> SearchUnit(SearchUnitDTO searchUnitDTO)
        {
            DataTable table = new DataTable();

            SQLCustomExecute sql = new SQLCustomExecute("exec get_search_all_unit_page " +
                "@pZoneID, " +
                "@pZoneSubID, " +
                "@pPage, " +
                "@pPerPage, " +
                "@pSortField, " +
                "@pSortType");

            SqlParameter pZoneID = new SqlParameter(@"pZoneID", SqlDbType.Int);
            pZoneID.Direction = ParameterDirection.Input;
            pZoneID.Value = searchUnitDTO.zoneID;
            sql.Parameters.Add(pZoneID);

            SqlParameter pZoneSubID = new SqlParameter(@"pZoneSubID", SqlDbType.Int);
            pZoneSubID.Direction = ParameterDirection.Input;
            pZoneSubID.Value = searchUnitDTO.zoneSubID;
            sql.Parameters.Add(pZoneSubID);

            SqlParameter pPage = new SqlParameter(@"pPage", SqlDbType.Int);
            pPage.Direction = ParameterDirection.Input;
            pPage.Value = searchUnitDTO.pageInt;
            sql.Parameters.Add(pPage);

            SqlParameter pPerPage = new SqlParameter(@"pPerPage", SqlDbType.Int);
            pPerPage.Direction = ParameterDirection.Input;
            pPerPage.Value = searchUnitDTO.perPage;
            sql.Parameters.Add(pPerPage);

            SqlParameter pSortField = new SqlParameter(@"pSortField", SqlDbType.Int);
            pSortField.Direction = ParameterDirection.Input;
            pSortField.Value = searchUnitDTO.sortField;
            sql.Parameters.Add(pSortField);

            SqlParameter pSortType = new SqlParameter(@"pSortType", SqlDbType.VarChar, 1);
            pSortType.Direction = ParameterDirection.Input;
            pSortType.Value = searchUnitDTO.sortType;
            sql.Parameters.Add(pSortType);

            table = sql.executeQueryWithReturnTable();

            Pagination<SearchMasterUnit> pagination = new Pagination<SearchMasterUnit>();


            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    SearchMasterUnit data = new SearchMasterUnit();
                    data.loadData(row);
                    pagination.data.Add(data);
                }
            }

            int total = GetTotalSearchUnit(searchUnitDTO);

            pagination.SetPagination(total, searchUnitDTO.perPage, searchUnitDTO.pageInt);

            return pagination;
        }

        public int GetTotalSearchUnit(SearchUnitDTO searchUnitDTO)
        {
            int total = 0;

            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec get_search_all_unit_total " +
                "@pZoneID, " +
                "@pZoneSubID ");

            SqlParameter pZoneID = new SqlParameter(@"pZoneID", SqlDbType.Int);
            pZoneID.Direction = ParameterDirection.Input;
            pZoneID.Value = searchUnitDTO.zoneID;
            sql.Parameters.Add(pZoneID);

            SqlParameter pZoneSubID = new SqlParameter(@"pZoneSubID", SqlDbType.Int);
            pZoneSubID.Direction = ParameterDirection.Input;
            pZoneSubID.Value = searchUnitDTO.zoneSubID;
            sql.Parameters.Add(pZoneSubID);

            table = sql.executeQueryWithReturnTable();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    DataRow dr = table.Rows[0];
                    total = int.Parse(dr["total"].ToString());
                }
            }

            return total;
        }

        public Pagination<SearchMasterZoneSub> SearchZoneSub(SearchZoneSubDTO searchZoneSubDTO)
        {
            DataTable table = new DataTable();

            SQLCustomExecute sql = new SQLCustomExecute("exec get_search_all_zone_sub_page " +
                "@pZoneID, " +
                "@pPage, " +
                "@pPerPage, " +
                "@pSortField, " +
                "@pSortType");

            SqlParameter pZoneID = new SqlParameter(@"pZoneID", SqlDbType.VarChar, 250);
            pZoneID.Direction = ParameterDirection.Input;
            pZoneID.Value = searchZoneSubDTO.zoneID;
            sql.Parameters.Add(pZoneID);

            SqlParameter pPage = new SqlParameter(@"pPage", SqlDbType.Int);
            pPage.Direction = ParameterDirection.Input;
            pPage.Value = searchZoneSubDTO.pageInt;
            sql.Parameters.Add(pPage);

            SqlParameter pPerPage = new SqlParameter(@"pPerPage", SqlDbType.Int);
            pPerPage.Direction = ParameterDirection.Input;
            pPerPage.Value = searchZoneSubDTO.perPage;
            sql.Parameters.Add(pPerPage);

            SqlParameter pSortField = new SqlParameter(@"pSortField", SqlDbType.Int);
            pSortField.Direction = ParameterDirection.Input;
            pSortField.Value = searchZoneSubDTO.sortField;
            sql.Parameters.Add(pSortField);

            SqlParameter pSortType = new SqlParameter(@"pSortType", SqlDbType.VarChar, 1);
            pSortType.Direction = ParameterDirection.Input;
            pSortType.Value = searchZoneSubDTO.sortType;
            sql.Parameters.Add(pSortType);

            table = sql.executeQueryWithReturnTable();

            Pagination<SearchMasterZoneSub> pagination = new Pagination<SearchMasterZoneSub>();


            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    SearchMasterZoneSub data = new SearchMasterZoneSub();
                    data.loadData(row);
                    pagination.data.Add(data);
                }
            }

            int total = GetTotalSearchZoneSub(searchZoneSubDTO);

            pagination.SetPagination(total, searchZoneSubDTO.perPage, searchZoneSubDTO.pageInt);

            return pagination;
        }

        public int GetTotalSearchZoneSub(SearchZoneSubDTO searchZoneSubDTO)
        {
            int total = 0;

            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec get_search_all_zone_sub_total " +
                "@pZoneID ");

            SqlParameter pZoneID = new SqlParameter(@"pZoneID", SqlDbType.VarChar, 250);
            pZoneID.Direction = ParameterDirection.Input;
            pZoneID.Value = searchZoneSubDTO.zoneID;
            sql.Parameters.Add(pZoneID);

            table = sql.executeQueryWithReturnTable();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    DataRow dr = table.Rows[0];
                    total = int.Parse(dr["total"].ToString());
                }
            }

            return total;
        }

        public Pagination<SearchMasterRateAmount> SearchRateAmount(SearchNameCenterDTO searchNameCenterDTO)
        {
            DataTable table = new DataTable();

            SQLCustomExecute sql = new SQLCustomExecute("exec get_search_all_rate_amount_page " +
                "@pName, " +
                "@pPage, " +
                "@pPerPage, " +
                "@pSortField, " +
                "@pSortType");

            SqlParameter pName = new SqlParameter(@"pName", SqlDbType.VarChar, 250);
            pName.Direction = ParameterDirection.Input;
            pName.Value = searchNameCenterDTO.name;
            sql.Parameters.Add(pName);

            SqlParameter pPage = new SqlParameter(@"pPage", SqlDbType.Int);
            pPage.Direction = ParameterDirection.Input;
            pPage.Value = searchNameCenterDTO.pageInt;
            sql.Parameters.Add(pPage);

            SqlParameter pPerPage = new SqlParameter(@"pPerPage", SqlDbType.Int);
            pPerPage.Direction = ParameterDirection.Input;
            pPerPage.Value = searchNameCenterDTO.perPage;
            sql.Parameters.Add(pPerPage);

            SqlParameter pSortField = new SqlParameter(@"pSortField", SqlDbType.Int);
            pSortField.Direction = ParameterDirection.Input;
            pSortField.Value = searchNameCenterDTO.sortField;
            sql.Parameters.Add(pSortField);

            SqlParameter pSortType = new SqlParameter(@"pSortType", SqlDbType.VarChar, 1);
            pSortType.Direction = ParameterDirection.Input;
            pSortType.Value = searchNameCenterDTO.sortType;
            sql.Parameters.Add(pSortType);

            table = sql.executeQueryWithReturnTable();

            Pagination<SearchMasterRateAmount> pagination = new Pagination<SearchMasterRateAmount>();


            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    SearchMasterRateAmount data = new SearchMasterRateAmount();
                    data.loadData(row);
                    pagination.data.Add(data);
                }
            }

            int total = GetTotalSearchRateAmount(searchNameCenterDTO);

            pagination.SetPagination(total, searchNameCenterDTO.perPage, searchNameCenterDTO.pageInt);

            return pagination;
        }

        public int GetTotalSearchRateAmount(SearchNameCenterDTO searchNameCenterDTO)
        {
            int total = 0;

            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec get_search_all_rate_amount_total " +
                "@pName ");

            SqlParameter pName = new SqlParameter(@"pName", SqlDbType.VarChar, 250);
            pName.Direction = ParameterDirection.Input;
            pName.Value = searchNameCenterDTO.name;
            sql.Parameters.Add(pName);

            table = sql.executeQueryWithReturnTable();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    DataRow dr = table.Rows[0];
                    total = int.Parse(dr["total"].ToString());
                }
            }

            return total;
        }

        public Pagination<SearchManageRenter> SearchManageRenter(SearchManageRenterDTO searchManageRenterDTO)
        {
            DataTable table = new DataTable();

            SQLCustomExecute sql = new SQLCustomExecute("exec get_search_all_manage_renter_page " +
                "@pNameOrMobile, " +
                "@pEmpType, " +
                "@pUnitNo, " +
                "@pStartDate, " +
                "@pEndDate, " +
                "@pPage, " +
                "@pPerPage, " +
                "@pSortField, " +
                "@pSortType");

            SqlParameter pNameOrMobile = new SqlParameter(@"pNameOrMobile", SqlDbType.VarChar, 250);
            pNameOrMobile.Direction = ParameterDirection.Input;
            pNameOrMobile.Value = searchManageRenterDTO.nameOrMobile;
            sql.Parameters.Add(pNameOrMobile);

            SqlParameter pEmpType = new SqlParameter(@"pEmpType", SqlDbType.Int);
            pEmpType.Direction = ParameterDirection.Input;
            pEmpType.Value = searchManageRenterDTO.empType;
            sql.Parameters.Add(pEmpType);

            SqlParameter pUnitNo = new SqlParameter(@"pUnitNo", SqlDbType.VarChar, 250);
            pUnitNo.Direction = ParameterDirection.Input;
            pUnitNo.Value = searchManageRenterDTO.unitNo;
            sql.Parameters.Add(pUnitNo);

            SqlParameter pStartDate = new SqlParameter(@"pStartDate", SqlDbType.VarChar, 15);
            pStartDate.Direction = ParameterDirection.Input;
            pStartDate.Value = searchManageRenterDTO.startDate;
            sql.Parameters.Add(pStartDate);

            SqlParameter pEndDate = new SqlParameter(@"pEndDate", SqlDbType.VarChar, 15);
            pEndDate.Direction = ParameterDirection.Input;
            pEndDate.Value = searchManageRenterDTO.endDate;
            sql.Parameters.Add(pEndDate);

            SqlParameter paramPage = new SqlParameter(@"pPage", SqlDbType.Int);
            paramPage.Direction = ParameterDirection.Input;
            paramPage.Value = searchManageRenterDTO.pageInt;
            sql.Parameters.Add(paramPage);

            SqlParameter paramPerPage = new SqlParameter(@"pPerPage", SqlDbType.Int);
            paramPerPage.Direction = ParameterDirection.Input;
            paramPerPage.Value = searchManageRenterDTO.perPage;
            sql.Parameters.Add(paramPerPage);

            SqlParameter pSortField = new SqlParameter(@"pSortField", SqlDbType.Int);
            pSortField.Direction = ParameterDirection.Input;
            pSortField.Value = searchManageRenterDTO.sortField;
            sql.Parameters.Add(pSortField);

            SqlParameter pSortType = new SqlParameter(@"pSortType", SqlDbType.VarChar, 1);
            pSortType.Direction = ParameterDirection.Input;
            pSortType.Value = searchManageRenterDTO.sortType;
            sql.Parameters.Add(pSortType);

            table = sql.executeQueryWithReturnTable();

            Pagination<SearchManageRenter> pagination = new Pagination<SearchManageRenter>();


            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    SearchManageRenter data = new SearchManageRenter();
                    data.loadData(row);
                    pagination.data.Add(data);
                }
            }

            int total = GetTotalSearchManageRenter(searchManageRenterDTO);

            pagination.SetPagination(total, searchManageRenterDTO.perPage, searchManageRenterDTO.pageInt);

            return pagination;
        }

        public int GetTotalSearchManageRenter(SearchManageRenterDTO searchManageRenterDTO)
        {
            int total = 0;

            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec get_search_all_manage_renter_total " +
                "@pNameOrMobile, " +
                "@pEmpType, " +
                "@pUnitNo, " +
                "@pStartDate, " +
                "@pEndDate ");

            SqlParameter pNameOrMobile = new SqlParameter(@"pNameOrMobile", SqlDbType.VarChar, 250);
            pNameOrMobile.Direction = ParameterDirection.Input;
            pNameOrMobile.Value = searchManageRenterDTO.nameOrMobile;
            sql.Parameters.Add(pNameOrMobile);

            SqlParameter pEmpType = new SqlParameter(@"pEmpType", SqlDbType.Int);
            pEmpType.Direction = ParameterDirection.Input;
            pEmpType.Value = searchManageRenterDTO.empType;
            sql.Parameters.Add(pEmpType);

            SqlParameter pUnitNo = new SqlParameter(@"pUnitNo", SqlDbType.VarChar, 250);
            pUnitNo.Direction = ParameterDirection.Input;
            pUnitNo.Value = searchManageRenterDTO.unitNo;
            sql.Parameters.Add(pUnitNo);

            SqlParameter pStartDate = new SqlParameter(@"pStartDate", SqlDbType.VarChar, 15);
            pStartDate.Direction = ParameterDirection.Input;
            pStartDate.Value = searchManageRenterDTO.startDate;
            sql.Parameters.Add(pStartDate);

            SqlParameter pEndDate = new SqlParameter(@"pEndDate", SqlDbType.VarChar, 15);
            pEndDate.Direction = ParameterDirection.Input;
            pEndDate.Value = searchManageRenterDTO.endDate;
            sql.Parameters.Add(pEndDate);

            table = sql.executeQueryWithReturnTable();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    DataRow dr = table.Rows[0];
                    total = int.Parse(dr["total"].ToString());
                }
            }

            return total;
        }

        public Pagination<SearchManageUnit> SearchManageUnit(SearchManageUnitDTO searchManageUnitDTO)
        {
            DataTable table = new DataTable();

            SQLCustomExecute sql = new SQLCustomExecute("exec get_search_all_manage_unit_page " +
                "@pZoneID, " +
                "@pZoneSubID, " +
                "@pUnitNo, " +
                "@pRentType, " +
                "@pIsUsed, " +
                "@pPage, " +
                "@pPerPage, " +
                "@pSortField, " +
                "@pSortType");

            SqlParameter pZoneID = new SqlParameter(@"pZoneID", SqlDbType.Int);
            pZoneID.Direction = ParameterDirection.Input;
            pZoneID.Value = searchManageUnitDTO.zoneID;
            sql.Parameters.Add(pZoneID);

            SqlParameter pZoneSubID = new SqlParameter(@"pZoneSubID", SqlDbType.Int);
            pZoneSubID.Direction = ParameterDirection.Input;
            pZoneSubID.Value = searchManageUnitDTO.zoneSubID;
            sql.Parameters.Add(pZoneSubID);

            SqlParameter pUnitNo = new SqlParameter(@"pUnitNo", SqlDbType.VarChar, 250);
            pUnitNo.Direction = ParameterDirection.Input;
            pUnitNo.Value = searchManageUnitDTO.unitNo;
            sql.Parameters.Add(pUnitNo);

            SqlParameter pRentType = new SqlParameter(@"pRentType", SqlDbType.Int);
            pRentType.Direction = ParameterDirection.Input;
            pRentType.Value = searchManageUnitDTO.rentType;
            sql.Parameters.Add(pRentType);

            SqlParameter pIsUsed = new SqlParameter(@"pIsUsed", SqlDbType.Int);
            pIsUsed.Direction = ParameterDirection.Input;
            pIsUsed.Value = searchManageUnitDTO.isUsed;
            sql.Parameters.Add(pIsUsed);

            SqlParameter paramPage = new SqlParameter(@"pPage", SqlDbType.Int);
            paramPage.Direction = ParameterDirection.Input;
            paramPage.Value = searchManageUnitDTO.pageInt;
            sql.Parameters.Add(paramPage);

            SqlParameter paramPerPage = new SqlParameter(@"pPerPage", SqlDbType.Int);
            paramPerPage.Direction = ParameterDirection.Input;
            paramPerPage.Value = searchManageUnitDTO.perPage;
            sql.Parameters.Add(paramPerPage);

            SqlParameter pSortField = new SqlParameter(@"pSortField", SqlDbType.Int);
            pSortField.Direction = ParameterDirection.Input;
            pSortField.Value = searchManageUnitDTO.sortField;
            sql.Parameters.Add(pSortField);

            SqlParameter pSortType = new SqlParameter(@"pSortType", SqlDbType.VarChar, 1);
            pSortType.Direction = ParameterDirection.Input;
            pSortType.Value = searchManageUnitDTO.sortType;
            sql.Parameters.Add(pSortType);

            table = sql.executeQueryWithReturnTable();

            Pagination<SearchManageUnit> pagination = new Pagination<SearchManageUnit>();


            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    SearchManageUnit data = new SearchManageUnit();
                    data.loadData(row);
                    pagination.data.Add(data);
                }
            }

            int total = GetTotalSearchManageUnit(searchManageUnitDTO);

            pagination.SetPagination(total, searchManageUnitDTO.perPage, searchManageUnitDTO.pageInt);

            return pagination;
        }

        public int GetTotalSearchManageUnit(SearchManageUnitDTO searchManageUnitDTO)
        {
            int total = 0;

            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec get_search_all_manage_unit_total " +
                "@pZoneID, " +
                "@pZoneSubID, " +
                "@pUnitNo, " +
                "@pRentType, " +
                "@pIsUsed ");

            SqlParameter pZoneID = new SqlParameter(@"pZoneID", SqlDbType.Int);
            pZoneID.Direction = ParameterDirection.Input;
            pZoneID.Value = searchManageUnitDTO.zoneID;
            sql.Parameters.Add(pZoneID);

            SqlParameter pZoneSubID = new SqlParameter(@"pZoneSubID", SqlDbType.Int);
            pZoneSubID.Direction = ParameterDirection.Input;
            pZoneSubID.Value = searchManageUnitDTO.zoneSubID;
            sql.Parameters.Add(pZoneSubID);

            SqlParameter pUnitNo = new SqlParameter(@"pUnitNo", SqlDbType.VarChar, 250);
            pUnitNo.Direction = ParameterDirection.Input;
            pUnitNo.Value = searchManageUnitDTO.unitNo;
            sql.Parameters.Add(pUnitNo);

            SqlParameter pRentType = new SqlParameter(@"pRentType", SqlDbType.Int);
            pRentType.Direction = ParameterDirection.Input;
            pRentType.Value = searchManageUnitDTO.rentType;
            sql.Parameters.Add(pRentType);

            SqlParameter pIsUsed = new SqlParameter(@"pIsUsed", SqlDbType.Int);
            pIsUsed.Direction = ParameterDirection.Input;
            pIsUsed.Value = searchManageUnitDTO.isUsed;
            sql.Parameters.Add(pIsUsed);

            table = sql.executeQueryWithReturnTable();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    DataRow dr = table.Rows[0];
                    total = int.Parse(dr["total"].ToString());
                }
            }

            return total;
        }

        public List<DropdownAllData> GetDropdownUser(string isAll)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec get_dropdown_user " +
                "@pIsAll");

            SqlParameter pIsAll = new SqlParameter(@"pIsAll", SqlDbType.VarChar);
            pIsAll.Direction = ParameterDirection.Input;
            pIsAll.Value = isAll;
            sql.Parameters.Add(pIsAll);

            table = sql.executeQueryWithReturnTable();

            List<DropdownAllData> listData = new List<DropdownAllData>();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    DropdownAllData data = new DropdownAllData();
                    data.loadData(row);
                    listData.Add(data);
                }
            }

            return listData;
        }

        public Pagination<SearchManageBill> SearchManageBill(SearchManageBillDTO searchManageBillDTO)
        {
            DataTable table = new DataTable();

            SQLCustomExecute sql = new SQLCustomExecute("exec get_search_all_manage_bill_page " +
                "@pBillCode, " +
                "@pStartDate, " +
                "@pEndDate, " +
                "@pIsComplete, " +
                "@pPage, " +
                "@pPerPage, " +
                "@pSortField, " +
                "@pSortType");

            SqlParameter pBillCode = new SqlParameter(@"pBillCode", SqlDbType.VarChar, 15);
            pBillCode.Direction = ParameterDirection.Input;
            pBillCode.Value = searchManageBillDTO.billCode;
            sql.Parameters.Add(pBillCode);

            SqlParameter pStartDate = new SqlParameter(@"pStartDate", SqlDbType.VarChar, 15);
            pStartDate.Direction = ParameterDirection.Input;
            pStartDate.Value = searchManageBillDTO.startDate;
            sql.Parameters.Add(pStartDate);

            SqlParameter pEndDate = new SqlParameter(@"pEndDate", SqlDbType.VarChar, 15);
            pEndDate.Direction = ParameterDirection.Input;
            pEndDate.Value = searchManageBillDTO.endDate;
            sql.Parameters.Add(pEndDate);

            SqlParameter pIsComplete = new SqlParameter(@"pIsComplete", SqlDbType.Int);
            pIsComplete.Direction = ParameterDirection.Input;
            pIsComplete.Value = searchManageBillDTO.isComplete;
            sql.Parameters.Add(pIsComplete);

            SqlParameter paramPage = new SqlParameter(@"pPage", SqlDbType.Int);
            paramPage.Direction = ParameterDirection.Input;
            paramPage.Value = searchManageBillDTO.pageInt;
            sql.Parameters.Add(paramPage);

            SqlParameter paramPerPage = new SqlParameter(@"pPerPage", SqlDbType.Int);
            paramPerPage.Direction = ParameterDirection.Input;
            paramPerPage.Value = searchManageBillDTO.perPage;
            sql.Parameters.Add(paramPerPage);

            SqlParameter pSortField = new SqlParameter(@"pSortField", SqlDbType.Int);
            pSortField.Direction = ParameterDirection.Input;
            pSortField.Value = searchManageBillDTO.sortField;
            sql.Parameters.Add(pSortField);

            SqlParameter pSortType = new SqlParameter(@"pSortType", SqlDbType.VarChar, 1);
            pSortType.Direction = ParameterDirection.Input;
            pSortType.Value = searchManageBillDTO.sortType;
            sql.Parameters.Add(pSortType);

            table = sql.executeQueryWithReturnTable();

            Pagination<SearchManageBill> pagination = new Pagination<SearchManageBill>();
            
            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    SearchManageBill data = new SearchManageBill();
                    data.loadData(row);
                    data.imageUrl = data.fileCode.Split(',');
                    pagination.data.Add(data);
                }
            }

            int total = GetTotalSearchManageBill(searchManageBillDTO);

            pagination.SetPagination(total, searchManageBillDTO.perPage, searchManageBillDTO.pageInt);

            return pagination;
        }

        public int GetTotalSearchManageBill(SearchManageBillDTO searchManageBillDTO)
        {
            int total = 0;

            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec get_search_all_manage_bill_total " +
                "@pBillCode, " +
                "@pStartDate, " +
                "@pEndDate, " +
                "@pIsComplete ");

            SqlParameter pBillCode = new SqlParameter(@"pBillCode", SqlDbType.VarChar, 15);
            pBillCode.Direction = ParameterDirection.Input;
            pBillCode.Value = searchManageBillDTO.billCode;
            sql.Parameters.Add(pBillCode);

            SqlParameter pStartDate = new SqlParameter(@"pStartDate", SqlDbType.VarChar, 15);
            pStartDate.Direction = ParameterDirection.Input;
            pStartDate.Value = searchManageBillDTO.startDate;
            sql.Parameters.Add(pStartDate);

            SqlParameter pEndDate = new SqlParameter(@"pEndDate", SqlDbType.VarChar, 15);
            pEndDate.Direction = ParameterDirection.Input;
            pEndDate.Value = searchManageBillDTO.endDate;
            sql.Parameters.Add(pEndDate);

            SqlParameter pIsComplete = new SqlParameter(@"pIsComplete", SqlDbType.Int);
            pIsComplete.Direction = ParameterDirection.Input;
            pIsComplete.Value = searchManageBillDTO.isComplete;
            sql.Parameters.Add(pIsComplete);

            table = sql.executeQueryWithReturnTable();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    DataRow dr = table.Rows[0];
                    total = int.Parse(dr["total"].ToString());
                }
            }

            return total;
        }

        public RenterByUserIDModel GetRenterByUserID(int userID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec get_renter_name_by_user_id " +
                "@pUserID");

            SqlParameter pUserID = new SqlParameter(@"pUserID", SqlDbType.VarChar);
            pUserID.Direction = ParameterDirection.Input;
            pUserID.Value = userID;
            sql.Parameters.Add(pUserID);

            table = sql.executeQueryWithReturnTable();

            RenterByUserIDModel data = new RenterByUserIDModel();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    data.loadData(row);
                }
            }

            return data;
        }

        public List<RentDetailModel> GetRenterMarketByUserID(int userID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec get_renter_by_user_id " +
                "@pUserID");

            SqlParameter pUserID = new SqlParameter(@"pUserID", SqlDbType.VarChar);
            pUserID.Direction = ParameterDirection.Input;
            pUserID.Value = userID;
            sql.Parameters.Add(pUserID);

            table = sql.executeQueryWithReturnTable();

            List<RentDetailModel> listData = new List<RentDetailModel>();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    RentDetailModel data = new RentDetailModel();
                    data.loadData(row);
                    listData.Add(data);
                }
            }

            return listData;
        }

        public List<RentDetailModel> GetRenterMarketNightByUserID(int userID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec get_renter_by_user_id_night " +
                "@pUserID");

            SqlParameter pUserID = new SqlParameter(@"pUserID", SqlDbType.VarChar);
            pUserID.Direction = ParameterDirection.Input;
            pUserID.Value = userID;
            sql.Parameters.Add(pUserID);

            table = sql.executeQueryWithReturnTable();

            List<RentDetailModel> listData = new List<RentDetailModel>();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    RentDetailModel data = new RentDetailModel();
                    data.loadData(row);
                    listData.Add(data);
                }
            }

            return listData;
        }

        public DashbordModel GetDashbord(GetDashbordDTO getDashbordDTO)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec get_dashboard_admin_amt " +
                "@pStartDate," +
                "@pEndDate");

            string startDate = "", endDate = "";

            if (!string.IsNullOrEmpty(getDashbordDTO.month))
            {
                if (getDashbordDTO.month.ToLower() == "thismonth")
                {
                    var firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                    startDate = firstDayOfMonth.ToString("yyyy-MM-dd");
                    endDate = lastDayOfMonth.ToString("yyyy-MM-dd");
                }
                else
                {
                    int year = 0, month = 0;
                    int.TryParse(getDashbordDTO.month.Substring(0, 4), out year);
                    int.TryParse(getDashbordDTO.month.Substring(4, 2), out month);
                    var firstDayOfMonth = new DateTime(year, month, 1);
                    var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                    startDate = firstDayOfMonth.ToString("yyyy-MM-dd");
                    endDate = lastDayOfMonth.ToString("yyyy-MM-dd");
                }
            }
            else if (!string.IsNullOrEmpty(getDashbordDTO.quarter))
            {
                if (getDashbordDTO.quarter == "1")
                {
                    int year = DateTime.Now.Year;
                    DateTime firstDay = new DateTime(year, 1, 1);
                    DateTime lastDay = new DateTime(year, 3, 31);

                    startDate = firstDay.ToString("yyyy-MM-dd");
                    endDate = lastDay.ToString("yyyy-MM-dd");
                }
                else if (getDashbordDTO.quarter == "2")
                {
                    int year = DateTime.Now.Year;
                    DateTime firstDay = new DateTime(year, 4, 1);
                    DateTime lastDay = new DateTime(year, 6, 30);

                    startDate = firstDay.ToString("yyyy-MM-dd");
                    endDate = lastDay.ToString("yyyy-MM-dd");
                }
                else if (getDashbordDTO.quarter == "3")
                {
                    int year = DateTime.Now.Year;
                    DateTime firstDay = new DateTime(year, 7, 1);
                    DateTime lastDay = new DateTime(year, 9, 30);

                    startDate = firstDay.ToString("yyyy-MM-dd");
                    endDate = lastDay.ToString("yyyy-MM-dd");
                }
                else if (getDashbordDTO.quarter == "4")
                {
                    int year = DateTime.Now.Year;
                    DateTime firstDay = new DateTime(year, 10, 1);
                    DateTime lastDay = new DateTime(year, 12, 31);

                    startDate = firstDay.ToString("yyyy-MM-dd");
                    endDate = lastDay.ToString("yyyy-MM-dd");
                }
                else
                {
                    int year = DateTime.Now.Year;
                    DateTime firstDay = new DateTime(year, 1, 1);
                    DateTime lastDay = new DateTime(year, 12, 31);

                    startDate = firstDay.ToString("yyyy-MM-dd");
                    endDate = lastDay.ToString("yyyy-MM-dd");
                }
            }

            SqlParameter pStartDate = new SqlParameter(@"pStartDate", SqlDbType.VarChar);
            pStartDate.Direction = ParameterDirection.Input;
            pStartDate.Value = startDate;
            sql.Parameters.Add(pStartDate);

            SqlParameter pEndDate = new SqlParameter(@"pEndDate", SqlDbType.VarChar);
            pEndDate.Direction = ParameterDirection.Input;
            pEndDate.Value = endDate;
            sql.Parameters.Add(pEndDate);

            table = sql.executeQueryWithReturnTable();

            DashbordModel data = new DashbordModel();
            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    data.loadData(row);
                }
            }

            return data;
        }

        public DataTable CheckDupicateTranPay(int billID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec check_duplicate_transaction_pay " +
                "@pBillID ");

            SqlParameter pBillID = new SqlParameter(@"pBillID", SqlDbType.Int);
            pBillID.Direction = ParameterDirection.Input;
            pBillID.Value = billID;
            sql.Parameters.Add(pBillID);

            table = sql.executeQueryWithReturnTable();

            return table;
        }

        public _ReturnIdModel InsertTranPay(SaveTranPayDTO saveTranPayDTO, int userID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec insert_billpay_file " +
                "@pBillID," +
                "@pPayamount," +
                "@pComment," +
                "@pRentIsPaid," +
                "@pElectricIsPaid," +
                "@pWaterIsPaid," +
                "@pLampunitIsPaid," +
                "@pElectricUnitIsPaid," +
                "@pCreateBy ");

            SqlParameter pBillID = new SqlParameter(@"pBillID", SqlDbType.VarChar, 100);
            pBillID.Direction = ParameterDirection.Input;
            pBillID.Value = saveTranPayDTO.billID;
            sql.Parameters.Add(pBillID);

            SqlParameter pPayamount = new SqlParameter(@"pPayamount", SqlDbType.Decimal);
            pPayamount.Direction = ParameterDirection.Input;
            pPayamount.Value = saveTranPayDTO.payAmount;
            sql.Parameters.Add(pPayamount);

            SqlParameter pComment = new SqlParameter(@"pComment", SqlDbType.VarChar);
            pComment.Direction = ParameterDirection.Input;
            pComment.Value = saveTranPayDTO.comment;
            sql.Parameters.Add(pComment);

            SqlParameter pRentIsPaid = new SqlParameter(@"pRentIsPaid", SqlDbType.Int);
            pRentIsPaid.Direction = ParameterDirection.Input;
            pRentIsPaid.Value = 1;
            sql.Parameters.Add(pRentIsPaid);

            SqlParameter pElectricIsPaid = new SqlParameter(@"pElectricIsPaid", SqlDbType.Int);
            pElectricIsPaid.Direction = ParameterDirection.Input;
            pElectricIsPaid.Value = 1;
            sql.Parameters.Add(pElectricIsPaid);

            SqlParameter pWaterIsPaid = new SqlParameter(@"pWaterIsPaid", SqlDbType.Int);
            pWaterIsPaid.Direction = ParameterDirection.Input;
            pWaterIsPaid.Value = 1;
            sql.Parameters.Add(pWaterIsPaid);

            SqlParameter pLampunitIsPaid = new SqlParameter(@"pLampunitIsPaid", SqlDbType.Int);
            pLampunitIsPaid.Direction = ParameterDirection.Input;
            pLampunitIsPaid.Value = 1;
            sql.Parameters.Add(pLampunitIsPaid);

            SqlParameter pElectricUnitIsPaid = new SqlParameter(@"pElectricUnitIsPaid", SqlDbType.Int);
            pElectricUnitIsPaid.Direction = ParameterDirection.Input;
            pElectricUnitIsPaid.Value = 1;
            sql.Parameters.Add(pElectricUnitIsPaid);

            SqlParameter pCreateBy = new SqlParameter(@"pCreateBy", SqlDbType.Int);
            pCreateBy.Direction = ParameterDirection.Input;
            pCreateBy.Value = userID;
            sql.Parameters.Add(pCreateBy);

            table = sql.executeQueryWithReturnTable();

            _ReturnIdModel data = new _ReturnIdModel();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    data.loadData(row);
                }
            }

            return data;
        }

        public HistoryPaidBillAdminTotalModel GetOutStandingBillUserTotal(GetHistoryUserBillDTO getHistoryUserBillDTO)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec get_outstanding_bill_user_total " +
                "@pUserID, " +
                "@pStartDate, " +
                "@pEndDate " );

            SqlParameter pUserID = new SqlParameter(@"pUserID", SqlDbType.Int);
            pUserID.Direction = ParameterDirection.Input;
            pUserID.Value = getHistoryUserBillDTO.userID;
            sql.Parameters.Add(pUserID);

            SqlParameter pStartDate = new SqlParameter(@"pStartDate", SqlDbType.VarChar, 255);
            pStartDate.Direction = ParameterDirection.Input;
            pStartDate.Value = getHistoryUserBillDTO.startDate;
            sql.Parameters.Add(pStartDate);

            SqlParameter pEndDate = new SqlParameter(@"pEndDate", SqlDbType.VarChar, 255);
            pEndDate.Direction = ParameterDirection.Input;
            pEndDate.Value = getHistoryUserBillDTO.endDate;
            sql.Parameters.Add(pEndDate);

            table = sql.executeQueryWithReturnTable();

            HistoryPaidBillAdminTotalModel data = new HistoryPaidBillAdminTotalModel();
            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    data.loadData(row);
                }
            }

            return data;
        }

        public _ReturnIdModel UpdateAdminApprove(GetIDCenterDTO getIDCenterDTO, int userID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec update_admin_approve " +
                "@pBillID," +
                "@pUpdateBy ");

            SqlParameter pBillID = new SqlParameter(@"pBillID", SqlDbType.Int);
            pBillID.Direction = ParameterDirection.Input;
            pBillID.Value = getIDCenterDTO.id;
            sql.Parameters.Add(pBillID);

            SqlParameter pUpdateBy = new SqlParameter(@"pUpdateBy", SqlDbType.Int);
            pUpdateBy.Direction = ParameterDirection.Input;
            pUpdateBy.Value = userID;
            sql.Parameters.Add(pUpdateBy);

            table = sql.executeQueryWithReturnTable();

            _ReturnIdModel data = new _ReturnIdModel();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    data.loadData(row);
                }
            }

            return data;
        }

        public TranPayImageModel GetTranPayImage(int billID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec get_tranrent_image " +
                "@pBillID");

            SqlParameter pBillID = new SqlParameter(@"pBillID", SqlDbType.Int);
            pBillID.Direction = ParameterDirection.Input;
            pBillID.Value = billID;
            sql.Parameters.Add(pBillID);

            table = sql.executeQueryWithReturnTable();

            TranPayImageModel data = new TranPayImageModel();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    data.loadData(row);
                }
            }

            data.imageUrl = data.fileCode.Split(',');

            return data;
        }
    }

    public class SQLCustomExecute
    {
        public List<SqlParameter> Parameters { get; set; }
        public string sqlCommand { get; set; }

        public SQLCustomExecute(string sqlCommand)
        {
            this.sqlCommand = sqlCommand;
            this.Parameters = new List<SqlParameter>();
        }

        public DataTable executeQueryWithReturnTable()
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US", false);

            CultureInfo cultureInfo = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            cultureInfo.DateTimeFormat.DateSeparator = "-";
            cultureInfo.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
            Thread.CurrentThread.CurrentCulture = cultureInfo;

            DataTable result = null;

            string connectionString = WebConfigurationManager.AppSettings["connectionStrings"];

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = this.sqlCommand;

                if (this.Parameters != null)
                    foreach (SqlParameter parameter in this.Parameters)
                        command.Parameters.Add(parameter);
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        result = new DataTable();
                        if (reader.HasRows)
                        {
                            result.Load(reader);
                        }
                        command.Parameters.Clear();
                    }
                    command.Parameters.Clear();

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    connection.Dispose();
                    connection.Close();
                }
            }
            return result;
        }
    }
}