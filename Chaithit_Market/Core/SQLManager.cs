using Chaithit_Market.DTO;
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

        public LoginData Login(string pUserName, string pPassword, string pTokenID, string pLang)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec login @pUserName, @pPassword, @pTokenID, @pLang");

            SqlParameter paramUserName = new SqlParameter(@"pUserName", SqlDbType.VarChar, 150);
            paramUserName.Direction = ParameterDirection.Input;
            paramUserName.Value = pUserName;

            SqlParameter paramPassword = new SqlParameter(@"pPassword", SqlDbType.VarChar, 250);
            paramPassword.Direction = ParameterDirection.Input;
            paramPassword.Value = pPassword;

            SqlParameter paramTokenID = new SqlParameter(@"pTokenID", SqlDbType.VarChar);
            paramTokenID.Direction = ParameterDirection.Input;
            paramTokenID.Value = pTokenID;

            SqlParameter paramLang = new SqlParameter(@"pLang", SqlDbType.VarChar, 5);
            paramLang.Direction = ParameterDirection.Input;
            paramLang.Value = pLang;

            sql.Parameters.Add(paramUserName);
            sql.Parameters.Add(paramPassword);
            sql.Parameters.Add(paramTokenID);
            sql.Parameters.Add(paramLang);

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

            sql = new SQLCustomExecute("exec check_duplicate_master @pMasterID,@pTableName,@pNameEN, @pNameTH");
            
            SqlParameter pMasterID = new SqlParameter(@"pMasterID", SqlDbType.Int);
            pMasterID.Direction = ParameterDirection.Input;
            pMasterID.Value = masterDataDTO.masterID;
            sql.Parameters.Add(pMasterID);

            SqlParameter pTableName = new SqlParameter(@"pTableName", SqlDbType.VarChar);
            pTableName.Direction = ParameterDirection.Input;
            pTableName.Value = TableName;
            sql.Parameters.Add(pTableName);

            SqlParameter pNameEN = new SqlParameter(@"pNameEN", SqlDbType.VarChar);
            pNameEN.Direction = ParameterDirection.Input;
            pNameEN.Value = masterDataDTO.nameEN;
            sql.Parameters.Add(pNameEN);

            SqlParameter pNameTH = new SqlParameter(@"pNameTH", SqlDbType.VarChar);
            pNameTH.Direction = ParameterDirection.Input;
            pNameTH.Value = masterDataDTO.nameTH;
            sql.Parameters.Add(pNameTH);

            table = sql.executeQueryWithReturnTable();

            return table;
        }

        public _ReturnIdModel InsertMasterData(MasterDataDTO masterDataDTO, string TableName, int userID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec insert_master_data " +
                "@pTableName," +
                "@pNameEN," +
                "@pNameTH," +
                "@pUserID ");

            SqlParameter pTableName = new SqlParameter(@"pTableName", SqlDbType.VarChar);
            pTableName.Direction = ParameterDirection.Input;
            pTableName.Value = TableName;
            sql.Parameters.Add(pTableName);

            SqlParameter pNameEN = new SqlParameter(@"pNameEN", SqlDbType.VarChar);
            pNameEN.Direction = ParameterDirection.Input;
            pNameEN.Value = masterDataDTO.nameEN;
            sql.Parameters.Add(pNameEN);

            SqlParameter pNameTH = new SqlParameter(@"pNameTH", SqlDbType.VarChar);
            pNameTH.Direction = ParameterDirection.Input;
            pNameTH.Value = masterDataDTO.nameTH;
            sql.Parameters.Add(pNameTH);

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
                "@pNameEN," +
                "@pNameTH," +
                "@pUserID ");

            SqlParameter pMasterID = new SqlParameter(@"pMasterID", SqlDbType.Int);
            pMasterID.Direction = ParameterDirection.Input;
            pMasterID.Value = masterDataDTO.masterID;
            sql.Parameters.Add(pMasterID);

            SqlParameter pTableName = new SqlParameter(@"pTableName", SqlDbType.VarChar);
            pTableName.Direction = ParameterDirection.Input;
            pTableName.Value = TableName;
            sql.Parameters.Add(pTableName);

            SqlParameter pNameEN = new SqlParameter(@"pNameEN", SqlDbType.VarChar);
            pNameEN.Direction = ParameterDirection.Input;
            pNameEN.Value = masterDataDTO.nameEN;
            sql.Parameters.Add(pNameEN);

            SqlParameter pNameTH = new SqlParameter(@"pNameTH", SqlDbType.VarChar);
            pNameTH.Direction = ParameterDirection.Input;
            pNameTH.Value = masterDataDTO.nameTH;
            sql.Parameters.Add(pNameTH);

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

        public MasterData GetMasterData(int id, string TableName)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec get_master_data " +
                "@pMasterID," +
                "@pTableName");

            SqlParameter pMasterID = new SqlParameter(@"pMasterID", SqlDbType.Int);
            pMasterID.Direction = ParameterDirection.Input;
            pMasterID.Value = id;
            sql.Parameters.Add(pMasterID);

            SqlParameter pTableName = new SqlParameter(@"pTableName", SqlDbType.VarChar);
            pTableName.Direction = ParameterDirection.Input;
            pTableName.Value = TableName;
            sql.Parameters.Add(pTableName);

            table = sql.executeQueryWithReturnTable();

            MasterData data = new MasterData();

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    data.loadData(row);
                }
            }

            return data;
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

        public DataTable CheckDupicateRental(SaveRentalDTO saveRentalDTO, int pUserID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec check_dupicate_rental " +
                "@pRentID, " +
                "@pRentCode, " +
                "@pName " );

            SqlParameter pRentID = new SqlParameter(@"pRentID", SqlDbType.Int);
            pRentID.Direction = ParameterDirection.Input;
            pRentID.Value = saveRentalDTO.rentalID;
            sql.Parameters.Add(pRentID);

            SqlParameter pRentCode = new SqlParameter(@"pRentCode", SqlDbType.VarChar, 150);
            pRentCode.Direction = ParameterDirection.Input;
            pRentCode.Value = saveRentalDTO.rentCode;
            sql.Parameters.Add(pRentCode);

            SqlParameter pName = new SqlParameter(@"pName", SqlDbType.VarChar, 150);
            pName.Direction = ParameterDirection.Input;
            pName.Value = saveRentalDTO.name;
            sql.Parameters.Add(pName);

            table = sql.executeQueryWithReturnTable();

            return table;
        }

        public DataTable CheckDupicateTransectionRent(InsertTransectionRentDTO insertTransectionRentDTO, int pUserID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec check_dupicate_transection_rent " +
                "@pTransCode ");

            SqlParameter pTransCode = new SqlParameter(@"pTransCode", SqlDbType.VarChar, 10);
            pTransCode.Direction = ParameterDirection.Input;
            pTransCode.Value = insertTransectionRentDTO.transCode;
            sql.Parameters.Add(pTransCode);

            table = sql.executeQueryWithReturnTable();

            return table;
        }

        public DataTable CheckDupicateTransectionBill(InsertTransectionBillDTO insertTransectionBillDTO, int pUserID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec check_dupicate_transection_bill " +
                "@pBillCode ");

            SqlParameter pBillCode = new SqlParameter(@"pBillCode", SqlDbType.VarChar, 150);
            pBillCode.Direction = ParameterDirection.Input;
            pBillCode.Value = insertTransectionBillDTO.billCode;
            sql.Parameters.Add(pBillCode);
            
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
                "@pIsEmp," +
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

            SqlParameter pIsEmp = new SqlParameter(@"pIsEmp", SqlDbType.Int);
            pIsEmp.Direction = ParameterDirection.Input;
            pIsEmp.Value = saveUserProfileDTO.isEmp;
            sql.Parameters.Add(pIsEmp);

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
                "@pEndDate," +
                "@pIsEmp," +
                "@pUpdateBy ");

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

            SqlParameter pEndDate = new SqlParameter(@"pEndDate", SqlDbType.VarChar);
            pEndDate.Direction = ParameterDirection.Input;
            pEndDate.Value = saveUserProfileDTO.endDate;
            sql.Parameters.Add(pEndDate);

            SqlParameter pIsEmp = new SqlParameter(@"pIsEmp", SqlDbType.Int);
            pIsEmp.Direction = ParameterDirection.Input;
            pIsEmp.Value = saveUserProfileDTO.isEmp;
            sql.Parameters.Add(pIsEmp);

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

        public _ReturnIdModel InsertRental(SaveRentalDTO saveRentalDTO, int userID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec insert_rental " +
                "@pRentCode," +
                "@pName," +
                "@pRentAmount," +
                "@pPlaceID," +
                "@pIsUsed," +
                "@pCreateBy");

            SqlParameter pRentCode = new SqlParameter(@"pRentCode", SqlDbType.VarChar);
            pRentCode.Direction = ParameterDirection.Input;
            pRentCode.Value = saveRentalDTO.rentCode;
            sql.Parameters.Add(pRentCode);

            SqlParameter pName = new SqlParameter(@"pName", SqlDbType.VarChar);
            pName.Direction = ParameterDirection.Input;
            pName.Value = saveRentalDTO.name;
            sql.Parameters.Add(pName);

            SqlParameter pRentAmount = new SqlParameter(@"pRentAmount", SqlDbType.Decimal);
            pRentAmount.Direction = ParameterDirection.Input;
            pRentAmount.Value = saveRentalDTO.rentAmount;
            sql.Parameters.Add(pRentAmount);

            SqlParameter pPlaceID = new SqlParameter(@"pPlaceID", SqlDbType.Int);
            pPlaceID.Direction = ParameterDirection.Input;
            pPlaceID.Value = saveRentalDTO.placeID;
            sql.Parameters.Add(pPlaceID);

            SqlParameter pIsUsed = new SqlParameter(@"pIsUsed", SqlDbType.Int);
            pIsUsed.Direction = ParameterDirection.Input;
            pIsUsed.Value = saveRentalDTO.isUsed;
            sql.Parameters.Add(pIsUsed);

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

        public _ReturnIdModel UpdateRental(SaveRentalDTO saveRentalDTO, int userID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec update_rental " +
                "@pRentalID," +
                "@pRentCode," +
                "@pName," +
                "@pRentAmount," +
                "@pPlaceID," +
                "@pIsUsed," +
                "@pUpdateBy ");

            SqlParameter pRentalID = new SqlParameter(@"pRentalID", SqlDbType.Int);
            pRentalID.Direction = ParameterDirection.Input;
            pRentalID.Value = saveRentalDTO.rentalID;
            sql.Parameters.Add(pRentalID);

            SqlParameter pRentCode = new SqlParameter(@"pRentCode", SqlDbType.VarChar);
            pRentCode.Direction = ParameterDirection.Input;
            pRentCode.Value = saveRentalDTO.rentCode;
            sql.Parameters.Add(pRentCode);

            SqlParameter pName = new SqlParameter(@"pName", SqlDbType.VarChar);
            pName.Direction = ParameterDirection.Input;
            pName.Value = saveRentalDTO.name;
            sql.Parameters.Add(pName);

            SqlParameter pRentAmount = new SqlParameter(@"pRentAmount", SqlDbType.Decimal);
            pRentAmount.Direction = ParameterDirection.Input;
            pRentAmount.Value = saveRentalDTO.rentAmount;
            sql.Parameters.Add(pRentAmount);

            SqlParameter pPlaceID = new SqlParameter(@"pPlaceID", SqlDbType.Int);
            pPlaceID.Direction = ParameterDirection.Input;
            pPlaceID.Value = saveRentalDTO.placeID;
            sql.Parameters.Add(pPlaceID);

            SqlParameter pIsUsed = new SqlParameter(@"pIsUsed", SqlDbType.Int);
            pIsUsed.Direction = ParameterDirection.Input;
            pIsUsed.Value = saveRentalDTO.isUsed;
            sql.Parameters.Add(pIsUsed);

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

        public _ReturnIdModel DeleteRental(SaveRentalDTO saveRentalDTO, int userID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec delete_rental " +
                "@pRentalID," +
                "@pUpdateBy ");

            SqlParameter pRentalID = new SqlParameter(@"pRentalID", SqlDbType.Int);
            pRentalID.Direction = ParameterDirection.Input;
            pRentalID.Value = saveRentalDTO.rentalID;
            sql.Parameters.Add(pRentalID);

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
                "@pRentalID," +
                "@pStartDate," +
                "@pEndDate," +
                "@pCreateBy");

            SqlParameter pTransCode = new SqlParameter(@"pTransCode", SqlDbType.VarChar);
            pTransCode.Direction = ParameterDirection.Input;
            pTransCode.Value = insertTransectionRentDTO.transCode;
            sql.Parameters.Add(pTransCode);

            SqlParameter pUserID = new SqlParameter(@"pUserID", SqlDbType.Int);
            pUserID.Direction = ParameterDirection.Input;
            pUserID.Value = insertTransectionRentDTO.userID;
            sql.Parameters.Add(pUserID);

            SqlParameter pRentalID = new SqlParameter(@"pRentalID", SqlDbType.Int);
            pRentalID.Direction = ParameterDirection.Input;
            pRentalID.Value = insertTransectionRentDTO.rentalID;
            sql.Parameters.Add(pRentalID);

            SqlParameter pStartDate = new SqlParameter(@"pStartDate", SqlDbType.VarChar);
            pStartDate.Direction = ParameterDirection.Input;
            pStartDate.Value = insertTransectionRentDTO.startDate;
            sql.Parameters.Add(pStartDate);

            SqlParameter pEndDate = new SqlParameter(@"pEndDate", SqlDbType.VarChar);
            pEndDate.Direction = ParameterDirection.Input;
            pEndDate.Value = insertTransectionRentDTO.endDate;
            sql.Parameters.Add(pEndDate);

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
                "@pBillCode," +
                "@pTranRentID," +
                "@pStartDate," +
                "@pEndDate," +
                "@pRentalAmount," +
                "@pElectricAmount," +
                "@pWaterAmount," +
                "@pTotalAmount," +
                "@pCreateBy");

            SqlParameter pBillCode = new SqlParameter(@"pBillCode", SqlDbType.VarChar);
            pBillCode.Direction = ParameterDirection.Input;
            pBillCode.Value = insertTransectionBillDTO.billCode;
            sql.Parameters.Add(pBillCode);

            SqlParameter pTranRentID = new SqlParameter(@"pTranRentID", SqlDbType.Int);
            pTranRentID.Direction = ParameterDirection.Input;
            pTranRentID.Value = insertTransectionBillDTO.tranRentID;
            sql.Parameters.Add(pTranRentID);

            SqlParameter pStartDate = new SqlParameter(@"pStartDate", SqlDbType.VarChar);
            pStartDate.Direction = ParameterDirection.Input;
            pStartDate.Value = insertTransectionBillDTO.startDate;
            sql.Parameters.Add(pStartDate);

            SqlParameter pEndDate = new SqlParameter(@"pEndDate", SqlDbType.VarChar);
            pEndDate.Direction = ParameterDirection.Input;
            pEndDate.Value = insertTransectionBillDTO.endDate;
            sql.Parameters.Add(pEndDate);

            SqlParameter pRentalAmount = new SqlParameter(@"pRentalAmount", SqlDbType.Decimal);
            pRentalAmount.Direction = ParameterDirection.Input;
            pRentalAmount.Value = insertTransectionBillDTO.rentalAmount;
            sql.Parameters.Add(pRentalAmount);

            SqlParameter pElectricAmount = new SqlParameter(@"pElectricAmount", SqlDbType.Decimal);
            pElectricAmount.Direction = ParameterDirection.Input;
            pElectricAmount.Value = insertTransectionBillDTO.electricAmount;
            sql.Parameters.Add(pElectricAmount);

            SqlParameter pWaterAmount = new SqlParameter(@"pWaterAmount", SqlDbType.Decimal);
            pWaterAmount.Direction = ParameterDirection.Input;
            pWaterAmount.Value = insertTransectionBillDTO.waterAmount;
            sql.Parameters.Add(pWaterAmount);

            SqlParameter pTotalAmount = new SqlParameter(@"pTotalAmount", SqlDbType.Decimal);
            pTotalAmount.Direction = ParameterDirection.Input;
            pTotalAmount.Value = insertTransectionBillDTO.totalAmount;
            sql.Parameters.Add(pTotalAmount);

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
                "@pUserName, " +
                "@pFirstName, " +
                "@pLastName, " +
                "@pMobile, " +
                "@pPosition, " +
                "@pPage, " +
                "@pPerPage, " +
                "@pSortField, " +
                "@pSortType");

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
                "@pUserName, " +
                "@pFirstName, " +
                "@pLastName, " +
                "@pMobile, " +
                "@pPosition ");

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