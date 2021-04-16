using System;
using System.Collections.Generic;
using System.Text;

namespace CustomGUI
{
    public class UserData
    {
        public string _projectname { get; set; }
        public string _projecttype { get; set; }
        public string _root_folder { get; set; }

        //Liste für Level erstellen
        public string _level_1 { get; set; }
        public string _level_2 { get; set; }
        public string _level_3 { get; set; }
        public string _permission { get; set; }
        public string _role_permission { get; set; }
        public string _user_email { get; set; }
        public string _industry_role { get; set; }
        public string _company { get; set; }
        public string _company_trade { get; set; }
        public string _local_folder { get; set; }

        /* // not sure if needed
        public Datalist(string projectname,string projecttype,string root_folder,
            string level_1,string level_2,string level_3, string permission,
            string role_permission,string user_email,string industry_role, 
            string company,string company_trade,string local_folder)
        {
            _projectname = projectname;
            _projecttype = projecttype;
            _root_folder = root_folder;
            _level_1 = level_1;
            _level_2 = level_2;
            _level_3 = level_3;
            _permission = permission;
            _role_permission = role_permission;
            _user_email = user_email;
            _industry_role = industry_role;
            _company = company;
            _company_trade = company_trade;
            _local_folder = local_folder;

        }
        public Datalist() 
        { }
        */

            /// <summary>
            /// Maps CSV to UserData input
            /// </summary>
        public void mapcsvtoinput() { }

    }

}
