using Chaithit_Market.Core;
using Chaithit_Market.DTO;
using Chaithit_Market.Models;
using Chaithit_Market.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Chaithit_Market.Controllers
{
    [RoutePrefix("api/2.0")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ValuesController : ApiController
    {
        private SQLManager _sql = SQLManager.Instance;
        private double timestampNow = Utility.DateTimeToUnixTimestamp(DateTime.Now);

        #region Page Login
        [Route("login")]
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
        #endregion

        #region Add User
        [Route("save/userProfile")]
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
                        if (key == "startDate")
                        {
                            saveUserProfileDTO.startDate = val;
                        }
                        if (key == "endDate")
                        {
                            saveUserProfileDTO.endDate = val;
                        }
                        if (key == "statusEmp")
                        {
                            saveUserProfileDTO.statusEmp = int.Parse(string.IsNullOrEmpty(val) ? "0" : val);
                        }
                        if (key == "empType")
                        {
                            saveUserProfileDTO.empType = int.Parse(string.IsNullOrEmpty(val) ? "0" : val);
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
                    if (string.IsNullOrEmpty(saveUserProfileDTO.startDate))
                    {
                        checkMissingOptional += "startDate ";
                    }
                    if (string.IsNullOrEmpty(saveUserProfileDTO.endDate))
                    {
                        checkMissingOptional += "endDate ";
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
                    if (string.IsNullOrEmpty(saveUserProfileDTO.startDate))
                    {
                        checkMissingOptional += "startDate ";
                    }
                    if (string.IsNullOrEmpty(saveUserProfileDTO.endDate))
                    {
                        checkMissingOptional += "endDate ";
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

        [Route("get/userProfile")]
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

        [Route("search/userProfile")]
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
                string json = JsonConvert.SerializeObject("");
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

        [Route("save/zone")]
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

                SaveService srv = new SaveService();
                var obj = new object();
                obj = srv.SaveZoneService(authHeader, lang, platform.ToLower(), logID, saveZoneDTO, data.user_id);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Route("save/zoneSub")]
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

                SaveService srv = new SaveService();
                var obj = new object();
                obj = srv.SaveZoneSubService(authHeader, lang, platform.ToLower(), logID, saveZoneSubDTO, data.user_id);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Route("save/unit")]
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

                if (saveUnitDTO.mode.ToLower().Equals("insert"))
                {
                    if (saveUnitDTO.zoneID != 0)
                    {
                        checkMissingOptional += "zoneID Must 0 ";
                    }
                    if (saveUnitDTO.zoneSubID != 0)
                    {
                        checkMissingOptional += "zoneSubID Must 0 ";
                    }
                    if (string.IsNullOrEmpty(saveUnitDTO.unitCode))
                    {
                        checkMissingOptional += "unitCode ";
                    }
                    if (string.IsNullOrEmpty(saveUnitDTO.name))
                    {
                        checkMissingOptional += "name ";
                    }
                    if (saveUnitDTO.rateID != 0)
                    {
                        checkMissingOptional += "rateID Must 0 ";
                    }
                }
                else if (saveUnitDTO.mode.ToLower().Equals("update"))
                {
                    if (saveUnitDTO.unitID != 0)
                    {
                        checkMissingOptional += "unitID Must 0 ";
                    }
                    if (saveUnitDTO.zoneID != 0)
                    {
                        checkMissingOptional += "zoneID Must 0 ";
                    }
                    if (saveUnitDTO.zoneSubID != 0)
                    {
                        checkMissingOptional += "zoneSubID Must 0 ";
                    }
                    if (string.IsNullOrEmpty(saveUnitDTO.unitCode))
                    {
                        checkMissingOptional += "unitCode ";
                    }
                    if (string.IsNullOrEmpty(saveUnitDTO.name))
                    {
                        checkMissingOptional += "name ";
                    }
                    if (saveUnitDTO.rateID != 0)
                    {
                        checkMissingOptional += "rateID Must 0 ";
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

                SaveService srv = new SaveService();
                var obj = new object();
                obj = srv.SaveUnitService(authHeader, lang, platform.ToLower(), logID, saveUnitDTO, data.user_id);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Route("save/rateAmount")]
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
                    if (saveRateAmountDTO.rentAmountDay != 0 && saveRateAmountDTO.rentAmountMonth != 0 && saveRateAmountDTO.electricAmount != 0 
                            && saveRateAmountDTO.waterAmount != 0 && saveRateAmountDTO.lampAmountPerOne != 0 && saveRateAmountDTO.electricEquipAmount != 0)
                    {
                        checkMissingOptional += "rentAmountDay, rentAmountMonth, electricAmount, waterAmount, lampAmountPerOne, electricEquipAmount Must 0 ";
                    }
                }
                else if (saveRateAmountDTO.mode.ToLower().Equals("update"))
                {
                    if (saveRateAmountDTO.RateID != 0)
                    {
                        checkMissingOptional += "RateID Must 0 ";
                    }
                    if (saveRateAmountDTO.rentAmountDay != 0 && saveRateAmountDTO.rentAmountMonth != 0 && saveRateAmountDTO.electricAmount != 0
                            && saveRateAmountDTO.waterAmount != 0 && saveRateAmountDTO.lampAmountPerOne != 0 && saveRateAmountDTO.electricEquipAmount != 0)
                    {
                        checkMissingOptional += "rentAmountDay, rentAmountMonth, electricAmount, waterAmount, lampAmountPerOne, electricEquipAmount Must 0 ";
                    }
                }
                else if (saveRateAmountDTO.mode.ToLower().Equals("delete"))
                {
                    if (saveRateAmountDTO.RateID == 0)
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

                SaveService srv = new SaveService();
                var obj = new object();
                obj = srv.SaveRateAmountService(authHeader, lang, platform.ToLower(), logID, saveRateAmountDTO, data.user_id);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Route("save/rental")]
        [HttpPost]
        public IHttpActionResult SaveRental(SaveRentalDTO saveRentalDTO)
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
                string json = JsonConvert.SerializeObject(saveRentalDTO);
                int logID = _sql.InsertLogReceiveData("SaveRental", json, timestampNow.ToString(), authHeader,
                    data.user_id, platform.ToLower());

                string checkMissingOptional = "";

                if (saveRentalDTO.mode.ToLower().Equals("insert"))
                {
                    if (saveRentalDTO.rentalID != 0)
                    {
                        checkMissingOptional += "rentalID Must 0 ";
                    }
                    if (string.IsNullOrEmpty(saveRentalDTO.rentCode))
                    {
                        checkMissingOptional += "rentCode ";
                    }
                    if (string.IsNullOrEmpty(saveRentalDTO.name))
                    {
                        checkMissingOptional += "name ";
                    }
                    if (saveRentalDTO.placeSubID == 0)
                    {
                        checkMissingOptional += "placeSubID ";
                    }
                }
                else if (saveRentalDTO.mode.ToLower().Equals("update"))
                {
                    if (saveRentalDTO.rentalID == 0)
                    {
                        checkMissingOptional += "rentalID ";
                    }
                    if (string.IsNullOrEmpty(saveRentalDTO.rentCode))
                    {
                        checkMissingOptional += "rentCode ";
                    }
                    if (string.IsNullOrEmpty(saveRentalDTO.name))
                    {
                        checkMissingOptional += "name ";
                    }
                    if (saveRentalDTO.placeSubID == 0)
                    {
                        checkMissingOptional += "placeSubID ";
                    }
                }
                else if (saveRentalDTO.mode.ToLower().Equals("delete"))
                {
                    if (saveRentalDTO.rentalID == 0)
                    {
                        checkMissingOptional += "rentalID ";
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

                SaveService srv = new SaveService();
                var obj = new object();
                obj = srv.SaveRentalService(authHeader, lang, platform.ToLower(), logID, saveRentalDTO, data.user_id);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Route("get/user/rental")]
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

        [Route("search/rental")]
        [HttpPost]
        public IHttpActionResult SearchRental(SearchRentDTO searchRentDTO)
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
                int logID = _sql.InsertLogReceiveData("SearchRental", json, timestampNow.ToString(), authHeader,
                    data.user_id, platform.ToLower());

                GetService srv = new GetService();

                var obj = new object();

                if (searchRentDTO.pageInt.Equals(null) || searchRentDTO.pageInt.Equals(0))
                {
                    throw new Exception("invalid : pageInt ");
                }

                if (searchRentDTO.perPage.Equals(null) || searchRentDTO.perPage.Equals(0))
                {
                    throw new Exception("invalid : perPage ");
                }

                if (searchRentDTO.sortField > 4)
                {
                    throw new Exception("invalid : sortField " + searchRentDTO.sortField);
                }

                if (!(searchRentDTO.sortType == "a" || searchRentDTO.sortType == "d" || searchRentDTO.sortType == "A" || searchRentDTO.sortType == "D" || searchRentDTO.sortType == ""))
                {
                    throw new Exception("invalid sortType");
                }

                obj = srv.SearchRentalService(authHeader, lang, platform.ToLower(), logID, searchRentDTO);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Route("upload/file")]
        [HttpPost]
        public async Task<HttpResponseMessage> UploadFile()
        {
            var request = HttpContext.Current.Request;
            string authHeader = (request.Headers["Authorization"] ?? "");
            string lang = (request.Headers["lang"] ?? WebConfigurationManager.AppSettings["default_language"]);
            string platform = request.Headers["platform"];
            string version = request.Headers["version"];
            
            UploadModel value = new UploadModel();
            value.data = new _ServiceUploadData();

            int userID = 0;
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

            foreach (MultipartFileData fileData in streamProvider.FileData)
            {
                fileSize = new FileInfo(fileData.LocalFileName).Length;
                if (fileSize > 3100000)
                {
                    throw new Exception("error file size limit 3.00 MB");
                }

                keyName = fileData.Headers.ContentDisposition.Name.Replace("\"", "");
                fileName = fileData.Headers.ContentDisposition.FileName.Replace("\"", "");
                newFileName = Guid.NewGuid() + Path.GetExtension(fileName);
                
                if (keyName == "upload_user_profile")
                {
                    subFolder = userID + "\\ProFilePath";
                    diskFolderPath = string.Format(WebConfigurationManager.AppSettings["file_user_path"], subFolder);
                    fileURL = string.Format(WebConfigurationManager.AppSettings["file_user_url"], userID + "/ProFilePath", newFileName);
                }

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
                    value.success = true;
                    value.data.img_url = fileURL;
                    value.data.file_size = fileSize.ToString();
                    value.msg = new MsgModel() { code = 0, text = "อัพโหลดสำเร็จ", topic = "สำเร็จ" };
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, value, Configuration.Formatters.JsonFormatter);
        }

        [Route("delete/file")]
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

        [Route("search/rental/stand")]
        [HttpPost]
        public IHttpActionResult SearchRentalStand(SearchRentStandDTO searchRentStandDTO)
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
                int logID = _sql.InsertLogReceiveData("SearchRentalStand", json, timestampNow.ToString(), authHeader,
                    data.user_id, platform.ToLower());

                GetService srv = new GetService();

                var obj = new object();

                if (searchRentStandDTO.pageInt.Equals(null) || searchRentStandDTO.pageInt.Equals(0))
                {
                    throw new Exception("invalid : pageInt ");
                }

                if (searchRentStandDTO.perPage.Equals(null) || searchRentStandDTO.perPage.Equals(0))
                {
                    throw new Exception("invalid : perPage ");
                }

                if (searchRentStandDTO.sortField > 4)
                {
                    throw new Exception("invalid : sortField " + searchRentStandDTO.sortField);
                }

                if (!(searchRentStandDTO.sortType == "a" || searchRentStandDTO.sortType == "d" || searchRentStandDTO.sortType == "A" || searchRentStandDTO.sortType == "D" || searchRentStandDTO.sortType == ""))
                {
                    throw new Exception("invalid sortType");
                }

                obj = srv.SearchRentalStandService(authHeader, lang, platform.ToLower(), logID, searchRentStandDTO);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Route("search/history/paidBill")]
        [HttpPost]
        public IHttpActionResult SearchHistoryPaidBill(SearchBillDTO searchBillDTO)
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
                int logID = _sql.InsertLogReceiveData("SearchHistoryPaidBill", json, timestampNow.ToString(), authHeader,
                    data.user_id, platform.ToLower());

                GetService srv = new GetService();

                var obj = new object();

                if (searchBillDTO.pageInt.Equals(null) || searchBillDTO.pageInt.Equals(0))
                {
                    throw new Exception("invalid : pageInt ");
                }

                if (searchBillDTO.perPage.Equals(null) || searchBillDTO.perPage.Equals(0))
                {
                    throw new Exception("invalid : perPage ");
                }

                if (searchBillDTO.sortField > 4)
                {
                    throw new Exception("invalid : sortField " + searchBillDTO.sortField);
                }

                if (!(searchBillDTO.sortType == "a" || searchBillDTO.sortType == "d" || searchBillDTO.sortType == "A" || searchBillDTO.sortType == "D" || searchBillDTO.sortType == ""))
                {
                    throw new Exception("invalid sortType");
                }

                obj = srv.SearchHistoryPaidBillService(authHeader, lang, platform.ToLower(), logID, searchBillDTO);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }
        #endregion

        #region Transection
        [Route("insert/transection/rent")]
        [HttpPost]
        public IHttpActionResult InsertTransectionRent(InsertTransectionRentDTO insertTransectionRentDTO)
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
                int logID = _sql.InsertLogReceiveData("InsertTransectionRent", json, timestampNow.ToString(), authHeader,
                    data.user_id, platform.ToLower());

                string checkMissingOptional = "";
                
                if (string.IsNullOrEmpty(insertTransectionRentDTO.transCode))
                {
                    checkMissingOptional += "transCode ";
                }
                if (insertTransectionRentDTO.userID == 0)
                {
                    checkMissingOptional += "userID ";
                }
                if (insertTransectionRentDTO.rentalID == 0)
                {
                    checkMissingOptional += "rentalID ";
                }
                if (string.IsNullOrEmpty(insertTransectionRentDTO.startDate))
                {
                    checkMissingOptional += "startDate ";
                }
                if (string.IsNullOrEmpty(insertTransectionRentDTO.endDate))
                {
                    checkMissingOptional += "endDate ";
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

        [Route("insert/transection/bill")]
        [HttpPost]
        public IHttpActionResult InsertTransectionBill(InsertTransectionBillDTO insertTransectionBillDTO)
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
                int logID = _sql.InsertLogReceiveData("InsertTransectionBill", json, timestampNow.ToString(), authHeader,
                    data.user_id, platform.ToLower());

                string checkMissingOptional = "";

                if (string.IsNullOrEmpty(insertTransectionBillDTO.billCode))
                {
                    checkMissingOptional += "billCode ";
                }
                if (insertTransectionBillDTO.tranRentID == 0)
                {
                    checkMissingOptional += "tranRentID ";
                }
                if (string.IsNullOrEmpty(insertTransectionBillDTO.startDate))
                {
                    checkMissingOptional += "startDate ";
                }
                if (string.IsNullOrEmpty(insertTransectionBillDTO.endDate))
                {
                    checkMissingOptional += "endDate ";
                }
                if (insertTransectionBillDTO.rentalAmount == 0)
                {
                    checkMissingOptional += "rentalAmount ";
                }
                if (insertTransectionBillDTO.electricAmount == 0)
                {
                    checkMissingOptional += "electricAmount ";
                }
                if (insertTransectionBillDTO.waterAmount == 0)
                {
                    checkMissingOptional += "waterAmount ";
                }
                if (insertTransectionBillDTO.totalAmount == 0)
                {
                    checkMissingOptional += "totalAmount ";
                }
                if ((insertTransectionBillDTO.rentalAmount + insertTransectionBillDTO.electricAmount + insertTransectionBillDTO.waterAmount) != insertTransectionBillDTO.totalAmount)
                {
                    checkMissingOptional += "The amount is not related ";
                }

                if (checkMissingOptional != "")
                {
                    throw new Exception("Missing Parameter : " + checkMissingOptional);
                }

                SaveService srv = new SaveService();
                var obj = new object();
                obj = srv.InsertTransectionBillService(authHeader, lang, platform.ToLower(), logID, insertTransectionBillDTO, data.user_id);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }
        #endregion

        #region Master
        [Route("save/master/place")]
        [HttpPost]
        public IHttpActionResult SaveMasterPlace(MasterDataDTO masterDataDTO)
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
                int logID = _sql.InsertLogReceiveData("SaveMasterPlace", json, timestampNow.ToString(), authHeader,
                    data.user_id, platform.ToLower());
                
                string checkMissingOptional = "";

                if (string.IsNullOrEmpty(masterDataDTO.mode))
                {
                    throw new Exception("Missing Parameter : mode ");
                }

                if (masterDataDTO.mode.ToLower().Equals("insert"))
                {
                    if (masterDataDTO.masterID != 0)
                    {
                        checkMissingOptional += "masterID Must 0 ";
                    }
                    if (string.IsNullOrEmpty(masterDataDTO.nameEN))
                    {
                        checkMissingOptional += "nameEN ";
                    }
                    if (string.IsNullOrEmpty(masterDataDTO.nameTH))
                    {
                        checkMissingOptional += "nameTH ";
                    }
                }
                else if (masterDataDTO.mode.ToLower().Equals("update"))
                {
                    if (masterDataDTO.masterID == 0)
                    {
                        checkMissingOptional += "masterID ";
                    }
                    if (string.IsNullOrEmpty(masterDataDTO.nameEN))
                    {
                        checkMissingOptional += "nameEN ";
                    }
                    if (string.IsNullOrEmpty(masterDataDTO.nameTH))
                    {
                        checkMissingOptional += "nameTH ";
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
                obj = srv.SaveMasterService(authHeader, lang, platform.ToLower(), logID, masterDataDTO, "system_place", data.user_id);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Route("get/master/place")]
        [HttpPost]
        public IHttpActionResult GetMasterPlace(MasterDataDTO masterDataDTO)
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
                int logID = _sql.InsertLogReceiveData("GetMasterPlace", json, timestampNow.ToString(), authHeader,
                    data.user_id, platform.ToLower());

                MasterDataService srv = new MasterDataService();

                var obj = new object();

                if (masterDataDTO.masterID != 0)
                {
                    obj = srv.GetMasterService(authHeader, lang, platform.ToLower(), logID, masterDataDTO.masterID, "system_place");
                }
                else
                {
                    throw new Exception("Missing Parameter : ID ");
                }


                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        [Route("save/master/subPlace")]
        [HttpPost]
        public IHttpActionResult SaveMasterSubPlace(MasterPlaceSubDTO masterPlaceSubDTO)
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
                string json = JsonConvert.SerializeObject(masterPlaceSubDTO);
                int logID = _sql.InsertLogReceiveData("SaveMasterSubPlace", json, timestampNow.ToString(), authHeader,
                    data.user_id, platform.ToLower());

                string checkMissingOptional = "";

                if (string.IsNullOrEmpty(masterPlaceSubDTO.mode))
                {
                    throw new Exception("Missing Parameter : mode ");
                }

                if (masterPlaceSubDTO.mode.ToLower().Equals("insert"))
                {
                    if (masterPlaceSubDTO.placeID != 0)
                    {
                        checkMissingOptional += "placeID Must 0 ";
                    }
                    if (string.IsNullOrEmpty(masterPlaceSubDTO.nameEN))
                    {
                        checkMissingOptional += "nameEN ";
                    }
                    if (string.IsNullOrEmpty(masterPlaceSubDTO.nameTH))
                    {
                        checkMissingOptional += "nameTH ";
                    }
                    if (masterPlaceSubDTO.amountRentDay != 0)
                    {
                        checkMissingOptional += "amountRentDay Must 0 ";
                    }
                    if (masterPlaceSubDTO.amountRentMonth != 0)
                    {
                        checkMissingOptional += "amountRentMonth Must 0 ";
                    }
                    if (masterPlaceSubDTO.amountRentSpecial != 0)
                    {
                        checkMissingOptional += "amountRentSpecial Must 0 ";
                    }
                    if (string.IsNullOrEmpty(masterPlaceSubDTO.specialExpireDate))
                    {
                        checkMissingOptional += "specialExpireDate ";
                    }
                    if (masterPlaceSubDTO.amountWater != 0)
                    {
                        checkMissingOptional += "amountWater Must 0 ";
                    }
                    if (masterPlaceSubDTO.amountElectricity != 0)
                    {
                        checkMissingOptional += "amountElectricity Must 0 ";
                    }
                }
                else if (masterPlaceSubDTO.mode.ToLower().Equals("update"))
                {
                    if (masterPlaceSubDTO.placeSubID == 0)
                    {
                        checkMissingOptional += "placeSubID ";
                    }
                    if (masterPlaceSubDTO.placeID == 0)
                    {
                        checkMissingOptional += "placeID ";
                    }
                    if (string.IsNullOrEmpty(masterPlaceSubDTO.nameEN))
                    {
                        checkMissingOptional += "nameEN ";
                    }
                    if (string.IsNullOrEmpty(masterPlaceSubDTO.nameTH))
                    {
                        checkMissingOptional += "nameTH ";
                    }
                    if (masterPlaceSubDTO.amountRentDay != 0)
                    {
                        checkMissingOptional += "amountRentDay Must 0 ";
                    }
                    if (masterPlaceSubDTO.amountRentMonth != 0)
                    {
                        checkMissingOptional += "amountRentMonth Must 0 ";
                    }
                    if (masterPlaceSubDTO.amountRentSpecial != 0)
                    {
                        checkMissingOptional += "amountRentSpecial Must 0 ";
                    }
                    if (string.IsNullOrEmpty(masterPlaceSubDTO.specialExpireDate))
                    {
                        checkMissingOptional += "specialExpireDate ";
                    }
                    if (masterPlaceSubDTO.amountWater != 0)
                    {
                        checkMissingOptional += "amountWater Must 0 ";
                    }
                    if (masterPlaceSubDTO.amountElectricity != 0)
                    {
                        checkMissingOptional += "amountElectricity Must 0 ";
                    }
                }
                else if (masterPlaceSubDTO.mode.ToLower().Equals("delete"))
                {
                    if (masterPlaceSubDTO.placeSubID == 0)
                    {
                        checkMissingOptional += "placeSubID ";
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
                obj = srv.SaveMasterPlaceSubService(authHeader, lang, platform.ToLower(), logID, masterPlaceSubDTO, "system_place_sub", data.user_id);

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
