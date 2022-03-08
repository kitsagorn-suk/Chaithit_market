using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Chaithit_Market.Models
{
    public class LoginModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public LoginData data { get; set; }
    }

    public class LoginData
    {
        public int id { get; set; } = 0;
        public string username { get; set; } = "";
        public string employeeName { get; set; } = "";
        public string imageUrl { get; set; } = "";
        public string token { get; set; } = "";
        public string platform { get; set; } = "";
        public int roleID { get; set; } = 0;

        public List<AccessRole> access_list { get; set; }
        public List<MenuList> menuList { get; set; }

        public void loadData(DataRow dr)
        {
            id = int.Parse(dr["id"].ToString());
            username = dr["username"].ToString();
            employeeName = dr["firstname"].ToString() + " " + dr["lastname"].ToString();
            imageUrl = dr["image_name"].ToString();
            roleID = int.Parse(dr["role_id"].ToString());
        }
    }

    public class AccessRole
    {
        public string object_id { set; get; }
        public string object_name { set; get; }

        public void loadDataAccessRole(DataRow dr)
        {
            object_id = dr["object_id"].ToString();
            object_name = dr["object_name"].ToString();
        }
    }

    public class MenuList
    {
        public int menuID { set; get; }
        public string menuName { set; get; }
        public string object_id { set; get; }
        public int numberNo { set; get; }
        public int parentID { set; get; }
        public List<MenuList> menuSub { get; set; }

        public void loadDataMenu(DataRow dr)
        {
            menuID = int.Parse(dr["id"].ToString());
            menuName = dr["name"].ToString();
            object_id = dr["object_id"].ToString();
            numberNo = int.Parse(dr["order"].ToString());
            parentID = int.Parse(dr["parent_id"].ToString());
        }

    }
}