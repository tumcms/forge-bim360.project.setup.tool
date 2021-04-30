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
            // Here we need to add an new object but i dont know how
            Map(m => m._sub_folder).Name("level_1");
            Map(m => m._sub_folder).Name("level_2");
            Map(m => m._permission).Name("permission");
            Map(m => m._role_permission).Name("role_permission");
            Map(m => m._user_email).Name("user_email");
            Map(m => m._industry_role).Name("industry_role");
            Map(m => m._company).Name("company");
            Map(m => m._company_trade).Name("company_trade");
            Map(m => m._local_folder).Name("local_folder");



        }

    }
}