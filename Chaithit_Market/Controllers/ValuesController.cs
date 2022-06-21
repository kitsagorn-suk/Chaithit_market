using Chaithit_Market.Core;
using Chaithit_Market.DTO;
using Chaithit_Market.Models;
using Chaithit_Market.Services;
using ExcelDataReader;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Cors;
using OfficeOpenXml;
using System.Data;
using System.Data.OleDb;
using System.Text.RegularExpressions;

namespace Chaithit_Market.Controllers
{
    [RoutePrefix("api")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ValuesController : ApiController
    {
        private SQLManager _sql = SQLManager.Instance;
        private double timestampNow = Utility.DateTimeToUnixTimestamp(DateTime.Now);

        #region Page Login
        [Route("1.0/login")]
        [HttpPost]
        public IHttpActionResult Login([FromBody] LoginRequestDTO loginRs)
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] == null ? "" : request.Headers["Authorization"]);
            string lang = (request.Headers["lang"] == null ? WebConfigurationManager.AppSettings["default_language"] : request.Headers["lang"]);
            string platform = request.Headers["platform"];
            string version = request.Headers["version"];

            try
            {
                string json = JsonConvert.SerializeObject(loginRs);
                int logID = _sql.InsertLogReceiveData("Login", json, timestampNow.ToString(), authHeader,
                    0, platform.ToLower());

                if (string.IsNullOrEmpty(loginRs.username))
                {
                    throw new Exception("invalid : username ");
                }
                if (string.IsNullOrEmpty(loginRs.password))
                {
                    throw new Exception("invalid : password ");
                }
                if (string.IsNullOrEmpty(loginRs.type))
                {
                    throw new Exception("invalid : type ");
                }
                else if (loginRs.type.ToLower() != "admin" && (loginRs.type.ToLower() != "customer"))
                {
                    throw new Exception("Type must be admin or customer ");
                }

                string username = loginRs.username;
                string password = loginRs.password;
                int type = loginRs.type.ToLower() == "admin" ? 1 : 2;

                LoginService srv = new LoginService();

                var obj = srv.Login(authHeader, lang, username, password, type, platform.ToLower(), logID);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Route("1.0/logout")]
        [HttpPost]
        public IHttpActionResult Logout()
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] == null ? "" : request.Headers["Authorization"]);
            string lang = (request.Headers["lang"] == null ? WebConfigurationManager.AppSettings["default_language"] : request.Headers["lang"]);
            string platform = "web";
            string businesscode = request.Headers["businesscode"];

            AuthenticationController _auth = AuthenticationController.Instance;
            AuthorizationModel data = _auth.ValidateHeader(authHeader, lang, true);

            try
            {
                LoginService srv = new LoginService();

                var obj = srv.Logout(authHeader, lang, data.user_id, platform.ToLower(), 1);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Route("1.0/changePassword")]
        [HttpPost]
        public IHttpActionResult ChangePassword(ChangePasswordDTO changePasswordDTO)
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] == null ? "" : request.Headers["Authorization"]);
            string lang = (request.Headers["lang"] == null ? WebConfigurationManager.AppSettings["default_language"] : request.Headers["lang"]);
            string platform = request.Headers["platform"];
            string version = request.Headers["version"];

            AuthenticationController _auth = AuthenticationController.Instance;
            AuthorizationModel data = _auth.ValidateHeader(authHeader, lang, true);

            try
            {
                string json = JsonConvert.SerializeObject(changePasswordDTO);
                int logID = _sql.InsertLogReceiveData("ChangePassword", json, timestampNow.ToString(), authHeader,
                    data.user_id, platform.ToLower());

                string currentDate = DateTime.Now.ToString("ddMMyyyy");

                string checkMissingOptional = "";

                if (changePasswordDTO.userID == 0)
                {
                    checkMissingOptional += "userID ";
                }
                if (string.IsNullOrEmpty(changePasswordDTO.passwordOld))
                {
                    checkMissingOptional += "passwordOld ";
                }
                if (string.IsNullOrEmpty(changePasswordDTO.passwordNew))
                {
                    checkMissingOptional += "passwordNew ";
                }
                if (string.IsNullOrEmpty(changePasswordDTO.passwordNewConfirm))
                {
                    checkMissingOptional += "passwordNewConfirm ";
                }
                if (changePasswordDTO.passwordNew != changePasswordDTO.passwordNewConfirm)
                {
                    checkMissingOptional += "passwordNew not same passwordNewConfirm";
                }

                if (checkMissingOptional != "")
                {
                    throw new Exception("Missing Parameter : " + checkMissingOptional);
                }

                SaveService srv = new SaveService();
                var obj = new object();
                obj = srv.ChangePasswordService(authHeader, lang, platform.ToLower(), logID, changePasswordDTO, data.user_id);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Route("1.0/forgetPassword")]
        [HttpPost]
        public IHttpActionResult ForgetPassword(ForgetPasswordDTO forgetPasswordDTO)
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] == null ? "" : request.Headers["Authorization"]);
            string lang = (request.Headers["lang"] == null ? WebConfigurationManager.AppSettings["default_language"] : request.Headers["lang"]);
            string platform = request.Headers["platform"];
            string version = request.Headers["version"];

            AuthenticationController _auth = AuthenticationController.Instance;
            AuthorizationModel data = _auth.ValidateHeader(authHeader, lang, true);

            try
            {
                string json = JsonConvert.SerializeObject(forgetPasswordDTO);
                int logID = _sql.InsertLogReceiveData("ForgetPassword", json, timestampNow.ToString(), authHeader,
                    data.user_id, platform.ToLower());

                string currentDate = DateTime.Now.ToString("ddMMyyyy");

                string checkMissingOptional = "";

                if (forgetPasswordDTO.userID == 0)
                {
                    checkMissingOptional += "userID ";
                }
                if (string.IsNullOrEmpty(forgetPasswordDTO.passwordNew))
                {
                    checkMissingOptional += "passwordNew ";
                }

                if (checkMissingOptional != "")
                {
                    throw new Exception("Missing Parameter : " + checkMissingOptional);
                }

                SaveService srv = new SaveService();
                var obj = new object();
                obj = srv.ForgetPasswordService(authHeader, lang, platform.ToLower(), logID, forgetPasswordDTO, data.user_id);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        #endregion

        #region Add User
        [Route("1.0/save/userProfile")]
        [HttpPost]
        public async Task<HttpResponseMessage> SaveUserProfile()
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] == null ? "" : request.Headers["Authorization"]);
            string lang = (request.Headers["lang"] ?? WebConfigurationManager.AppSettings["default_language"]);
            string platform = request.Headers["platform"];
            string version = request.Headers["version"];

            AuthenticationController _auth = AuthenticationController.Instance;
            AuthorizationModel data = _auth.ValidateHeader(authHeader, lang, true);

            try
            { 
                var obj = new Object();
                SaveUserProfileDTO saveUserProfileDTO = new SaveUserProfileDTO();
                string diskFolderPath = string.Empty;
                string subFolder = string.Empty;
                string keyName = string.Empty;
                string fileName = string.Empty;
                string newFileName = string.Empty;
                string fileURL = string.Empty;
                var fileSize = long.MinValue;

                var path = WebConfigurationManager.AppSettings["body_path"];
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                if (!Request.Content.IsMimeMultipartContent("form-data"))
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.UnsupportedMediaType));
                }

                MultipartFormDataStreamProvider streamProvider = new MultipartFormDataStreamProvider(path);
                await Request.Content.ReadAsMultipartAsync(streamProvider);

                foreach (var key in streamProvider.FormData.AllKeys)
                {
                    foreach (var val in streamProvider.FormData.GetValues(key))
                    {
                        if (key == "mode")
                        {
                            saveUserProfileDTO.mode = val;
                        }
                        if (key == "userProfileID")
                        {
                            saveUserProfileDTO.userProfileID = int.Parse(string.IsNullOrEmpty(val) ? "0" : val);
                        }
                        if (key == "userName")
                        {
                            saveUserProfileDTO.userName = val;
                        }
                        if (key == "password")
                        {
                            saveUserProfileDTO.password = val;
                        }
                        if (key == "firstName")
                        {
                            saveUserProfileDTO.firstName = val;
                        }
                        if (key == "lastName")
                        {
                            saveUserProfileDTO.lastName = val;
                        }
                        if (key == "mobile")
                        {
                            saveUserProfileDTO.mobile = val;
                        }
                        if (key == "position")
                        {
                            saveUserProfileDTO.position = val;
                        }
                        if (key == "statusEmp")
                        {
                            saveUserProfileDTO.statusEmp = int.Parse(string.IsNullOrEmpty(val) ? "0" : val);
                        }
                        if (key == "empType")
                        {
                            saveUserProfileDTO.empType = int.Parse(string.IsNullOrEmpty(val) ? "0" : val);
                        }
                        if (key == "recommender")
                        {
                            saveUserProfileDTO.recommender = val;
                        }
                        if (key == "startDate")
                        {
                            saveUserProfileDTO.startDate = val;
                        }
                    }
                }

                string json = JsonConvert.SerializeObject(saveUserProfileDTO);
                int logID = _sql.InsertLogReceiveData("SaveUserProfile", json, timestampNow.ToString(), authHeader,
                        data.user_id, platform.ToLower());

                #region msgCheck
                string checkMissingOptional = "";

                if (saveUserProfileDTO.mode.ToLower().Equals("insert"))
                {
                    if (saveUserProfileDTO.userProfileID != 0)
                    {
                        checkMissingOptional += "userProfileID Must 0 ";
                    }
                    if (string.IsNullOrEmpty(saveUserProfileDTO.userName))
                    {
                        checkMissingOptional += "userName ";
                    }
                    else
                    {
                        Regex r = new Regex("^[a-zA-Z0-9]*$");
                        if (!r.IsMatch(saveUserProfileDTO.userName))
                        {
                            checkMissingOptional += "username must English ";
                        }
                    }
                    if (string.IsNullOrEmpty(saveUserProfileDTO.password))
                    {
                        checkMissingOptional += "password ";
                    }
                    if (string.IsNullOrEmpty(saveUserProfileDTO.firstName))
                    {
                        checkMissingOptional += "firstName ";
                    }
                    if (string.IsNullOrEmpty(saveUserProfileDTO.lastName))
                    {
                        checkMissingOptional += "lastName ";
                    }
                    if (string.IsNullOrEmpty(saveUserProfileDTO.mobile))
                    {
                        checkMissingOptional += "mobile ";
                    }
                    if (saveUserProfileDTO.mobile.Count() < 9 || saveUserProfileDTO.mobile.Count() > 10)
                    {
                        checkMissingOptional += "mobile is incomplete ";
                    }
                    if (string.IsNullOrEmpty(saveUserProfileDTO.position))
                    {
                        checkMissingOptional += "position ";
                    }
                    if (saveUserProfileDTO.statusEmp == 0)
                    {
                        checkMissingOptional += "statusEmp ";
                    }
                    else
                    {
                        if (saveUserProfileDTO.statusEmp == 1 && saveUserProfileDTO.empType != 0)
                        {
                            checkMissingOptional += "statusEmp 1 empType must 0 ";
                        }
                        else if (saveUserProfileDTO.statusEmp == 2 && saveUserProfileDTO.empType == 0)
                        {
                            checkMissingOptional += "statusEmp 2 empType must not 0 ";
                        }
                        if (saveUserProfileDTO.statusEmp == 1)
                        {
                            if (string.IsNullOrEmpty(saveUserProfileDTO.startDate))
                            {
                                checkMissingOptional += "startDate ";
                            }
                        }
                    }
                }
                else if (saveUserProfileDTO.mode.ToLower().Equals("update"))
                {
                    if (saveUserProfileDTO.userProfileID == 0)
                    {
                        checkMissingOptional += "userProfileID ";
                    }
                    if (string.IsNullOrEmpty(saveUserProfileDTO.firstName))
                    {
                        checkMissingOptional += "firstName ";
                    }
                    if (string.IsNullOrEmpty(saveUserProfileDTO.lastName))
                    {
                        checkMissingOptional += "lastName ";
                    }
                    if (string.IsNullOrEmpty(saveUserProfileDTO.mobile))
                    {
                        checkMissingOptional += "mobile ";
                    }
                    if (saveUserProfileDTO.mobile.Count() < 9 || saveUserProfileDTO.mobile.Count() > 10)
                    {
                        checkMissingOptional += "mobile is incomplete ";
                    }
                    if (string.IsNullOrEmpty(saveUserProfileDTO.position))
                    {
                        checkMissingOptional += "position ";
                    }
                    if (saveUserProfileDTO.statusEmp == 0)
                    {
                        checkMissingOptional += "statusEmp ";
                    }
                    else
                    {
                        if (saveUserProfileDTO.statusEmp == 1 && saveUserProfileDTO.empType != 0)
                        {
                            checkMissingOptional += "statusEmp 1 empType must 0 ";
                        }
                        else if (saveUserProfileDTO.statusEmp == 2 && saveUserProfileDTO.empType == 0)
                        {
                            checkMissingOptional += "statusEmp 2 empType must not 0 ";
                        }
                        if (saveUserProfileDTO.statusEmp == 1)
                        {
                            if (string.IsNullOrEmpty(saveUserProfileDTO.startDate))
                            {
                                checkMissingOptional += "startDate ";
                            }
                        }
                    }
                }
                else if (saveUserProfileDTO.mode.ToLower().Equals("delete"))
                {
                    if (saveUserProfileDTO.userProfileID == 0)
                    {
                        checkMissingOptional += "userProfileID ";
                    }
                }
                else
                {
                    throw new Exception("Choose Mode Insert or Update or Delete");
                }

                if (checkMissingOptional != "")
                {
                    throw new Exception("Missing Parameter : " + checkMissingOptional);
                }
                #endregion

                ReturnIdModel userProfileID = new ReturnIdModel();
                SaveService srv = new SaveService();
                userProfileID = srv.SaveUserProfileService(authHeader, lang, platform.ToLower(), logID, saveUserProfileDTO, data.user_id);

                if (userProfileID.data != null && !userProfileID.data.id.Equals(0))
                {
                    foreach (MultipartFileData dataitem in streamProvider.FileData)
                    {
                        try
                        {
                            fileSize = new FileInfo(dataitem.LocalFileName).Length;
                            if (fileSize > 3100000)
                            {
                                throw new Exception("error file size limit 3.00 MB");
                            }
                        
                            subFolder = data.user_id + "\\ProFilePath";
                            diskFolderPath = string.Format(WebConfigurationManager.AppSettings["file_user_path"], subFolder);
                            

                            keyName = dataitem.Headers.ContentDisposition.Name.Replace("\"", "");
                            fileName = dataitem.Headers.ContentDisposition.FileName.Replace("\"", "");
                            newFileName = Guid.NewGuid() + Path.GetExtension(fileName);

                            var fullPath = Path.Combine(diskFolderPath, newFileName);
                            var fileInfo = new FileInfo(fullPath);
                            while (fileInfo.Exists)
                            {
                                newFileName = fileInfo.Name.Replace(fileInfo.Extension, "");
                                newFileName = newFileName + Guid.NewGuid().ToString() + fileInfo.Extension;

                                fullPath = Path.Combine(diskFolderPath, newFileName);
                                fileInfo = new FileInfo(fullPath);
                            }

                            fileURL = string.Format(WebConfigurationManager.AppSettings["file_user_url"], data.user_id + "/ProFilePath", newFileName);

                            if (!Directory.Exists(fileInfo.Directory.FullName))
                            {
                                Directory.CreateDirectory(fileInfo.Directory.FullName);
                            }
                            File.Move(dataitem.LocalFileName, fullPath);

                            if (userProfileID.data.id != 0 && !string.IsNullOrEmpty(newFileName))
                            {
                                UploadFileDTO uploadFileDTO = new UploadFileDTO();
                                uploadFileDTO.actionID = userProfileID.data.id;
                                uploadFileDTO.actionName = "user_profile";
                                uploadFileDTO.fileExtension = fileInfo.Extension.Split('.')[1];
                                uploadFileDTO.name = newFileName;
                                uploadFileDTO.url = fileURL;

                                _sql.InsertUploadFile(uploadFileDTO, data.user_id);
                            }
                        }
                        catch (Exception ex)
                        {
                            string message = ex.StackTrace;
                        }
                    }
                }
                else
                {
                    //insert fail
                }

                obj = userProfileID;

                return Request.CreateResponse(HttpStatusCode.OK, obj, Configuration.Formatters.JsonFormatter);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Route("1.0/get/userProfile")]
        [HttpPost]
        public IHttpActionResult GetUserProfile()
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] ?? "");
            string lang = (request.Headers["lang"] == null ? WebConfigurationManager.AppSettings["default_language"] : request.Headers["lang"]);
            string platform = request.Headers["platform"];
            string version = request.Headers["version"];

            AuthenticationController _auth = AuthenticationController.Instance;
            AuthorizationModel data = _auth.ValidateHeader(authHeader, lang, true);

            try
            {
                string json = JsonConvert.SerializeObject(data.user_id);
                int logID = _sql.InsertLogReceiveData("GetUserProfile", json, timestampNow.ToString(), authHeader,
                        data.user_id, platform.ToLower());

                GetService srv = new GetService();

                var obj = new object();

                if (data.user_id != 0)
                {
                    obj = srv.GetUserProfileService(authHeader, lang, platform.ToLower(), logID, data.user_id);
                }
                else
                {
                    throw new Exception("Missing Parameter : userProfileID");
                }

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Route("1.0/search/userProfile")]
        [HttpPost]
        public IHttpActionResult SearchUserProfile(SearchUserProfileDTO searchUserProfileDTO)
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] == null ? "" : request.Headers["Authorization"]);
            string lang = (request.Headers["lang"] == null ? WebConfigurationManager.AppSettings["default_language"] : request.Headers["lang"]);
            string platform = request.Headers["platform"];
            string version = request.Headers["version"];

            AuthenticationController _auth = AuthenticationController.Instance;
            AuthorizationModel data = _auth.ValidateHeader(authHeader, lang, true);

            try
            {
                string json = JsonConvert.SerializeObject(searchUserProfileDTO);
                int logID = _sql.InsertLogReceiveData("SearchUserProfile", json, timestampNow.ToString(), authHeader,
                    data.user_id, platform.ToLower());

                GetService srv = new GetService();

                var obj = new object();

                if (searchUserProfileDTO.pageInt.Equals(null) || searchUserProfileDTO.pageInt.Equals(0))
                {
                    throw new Exception("invalid : pageInt ");
                }

                if (searchUserProfileDTO.perPage.Equals(null) || searchUserProfileDTO.perPage.Equals(0))
                {
                    throw new Exception("invalid : perPage ");
                }

                if (searchUserProfileDTO.sortField > 4)
                {
                    throw new Exception("invalid : sortField " + searchUserProfileDTO.sortField);
                }

                if (!(searchUserProfileDTO.sortType == "a" || searchUserProfileDTO.sortType == "d" || searchUserProfileDTO.sortType == "A" || searchUserProfileDTO.sortType == "D" || searchUserProfileDTO.sortType == ""))
                {
                    throw new Exception("invalid sortType");
                }

                obj = srv.SearchUserProfileService(authHeader, lang, platform.ToLower(), logID, searchUserProfileDTO);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Route("1.0/get/user/rental")]
        [HttpPost]
        public IHttpActionResult GetUserRental(GetIDCenterDTO getIDCenterDTO)
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] ?? "");
            string lang = (request.Headers["lang"] == null ? WebConfigurationManager.AppSettings["default_language"] : request.Headers["lang"]);
            string platform = request.Headers["platform"];
            string version = request.Headers["version"];

            AuthenticationController _auth = AuthenticationController.Instance;
            AuthorizationModel data = _auth.ValidateHeader(authHeader, lang, true);

            try
            {
                string json = JsonConvert.SerializeObject(getIDCenterDTO);
                int logID = _sql.InsertLogReceiveData("GetUserRental", json, timestampNow.ToString(), authHeader,
                        data.user_id, platform.ToLower());

                GetService srv = new GetService();

                var obj = new object();

                if (getIDCenterDTO.id != 0)
                {
                    obj = srv.GetUserRentalService(authHeader, lang, platform.ToLower(), logID, getIDCenterDTO.id);
                }
                else
                {
                    throw new Exception("Missing Parameter : userID");
                }

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Route("1.0/upload/file")]
        [HttpPost]
        public async Task<HttpResponseMessage> UploadFileExcel()
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] ?? "");
            string lang = (request.Headers["lang"] ?? WebConfigurationManager.AppSettings["default_language"]);
            string platform = request.Headers["platform"];
            string version = request.Headers["version"];
            
            UploadModel value = new UploadModel();
            value.data = new _ServiceUploadData();

            string diskFolderPath = string.Empty;
            string keyName = string.Empty;
            string fileName = string.Empty;
            string newFileName = string.Empty;
            string fileExtension = string.Empty;
            var fileSize = long.MinValue;

            var path = WebConfigurationManager.AppSettings["body_path"];
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (!Request.Content.IsMimeMultipartContent("form-data"))
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.UnsupportedMediaType));
            }

            MultipartFormDataStreamProvider streamProvider = new MultipartFormDataStreamProvider(path);

            await Request.Content.ReadAsMultipartAsync(streamProvider);

            foreach (MultipartFileData fileData in streamProvider.FileData)
            {
                fileName = fileData.Headers.ContentDisposition.FileName.Replace("\"", "");
                fileSize = new FileInfo(fileData.LocalFileName).Length;
                fileExtension = Path.GetExtension(fileName);

                if ((fileExtension != ".xls") && (fileExtension != ".xlsx"))
                {
                    value.success = false;
                    value.data.file_size = fileSize.ToString();
                    value.msg = new MsgModel() { code = 0, text = "กรุณาเลือกไฟล์นามสกุล xls หรือ xlsx", topic = "ไม่สำเร็จ" };
                }
                else
                {
                    
                    if (fileSize > 3100000)
                    {
                        throw new Exception("error file size limit 3.00 MB");
                    }

                    
                    newFileName = Guid.NewGuid() + Path.GetExtension(fileName);

                    diskFolderPath = WebConfigurationManager.AppSettings["file_electric_path"];

                    var fullPath = Path.Combine(diskFolderPath, newFileName);
                    var fileInfo = new FileInfo(fullPath);
                    while (fileInfo.Exists)
                    {
                        newFileName = fileInfo.Name.Replace(fileInfo.Extension, "");
                        newFileName = newFileName + Guid.NewGuid().ToString() + fileInfo.Extension;

                        fullPath = Path.Combine(diskFolderPath, newFileName);
                        fileInfo = new FileInfo(fullPath);
                    }

                    if (!Directory.Exists(fileInfo.Directory.FullName))
                    {
                        Directory.CreateDirectory(fileInfo.Directory.FullName);
                    }
                    File.Move(fileData.LocalFileName, fullPath);

                    if (!File.Exists(fullPath))
                    {
                        value.success = false;
                        value.data.file_size = fileSize.ToString();
                        value.msg = new MsgModel() { code = 0, text = "อัพโหลดไม่สำเร็จ", topic = "ไม่สำเร็จ" };
                    }
                    else
                    {
                        DataTable dt = new DataTable();
                        string strConnection;

                        if (Path.GetExtension(fullPath.ToLower()) == ".xls")
                            strConnection = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fullPath + ";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1;\"";
                        else
                            strConnection = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fullPath + ";Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\"";
                        OleDbConnection exconn = new OleDbConnection(strConnection);
                        exconn.Open();
                        OleDbDataAdapter da = new OleDbDataAdapter("SELECT GroupName, DSN, Value, ValueTimeStamp, StatusTextEN FROM[ต้นฉบับ$]", exconn);
                        da.Fill(dt);
                        exconn.Close();
                        if (dt.AsEnumerable().Any())
                        {
                            List<ElectricModel> electricModel = dt.AsEnumerable().Select(c => new ElectricModel()
                            {
                                GroupName = c["GroupName"] != DBNull.Value ? c.Field<string>("GroupName") : "",
                                DSN = c["DSN"] != DBNull.Value ? c.Field<string>("DSN") : "",
                                Value = c["Value"] != DBNull.Value ? (decimal)(c.Field<decimal>("Value")) : 0,
                                ValueTimeStamp = c["ValueTimeStamp"] != DBNull.Value ? c.Field<DateTime>("ValueTimeStamp") : DateTime.Now,
                                StatusTextEN = c["StatusTextEN"] != DBNull.Value ? c.Field<string>("StatusTextEN") : ""

                            }).Where(c => c.DSN != "").ToList();

                            //srv.RenewHollidayFromUpload(hollidays, user);
                            //return "";
                        }
                        else
                        {
                            //return "File has no data!!";
                        }

                        value.success = true;
                        value.data.img_url = "";
                        value.data.file_size = fileSize.ToString();
                        value.msg = new MsgModel() { code = 0, text = "อัพโหลดสำเร็จ", topic = "สำเร็จ" };
                    }
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, value, Configuration.Formatters.JsonFormatter);
        }

        [Route("1.0/delete/file")]
        [HttpPost]
        public async Task<HttpResponseMessage> DeleteFile()
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] == null ? "" : request.Headers["Authorization"]);
            string lang = (request.Headers["lang"] ?? WebConfigurationManager.AppSettings["default_language"]);
            string platform = request.Headers["platform"];
            string version = request.Headers["version"];

            AuthenticationController _auth = AuthenticationController.Instance;
            AuthorizationModel data = _auth.ValidateHeader(authHeader, lang, true);

            try
            { 
                UploadModel value = new UploadModel();
                value.data = new _ServiceUploadData();

                string fullPath = "", authorsFile = "6ad16852-7fd1-45d9-b1a5-c9c844a4c461.jpg", subFolder = "";
                subFolder = data.user_id + "\\ProFilePath";
                fullPath = string.Format(WebConfigurationManager.AppSettings["file_user_path"], subFolder);
                

                if (File.Exists(Path.Combine(fullPath, authorsFile)))
                {
                    // If file found, delete it    
                    File.Delete(Path.Combine(fullPath, authorsFile));
                    Console.WriteLine("File deleted.");
                }
                
                if (!File.Exists(fullPath))
                {
                    value.success = false;
                    //value.data.file_size = fileSize.ToString();
                    value.msg = new MsgModel() { code = 0, text = "อัพโหลดไม่สำเร็จ", topic = "ไม่สำเร็จ" };
                }
                else
                {
                    value.success = true;
                    //value.data.img_url = fileURL;
                    //value.data.file_size = fileSize.ToString();
                    value.msg = new MsgModel() { code = 0, text = "อัพโหลดสำเร็จ", topic = "สำเร็จ" };
                }
                

                return Request.CreateResponse(HttpStatusCode.OK, value, Configuration.Formatters.JsonFormatter);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }
        
        [Route("1.0/search/unit/stand")]
        [HttpPost]
        public IHttpActionResult SearchUnitStand(SearchUnitstandDTO searchUnitstandDTO)
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] == null ? "" : request.Headers["Authorization"]);
            string lang = (request.Headers["lang"] == null ? WebConfigurationManager.AppSettings["default_language"] : request.Headers["lang"]);
            string platform = request.Headers["platform"];
            string version = request.Headers["version"];

            AuthenticationController _auth = AuthenticationController.Instance;
            AuthorizationModel data = _auth.ValidateHeader(authHeader, lang, true);

            try
            {
                string json = JsonConvert.SerializeObject(searchUnitstandDTO);
                int logID = _sql.InsertLogReceiveData("SearchUnitStand", json, timestampNow.ToString(), authHeader,
                    data.user_id, platform.ToLower());

                GetService srv = new GetService();

                var obj = new object();

                if (searchUnitstandDTO.pageInt.Equals(null) || searchUnitstandDTO.pageInt.Equals(0))
                {
                    throw new Exception("invalid : pageInt ");
                }

                if (searchUnitstandDTO.perPage.Equals(null) || searchUnitstandDTO.perPage.Equals(0))
                {
                    throw new Exception("invalid : perPage ");
                }

                if (searchUnitstandDTO.sortField > 4)
                {
                    throw new Exception("invalid : sortField " + searchUnitstandDTO.sortField);
                }

                if (!(searchUnitstandDTO.sortType == "a" || searchUnitstandDTO.sortType == "d" || searchUnitstandDTO.sortType == "A" || searchUnitstandDTO.sortType == "D" || searchUnitstandDTO.sortType == ""))
                {
                    throw new Exception("invalid sortType");
                }

                obj = srv.SearchUnitStandService(authHeader, lang, platform.ToLower(), logID, searchUnitstandDTO);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Route("1.0/search/history/paidBill/admin")]
        [HttpPost]
        public IHttpActionResult SearchHistoryPaidBillAdmin(SearchHistoryAdminBillDTO searchHistoryAdminBillDTO)
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] == null ? "" : request.Headers["Authorization"]);
            string lang = (request.Headers["lang"] == null ? WebConfigurationManager.AppSettings["default_language"] : request.Headers["lang"]);
            string platform = request.Headers["platform"];
            string version = request.Headers["version"];

            AuthenticationController _auth = AuthenticationController.Instance;
            AuthorizationModel data = _auth.ValidateHeader(authHeader, lang, true);

            try
            {
                string json = JsonConvert.SerializeObject(searchHistoryAdminBillDTO);
                int logID = _sql.InsertLogReceiveData("SearchHistoryPaidBillAdmin", json, timestampNow.ToString(), authHeader,
                    data.user_id, platform.ToLower());

                GetService srv = new GetService();

                var obj = new object();

                if (searchHistoryAdminBillDTO.pageInt.Equals(null) || searchHistoryAdminBillDTO.pageInt.Equals(0))
                {
                    throw new Exception("invalid : pageInt ");
                }

                if (searchHistoryAdminBillDTO.perPage.Equals(null) || searchHistoryAdminBillDTO.perPage.Equals(0))
                {
                    throw new Exception("invalid : perPage ");
                }

                if (searchHistoryAdminBillDTO.sortField > 4)
                {
                    throw new Exception("invalid : sortField " + searchHistoryAdminBillDTO.sortField);
                }

                if (!(searchHistoryAdminBillDTO.sortType == "a" || searchHistoryAdminBillDTO.sortType == "d" || searchHistoryAdminBillDTO.sortType == "A" || searchHistoryAdminBillDTO.sortType == "D" || searchHistoryAdminBillDTO.sortType == ""))
                {
                    throw new Exception("invalid sortType");
                }

                obj = srv.SearchHistoryPaidBillAdminService(authHeader, lang, platform.ToLower(), logID, searchHistoryAdminBillDTO);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Route("1.0/search/history/paidBill/user")]
        [HttpPost]
        public IHttpActionResult SearchHistoryPaidBillUser(SearchHistoryUserBillDTO searchHistoryUserBillDTO)
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] == null ? "" : request.Headers["Authorization"]);
            string lang = (request.Headers["lang"] == null ? WebConfigurationManager.AppSettings["default_language"] : request.Headers["lang"]);
            string platform = request.Headers["platform"];
            string version = request.Headers["version"];

            AuthenticationController _auth = AuthenticationController.Instance;
            AuthorizationModel data = _auth.ValidateHeader(authHeader, lang, true);

            try
            {
                string json = JsonConvert.SerializeObject(searchHistoryUserBillDTO);
                int logID = _sql.InsertLogReceiveData("SearchHistoryPaidBillUser", json, timestampNow.ToString(), authHeader,
                    data.user_id, platform.ToLower());

                GetService srv = new GetService();

                var obj = new object();

                if (searchHistoryUserBillDTO.userID.Equals(0))
                {
                    throw new Exception("invalid : userID ");
                }

                if (searchHistoryUserBillDTO.pageInt.Equals(null) || searchHistoryUserBillDTO.pageInt.Equals(0))
                {
                    throw new Exception("invalid : pageInt ");
                }

                if (searchHistoryUserBillDTO.perPage.Equals(null) || searchHistoryUserBillDTO.perPage.Equals(0))
                {
                    throw new Exception("invalid : perPage ");
                }

                if (searchHistoryUserBillDTO.sortField > 4)
                {
                    throw new Exception("invalid : sortField " + searchHistoryUserBillDTO.sortField);
                }

                if (!(searchHistoryUserBillDTO.sortType == "a" || searchHistoryUserBillDTO.sortType == "d" || searchHistoryUserBillDTO.sortType == "A" || searchHistoryUserBillDTO.sortType == "D" || searchHistoryUserBillDTO.sortType == ""))
                {
                    throw new Exception("invalid sortType");
                }

                obj = srv.SearchHistoryPaidBillUserService(authHeader, lang, platform.ToLower(), logID, searchHistoryUserBillDTO);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Route("1.0/search/history/paidBill/user/noPage")]
        [HttpPost]
        public IHttpActionResult SearchHistoryPaidBillUserNoPage(SearchHistoryUserBillDTO searchHistoryUserBillDTO)
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] == null ? "" : request.Headers["Authorization"]);
            string lang = (request.Headers["lang"] == null ? WebConfigurationManager.AppSettings["default_language"] : request.Headers["lang"]);
            string platform = request.Headers["platform"];
            string version = request.Headers["version"];

            AuthenticationController _auth = AuthenticationController.Instance;
            AuthorizationModel data = _auth.ValidateHeader(authHeader, lang, true);

            try
            {
                string json = JsonConvert.SerializeObject(searchHistoryUserBillDTO);
                int logID = _sql.InsertLogReceiveData("SearchHistoryPaidBillUserNoPage", json, timestampNow.ToString(), authHeader,
                    data.user_id, platform.ToLower());

                GetService srv = new GetService();

                var obj = new object();

                if (searchHistoryUserBillDTO.userID.Equals(0))
                {
                    throw new Exception("invalid : userID ");
                }

                if (searchHistoryUserBillDTO.pageInt.Equals(null) || searchHistoryUserBillDTO.pageInt.Equals(0))
                {
                    throw new Exception("invalid : pageInt ");
                }

                if (searchHistoryUserBillDTO.perPage.Equals(null) || searchHistoryUserBillDTO.perPage.Equals(0))
                {
                    throw new Exception("invalid : perPage ");
                }

                if (searchHistoryUserBillDTO.sortField > 4)
                {
                    throw new Exception("invalid : sortField " + searchHistoryUserBillDTO.sortField);
                }

                if (!(searchHistoryUserBillDTO.sortType == "a" || searchHistoryUserBillDTO.sortType == "d" || searchHistoryUserBillDTO.sortType == "A" || searchHistoryUserBillDTO.sortType == "D" || searchHistoryUserBillDTO.sortType == ""))
                {
                    throw new Exception("invalid sortType");
                }

                obj = srv.SearchHistoryPaidBillUserNopageService(authHeader, lang, platform.ToLower(), logID, searchHistoryUserBillDTO);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Route("1.0/search/outstanding/bill/user")]
        [HttpPost]
        public IHttpActionResult SearchOutStandingBillUser(SearchHistoryUserBillDTO searchHistoryUserBillDTO)
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] == null ? "" : request.Headers["Authorization"]);
            string lang = (request.Headers["lang"] == null ? WebConfigurationManager.AppSettings["default_language"] : request.Headers["lang"]);
            string platform = request.Headers["platform"];
            string version = request.Headers["version"];

            AuthenticationController _auth = AuthenticationController.Instance;
            AuthorizationModel data = _auth.ValidateHeader(authHeader, lang, true);

            try
            {
                string json = JsonConvert.SerializeObject(searchHistoryUserBillDTO);
                int logID = _sql.InsertLogReceiveData("SearchOutStandingBillUser", json, timestampNow.ToString(), authHeader,
                    data.user_id, platform.ToLower());

                GetService srv = new GetService();

                var obj = new object();

                if (searchHistoryUserBillDTO.userID.Equals(0))
                {
                    throw new Exception("invalid : userID ");
                }

                if (searchHistoryUserBillDTO.pageInt.Equals(null) || searchHistoryUserBillDTO.pageInt.Equals(0))
                {
                    throw new Exception("invalid : pageInt ");
                }

                if (searchHistoryUserBillDTO.perPage.Equals(null) || searchHistoryUserBillDTO.perPage.Equals(0))
                {
                    throw new Exception("invalid : perPage ");
                }

                if (searchHistoryUserBillDTO.sortField > 4)
                {
                    throw new Exception("invalid : sortField " + searchHistoryUserBillDTO.sortField);
                }

                if (!(searchHistoryUserBillDTO.sortType == "a" || searchHistoryUserBillDTO.sortType == "d" || searchHistoryUserBillDTO.sortType == "A" || searchHistoryUserBillDTO.sortType == "D" || searchHistoryUserBillDTO.sortType == ""))
                {
                    throw new Exception("invalid sortType");
                }

                obj = srv.SearchOutStandingBillUserService(authHeader, lang, platform.ToLower(), logID, searchHistoryUserBillDTO);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Route("1.0/get/outstanding/bill/user/total")]
        [HttpPost]
        public IHttpActionResult GetOutStandingBillUserTotal(GetHistoryUserBillDTO getHistoryUserBillDTO)
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] ?? "");
            string lang = (request.Headers["lang"] == null ? WebConfigurationManager.AppSettings["default_language"] : request.Headers["lang"]);
            string platform = request.Headers["platform"];
            string version = request.Headers["version"];

            AuthenticationController _auth = AuthenticationController.Instance;
            AuthorizationModel data = _auth.ValidateHeader(authHeader, lang, true);

            try
            {
                string json = JsonConvert.SerializeObject(getHistoryUserBillDTO);
                int logID = _sql.InsertLogReceiveData("GetOutStandingBillUserTotal", json, timestampNow.ToString(), authHeader,
                        data.user_id, platform.ToLower());

                GetService srv = new GetService();

                var obj = new object();

                if (getHistoryUserBillDTO.userID != 0)
                {
                    obj = srv.GetOutStandingBillUserTotalService(authHeader, lang, platform.ToLower(), logID, getHistoryUserBillDTO);
                }
                else
                {
                    throw new Exception("Missing Parameter : userID");
                }

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Route("1.0/search/manage/renter")]
        [HttpPost]
        public IHttpActionResult SearchManageRenter(SearchManageRenterDTO searchManageRenterDTO)
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] == null ? "" : request.Headers["Authorization"]);
            string lang = (request.Headers["lang"] == null ? WebConfigurationManager.AppSettings["default_language"] : request.Headers["lang"]);
            string platform = request.Headers["platform"];
            string version = request.Headers["version"];

            AuthenticationController _auth = AuthenticationController.Instance;
            AuthorizationModel data = _auth.ValidateHeader(authHeader, lang, true);

            try
            {
                string json = JsonConvert.SerializeObject(searchManageRenterDTO);
                int logID = _sql.InsertLogReceiveData("SearchManageRenter", json, timestampNow.ToString(), authHeader,
                    data.user_id, platform.ToLower());

                GetService srv = new GetService();

                var obj = new object();

                searchManageRenterDTO.showRenter = searchManageRenterDTO.showRenter.ToLower();

                if (string.IsNullOrEmpty(searchManageRenterDTO.type))
                {
                    throw new Exception("Missing Parameter : type");
                }
                else if(searchManageRenterDTO.type.ToLower() != "employee" && searchManageRenterDTO.type.ToLower() != "renter")
                {
                    throw new Exception("Type must value employee or renter");
                }

                if (searchManageRenterDTO.pageInt.Equals(null) || searchManageRenterDTO.pageInt.Equals(0))
                {
                    throw new Exception("invalid : pageInt ");
                }

                if (searchManageRenterDTO.perPage.Equals(null) || searchManageRenterDTO.perPage.Equals(0))
                {
                    throw new Exception("invalid : perPage ");
                }

                if (searchManageRenterDTO.sortField > 7)
                {
                    throw new Exception("invalid : sortField " + searchManageRenterDTO.sortField);
                }

                if (!(searchManageRenterDTO.sortType == "a" || searchManageRenterDTO.sortType == "d" || searchManageRenterDTO.sortType == "A" || searchManageRenterDTO.sortType == "D" || searchManageRenterDTO.sortType == ""))
                {
                    throw new Exception("invalid sortType");
                }

                obj = srv.SearchManageRenterService(authHeader, lang, platform.ToLower(), logID, searchManageRenterDTO);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Route("1.0/search/manage/unit")]
        [HttpPost]
        public IHttpActionResult SearchManageUnit(SearchManageUnitDTO searchManageUnitDTO)
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] == null ? "" : request.Headers["Authorization"]);
            string lang = (request.Headers["lang"] == null ? WebConfigurationManager.AppSettings["default_language"] : request.Headers["lang"]);
            string platform = request.Headers["platform"];
            string version = request.Headers["version"];

            AuthenticationController _auth = AuthenticationController.Instance;
            AuthorizationModel data = _auth.ValidateHeader(authHeader, lang, true);

            try
            {
                string json = JsonConvert.SerializeObject(searchManageUnitDTO);
                int logID = _sql.InsertLogReceiveData("SearchManageUnit", json, timestampNow.ToString(), authHeader,
                    data.user_id, platform.ToLower());

                GetService srv = new GetService();

                var obj = new object();

                if (searchManageUnitDTO.pageInt.Equals(null) || searchManageUnitDTO.pageInt.Equals(0))
                {
                    throw new Exception("invalid : pageInt ");
                }

                if (searchManageUnitDTO.perPage.Equals(null) || searchManageUnitDTO.perPage.Equals(0))
                {
                    throw new Exception("invalid : perPage ");
                }

                if (searchManageUnitDTO.sortField > 4)
                {
                    throw new Exception("invalid : sortField " + searchManageUnitDTO.sortField);
                }

                if (!(searchManageUnitDTO.sortType == "a" || searchManageUnitDTO.sortType == "d" || searchManageUnitDTO.sortType == "A" || searchManageUnitDTO.sortType == "D" || searchManageUnitDTO.sortType == ""))
                {
                    throw new Exception("invalid sortType");
                }

                obj = srv.SearchManageUnitService(authHeader, lang, platform.ToLower(), logID, searchManageUnitDTO);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Route("1.0/get/dropdown/user")]
        [HttpPost]
        public IHttpActionResult GetDropdownUser(GetDropdownIsAllDTO getDropdownIsAllDTO)
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] == null ? "" : request.Headers["Authorization"]);
            string lang = (request.Headers["lang"] == null ? WebConfigurationManager.AppSettings["default_language"] : request.Headers["lang"]);
            string platform = request.Headers["platform"];
            string version = request.Headers["version"];

            AuthenticationController _auth = AuthenticationController.Instance;
            AuthorizationModel data = _auth.ValidateHeader(authHeader, lang, true);

            try
            {
                string json = JsonConvert.SerializeObject(getDropdownIsAllDTO);
                int logID = _sql.InsertLogReceiveData("GetDropdownUser", json, timestampNow.ToString(), authHeader,
                    data.user_id, platform.ToLower());

                if (getDropdownIsAllDTO.isAll.ToLower() != "true" && getDropdownIsAllDTO.isAll.ToLower() != "false")
                {
                    throw new Exception("isAll value must be true or false");
                }

                GetService srv = new GetService();
                var obj = srv.GetDropdownUserService(authHeader, lang, platform.ToLower(), logID, getDropdownIsAllDTO.isAll.ToLower());

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Route("1.0/search/manage/bill")]
        [HttpPost]
        public IHttpActionResult SearchManageBill(SearchManageBillDTO searchManageBillDTO)
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] == null ? "" : request.Headers["Authorization"]);
            string lang = (request.Headers["lang"] == null ? WebConfigurationManager.AppSettings["default_language"] : request.Headers["lang"]);
            string platform = request.Headers["platform"];
            string version = request.Headers["version"];

            AuthenticationController _auth = AuthenticationController.Instance;
            AuthorizationModel data = _auth.ValidateHeader(authHeader, lang, true);

            try
            {
                string json = JsonConvert.SerializeObject(searchManageBillDTO);
                int logID = _sql.InsertLogReceiveData("SearchManageBill", json, timestampNow.ToString(), authHeader,
                    data.user_id, platform.ToLower());

                GetService srv = new GetService();

                var obj = new object();

                if (searchManageBillDTO.pageInt.Equals(null) || searchManageBillDTO.pageInt.Equals(0))
                {
                    throw new Exception("invalid : pageInt ");
                }

                if (searchManageBillDTO.perPage.Equals(null) || searchManageBillDTO.perPage.Equals(0))
                {
                    throw new Exception("invalid : perPage ");
                }

                if (searchManageBillDTO.sortField > 4)
                {
                    throw new Exception("invalid : sortField " + searchManageBillDTO.sortField);
                }

                if (!(searchManageBillDTO.sortType == "a" || searchManageBillDTO.sortType == "d" || searchManageBillDTO.sortType == "A" || searchManageBillDTO.sortType == "D" || searchManageBillDTO.sortType == ""))
                {
                    throw new Exception("invalid sortType");
                }

                obj = srv.SearchManageBillService(authHeader, lang, platform.ToLower(), logID, searchManageBillDTO);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Route("1.0/get/renter/byUserID")]
        [HttpPost]
        public IHttpActionResult GetRenterByUserID(GetIDCenterDTO getIDCenterDTO)
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] ?? "");
            string lang = (request.Headers["lang"] == null ? WebConfigurationManager.AppSettings["default_language"] : request.Headers["lang"]);
            string platform = request.Headers["platform"];
            string version = request.Headers["version"];

            AuthenticationController _auth = AuthenticationController.Instance;
            AuthorizationModel data = _auth.ValidateHeader(authHeader, lang, true);

            try
            {
                string json = JsonConvert.SerializeObject(getIDCenterDTO);
                int logID = _sql.InsertLogReceiveData("GetRenterByUserID", json, timestampNow.ToString(), authHeader,
                        data.user_id, platform.ToLower());

                GetService srv = new GetService();

                var obj = new object();

                if (getIDCenterDTO.id != 0)
                {
                    obj = srv.GetRenterByUserIDService(authHeader, lang, platform.ToLower(), logID, getIDCenterDTO.id);
                }
                else
                {
                    throw new Exception("Missing Parameter : userID");
                }

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Route("1.0/save/tranPay")]
        [HttpPost]
        public async Task<HttpResponseMessage> SaveTranPay()
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] == null ? "" : request.Headers["Authorization"]);
            string lang = (request.Headers["lang"] ?? WebConfigurationManager.AppSettings["default_language"]);
            string platform = request.Headers["platform"];
            string version = request.Headers["version"];

            AuthenticationController _auth = AuthenticationController.Instance;
            AuthorizationModel data = _auth.ValidateHeader(authHeader, lang, true);

            try
            {
                var obj = new Object();
                SaveTranPayDTO saveTranPayDTO = new SaveTranPayDTO();
                string diskFolderPath = string.Empty;
                string subFolder = string.Empty;
                string keyName = string.Empty;
                string fileName = string.Empty;
                string newFileName = string.Empty;
                string fileURL = string.Empty;
                var fileSize = long.MinValue;

                var path = WebConfigurationManager.AppSettings["body_path"];
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                if (!Request.Content.IsMimeMultipartContent("form-data"))
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.UnsupportedMediaType));
                }

                MultipartFormDataStreamProvider streamProvider = new MultipartFormDataStreamProvider(path);
                await Request.Content.ReadAsMultipartAsync(streamProvider);

                bool havefile = false;
                foreach (MultipartFileData dataitem in streamProvider.FileData)
                {
                    fileName = dataitem.Headers.ContentDisposition.FileName.Replace("\"", "");
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        havefile = true;
                    }
                }

                if (!havefile)
                {
                    throw new Exception("กรุณาเเนบรูป");
                }

                foreach (var key in streamProvider.FormData.AllKeys)
                {
                    foreach (var val in streamProvider.FormData.GetValues(key))
                    {
                        if (key == "billID")
                        {
                            saveTranPayDTO.billID = val;
                        }
                        if (key == "payAmount")
                        {
                            saveTranPayDTO.payAmount = decimal.Parse(string.IsNullOrEmpty(val) ? "0" : val);
                        }
                        if (key == "comment")
                        {
                            saveTranPayDTO.comment = val;
                        }
                    }
                }

                string json = JsonConvert.SerializeObject(saveTranPayDTO);
                int logID = _sql.InsertLogReceiveData("SaveTranPay", json, timestampNow.ToString(), authHeader,
                        data.user_id, platform.ToLower());

                #region msgCheck
                string checkMissingOptional = "";
                
                if (string.IsNullOrEmpty(saveTranPayDTO.billID))
                {
                    checkMissingOptional += "saveTranPayDTO  ";
                }
                if (saveTranPayDTO.payAmount == 0)
                {
                    checkMissingOptional += "payAmount Must Not 0";
                }
                
                if (checkMissingOptional != "")
                {
                    throw new Exception("Missing Parameter : " + checkMissingOptional);
                }
                #endregion

                ReturnIdModel payID = new ReturnIdModel();
                SaveService srv = new SaveService();
                payID = srv.SaveTranPayService(authHeader, lang, platform.ToLower(), logID, saveTranPayDTO, data.user_id);

                if (payID.data != null && !payID.data.id.Equals(0))
                {
                    foreach (MultipartFileData dataitem in streamProvider.FileData)
                    {
                        try
                        {
                            
                            fileSize = new FileInfo(dataitem.LocalFileName).Length;
                            if (fileSize > 3100000)
                            {
                                throw new Exception("error file size limit 3.00 MB");
                            }

                            subFolder = data.user_id + "\\PayPath";
                            diskFolderPath = string.Format(WebConfigurationManager.AppSettings["file_pay_path"], subFolder);


                            keyName = dataitem.Headers.ContentDisposition.Name.Replace("\"", "");
                            fileName = dataitem.Headers.ContentDisposition.FileName.Replace("\"", "");
                            
                            newFileName = Guid.NewGuid() + Path.GetExtension(fileName);

                            var fullPath = Path.Combine(diskFolderPath, newFileName);
                            var fileInfo = new FileInfo(fullPath);
                            while (fileInfo.Exists)
                            {
                                newFileName = fileInfo.Name.Replace(fileInfo.Extension, "");
                                newFileName = newFileName + Guid.NewGuid().ToString() + fileInfo.Extension;

                                fullPath = Path.Combine(diskFolderPath, newFileName);
                                fileInfo = new FileInfo(fullPath);
                            }

                            fileURL = string.Format(WebConfigurationManager.AppSettings["file_pay_url"], data.user_id + "/PayPath", newFileName);

                            if (!Directory.Exists(fileInfo.Directory.FullName))
                            {
                                Directory.CreateDirectory(fileInfo.Directory.FullName);
                            }
                            File.Move(dataitem.LocalFileName, fullPath);

                            if (payID.data.id != 0 && !string.IsNullOrEmpty(newFileName))
                            {
                                UploadFileDTO uploadFileDTO = new UploadFileDTO();
                                uploadFileDTO.actionID = payID.data.id;
                                uploadFileDTO.actionName = "transaction_pay";
                                uploadFileDTO.fileExtension = fileInfo.Extension.Split('.')[1];
                                uploadFileDTO.name = newFileName;
                                uploadFileDTO.url = fileURL;

                                _sql.InsertUploadFile(uploadFileDTO, data.user_id);
                            }
                        }
                        catch (Exception ex)
                        {
                            string message = ex.StackTrace;
                        }
                    }
                }
                else
                {
                    //insert fail
                }

                obj = payID;

                return Request.CreateResponse(HttpStatusCode.OK, obj, Configuration.Formatters.JsonFormatter);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Route("1.0/update/admin/approve")]
        [HttpPost]
        public IHttpActionResult UpdateAdminApprove(UpdateAdminApproveDTO updateAdminApproveDTO)
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] == null ? "" : request.Headers["Authorization"]);
            string lang = (request.Headers["lang"] == null ? WebConfigurationManager.AppSettings["default_language"] : request.Headers["lang"]);
            string platform = request.Headers["platform"];
            string version = request.Headers["version"];

            AuthenticationController _auth = AuthenticationController.Instance;
            AuthorizationModel data = _auth.ValidateHeader(authHeader, lang, true);

            try
            {
                string json = JsonConvert.SerializeObject(updateAdminApproveDTO);
                int logID = _sql.InsertLogReceiveData("UpdateAdminApprove", json, timestampNow.ToString(), authHeader,
                    data.user_id, platform.ToLower());

                string currentDate = DateTime.Now.ToString("ddMMyyyy");

                string checkMissingOptional = "";
                
                if (updateAdminApproveDTO.id == 0)
                {
                    checkMissingOptional += "billID ";
                }
                
                if (checkMissingOptional != "")
                {
                    throw new Exception("Missing Parameter : " + checkMissingOptional);
                }

                SaveService srv = new SaveService();
                var obj = new object();
                obj = srv.UpdateAdminApproveService(authHeader, lang, platform.ToLower(), logID, updateAdminApproveDTO, data.user_id);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Route("1.0/get/tranpay/image")]
        [HttpPost]
        public IHttpActionResult GetTranPayImage(GetIDCenterDTO getIDCenterDTO)
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] ?? "");
            string lang = (request.Headers["lang"] == null ? WebConfigurationManager.AppSettings["default_language"] : request.Headers["lang"]);
            string platform = request.Headers["platform"];
            string version = request.Headers["version"];

            AuthenticationController _auth = AuthenticationController.Instance;
            AuthorizationModel data = _auth.ValidateHeader(authHeader, lang, true);

            try
            {
                string json = JsonConvert.SerializeObject(data.user_id);
                int logID = _sql.InsertLogReceiveData("GetTranPayImage", json, timestampNow.ToString(), authHeader,
                        data.user_id, platform.ToLower());

                GetService srv = new GetService();

                var obj = new object();

                if (data.user_id != 0)
                {
                    obj = srv.GetTranPayImageService(authHeader, lang, platform.ToLower(), logID, getIDCenterDTO);
                }
                else
                {
                    throw new Exception("Missing Parameter : billID");
                }

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Route("1.0/upload/electric")]
        [HttpPost]
        public async Task<HttpResponseMessage> UploadFileExcelElectric()
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] ?? "");
            string lang = (request.Headers["lang"] ?? WebConfigurationManager.AppSettings["default_language"]);
            string platform = request.Headers["platform"];
            string version = request.Headers["version"];

            AuthenticationController _auth = AuthenticationController.Instance;
            AuthorizationModel data = _auth.ValidateHeader(authHeader, lang, true);

            UploadModel value = new UploadModel();
            value.data = new _ServiceUploadData();


            try
            {

                #region Variable Declaration
                HttpResponseMessage ResponseMessage = null;
                var httpRequest = HttpContext.Current.Request;
                DataSet dsexcelRecords = new DataSet();
                IExcelDataReader reader = null;
                HttpPostedFile Inputfile = null;
                Stream FileStream = null;
                #endregion

                var path = WebConfigurationManager.AppSettings["body_path"];
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                MultipartFormDataStreamProvider streamProvider = new MultipartFormDataStreamProvider(path);
                await Request.Content.ReadAsMultipartAsync(streamProvider);

                string startDate = "", endDate = "";
                foreach (var key in streamProvider.FormData.AllKeys)
                {
                    foreach (var val in streamProvider.FormData.GetValues(key))
                    {
                        if (key == "startDate")
                        {
                            if (string.IsNullOrEmpty(val))
                            {
                                throw new Exception("Missing Parameter : startDate");
                            }
                            startDate = val;
                        }
                        if (key == "endDate")
                        {
                            if (string.IsNullOrEmpty(val))
                            {
                                throw new Exception("Missing Parameter : endDate");
                            }
                            endDate = val;
                        }
                    }
                }

                #region Save Student Detail From Excel
                using (Chaithit_MarketEntities objEntity = new Chaithit_MarketEntities())
                {
                    if (httpRequest.Files.Count > 0)
                    {
                        Inputfile = httpRequest.Files[0];
                        FileStream = Inputfile.InputStream;

                        if (Inputfile != null && FileStream != null)
                        {
                            if (Inputfile.FileName.EndsWith(".xls"))
                            {
                                reader = ExcelReaderFactory.CreateBinaryReader(FileStream);
                            }
                            else if (Inputfile.FileName.EndsWith(".xlsx"))
                            {
                                reader = ExcelReaderFactory.CreateOpenXmlReader(FileStream);
                            }
                            else
                            {
                                value.success = false;
                                value.msg = new MsgModel() { code = 0, text = "The file format is not supported.", topic = "No Success" };
                            }

                            dsexcelRecords = reader.AsDataSet();
                            reader.Close();

                            DataTable dtElec = _sql.GetElecticUnit();

                            if (dsexcelRecords != null && dsexcelRecords.Tables.Count > 0)
                            {
                                DataTable dtElectric = dsexcelRecords.Tables[0];
                                for (int i = 3; i < dtElectric.Rows.Count; i++)
                                {
                                    system_electric objElectric = new system_electric();
                                    DataRow[] dr = dtElec.Select("dns_meter='" + dtElectric.Rows[i][1].ToString() + "'");
                                    if (dr.Length > 0)
                                    {
                                        objElectric.value_now = Convert.ToDecimal(dtElectric.Rows[i][2]) - Convert.ToDecimal(dr[0]["value"].ToString());
                                    }
                                    else
                                    {
                                        objElectric.value_now = Convert.ToDecimal(dtElectric.Rows[i][2]);
                                    }
                                    objElectric.group_name = Convert.ToString(dtElectric.Rows[i][0]);
                                    objElectric.dns_meter = Convert.ToString(dtElectric.Rows[i][1]);
                                    objElectric.value = Convert.ToDecimal(dtElectric.Rows[i][2]);
                                    objElectric.value_datetime = Convert.ToDateTime(dtElectric.Rows[i][3]);
                                    objElectric.status_text_en = Convert.ToString(dtElectric.Rows[i][4]);
                                    objElectric.create_date = DateTime.Now;
                                    objElectric.create_by = data.user_id;
                                    objElectric.no = 1;
                                    objElectric.year = Convert.ToDateTime(startDate).Year;
                                    objElectric.month = Convert.ToDateTime(startDate).Month;
                                    objElectric.start_date = Convert.ToDateTime(startDate);
                                    objElectric.end_date = Convert.ToDateTime(endDate);

                                    objEntity.system_electric.Add(objElectric);
                                }

                                int output = objEntity.SaveChanges();
                                if (output > 0)
                                {
                                    value.success = true;
                                    value.msg = new MsgModel() { code = 0, text = "The Excel file has been successfully uploaded.", topic = "Success" };
                                }
                                else
                                {
                                    value.success = false;
                                    value.msg = new MsgModel() { code = 0, text = "Something Went Wrong!, The Excel file uploaded has fiald.", topic = "No Success" };
                                }
                            }
                            else
                            {
                                value.success = false;
                                value.msg = new MsgModel() { code = 0, text = "Selected file is empty.", topic = "No Success" };
                            }

                        }
                        else
                        {
                            value.success = false;
                            value.msg = new MsgModel() { code = 0, text = "Invalid File.", topic = "No Success" };
                        }

                    }
                    else
                    {
                        ResponseMessage = Request.CreateResponse(HttpStatusCode.BadRequest);
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, value, Configuration.Formatters.JsonFormatter);
                #endregion


            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Route("1.0/get/default/electric")]
        [HttpPost]
        public IHttpActionResult GetDefaultElectric(GetDefaultElectricDTO getDefaultElectricDTO)
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] ?? "");
            string lang = (request.Headers["lang"] == null ? WebConfigurationManager.AppSettings["default_language"] : request.Headers["lang"]);
            string platform = request.Headers["platform"];
            string version = request.Headers["version"];

            AuthenticationController _auth = AuthenticationController.Instance;
            AuthorizationModel data = _auth.ValidateHeader(authHeader, lang, true);

            try
            {
                string json = JsonConvert.SerializeObject(getDefaultElectricDTO);
                int logID = _sql.InsertLogReceiveData("GetDefaultElectric", json, timestampNow.ToString(), authHeader,
                        data.user_id, platform.ToLower());

                GetService srv = new GetService();

                string checkMissingOptional = "";

                if (string.IsNullOrEmpty(getDefaultElectricDTO.dnsMeter))
                {
                    checkMissingOptional += "dnsMeter ";
                }
                if (string.IsNullOrEmpty(getDefaultElectricDTO.startDate))
                {
                    checkMissingOptional += "startDate ";
                }
                if (checkMissingOptional != "")
                {
                    throw new Exception("Missing Parameter : " + checkMissingOptional);
                }

                var obj = new object();
                obj = srv.GetDefaultElectricService(authHeader, lang, platform.ToLower(), logID, getDefaultElectricDTO);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Route("1.0/auto/update/unitIsUsed")]
        [HttpGet]
        public IHttpActionResult AutoUpdateUnitIsUsed()
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] ?? "");
            string lang = (request.Headers["lang"] == null ? WebConfigurationManager.AppSettings["default_language"] : request.Headers["lang"]);
            string platform = request.Headers["platform"]; //
            string version = request.Headers["version"];

            try
            {
                string currentDate = DateTime.Now.ToString("yyyy-MM-dd", new CultureInfo("en-US"));
                int logID = _sql.InsertLogReceiveData("AutoUpdateUnitIsUsed", currentDate, timestampNow.ToString(), "",
                        0, "");

                SaveService srv = new SaveService();

                var obj = new object();
                obj = srv.AutoUpdateUnitIsUsedService(currentDate);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Route("1.0/search/pay/user")]
        [HttpPost]
        public IHttpActionResult SearchPayUser(SearchHistoryUserPayDTO searchHistoryUserPayDTO)
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] == null ? "" : request.Headers["Authorization"]);
            string lang = (request.Headers["lang"] == null ? WebConfigurationManager.AppSettings["default_language"] : request.Headers["lang"]);
            string platform = request.Headers["platform"];
            string version = request.Headers["version"];

            AuthenticationController _auth = AuthenticationController.Instance;
            AuthorizationModel data = _auth.ValidateHeader(authHeader, lang, true);

            try
            {
                string json = JsonConvert.SerializeObject(searchHistoryUserPayDTO);
                int logID = _sql.InsertLogReceiveData("SearchPayUser", json, timestampNow.ToString(), authHeader,
                    data.user_id, platform.ToLower());

                GetService srv = new GetService();

                var obj = new object();

                if (searchHistoryUserPayDTO.userID.Equals(0))
                {
                    throw new Exception("invalid : userID ");
                }

                if (searchHistoryUserPayDTO.pageInt.Equals(null) || searchHistoryUserPayDTO.pageInt.Equals(0))
                {
                    throw new Exception("invalid : pageInt ");
                }

                if (searchHistoryUserPayDTO.perPage.Equals(null) || searchHistoryUserPayDTO.perPage.Equals(0))
                {
                    throw new Exception("invalid : perPage ");
                }

                if (searchHistoryUserPayDTO.sortField > 4)
                {
                    throw new Exception("invalid : sortField " + searchHistoryUserPayDTO.sortField);
                }

                if (!(searchHistoryUserPayDTO.sortType == "a" || searchHistoryUserPayDTO.sortType == "d" || searchHistoryUserPayDTO.sortType == "A" || searchHistoryUserPayDTO.sortType == "D" || searchHistoryUserPayDTO.sortType == ""))
                {
                    throw new Exception("invalid sortType");
                }

                obj = srv.SearchPayUserService(authHeader, lang, platform.ToLower(), logID, searchHistoryUserPayDTO);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        #endregion

        #region Transection
        [Route("1.0/save/transection/rent")]
        [HttpPost]
        public IHttpActionResult SaveTransectionRent(InsertTransectionRentDTO insertTransectionRentDTO)
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] == null ? "" : request.Headers["Authorization"]);
            string lang = (request.Headers["lang"] == null ? WebConfigurationManager.AppSettings["default_language"] : request.Headers["lang"]);
            string platform = request.Headers["platform"];
            string version = request.Headers["version"];

            AuthenticationController _auth = AuthenticationController.Instance;
            AuthorizationModel data = _auth.ValidateHeader(authHeader, lang, true);

            try
            {
                string json = JsonConvert.SerializeObject(insertTransectionRentDTO);
                int logID = _sql.InsertLogReceiveData("SaveTransectionRent", json, timestampNow.ToString(), authHeader,
                    data.user_id, platform.ToLower());
                
                string currentDate = DateTime.Now.ToString("ddMMyyyy");

                string checkMissingOptional = "";

                if(insertTransectionRentDTO.mode.ToLower().Equals("insert"))
                {
                    if (string.IsNullOrEmpty(insertTransectionRentDTO.transCode))
                    {
                        checkMissingOptional += "transCode ";
                    }
                    if (insertTransectionRentDTO.userID == 0)
                    {
                        checkMissingOptional += "userID ";
                    }
                    if (insertTransectionRentDTO.unitID == 0)
                    {
                        checkMissingOptional += "unitID ";
                    }
                    if (string.IsNullOrEmpty(insertTransectionRentDTO.startDate))
                    {
                        checkMissingOptional += "startDate ";
                    }
                    if (insertTransectionRentDTO.rentType == 0)
                    {
                        checkMissingOptional += "rentType ";
                    }
                    if (insertTransectionRentDTO.rentTypeAmount == 0)
                    {
                        checkMissingOptional += "rentTypeAmount ";
                    }

                    insertTransectionRentDTO.transCode += currentDate;
                }
                else if (insertTransectionRentDTO.mode.ToLower().Equals("update"))
                {
                    if (insertTransectionRentDTO.tranRentID == 0)
                    {
                        checkMissingOptional += "tranRentID ";
                    }
                    if (insertTransectionRentDTO.userID == 0)
                    {
                        checkMissingOptional += "userID ";
                    }
                    if (insertTransectionRentDTO.unitID == 0)
                    {
                        checkMissingOptional += "unitID ";
                    }
                    if (string.IsNullOrEmpty(insertTransectionRentDTO.startDate))
                    {
                        checkMissingOptional += "startDate ";
                    }
                    if (insertTransectionRentDTO.rentType == 0)
                    {
                        checkMissingOptional += "rentType ";
                    }
                    if (insertTransectionRentDTO.rentTypeAmount == 0)
                    {
                        checkMissingOptional += "rentTypeAmount ";
                    }
                }
                else
                {
                    throw new Exception("Choose Mode Insert or Update");
                }
                
                if (checkMissingOptional != "")
                {
                    throw new Exception("Missing Parameter : " + checkMissingOptional);
                }

                SaveService srv = new SaveService();
                var obj = new object();
                obj = srv.InsertTransectionRentService(authHeader, lang, platform.ToLower(), logID, insertTransectionRentDTO, data.user_id);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Route("1.0/save/transection/bill")]
        [HttpPost]
        public IHttpActionResult SaveTransectionBill(InsertTransectionBillDTO insertTransectionBillDTO)
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] == null ? "" : request.Headers["Authorization"]);
            string lang = (request.Headers["lang"] == null ? WebConfigurationManager.AppSettings["default_language"] : request.Headers["lang"]);
            string platform = request.Headers["platform"];
            string version = request.Headers["version"];

            AuthenticationController _auth = AuthenticationController.Instance;
            AuthorizationModel data = _auth.ValidateHeader(authHeader, lang, true);

            try
            {
                string json = JsonConvert.SerializeObject(insertTransectionBillDTO);
                int logID = _sql.InsertLogReceiveData("SaveTransectionBill", json, timestampNow.ToString(), authHeader,
                    data.user_id, platform.ToLower());

                string checkMissingOptional = "";

                string currentDate = DateTime.Now.ToString("ddMMyyyy");

                if (!insertTransectionBillDTO.mode.ToLower().Equals("insert") && !insertTransectionBillDTO.mode.ToLower().Equals("update"))
                {
                    throw new Exception("Choose Mode Insert or Update");
                }

                if (insertTransectionBillDTO.mode.ToLower().Equals("insert"))
                {
                    if (insertTransectionBillDTO.zoneID == 0)
                    {
                        checkMissingOptional += "กรุณาระบุโซน ";
                    }
                    if (insertTransectionBillDTO.tranRentID == 0)
                    {
                        checkMissingOptional += "กรุณาระบุtranRentID ";
                    }
                }

                if (insertTransectionBillDTO.mode.ToLower().Equals("update"))
                {
                    if (insertTransectionBillDTO.tranBillID == 0)
                    {
                        checkMissingOptional += "กรุณาระบุtranBillID ";
                    }
                }
                
                if (string.IsNullOrEmpty(insertTransectionBillDTO.startDate))
                {
                    checkMissingOptional += "กรุณาระบุวันที่เริ่ม ";
                }
                if (string.IsNullOrEmpty(insertTransectionBillDTO.endDate))
                {
                    checkMissingOptional += "กรุณาระบุวันที่สิ้นสุด ";
                }
                if (insertTransectionBillDTO.rentAmount == 0)
                {
                    checkMissingOptional += "กรุณาระบุrentAmount ";
                }
                if (string.IsNullOrEmpty(insertTransectionBillDTO.payDate))
                {
                    checkMissingOptional += "กรุณาระบุวันที่จ่าย ";
                }

                if (insertTransectionBillDTO.zoneID == 2)
                {
                    if (insertTransectionBillDTO.lampUnit == 0)
                    {
                        checkMissingOptional += "กรุณาระบุจำนวนหลอดไฟ ";
                    }
                    if (insertTransectionBillDTO.electricEquipUnit == 0)
                    {
                        checkMissingOptional += "กรุณาระบุค่าไฟฟ้าตลาดกลางคืน ";
                    }
                }
                else
                {
                    if (insertTransectionBillDTO.electricUnit == 0)
                    {
                        checkMissingOptional += "กรุณาระบุค่าไฟยูนิต ";
                    }
                    if (insertTransectionBillDTO.waterUnit == 0)
                    {
                        checkMissingOptional += "กรุณาระบุค่าน้ำยูนิต ";
                    }
                }

                if (insertTransectionBillDTO.discountAmount != 0 && insertTransectionBillDTO.discountPercent != 0)
                {
                    checkMissingOptional += "กรุณาระบุส่วนลดเป็นจำนวนเงินหรือเปอร์เซ็นต์ อย่างใดอย่างหนึ่งเท่านั้น";
                }

                insertTransectionBillDTO.billCode = insertTransectionBillDTO.tranRentID.ToString() + currentDate;

                SaveService srv = new SaveService();
                var obj = new object();
                if (checkMissingOptional != "")
                {
                    ReturnIdModel value = new ReturnIdModel();
                    value.success = false;
                    value.msg = new MsgModel() { code = 1, text = checkMissingOptional, topic = "ไม่สำเร็จ" };

                    obj = value;
                }
                else
                {
                    obj = srv.InsertTransectionBillService(authHeader, lang, platform.ToLower(), logID, insertTransectionBillDTO, data.user_id);
                }

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }
        #endregion

        #region Master
        [Route("1.0/save/master/empType")]
        [HttpPost]
        public IHttpActionResult SaveEmpType(MasterDataDTO masterDataDTO)
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] == null ? "" : request.Headers["Authorization"]);
            string lang = (request.Headers["lang"] == null ? WebConfigurationManager.AppSettings["default_language"] : request.Headers["lang"]);
            string platform = request.Headers["platform"];
            string version = request.Headers["version"];

            AuthenticationController _auth = AuthenticationController.Instance;
            AuthorizationModel data = _auth.ValidateHeader(authHeader, lang, true);

            try
            {
                string json = JsonConvert.SerializeObject(masterDataDTO);
                int logID = _sql.InsertLogReceiveData("SaveEmpType", json, timestampNow.ToString(), authHeader,
                    data.user_id, platform.ToLower());

                string checkMissingOptional = "";

                if (masterDataDTO.mode.ToLower().Equals("insert"))
                {
                    if (string.IsNullOrEmpty(masterDataDTO.name))
                    {
                        checkMissingOptional += "name ";
                    }
                }
                else if (masterDataDTO.mode.ToLower().Equals("update"))
                {
                    if (masterDataDTO.masterID == 0)
                    {
                        checkMissingOptional += "masterID ";
                    }
                    if (string.IsNullOrEmpty(masterDataDTO.name))
                    {
                        checkMissingOptional += "name ";
                    }
                }
                else if (masterDataDTO.mode.ToLower().Equals("delete"))
                {
                    if (masterDataDTO.masterID == 0)
                    {
                        checkMissingOptional += "masterID ";
                    }
                }
                else
                {
                    throw new Exception("Choose Mode Insert or Update or Delete");
                }

                if (checkMissingOptional != "")
                {
                    throw new Exception("Missing Parameter : " + checkMissingOptional);
                }

                MasterDataService srv = new MasterDataService();
                var obj = new object();
                obj = srv.SaveMasterService(authHeader, lang, platform.ToLower(), logID, masterDataDTO, "system_emptype", data.user_id);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }
        
        [Route("1.0/save/master/zone")]
        [HttpPost]
        public IHttpActionResult SaveZone(SaveZoneDTO saveZoneDTO)
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] == null ? "" : request.Headers["Authorization"]);
            string lang = (request.Headers["lang"] == null ? WebConfigurationManager.AppSettings["default_language"] : request.Headers["lang"]);
            string platform = request.Headers["platform"];
            string version = request.Headers["version"];

            AuthenticationController _auth = AuthenticationController.Instance;
            AuthorizationModel data = _auth.ValidateHeader(authHeader, lang, true);

            try
            {
                string json = JsonConvert.SerializeObject(saveZoneDTO);
                int logID = _sql.InsertLogReceiveData("SaveZone", json, timestampNow.ToString(), authHeader,
                    data.user_id, platform.ToLower());

                string checkMissingOptional = "";

                if (saveZoneDTO.mode.ToLower().Equals("insert"))
                {
                    if (string.IsNullOrEmpty(saveZoneDTO.name))
                    {
                        checkMissingOptional += "name ";
                    }
                }
                else if (saveZoneDTO.mode.ToLower().Equals("update"))
                {
                    if (saveZoneDTO.zoneID == 0)
                    {
                        checkMissingOptional += "zoneID ";
                    }
                    if (string.IsNullOrEmpty(saveZoneDTO.name))
                    {
                        checkMissingOptional += "name ";
                    }
                }
                else if (saveZoneDTO.mode.ToLower().Equals("delete"))
                {
                    if (saveZoneDTO.zoneID == 0)
                    {
                        checkMissingOptional += "zoneID ";
                    }
                }
                else
                {
                    throw new Exception("Choose Mode Insert or Update or Delete");
                }

                if (checkMissingOptional != "")
                {
                    throw new Exception("Missing Parameter : " + checkMissingOptional);
                }

                MasterDataService srv = new MasterDataService();
                var obj = new object();
                obj = srv.SaveZoneService(authHeader, lang, platform.ToLower(), logID, saveZoneDTO, data.user_id);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Route("1.0/save/master/zoneSub")]
        [HttpPost]
        public IHttpActionResult SaveZoneSub(SaveZoneSubDTO saveZoneSubDTO)
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] == null ? "" : request.Headers["Authorization"]);
            string lang = (request.Headers["lang"] == null ? WebConfigurationManager.AppSettings["default_language"] : request.Headers["lang"]);
            string platform = request.Headers["platform"];
            string version = request.Headers["version"];

            AuthenticationController _auth = AuthenticationController.Instance;
            AuthorizationModel data = _auth.ValidateHeader(authHeader, lang, true);

            try
            {
                string json = JsonConvert.SerializeObject(saveZoneSubDTO);
                int logID = _sql.InsertLogReceiveData("SaveZoneSub", json, timestampNow.ToString(), authHeader,
                    data.user_id, platform.ToLower());

                string checkMissingOptional = "";

                if (saveZoneSubDTO.mode.ToLower().Equals("insert"))
                {
                    if (saveZoneSubDTO.zoneID == 0)
                    {
                        checkMissingOptional += "zoneID ";
                    }
                    if (string.IsNullOrEmpty(saveZoneSubDTO.name))
                    {
                        checkMissingOptional += "name ";
                    }
                }
                else if (saveZoneSubDTO.mode.ToLower().Equals("update"))
                {
                    if (saveZoneSubDTO.zoneSubID == 0)
                    {
                        checkMissingOptional += "zoneSubID ";
                    }
                    if (saveZoneSubDTO.zoneID == 0)
                    {
                        checkMissingOptional += "zoneID ";
                    }
                    if (string.IsNullOrEmpty(saveZoneSubDTO.name))
                    {
                        checkMissingOptional += "name ";
                    }
                }
                else if (saveZoneSubDTO.mode.ToLower().Equals("delete"))
                {
                    if (saveZoneSubDTO.zoneSubID == 0)
                    {
                        checkMissingOptional += "zoneSubID ";
                    }
                }
                else
                {
                    throw new Exception("Choose Mode Insert or Update or Delete");
                }

                if (checkMissingOptional != "")
                {
                    throw new Exception("Missing Parameter : " + checkMissingOptional);
                }

                MasterDataService srv = new MasterDataService();
                var obj = new object();
                obj = srv.SaveZoneSubService(authHeader, lang, platform.ToLower(), logID, saveZoneSubDTO, data.user_id);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Route("1.0/save/master/unit")]
        [HttpPost]
        public IHttpActionResult SaveUnit(SaveUnitDTO saveUnitDTO)
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] == null ? "" : request.Headers["Authorization"]);
            string lang = (request.Headers["lang"] == null ? WebConfigurationManager.AppSettings["default_language"] : request.Headers["lang"]);
            string platform = request.Headers["platform"];
            string version = request.Headers["version"];

            AuthenticationController _auth = AuthenticationController.Instance;
            AuthorizationModel data = _auth.ValidateHeader(authHeader, lang, true);

            try
            {
                string json = JsonConvert.SerializeObject(saveUnitDTO);
                int logID = _sql.InsertLogReceiveData("SaveUnit", json, timestampNow.ToString(), authHeader,
                    data.user_id, platform.ToLower());

                string checkMissingOptional = "";

                if (string.IsNullOrEmpty(saveUnitDTO.electricMeter))
                {
                    saveUnitDTO.electricMeter = "";
                }
                if (saveUnitDTO.mode.ToLower().Equals("insert"))
                {
                    if (saveUnitDTO.zoneID == 0)
                    {
                        checkMissingOptional += "zoneID Must 0 ";
                    }
                    if (string.IsNullOrEmpty(saveUnitDTO.unitCode))
                    {
                        checkMissingOptional += "unitCode ";
                    }
                    if (string.IsNullOrEmpty(saveUnitDTO.name))
                    {
                        checkMissingOptional += "name ";
                    }
                    if (saveUnitDTO.rateID == 0)
                    {
                        checkMissingOptional += "rateID Must 0";
                    }
                }
                else if (saveUnitDTO.mode.ToLower().Equals("update"))
                {
                    if (saveUnitDTO.unitID == 0)
                    {
                        checkMissingOptional += "unitID Must 0 ";
                    }
                    if (saveUnitDTO.zoneID == 0)
                    {
                        checkMissingOptional += "zoneID Must 0 ";
                    }
                    if (string.IsNullOrEmpty(saveUnitDTO.unitCode))
                    {
                        checkMissingOptional += "unitCode ";
                    }
                    if (string.IsNullOrEmpty(saveUnitDTO.name))
                    {
                        checkMissingOptional += "name ";
                    }
                    if (saveUnitDTO.rateID == 0)
                    {
                        checkMissingOptional += "rateID Must 0";
                    }
                }
                else if (saveUnitDTO.mode.ToLower().Equals("delete"))
                {
                    if (saveUnitDTO.unitID == 0)
                    {
                        checkMissingOptional += "unitID Must 0 ";
                    }
                }
                else
                {
                    throw new Exception("Choose Mode Insert or Update or Delete");
                }

                if (checkMissingOptional != "")
                {
                    throw new Exception("Missing Parameter : " + checkMissingOptional);
                }

                MasterDataService srv = new MasterDataService();
                var obj = new object();
                obj = srv.SaveUnitService(authHeader, lang, platform.ToLower(), logID, saveUnitDTO, data.user_id);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Route("1.0/save/master/rateAmount")]
        [HttpPost]
        public IHttpActionResult SaveRateAmount(SaveRateAmountDTO saveRateAmountDTO)
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] == null ? "" : request.Headers["Authorization"]);
            string lang = (request.Headers["lang"] == null ? WebConfigurationManager.AppSettings["default_language"] : request.Headers["lang"]);
            string platform = request.Headers["platform"];
            string version = request.Headers["version"];

            AuthenticationController _auth = AuthenticationController.Instance;
            AuthorizationModel data = _auth.ValidateHeader(authHeader, lang, true);

            try
            {
                string json = JsonConvert.SerializeObject(saveRateAmountDTO);
                int logID = _sql.InsertLogReceiveData("SaveRateAmount", json, timestampNow.ToString(), authHeader,
                    data.user_id, platform.ToLower());

                string checkMissingOptional = "";

                if (saveRateAmountDTO.mode.ToLower().Equals("insert"))
                {
                    if (string.IsNullOrEmpty(saveRateAmountDTO.name))
                    {
                        checkMissingOptional += "name ";
                    }
                }
                else if (saveRateAmountDTO.mode.ToLower().Equals("update"))
                {
                    if (saveRateAmountDTO.rateID == 0)
                    {
                        checkMissingOptional += "RateID Must 0 ";
                    }
                    if (string.IsNullOrEmpty(saveRateAmountDTO.name))
                    {
                        checkMissingOptional += "name ";
                    }
                }
                else if (saveRateAmountDTO.mode.ToLower().Equals("delete"))
                {
                    if (saveRateAmountDTO.rateID == 0)
                    {
                        checkMissingOptional += "RateID Must 0 ";
                    }
                }
                else
                {
                    throw new Exception("Choose Mode Insert or Update or Delete");
                }

                if (checkMissingOptional != "")
                {
                    throw new Exception("Missing Parameter : " + checkMissingOptional);
                }

                MasterDataService srv = new MasterDataService();
                var obj = new object();
                obj = srv.SaveRateAmountService(authHeader, lang, platform.ToLower(), logID, saveRateAmountDTO, data.user_id);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }
        
        [Route("1.0/get/dropdown/master/empType")]
        [HttpPost]
        public IHttpActionResult GetDropdownEmpType(GetDropdownIsAllDTO getDropdownIsAllDTO)
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] == null ? "" : request.Headers["Authorization"]);
            string lang = (request.Headers["lang"] == null ? WebConfigurationManager.AppSettings["default_language"] : request.Headers["lang"]);
            string platform = request.Headers["platform"];
            string version = request.Headers["version"];

            AuthenticationController _auth = AuthenticationController.Instance;
            AuthorizationModel data = _auth.ValidateHeader(authHeader, lang, true);

            try
            {
                string json = JsonConvert.SerializeObject(getDropdownIsAllDTO);
                int logID = _sql.InsertLogReceiveData("GetDropdownEmpType", json, timestampNow.ToString(), authHeader,
                    data.user_id, platform.ToLower());
                
                if (getDropdownIsAllDTO.isAll.ToLower() != "true" && getDropdownIsAllDTO.isAll.ToLower() != "false")
                {
                    throw new Exception("isAll value must be true or false");
                }

                MasterDataService srv = new MasterDataService();
                var obj = srv.GetDropdownMasterService(authHeader, lang, platform.ToLower(), logID, 0, "system_emptype", getDropdownIsAllDTO.isAll.ToLower());

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Route("1.0/get/dropdown/master/zone")]
        [HttpPost]
        public IHttpActionResult GetDropdownZone(GetDropdownIsAllDTO getDropdownIsAllDTO)
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] == null ? "" : request.Headers["Authorization"]);
            string lang = (request.Headers["lang"] == null ? WebConfigurationManager.AppSettings["default_language"] : request.Headers["lang"]);
            string platform = request.Headers["platform"];
            string version = request.Headers["version"];

            AuthenticationController _auth = AuthenticationController.Instance;
            AuthorizationModel data = _auth.ValidateHeader(authHeader, lang, true);

            try
            {
                string json = JsonConvert.SerializeObject(getDropdownIsAllDTO);
                int logID = _sql.InsertLogReceiveData("GetDropdownZone", json, timestampNow.ToString(), authHeader,
                    data.user_id, platform.ToLower());

                if (getDropdownIsAllDTO.isAll.ToLower() != "true" && getDropdownIsAllDTO.isAll.ToLower() != "false")
                {
                    throw new Exception("isAll value must be true or false");
                }

                MasterDataService srv = new MasterDataService();
                var obj = srv.GetDropdownZoneService(authHeader, lang, platform.ToLower(), logID, getDropdownIsAllDTO.isAll.ToLower());
                
                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Route("1.0/get/dropdown/master/zoneSub")]
        [HttpPost]
        public IHttpActionResult GetDropdownZoneSub(GetDropdownZoneSubIsAllDTO getDropdownZoneSubIsAllDTO)
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] == null ? "" : request.Headers["Authorization"]);
            string lang = (request.Headers["lang"] == null ? WebConfigurationManager.AppSettings["default_language"] : request.Headers["lang"]);
            string platform = request.Headers["platform"];
            string version = request.Headers["version"];

            AuthenticationController _auth = AuthenticationController.Instance;
            AuthorizationModel data = _auth.ValidateHeader(authHeader, lang, true);

            try
            {
                string json = JsonConvert.SerializeObject(getDropdownZoneSubIsAllDTO);
                int logID = _sql.InsertLogReceiveData("GetDropdownZoneSub", json, timestampNow.ToString(), authHeader,
                    data.user_id, platform.ToLower());
                
                var obj = new object();
                if (getDropdownZoneSubIsAllDTO.ID == 0)
                {
                    throw new Exception("Missing Parameter : ID");
                }
                if (getDropdownZoneSubIsAllDTO.isAll.ToLower() != "true" && getDropdownZoneSubIsAllDTO.isAll.ToLower() != "false")
                {
                    throw new Exception("isAll value must be true or false");
                }

                MasterDataService srv = new MasterDataService();
                obj = srv.GetDropdownZoneSubService(authHeader, lang, platform.ToLower(), logID, getDropdownZoneSubIsAllDTO.ID, getDropdownZoneSubIsAllDTO.isAll.ToLower());

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Route("1.0/get/dropdown/master/unit")]
        [HttpPost]
        public IHttpActionResult GetDropdownUnit(GetDropdownUnitIsAllDTO getDropdownUnitIsAllDTO)
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] == null ? "" : request.Headers["Authorization"]);
            string lang = (request.Headers["lang"] == null ? WebConfigurationManager.AppSettings["default_language"] : request.Headers["lang"]);
            string platform = request.Headers["platform"];
            string version = request.Headers["version"];

            AuthenticationController _auth = AuthenticationController.Instance;
            AuthorizationModel data = _auth.ValidateHeader(authHeader, lang, true);

            try
            {
                string json = JsonConvert.SerializeObject(getDropdownUnitIsAllDTO);
                int logID = _sql.InsertLogReceiveData("GetDropdownUnit", json, timestampNow.ToString(), authHeader,
                    data.user_id, platform.ToLower());

                var obj = new object();
                if (getDropdownUnitIsAllDTO.zoneID == 0)
                {
                    throw new Exception("Missing Parameter : zoneID");
                }
                if (getDropdownUnitIsAllDTO.isAll.ToLower() != "true" && getDropdownUnitIsAllDTO.isAll.ToLower() != "false")
                {
                    throw new Exception("isAll value must be true or false");
                }

                MasterDataService srv = new MasterDataService();
                obj = srv.GetDropdownUnitService(authHeader, lang, platform.ToLower(), logID, getDropdownUnitIsAllDTO.zoneID, getDropdownUnitIsAllDTO.zoneSubID, getDropdownUnitIsAllDTO.isAll.ToLower());

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Route("1.0/get/dropdown/master/rateAmount")]
        [HttpPost]
        public IHttpActionResult GetDropdownRateAmount(GetDropdownIsAllDTO getDropdownIsAllDTO)
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] == null ? "" : request.Headers["Authorization"]);
            string lang = (request.Headers["lang"] == null ? WebConfigurationManager.AppSettings["default_language"] : request.Headers["lang"]);
            string platform = request.Headers["platform"];
            string version = request.Headers["version"];

            AuthenticationController _auth = AuthenticationController.Instance;
            AuthorizationModel data = _auth.ValidateHeader(authHeader, lang, true);

            try
            {
                string json = JsonConvert.SerializeObject(getDropdownIsAllDTO);
                int logID = _sql.InsertLogReceiveData("GetDropdownRateAmount", json, timestampNow.ToString(), authHeader,
                    data.user_id, platform.ToLower());
                
                if (getDropdownIsAllDTO.isAll.ToLower() != "true" && getDropdownIsAllDTO.isAll.ToLower() != "false")
                {
                    throw new Exception("isAll value must be true or false");
                }

                MasterDataService srv = new MasterDataService();
                var obj = srv.GetDropdownRateAmountService(authHeader, lang, platform.ToLower(), logID, getDropdownIsAllDTO.isAll.ToLower());

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Route("1.0/get/dropdown/unit/status")]
        [HttpPost]
        public IHttpActionResult GetDropdownUnitStatus()
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] == null ? "" : request.Headers["Authorization"]);
            string lang = (request.Headers["lang"] == null ? WebConfigurationManager.AppSettings["default_language"] : request.Headers["lang"]);
            string platform = request.Headers["platform"];
            string version = request.Headers["version"];

            AuthenticationController _auth = AuthenticationController.Instance;
            AuthorizationModel data = _auth.ValidateHeader(authHeader, lang, true);

            try
            {
                //ไม่ได้ดึงจาดbase ใช้hardcode 1 = ว่าง, 2 = ไม่ว่าง;
                string json = JsonConvert.SerializeObject("");
                int logID = _sql.InsertLogReceiveData("GetDropdownUnitStatus", json, timestampNow.ToString(), authHeader,
                    data.user_id, platform.ToLower());

                MasterDataService srv = new MasterDataService();

                var obj = srv.GetDropdownUnitStatusService(authHeader, lang, platform.ToLower(), logID);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Route("1.0/get/dropdown/rentType")]
        [HttpPost]
        public IHttpActionResult GetDropdownRentType()
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] == null ? "" : request.Headers["Authorization"]);
            string lang = (request.Headers["lang"] == null ? WebConfigurationManager.AppSettings["default_language"] : request.Headers["lang"]);
            string platform = request.Headers["platform"];
            string version = request.Headers["version"];

            AuthenticationController _auth = AuthenticationController.Instance;
            AuthorizationModel data = _auth.ValidateHeader(authHeader, lang, true);

            try
            {
                //ไม่ได้ดึงจากbase ใช้hardcode 1 = วัน, 2 = เดือน;
                string json = JsonConvert.SerializeObject("");
                int logID = _sql.InsertLogReceiveData("GetDropdownRentType", json, timestampNow.ToString(), authHeader,
                    data.user_id, platform.ToLower());

                MasterDataService srv = new MasterDataService();

                var obj = srv.GetDropdownRentTypeService(authHeader, lang, platform.ToLower(), logID);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Route("1.0/get/dropdown/sixMonthAgo")]
        [HttpPost]
        public IHttpActionResult GetDropdownSixMonthAgo()
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] == null ? "" : request.Headers["Authorization"]);
            string lang = (request.Headers["lang"] == null ? WebConfigurationManager.AppSettings["default_language"] : request.Headers["lang"]);
            string platform = request.Headers["platform"];
            string version = request.Headers["version"];

            AuthenticationController _auth = AuthenticationController.Instance;
            AuthorizationModel data = _auth.ValidateHeader(authHeader, lang, true);

            try
            {
                string json = JsonConvert.SerializeObject("");
                int logID = _sql.InsertLogReceiveData("GetDropdownSixMonthAgo", json, timestampNow.ToString(), authHeader,
                    data.user_id, platform.ToLower());

                MasterDataService srv = new MasterDataService();

                var obj = srv.GetDropdownSixMonthAgoService(authHeader, lang, platform.ToLower(), logID);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Route("1.0/search/master/empType")]
        [HttpPost]
        public IHttpActionResult SearchEmpType(SearchNameCenterDTO searchNameCenterDTO)
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] == null ? "" : request.Headers["Authorization"]);
            string lang = (request.Headers["lang"] == null ? WebConfigurationManager.AppSettings["default_language"] : request.Headers["lang"]);
            string platform = request.Headers["platform"];
            string version = request.Headers["version"];

            AuthenticationController _auth = AuthenticationController.Instance;
            AuthorizationModel data = _auth.ValidateHeader(authHeader, lang, true);

            try
            {
                string json = JsonConvert.SerializeObject(searchNameCenterDTO);
                int logID = _sql.InsertLogReceiveData("SearchEmpType", json, timestampNow.ToString(), authHeader,
                    data.user_id, platform.ToLower());

                MasterDataService srv = new MasterDataService();

                var obj = new object();

                if (searchNameCenterDTO.pageInt.Equals(null) || searchNameCenterDTO.pageInt.Equals(0))
                {
                    throw new Exception("invalid : pageInt ");
                }

                if (searchNameCenterDTO.perPage.Equals(null) || searchNameCenterDTO.perPage.Equals(0))
                {
                    throw new Exception("invalid : perPage ");
                }

                if (searchNameCenterDTO.sortField > 4)
                {
                    throw new Exception("invalid : sortField " + searchNameCenterDTO.sortField);
                }

                if (!(searchNameCenterDTO.sortType == "a" || searchNameCenterDTO.sortType == "d" || searchNameCenterDTO.sortType == "A" || searchNameCenterDTO.sortType == "D" || searchNameCenterDTO.sortType == ""))
                {
                    throw new Exception("invalid sortType");
                }

                obj = srv.SearchMasterDataService(authHeader, lang, platform.ToLower(), logID, searchNameCenterDTO, "system_emptype");

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Route("1.0/search/master/zone")]
        [HttpPost]
        public IHttpActionResult SearchZone(SearchNameCenterDTO searchNameCenterDTO)
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] == null ? "" : request.Headers["Authorization"]);
            string lang = (request.Headers["lang"] == null ? WebConfigurationManager.AppSettings["default_language"] : request.Headers["lang"]);
            string platform = request.Headers["platform"];
            string version = request.Headers["version"];

            AuthenticationController _auth = AuthenticationController.Instance;
            AuthorizationModel data = _auth.ValidateHeader(authHeader, lang, true);

            try
            {
                string json = JsonConvert.SerializeObject(searchNameCenterDTO);
                int logID = _sql.InsertLogReceiveData("SearchZone", json, timestampNow.ToString(), authHeader,
                    data.user_id, platform.ToLower());

                MasterDataService srv = new MasterDataService();

                var obj = new object();

                if (searchNameCenterDTO.pageInt.Equals(null) || searchNameCenterDTO.pageInt.Equals(0))
                {
                    throw new Exception("invalid : pageInt ");
                }

                if (searchNameCenterDTO.perPage.Equals(null) || searchNameCenterDTO.perPage.Equals(0))
                {
                    throw new Exception("invalid : perPage ");
                }

                if (searchNameCenterDTO.sortField > 4)
                {
                    throw new Exception("invalid : sortField " + searchNameCenterDTO.sortField);
                }

                if (!(searchNameCenterDTO.sortType == "a" || searchNameCenterDTO.sortType == "d" || searchNameCenterDTO.sortType == "A" || searchNameCenterDTO.sortType == "D" || searchNameCenterDTO.sortType == ""))
                {
                    throw new Exception("invalid sortType");
                }

                obj = srv.SearchZoneService(authHeader, lang, platform.ToLower(), logID, searchNameCenterDTO);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Route("1.0/search/master/zoneSub")]
        [HttpPost]
        public IHttpActionResult SearchZoneSub(SearchZoneSubDTO searchZoneSubDTO)
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] == null ? "" : request.Headers["Authorization"]);
            string lang = (request.Headers["lang"] == null ? WebConfigurationManager.AppSettings["default_language"] : request.Headers["lang"]);
            string platform = request.Headers["platform"];
            string version = request.Headers["version"];

            AuthenticationController _auth = AuthenticationController.Instance;
            AuthorizationModel data = _auth.ValidateHeader(authHeader, lang, true);

            try
            {
                string json = JsonConvert.SerializeObject(searchZoneSubDTO);
                int logID = _sql.InsertLogReceiveData("SearchZoneSub", json, timestampNow.ToString(), authHeader,
                    data.user_id, platform.ToLower());

                MasterDataService srv = new MasterDataService();

                var obj = new object();

                if (searchZoneSubDTO.pageInt.Equals(null) || searchZoneSubDTO.pageInt.Equals(0))
                {
                    throw new Exception("invalid : pageInt ");
                }

                if (searchZoneSubDTO.perPage.Equals(null) || searchZoneSubDTO.perPage.Equals(0))
                {
                    throw new Exception("invalid : perPage ");
                }

                if (searchZoneSubDTO.sortField > 4)
                {
                    throw new Exception("invalid : sortField " + searchZoneSubDTO.sortField);
                }

                if (!(searchZoneSubDTO.sortType == "a" || searchZoneSubDTO.sortType == "d" || searchZoneSubDTO.sortType == "A" || searchZoneSubDTO.sortType == "D" || searchZoneSubDTO.sortType == ""))
                {
                    throw new Exception("invalid sortType");
                }

                obj = srv.SearchZoneSubService(authHeader, lang, platform.ToLower(), logID, searchZoneSubDTO);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Route("1.0/search/master/unit")]
        [HttpPost]
        public IHttpActionResult SearchUnit(SearchUnitDTO searchUnitDTO)
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] == null ? "" : request.Headers["Authorization"]);
            string lang = (request.Headers["lang"] == null ? WebConfigurationManager.AppSettings["default_language"] : request.Headers["lang"]);
            string platform = request.Headers["platform"];
            string version = request.Headers["version"];

            AuthenticationController _auth = AuthenticationController.Instance;
            AuthorizationModel data = _auth.ValidateHeader(authHeader, lang, true);

            try
            {
                string json = JsonConvert.SerializeObject(searchUnitDTO);
                int logID = _sql.InsertLogReceiveData("SearchUnit", json, timestampNow.ToString(), authHeader,
                    data.user_id, platform.ToLower());

                MasterDataService srv = new MasterDataService();

                var obj = new object();

                if (searchUnitDTO.pageInt.Equals(null) || searchUnitDTO.pageInt.Equals(0))
                {
                    throw new Exception("invalid : pageInt ");
                }

                if (searchUnitDTO.perPage.Equals(null) || searchUnitDTO.perPage.Equals(0))
                {
                    throw new Exception("invalid : perPage ");
                }

                if (searchUnitDTO.sortField > 4)
                {
                    throw new Exception("invalid : sortField " + searchUnitDTO.sortField);
                }

                if (!(searchUnitDTO.sortType == "a" || searchUnitDTO.sortType == "d" || searchUnitDTO.sortType == "A" || searchUnitDTO.sortType == "D" || searchUnitDTO.sortType == ""))
                {
                    throw new Exception("invalid sortType");
                }

                obj = srv.SearchUnitService(authHeader, lang, platform.ToLower(), logID, searchUnitDTO);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Route("1.0/search/master/rateAmount")]
        [HttpPost]
        public IHttpActionResult SearchRateAmount(SearchNameCenterDTO searchNameCenterDTO)
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] == null ? "" : request.Headers["Authorization"]);
            string lang = (request.Headers["lang"] == null ? WebConfigurationManager.AppSettings["default_language"] : request.Headers["lang"]);
            string platform = request.Headers["platform"];
            string version = request.Headers["version"];

            AuthenticationController _auth = AuthenticationController.Instance;
            AuthorizationModel data = _auth.ValidateHeader(authHeader, lang, true);

            try
            {
                string json = JsonConvert.SerializeObject(searchNameCenterDTO);
                int logID = _sql.InsertLogReceiveData("SearchRateAmount", json, timestampNow.ToString(), authHeader,
                    data.user_id, platform.ToLower());

                MasterDataService srv = new MasterDataService();

                var obj = new object();

                if (searchNameCenterDTO.pageInt.Equals(null) || searchNameCenterDTO.pageInt.Equals(0))
                {
                    throw new Exception("invalid : pageInt ");
                }

                if (searchNameCenterDTO.perPage.Equals(null) || searchNameCenterDTO.perPage.Equals(0))
                {
                    throw new Exception("invalid : perPage ");
                }

                if (searchNameCenterDTO.sortField > 4)
                {
                    throw new Exception("invalid : sortField " + searchNameCenterDTO.sortField);
                }

                if (!(searchNameCenterDTO.sortType == "a" || searchNameCenterDTO.sortType == "d" || searchNameCenterDTO.sortType == "A" || searchNameCenterDTO.sortType == "D" || searchNameCenterDTO.sortType == ""))
                {
                    throw new Exception("invalid sortType");
                }

                obj = srv.SearchRateAmountService(authHeader, lang, platform.ToLower(), logID, searchNameCenterDTO);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }
        
        [Route("1.0/get/dashbord")]
        [HttpPost]
        public IHttpActionResult GetDashbord(GetDashbordDTO getDashbordDTO)
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] ?? "");
            string lang = (request.Headers["lang"] == null ? WebConfigurationManager.AppSettings["default_language"] : request.Headers["lang"]);
            string platform = request.Headers["platform"];
            string version = request.Headers["version"];

            AuthenticationController _auth = AuthenticationController.Instance;
            AuthorizationModel data = _auth.ValidateHeader(authHeader, lang, true);

            try
            {
                string json = JsonConvert.SerializeObject(getDashbordDTO);
                int logID = _sql.InsertLogReceiveData("GetDashbord", json, timestampNow.ToString(), authHeader,
                        data.user_id, platform.ToLower());

                GetService srv = new GetService();

                var obj = new object();

                string checkMissingOptional = "";
                
                if (string.IsNullOrEmpty(getDashbordDTO.month) && string.IsNullOrEmpty(getDashbordDTO.quarter))
                {
                    checkMissingOptional += "month and quarter";
                }
                if (!string.IsNullOrEmpty(getDashbordDTO.month) && !string.IsNullOrEmpty(getDashbordDTO.quarter))
                {
                    checkMissingOptional += "Choose to enter value month or quarter ";
                }
                if (!string.IsNullOrEmpty(getDashbordDTO.quarter))
                {
                    if (getDashbordDTO.quarter != "1" && getDashbordDTO.quarter != "2" && getDashbordDTO.quarter != "3" && getDashbordDTO.quarter != "4" && getDashbordDTO.quarter != "5")
                    {
                        checkMissingOptional += "quarter must value 1,2,3,4,5 ";
                    }
                }
                if (checkMissingOptional != "")
                {
                    throw new Exception("Missing Parameter : " + checkMissingOptional);
                }

                obj = srv.GetDashbordService(authHeader, lang, platform.ToLower(), logID, getDashbordDTO);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Route("1.0/get/total/history/paidBill/admin")]
        [HttpPost]
        public IHttpActionResult GetTotalHistoryPaidBillAdmin(SearchHistoryAdminBillDTO searchHistoryAdminBillDTO)
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] == null ? "" : request.Headers["Authorization"]);
            string lang = (request.Headers["lang"] == null ? WebConfigurationManager.AppSettings["default_language"] : request.Headers["lang"]);
            string platform = request.Headers["platform"];
            string version = request.Headers["version"];

            AuthenticationController _auth = AuthenticationController.Instance;
            AuthorizationModel data = _auth.ValidateHeader(authHeader, lang, true);

            try
            {
                string json = JsonConvert.SerializeObject(searchHistoryAdminBillDTO);
                int logID = _sql.InsertLogReceiveData("GetTotalHistoryPaidBillAdmin", json, timestampNow.ToString(), authHeader,
                    data.user_id, platform.ToLower());

                GetService srv = new GetService();

                var obj = new object(); 

                obj = srv.GetTotalHistoryPaidBillAdminService(authHeader, lang, platform.ToLower(), logID, searchHistoryAdminBillDTO);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        #endregion
    }
}
