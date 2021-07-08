using CsvHelper.Configuration;
using System;
using System.Collections.Generic;

namespace CustomGUI
{
    public class UserData
    {

        public string _project_name { get; set; }
        public string _project_type { get; set; }
        public string _root_folder { get; set; }

        //Liste für Level erstellen
        public List<String> _sub_folder { get; set; }
        public string _level_1 { get; set; }
        public string _level_2 { get; set; }
        public string _level_3 { get; set; }
        public string _level_4 { get; set; }
        public string _level_5 { get; set; }
        public string _level_6 { get; set; }
        public string _level_7 { get; set; }
        public string _level_8 { get; set; }
        public string _level_9 { get; set; }
        public string _level_10 { get; set; }
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
        public void MapCsvToInput() { }

    }

    /// <summary>
    /// Maps CSV Header to UserData attributs (Import) 
    /// </summary>
    public class UserDataMap : ClassMap<UserData>
    {
        public UserDataMap()
        {
            Map(m => m._project_name).Name("project_name");
            Map(m => m._project_type).Name("project_type");
            Map(m => m._root_folder).Name("root_folder");
            Map(m => m._level_1).Name("level_1");
            Map(m => m._level_2).Name("level_2");
            Map(m => m._level_3).Name("level_3");
            Map(m => m._level_4).Name("level_4");
            Map(m => m._level_5).Name("level_5");
            Map(m => m._level_6).Name("level_6");
            Map(m => m._level_7).Name("level_7");
            Map(m => m._level_8).Name("level_8");
            Map(m => m._level_9).Name("level_9");
            Map(m => m._level_10).Name("level_10");
            Map(m => m._permission).Name("permission");
            Map(m => m._role_permission).Name("role_permission");
            Map(m => m._user_email).Name("user_email");
            Map(m => m._industry_role).Name("industry_role");
            Map(m => m._company).Name("company");
            Map(m => m._company_trade).Name("company_trade");
            Map(m => m._local_folder).Name("local_folder");



        }

    }

    public class UserDataExport : ClassMap<UserData>
    {
        public UserDataExport()
        {

            Map(m => m._project_name).Index(0).Name("project_name");
            Map(m => m._project_type).Index(1).Name("project_type");
            Map(m => m._root_folder).Index(2).Name("root_folder");
            Map(m => m._level_1).Index(3).Name("level_1");
            Map(m => m._level_2).Index(4).Name("level_2");
            Map(m => m._level_3).Index(5).Name("level_3");
            Map(m => m._level_4).Index(6).Name("level_4");
            Map(m => m._level_5).Index(7).Name("level_5");
            Map(m => m._level_6).Index(8).Name("level_6");
            Map(m => m._level_7).Index(9).Name("level_7");
            Map(m => m._level_8).Index(10).Name("level_8");
            Map(m => m._level_9).Index(11).Name("level_9");
            Map(m => m._level_10).Index(12).Name("level_10");
            Map(m => m._permission).Index(13).Name("permission");
            Map(m => m._role_permission).Index(14).Name("role_permission");
            Map(m => m._user_email).Index(15).Name("user_email");
            Map(m => m._industry_role).Index(16).Name("industry_role");
            Map(m => m._company).Index(17).Name("company");
            Map(m => m._company_trade).Index(18).Name("company_trade");
            Map(m => m._local_folder).Index(19).Name("local_folder");

        }
    }

}