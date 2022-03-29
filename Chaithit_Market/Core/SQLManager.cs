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
                "@pTransCode, " +
                "@pRentID");

            SqlParameter pTransCode = new SqlParameter(@"pTransCode", SqlDbType.VarChar, 10);
            pTransCode.Direction = ParameterDirection.Input;
            pTransCode.Value = insertTransectionRentDTO.transCode;
            sql.Parameters.Add(pTransCode);

            SqlParameter pRentID = new SqlParameter(@"pRentID", SqlDbType.Int);
            pRentID.Direction = ParameterDirection.Input;
            pRentID.Value = insertTransectionRentDTO.rentalID;
            sql.Parameters.Add(pRentID);

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
                "@pEndDate," +
                "@pStatusEmp," +
                "@pEmpType," +
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
                "@pPlaceSubID," +
                "@pName," +
                "@pIsUsed," +
                "@pCreateBy");

            SqlParameter pRentCode = new SqlParameter(@"pRentCode", SqlDbType.VarChar);
            pRentCode.Direction = ParameterDirection.Input;
            pRentCode.Value = saveRentalDTO.rentCode;
            sql.Parameters.Add(pRentCode);

            SqlParameter pPlaceSubID = new SqlParameter(@"pPlaceSubID", SqlDbType.Int);
            pPlaceSubID.Direction = ParameterDirection.Input;
            pPlaceSubID.Value = saveRentalDTO.placeSubID;
            sql.Parameters.Add(pPlaceSubID);

            SqlParameter pName = new SqlParameter(@"pName", SqlDbType.VarChar);
            pName.Direction = ParameterDirection.Input;
            pName.Value = saveRentalDTO.name;
            sql.Parameters.Add(pName);
            
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
                "@pPlaceSubID," +
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
            
            SqlParameter pPlaceSubID = new SqlParameter(@"pPlaceSubID", SqlDbType.Int);
            pPlaceSubID.Direction = ParameterDirection.Input;
            pPlaceSubID.Value = saveRentalDTO.placeSubID;
            sql.Parameters.Add(pPlaceSubID);

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

        public Pagination<SearchRental> SearchRental(SearchRentDTO searchRentDTO)
        {
            DataTable table = new DataTable();

            SQLCustomExecute sql = new SQLCustomExecute("exec get_search_all_rent_page " +
                "@pName, " +
                "@pPage, " +
                "@pPerPage, " +
                "@pSortField, " +
                "@pSortType");

            SqlParameter pName = new SqlParameter(@"pName", SqlDbType.VarChar, 255);
            pName.Direction = ParameterDirection.Input;
            pName.Value = searchRentDTO.name;
            sql.Parameters.Add(pName);
            
            SqlParameter pPage = new SqlParameter(@"pPage", SqlDbType.Int);
            pPage.Direction = ParameterDirection.Input;
            pPage.Value = searchRentDTO.pageInt;
            sql.Parameters.Add(pPage);

            SqlParameter pPerPage = new SqlParameter(@"pPerPage", SqlDbType.Int);
            pPerPage.Direction = ParameterDirection.Input;
            pPerPage.Value = searchRentDTO.perPage;
            sql.Parameters.Add(pPerPage);

            SqlParameter pSortField = new SqlParameter(@"pSortField", SqlDbType.Int);
            pSortField.Direction = ParameterDirection.Input;
            pSortField.Value = searchRentDTO.sortField;
            sql.Parameters.Add(pSortField);

            SqlParameter pSortType = new SqlParameter(@"pSortType", SqlDbType.VarChar, 1);
            pSortType.Direction = ParameterDirection.Input;
            pSortType.Value = searchRentDTO.sortType;
            sql.Parameters.Add(pSortType);

            table = sql.executeQueryWithReturnTable();

            Pagination<SearchRental> pagination = new Pagination<SearchRental>();


            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    SearchRental data = new SearchRental();
                    data.loadData(row);
                    pagination.data.Add(data);
                }
            }

            int total = GetTotalSearchRent(searchRentDTO);

            pagination.SetPagination(total, searchRentDTO.perPage, searchRentDTO.pageInt);

            return pagination;
        }

        public int GetTotalSearchRent(SearchRentDTO searchRentDTO)
        {
            int total = 0;

            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec get_search_all_rent_total " +
                "@pName ");

            SqlParameter pName = new SqlParameter(@"pName", SqlDbType.VarChar, 255);
            pName.Direction = ParameterDirection.Input;
            pName.Value = searchRentDTO.name;
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

        public Pagination<SearchRentalStand> SearchRentalStand(SearchRentStandDTO searchRentStandDTO)
        {
            DataTable table = new DataTable();

            SQLCustomExecute sql = new SQLCustomExecute("exec get_search_all_rent_stand_page " +
                "@pRentCode, " +
                "@pRentName, " +
                "@pPage, " +
                "@pPerPage, " +
                "@pSortField, " +
                "@pSortType");

            SqlParameter pRentCode = new SqlParameter(@"pRentCode", SqlDbType.VarChar, 255);
            pRentCode.Direction = ParameterDirection.Input;
            pRentCode.Value = searchRentStandDTO.rentCode;
            sql.Parameters.Add(pRentCode);

            SqlParameter pRentName = new SqlParameter(@"pRentName", SqlDbType.VarChar, 255);
            pRentName.Direction = ParameterDirection.Input;
            pRentName.Value = searchRentStandDTO.rentName;
            sql.Parameters.Add(pRentName);

            SqlParameter pPage = new SqlParameter(@"pPage", SqlDbType.Int);
            pPage.Direction = ParameterDirection.Input;
            pPage.Value = searchRentStandDTO.pageInt;
            sql.Parameters.Add(pPage);

            SqlParameter pPerPage = new SqlParameter(@"pPerPage", SqlDbType.Int);
            pPerPage.Direction = ParameterDirection.Input;
            pPerPage.Value = searchRentStandDTO.perPage;
            sql.Parameters.Add(pPerPage);

            SqlParameter pSortField = new SqlParameter(@"pSortField", SqlDbType.Int);
            pSortField.Direction = ParameterDirection.Input;
            pSortField.Value = searchRentStandDTO.sortField;
            sql.Parameters.Add(pSortField);

            SqlParameter pSortType = new SqlParameter(@"pSortType", SqlDbType.VarChar, 1);
            pSortType.Direction = ParameterDirection.Input;
            pSortType.Value = searchRentStandDTO.sortType;
            sql.Parameters.Add(pSortType);

            table = sql.executeQueryWithReturnTable();

            Pagination<SearchRentalStand> pagination = new Pagination<SearchRentalStand>();


            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    SearchRentalStand data = new SearchRentalStand();
                    data.loadData(row);
                    pagination.data.Add(data);
                }
            }

            int total = GetTotalSearchRentStand(searchRentStandDTO);

            pagination.SetPagination(total, searchRentStandDTO.perPage, searchRentStandDTO.pageInt);

            return pagination;
        }

        public int GetTotalSearchRentStand(SearchRentStandDTO searchRentStandDTO)
        {
            int total = 0;

            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec get_search_all_rent_stand_total " +
                "@pRentCode, " +
                "@pRentName ");

            SqlParameter pRentCode = new SqlParameter(@"pRentCode", SqlDbType.VarChar, 255);
            pRentCode.Direction = ParameterDirection.Input;
            pRentCode.Value = searchRentStandDTO.rentCode;
            sql.Parameters.Add(pRentCode);

            SqlParameter pRentName = new SqlParameter(@"pRentName", SqlDbType.VarChar, 255);
            pRentName.Direction = ParameterDirection.Input;
            pRentName.Value = searchRentStandDTO.rentName;
            sql.Parameters.Add(pRentName);

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

        public Pagination<SearchHistoryPaidBill> SearchHistoryPaidBill(SearchBillDTO searchBillDTO)
        {
            DataTable table = new DataTable();

            SQLCustomExecute sql = new SQLCustomExecute("exec get_search_all_history_paid_bill_page " +
                "@pUserID, " +
                "@pBillCode, " +
                "@pStartDate, " +
                "@pEndDate, " +
                "@pPage, " +
                "@pPerPage, " +
                "@pSortField, " +
                "@pSortType");

            SqlParameter pUserID = new SqlParameter(@"pUserID", SqlDbType.Int);
            pUserID.Direction = ParameterDirection.Input;
            pUserID.Value = searchBillDTO.userID;
            sql.Parameters.Add(pUserID);

            SqlParameter pBillCode = new SqlParameter(@"pBillCode", SqlDbType.VarChar, 255);
            pBillCode.Direction = ParameterDirection.Input;
            pBillCode.Value = searchBillDTO.billCode;
            sql.Parameters.Add(pBillCode);

            SqlParameter pStartDate = new SqlParameter(@"pStartDate", SqlDbType.VarChar, 255);
            pStartDate.Direction = ParameterDirection.Input;
            pStartDate.Value = searchBillDTO.startDate;
            sql.Parameters.Add(pStartDate);

            SqlParameter pEndDate = new SqlParameter(@"pEndDate", SqlDbType.VarChar, 255);
            pEndDate.Direction = ParameterDirection.Input;
            pEndDate.Value = searchBillDTO.endDate;
            sql.Parameters.Add(pEndDate);

            SqlParameter pPage = new SqlParameter(@"pPage", SqlDbType.Int);
            pPage.Direction = ParameterDirection.Input;
            pPage.Value = searchBillDTO.pageInt;
            sql.Parameters.Add(pPage);

            SqlParameter pPerPage = new SqlParameter(@"pPerPage", SqlDbType.Int);
            pPerPage.Direction = ParameterDirection.Input;
            pPerPage.Value = searchBillDTO.perPage;
            sql.Parameters.Add(pPerPage);

            SqlParameter pSortField = new SqlParameter(@"pSortField", SqlDbType.Int);
            pSortField.Direction = ParameterDirection.Input;
            pSortField.Value = searchBillDTO.sortField;
            sql.Parameters.Add(pSortField);

            SqlParameter pSortType = new SqlParameter(@"pSortType", SqlDbType.VarChar, 1);
            pSortType.Direction = ParameterDirection.Input;
            pSortType.Value = searchBillDTO.sortType;
            sql.Parameters.Add(pSortType);

            table = sql.executeQueryWithReturnTable();

            Pagination<SearchHistoryPaidBill> pagination = new Pagination<SearchHistoryPaidBill>();


            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    SearchHistoryPaidBill data = new SearchHistoryPaidBill();
                    data.loadData(row);
                    pagination.data.Add(data);
                }
            }

            int total = GetTotalSearchHistoryPaidBill(searchBillDTO);

            pagination.SetPagination(total, searchBillDTO.perPage, searchBillDTO.pageInt);

            return pagination;
        }

        public int GetTotalSearchHistoryPaidBill(SearchBillDTO searchBillDTO)
        {
            int total = 0;

            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec get_search_all_history_paid_bill_total " +
                "@pUserID, " +
                "@pBillCode, " +
                "@pStartDate, " +
                "@pEndDate ");

            SqlParameter pUserID = new SqlParameter(@"pUserID", SqlDbType.Int);
            pUserID.Direction = ParameterDirection.Input;
            pUserID.Value = searchBillDTO.userID;
            sql.Parameters.Add(pUserID);

            SqlParameter pBillCode = new SqlParameter(@"pBillCode", SqlDbType.VarChar, 255);
            pBillCode.Direction = ParameterDirection.Input;
            pBillCode.Value = searchBillDTO.billCode;
            sql.Parameters.Add(pBillCode);

            SqlParameter pStartDate = new SqlParameter(@"pStartDate", SqlDbType.VarChar, 255);
            pStartDate.Direction = ParameterDirection.Input;
            pStartDate.Value = searchBillDTO.startDate;
            sql.Parameters.Add(pStartDate);

            SqlParameter pEndDate = new SqlParameter(@"pEndDate", SqlDbType.VarChar, 255);
            pEndDate.Direction = ParameterDirection.Input;
            pEndDate.Value = searchBillDTO.endDate;
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

        public List<DropdownAllData> GetZone()
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec get_dropdown_zone ");
            
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

        public List<DropdownAllData> GetZoneSub(int ZoneID)
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec get_dropdown_zone_sub " +
                 "@pZoneID ");

            SqlParameter pZoneID = new SqlParameter(@"pZoneID", SqlDbType.Int);
            pZoneID.Direction = ParameterDirection.Input;
            pZoneID.Value = ZoneID;
            sql.Parameters.Add(pZoneID);

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

        public List<DropdownAllData> GetRateAmount()
        {
            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec get_dropdown_rate_amount ");

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

        public Pagination<SearchMasterZoneSub> SearchZoneSub(SearchNameCenterDTO searchNameCenterDTO)
        {
            DataTable table = new DataTable();

            SQLCustomExecute sql = new SQLCustomExecute("exec get_search_all_zone_sub_page " +
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

            int total = GetTotalSearchZoneSub(searchNameCenterDTO);

            pagination.SetPagination(total, searchNameCenterDTO.perPage, searchNameCenterDTO.pageInt);

            return pagination;
        }

        public int GetTotalSearchZoneSub(SearchNameCenterDTO searchNameCenterDTO)
        {
            int total = 0;

            DataTable table = new DataTable();
            SQLCustomExecute sql = new SQLCustomExecute("exec get_search_all_zone_sub_total " +
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