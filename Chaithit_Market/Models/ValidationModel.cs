using Chaithit_Market.Core;
using Chaithit_Market.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chaithit_Market.Models
{
    public class ValidationModel
    {
        public bool Success { get; set; }
        public int InvalidCode { get; set; }
        public string InvalidText { get; set; }
        public string InvalidMessage { get; set; }

        public enum InvalidState
        {
            E501, E502,
            S200001, S201001, S200002, S200003, S200004,
            E300001, E300002, E300003, E300004, E300005, E300006, E300007, E300008,
            E301001, E301002, E301003, E301004, E301005, E301006, E301007, E301008, E301009, E301010, E301011, E301012, E301013, E301014, E301015, E301016, E301017,
            E302001, E302002, E302003,
            E303001, E303002

        }

        public static int GetInvalidCode(InvalidState state)
        {
            switch (state)
            {
                case InvalidState.E501:
                    return 501;
                case InvalidState.E502:
                    return 502;
                case InvalidState.E301001:
                    return 301001;
                case InvalidState.E301002:
                    return 301002;
                case InvalidState.E301003:
                    return 301003;
                case InvalidState.E301004:
                    return 301004;
                case InvalidState.E301005:
                    return 301005;
                case InvalidState.E301007:
                    return 301007;
                case InvalidState.E301008:
                    return 301008;
                case InvalidState.E301009:
                    return 301009;
                case InvalidState.E301010:
                    return 301010;
                case InvalidState.E301011:
                    return 301011;
                case InvalidState.E301012:
                    return 301012;
                case InvalidState.E301013:
                    return 301013;
                case InvalidState.E301014:
                    return 301014;
                case InvalidState.E301015:
                    return 301015;
                case InvalidState.E301016:
                    return 301016;
                case InvalidState.E301017:
                    return 301016;
                case InvalidState.E302001:
                    return 302001;
                case InvalidState.E300001:
                    return 300001;
                default:
                    return 0;
            }
        }

        public static GetMessageTopicDTO GetInvalidMessage(InvalidState state, string lang)
        {
            SQLManager _sql = SQLManager.Instance;
            GetMessageTopicDTO message = new GetMessageTopicDTO();
            int messagecode = 0;
            switch (state)
            {
                case InvalidState.E501:
                    message.message = "เข้าสู่ระบบด้วยวิธีการไม่ถูกต้อง";
                    return message;
                case InvalidState.E502:
                    message.message = "token หมดอายุ";
                    return message;
                case InvalidState.E301001:
                    messagecode = 301001;
                    message = _sql.GetMessageLang(lang.ToLower(), messagecode);
                    //message = "ชื่อผู้ใช้หรือรหัสผ่านไม่ถูกต้อง";
                    return message;
                case InvalidState.E301002:
                    messagecode = 301002;
                    message = _sql.GetMessageLang(lang.ToLower(), messagecode);
                    //message = "คุณยังไม่ได้เปลี่ยนรหัสผ่านครั้งแรก";
                    return message;
                case InvalidState.E301003:
                    messagecode = 301003;
                    message = _sql.GetMessageLang(lang.ToLower(), messagecode);
                    //message = "บัญชีของคุณถูกระงับ โปรดติดต่อฝ่ายบุคคล";
                    return message;
                case InvalidState.E301004:
                    messagecode = 301004;
                    message = _sql.GetMessageLang(lang.ToLower(), messagecode);
                    return message;
                case InvalidState.E301005:
                    messagecode = 301005;
                    message = _sql.GetMessageLang(lang.ToLower(), messagecode);
                    return message;
                case InvalidState.E301006:
                    messagecode = 301006;
                    message = _sql.GetMessageLang(lang.ToLower(), messagecode);
                    return message;
                case InvalidState.E301007:
                    messagecode = 301007;
                    message = _sql.GetMessageLang(lang.ToLower(), messagecode);
                    return message;
                case InvalidState.E301008:
                    messagecode = 301008;
                    message = _sql.GetMessageLang(lang.ToLower(), messagecode);
                    return message;
                case InvalidState.E301009:
                    messagecode = 301009;
                    message = _sql.GetMessageLang(lang.ToLower(), messagecode);
                    return message;
                case InvalidState.E301010:
                    messagecode = 301010;
                    message = _sql.GetMessageLang(lang.ToLower(), messagecode);
                    return message;
                case InvalidState.E301011:
                    messagecode = 301011;
                    message = _sql.GetMessageLang(lang.ToLower(), messagecode);
                    return message;
                case InvalidState.E301012:
                    messagecode = 301012;
                    message = _sql.GetMessageLang(lang.ToLower(), messagecode);
                    return message;
                case InvalidState.E301013:
                    messagecode = 301013;
                    message = _sql.GetMessageLang(lang.ToLower(), messagecode);
                    return message;
                case InvalidState.E301014:
                    messagecode = 301014;
                    message = _sql.GetMessageLang(lang.ToLower(), messagecode);
                    return message;
                case InvalidState.E301015:
                    messagecode = 301015;
                    message = _sql.GetMessageLang(lang.ToLower(), messagecode);
                    return message;
                case InvalidState.E301016:
                    messagecode = 301016;
                    message = _sql.GetMessageLang(lang.ToLower(), messagecode);
                    return message;
                case InvalidState.E301017:
                    messagecode = 301017;
                    message = _sql.GetMessageLang(lang.ToLower(), messagecode);
                    return message;
                case InvalidState.E302001:
                    messagecode = 302001;
                    message = _sql.GetMessageLang(lang.ToLower(), messagecode);
                    //message = "ไม่พบข้อมูล";
                    return message;
                case InvalidState.E300001:
                    messagecode = 300001;
                    message = _sql.GetMessageLang(lang.ToLower(), messagecode);
                    //message = "error platform";
                    return message;
                case InvalidState.S200001:
                    messagecode = 200001;
                    message = _sql.GetMessageLang(lang.ToLower(), messagecode);
                    //message = "success all";
                    return message;
                case InvalidState.S201001:
                    messagecode = 201001;
                    message = _sql.GetMessageLang(lang.ToLower(), messagecode);
                    //message = "success all";
                    return message;
                case InvalidState.S200002:
                    messagecode = 200002;
                    message = _sql.GetMessageLang(lang.ToLower(), messagecode);
                    return message;
                case InvalidState.S200003:
                    messagecode = 200003;
                    message = _sql.GetMessageLang(lang.ToLower(), messagecode);
                    return message;
                //message = "error upload";
                case InvalidState.E300002:
                    messagecode = 300002;
                    message = _sql.GetMessageLang(lang.ToLower(), messagecode);
                    //message = "error upload";
                    return message;
                case InvalidState.E300003:
                    messagecode = 300003;
                    message = _sql.GetMessageLang(lang.ToLower(), messagecode);
                    //message = "error upload";
                    return message;
                case InvalidState.E300004:
                    messagecode = 300004;
                    message = _sql.GetMessageLang(lang.ToLower(), messagecode);
                    //message = "error upload";
                    return message;
                case InvalidState.E300005:
                    messagecode = 300005;
                    message = _sql.GetMessageLang(lang.ToLower(), messagecode);
                    //message = "error captcha";
                    return message;
                case InvalidState.E300006:
                    messagecode = 300006;
                    message = _sql.GetMessageLang(lang.ToLower(), messagecode);
                    //message = "error captcha";
                    return message;
                case InvalidState.E300007:
                    messagecode = 300007;
                    message = _sql.GetMessageLang(lang.ToLower(), messagecode);
                    //message = "error captcha";
                    return message;
                case InvalidState.E300008:
                    messagecode = 300008;
                    message = _sql.GetMessageLang(lang.ToLower(), messagecode);
                    //message = "error captcha";
                    return message;
                case InvalidState.E302002:
                    messagecode = 302002;
                    message = _sql.GetMessageLang(lang.ToLower(), messagecode);
                    //message = "error name employee";
                    return message;
                case InvalidState.E303001:
                    messagecode = 303001;
                    message = _sql.GetMessageLang(lang.ToLower(), messagecode);
                    //message = "error Day of leave";
                    return message;
                case InvalidState.E303002:
                    messagecode = 303002;
                    message = _sql.GetMessageLang(lang.ToLower(), messagecode);
                    //message = "You are not entitled to use this type of leave. (not passed the promotion)";
                    return message;
                default:
                    return message;

            }
        }
    }
}