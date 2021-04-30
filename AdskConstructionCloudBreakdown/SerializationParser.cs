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
            input.Context.RegisterClassMap<UserDataMap>();

            //loop over all rows
            while (input.Read())
            {
                var tmp = input.GetRecord<UserData>();

                //create only new Projects if their is a new name
                if (tmp._project_name!=null)
                {
                    output.Add(new Bim360Project(tmp._project_name));
                    activeFolder = null;
                }

                //set type
                if (tmp._project_type.Equals("Office"))
                {
                    output[output.Count].ProjectType = ProjectTypeEnum.Office;
                }
                else if (tmp._project_type.Equals("Library"))
                {
                    output[output.Count].ProjectType = ProjectTypeEnum.Library;
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
                else
                {
                    throw new Exception("Unexpected root folder!");
                }

                //ToDo: Testing
                //add subfolder into the roots
                //currently only supports until down to level3
                if (tmp._level_1 != null)
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
                else if (tmp._level_2 != null)
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
                else if (tmp._level_3 != null)
                {
                    Folder subfold = new Folder(tmp._level_3);
                    activeFolder.RootFolder.AddSubFolder(subfold);
                    activeFolder = subfold;
                    
                }



                //ToDo: Testing
                //add userpermission to active folder
                if (tmp._user_email != null)
                {
                    try
                    {
                        User user;
                        //assign company to user if exists
                        if (tmp._company != null)
                        {
                            Company comp = tmp._company_trade != null ? new Company(tmp._company,
                                tmp._company_trade) : new Company(tmp._company);

                            user = new User(tmp._user_email, comp);
                        }
                        else
                        {
                            user = new User(tmp._user_email);
                        }

                        activeFolder.GeneralPermission.Add(new UserPermission(
                            user,Permission.SelectPermission(tmp._permission)));
                    }
                    catch
                    {
                        new NullReferenceException("No folder to add user!");
                    }
                }

                //ToDo: Testing
                //add rolepermission to active folder
                if (tmp._role_permission != null)
                {
                    try
                    {
                        activeFolder.GeneralPermission.Add(new UserPermission(
                            tmp._role_permission, Permission.SelectPermission(tmp._permission)));
                    }
                    catch
                    {
                        new NullReferenceException("No folder to add Role!");
                    }
                }

                //Set local folder 
                if (tmp._local_folder != null)
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
        public static void ExportBim360ToCSV()
        {
            throw new NotImplementedException("YOU SHALL NOT PASS \ndata out currently");
        }

        





    }
}