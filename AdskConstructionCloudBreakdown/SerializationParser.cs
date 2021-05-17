using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using CustomGUI;

namespace AdskConstructionCloudBreakdown
{
    public class SerializationParser
    {
        public SerializationParser()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Bim360Project> LoadBim360ProjectsFromCsv(CsvReader input)
        {
            //var declaration
            var output = new List<Bim360Project>();
            Folder activeFolder = new Folder();
            //Maps the Header of the CSV Data to the class attributs
            input.Context.RegisterClassMap<UserDataMap>();

            //loop over all rows
            while (input.Read())
            {
                UserData tmp;
                try
                {
                    tmp  = input.GetRecord<UserData>();
                }
                catch(BadDataException)
                {
                    return null;
                }

                //create only new Projects if their is a new name
                if (tmp._project_name!="")
                {
                    output.Add(new Bim360Project(tmp._project_name));
                    activeFolder = null;
                }

                //set type
                if (tmp._project_type != "")
                {
                    output.Last().ProjectType = Selection.SelectProjectType(tmp._project_type);
                }

                //change where sub folder shoulde be located
                if (tmp._root_folder.Equals("Plans"))
                {
                    activeFolder = output.Last().Plans;
                }
                else if (tmp._root_folder.Equals("Project Files"))
                {
                    activeFolder = output.Last().ProjectFiles;
                }
                else if(tmp._root_folder!="")
                {
                    throw new Exception("Unexpected root folder!");
                }


                //add subfolder into the roots
                //currently only supports until down to level10
                if (!string.IsNullOrEmpty(tmp._level_1))
                {
                    Folder subfold = new Folder(tmp._level_1);
                    //decide where to put it
                    while (activeFolder.level > 0)
                    {
                        activeFolder = activeFolder.RootFolder;
                    }
                    activeFolder.AddSubFolder(subfold);
                    activeFolder = subfold;
                }
                else if (!string.IsNullOrEmpty(tmp._level_2))
                {
                    Folder subfold = new Folder(tmp._level_2);
                    //decide where to put it
                    while (activeFolder.level > 1)
                    {
                        activeFolder = activeFolder.RootFolder;
                    }
                    activeFolder.AddSubFolder(subfold);
                    activeFolder = subfold;
                }
                else if (!string.IsNullOrEmpty(tmp._level_3))
                {
                    Folder subfold = new Folder(tmp._level_3);
                    //decide where to put it
                    while (activeFolder.level > 2)
                    {
                        activeFolder = activeFolder.RootFolder;
                    }
                    activeFolder.AddSubFolder(subfold);
                    activeFolder = subfold;
                }
                else if (!string.IsNullOrEmpty(tmp._level_4))
                {
                    Folder subfold = new Folder(tmp._level_4);
                    //decide where to put it
                    while (activeFolder.level > 3)
                    {
                        activeFolder = activeFolder.RootFolder;
                    }
                    activeFolder.AddSubFolder(subfold);
                    activeFolder = subfold;
                }
                else if (!string.IsNullOrEmpty(tmp._level_5))
                {
                    Folder subfold = new Folder(tmp._level_5);
                    //decide where to put it
                    while (activeFolder.level > 4)
                    {
                        activeFolder = activeFolder.RootFolder;
                    }
                    activeFolder.AddSubFolder(subfold);
                    activeFolder = subfold;
                }
                else if (!string.IsNullOrEmpty(tmp._level_6))
                {
                    Folder subfold = new Folder(tmp._level_6);
                    //decide where to put it
                    while (activeFolder.level > 5)
                    {
                        activeFolder = activeFolder.RootFolder;
                    }
                    activeFolder.AddSubFolder(subfold);
                    activeFolder = subfold;
                }
                else if (!string.IsNullOrEmpty(tmp._level_7))
                {
                    Folder subfold = new Folder(tmp._level_7);
                    //decide where to put it
                    while (activeFolder.level > 6)
                    {
                        activeFolder = activeFolder.RootFolder;
                    }
                    activeFolder.AddSubFolder(subfold);
                    activeFolder = subfold;
                }
                else if (!string.IsNullOrEmpty(tmp._level_8))
                {
                    Folder subfold = new Folder(tmp._level_8);
                    //decide where to put it
                    while (activeFolder.level > 7)
                    {
                        activeFolder = activeFolder.RootFolder;
                    }
                    activeFolder.AddSubFolder(subfold);
                    activeFolder = subfold;
                }
                else if (!string.IsNullOrEmpty(tmp._level_9))
                {
                    Folder subfold = new Folder(tmp._level_9);
                    //decide where to put it
                    while (activeFolder.level > 8)
                    {
                        activeFolder = activeFolder.RootFolder;
                    }
                    activeFolder.AddSubFolder(subfold);
                    activeFolder = subfold;
                }
                else if (!string.IsNullOrEmpty(tmp._level_10))
                {
                    Folder subfold = new Folder(tmp._level_10);
                    //decide where to put it
                    while (activeFolder.level > 9)
                    {
                        activeFolder = activeFolder.RootFolder;
                    }
                    activeFolder.AddSubFolder(subfold);
                    activeFolder = subfold;
                }


                //add userpermission to active folder
                if (!string.IsNullOrEmpty(tmp._user_email))
                {
                    User user;
                    //assign company to user if exists
                    if (tmp._company != "")
                    {
                        Company comp = !string.IsNullOrEmpty(tmp._company_trade) ? new Company(tmp._company,
                            tmp._company_trade) : new Company(tmp._company);

                        user = new User(tmp._user_email, comp);

                    }
                    else
                    {
                        user = new User(tmp._user_email);
                    }

                    var userperadd = new UserPermission(user, Permission.SelectPermission(tmp._permission));
                    //add industry role
                    string[] tobeadd = tmp._industry_role.Split(',');
                    foreach (string iter in tobeadd)
                    {
                        userperadd.AssignedUsers.IndustryRoles.Add((iter.Trim()));
                    }

                    //add permission to Folder
                    activeFolder.UserPermissions.Add(userperadd);

                }


                //add rolepermission to active folder
                if (!string.IsNullOrEmpty(tmp._role_permission))
                {
                    try
                    {
                        activeFolder.RolePermissions.Add(new RolePermission(
                            tmp._role_permission, Permission.SelectPermission(tmp._permission)));
                    }
                    catch
                    {
                        new NullReferenceException("No folder to add Role!");
                    }
                }

                //Set local folder 
                if (!string.IsNullOrEmpty(tmp._local_folder))
                {
                    activeFolder.SampleFilesDirectory = tmp._local_folder;
                }


            }

            return output ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Bim360Project> LoadBim360ProjectsFromJson()
        {
            throw new NotImplementedException("this is openSource. Do it yourself. ");
        }


        ///<summary>
        ///
        /// </summary>
        public static List<UserData> ExportBim360ToCSV(List<Bim360Project> input)
        {
            List<UserData> output = new List<UserData>();

            //iterate over all BimProjects
            foreach (var iter in input)
            {
                //add new Row in the "CSV"
                output.Add(new UserData());
                var activeRow = output.Last();
                activeRow._project_name = iter.ProjectName;

                //add projectTypes
                activeRow._project_type = Selection.SelectProjectType(iter.ProjectType);
                

                //Add folder structure under Plans
                //Hardcoded for Plans & Projcet Files
                activeRow._root_folder = "Plans";
                activeRow._local_folder = iter.Plans.SampleFilesDirectory;

                //add User permissions
                foreach (var iterperm in iter.Plans.UserPermissions)
                {
                    AddUserPermission(activeRow, iterperm);
                    output.Add(new UserData());
                    activeRow = output.Last();
                }
                //add role permissions
                foreach (var iterperm in iter.Plans.RolePermissions)
                {
                    activeRow._permission=Permission.SelectPermission(iterperm.AccessPermission);
                    activeRow._role_permission = iterperm.Role;
                    output.Add(new UserData());
                    activeRow = output.Last();
                }

                //if no permission is inserted ->add new row for new Folder
                if (!iter.Plans.UserPermissions.Any() && !iter.Plans.RolePermissions.Any())
                {
                    activeRow = output.Last();
                }


                //add subfolder for Plans
                foreach (var subfolder in iter.Plans.Subfolders)
                {
                    AddAllSubFolder(output, subfolder);
                }

                //Add folder structure under Project Files
                output.Add(new UserData());
                activeRow = output.Last();

                activeRow._root_folder = "Project Files";
                activeRow._local_folder = iter.Plans.SampleFilesDirectory;

                //add User permissions
                foreach (var iterperm in iter.ProjectFiles.UserPermissions)
                {
                    AddUserPermission(activeRow, iterperm);
                    output.Add(new UserData());
                    activeRow = output.Last();
                }
                //add role permissions
                foreach (var iterperm in iter.ProjectFiles.RolePermissions)
                {
                    activeRow._permission = Permission.SelectPermission(iterperm.AccessPermission);
                    activeRow._role_permission = iterperm.Role;
                    output.Add(new UserData());
                    activeRow = output.Last();
                }

                //if no permission is inserted ->add new row for new Folder
                if (!iter.ProjectFiles.UserPermissions.Any() && !iter.ProjectFiles.RolePermissions.Any())
                {
                    activeRow = output.Last();
                }


                //add subfolder
                foreach (var subfolder in iter.ProjectFiles.Subfolders)
                {
                    AddAllSubFolder(output, subfolder);
                }

                //Create an empty row
                output.Add(new UserData());
            }


            return output;
        }

        //helpful function dont know if this is the right place for it

        public static void AddUserPermission(UserData addto, UserPermission from )
        {
            if (from.AssignedUsers == null)
            {
                return;
            }
            addto._permission = Permission.SelectPermission(from.AccessPermission);
            addto._user_email = from.AssignedUsers.MailAddress;
            string tmp = "";
            //add all roles up to one string
            if (from.AssignedUsers.IndustryRoles != null)
            {
                for (int i = 0; i < from.AssignedUsers.IndustryRoles.Count; i++)
                {
                    if (i != 0)
                    {
                        tmp += ",";
                    }

                    tmp += from.AssignedUsers.IndustryRoles[i];

                }

                addto._industry_role = tmp;

            }

            if (from.AssignedUsers.AssignedCompany != null)
            {
                addto._company = from.AssignedUsers.AssignedCompany.Name;
                addto._company_trade = Selection.SelectTrade(from.AssignedUsers.AssignedCompany.Trade);
            }

        }

        public static void AddAllSubFolder(List<UserData> addto, Folder from)
        {
            var activeRow = addto.Last();
            if (from.Name.Equals("Folder1.1.1"))
            {
                //just for debugging
                var t = from;
            }
            //select Level
            switch (from.level)
            {
                case 1:
                    activeRow._level_1 = from.Name;
                    break;
                case 2:
                    activeRow._level_2 = from.Name;
                    break;
                case 3:
                    activeRow._level_3 = from.Name;
                    break;
                case 4:
                    activeRow._level_4 = from.Name;
                    break;
                case 5:
                    activeRow._level_5 = from.Name;
                    break;
                case 6:
                    activeRow._level_6 = from.Name;
                    break;
                case 7:
                    activeRow._level_7 = from.Name;
                    break;
                case 8:
                    activeRow._level_8 = from.Name;
                    break;
                case 9:
                    activeRow._level_9 = from.Name;
                    break;
                case 10:
                    activeRow._level_10 = from.Name;
                    break;
            }
            activeRow._local_folder = from.SampleFilesDirectory;
            //add user Permissions
            foreach (var iterperm in from.UserPermissions)
            {
                AddUserPermission(activeRow, iterperm);
                addto.Add(new UserData());
                activeRow = addto.Last();
            }

            //add role Permission
            foreach (var iterperm in from.RolePermissions)
            {
                activeRow._permission = Permission.SelectPermission(iterperm.AccessPermission);
                activeRow._role_permission = iterperm.Role;
                addto.Add(new UserData());
                activeRow = addto.Last();
            }

            //if no permission is inserted ->add new row for new Folder
            if (!from.UserPermissions.Any() && !from.RolePermissions.Any())
            {
                activeRow = addto.Last();
                addto.Add(new UserData());
                activeRow = addto.Last();
            }


            foreach (var subfolder in from.Subfolders)
            {
                AddAllSubFolder(addto,subfolder);
            }

        }



    }
}