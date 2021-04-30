using System;
using System.Collections.Generic;

namespace AdskConstructionCloudBreakdown
{
    public class Folder
    {
         private string _name;
        public string Name
        {
            get => this._name;
            set
            { _name = value;
            }
        }

        // Permission management 
        // user and company are assignment int Permission
        public List<UserPermission> GeneralPermission { get; set; }

        //maybe need to change role permission
        public List<RolePermission> RolePermission { get; set; }
        
        // nested Folders
        public List<Folder> Subfolders { get; set; }
        
        // file path to sample files
        public string SampleFilesDirectory { get; set; }


        //Constructor
        public Folder()
        {
            GeneralPermission = new List<UserPermission>();
            Subfolders = new List<Folder>();
            RolePermission = new List<RolePermission>();
        }

        public Folder(string name)
        {
            this.Name = name;
            GeneralPermission = new List<UserPermission>();
            Subfolders = new List<Folder>();
            RolePermission = new List<RolePermission>();
        }


        //Add Permissions Methods
        public void AddRolePermission(string role, AccesPermissionEnum accesPerm)
        {
            RolePermission.Add(new RolePermission(role, accesPerm));
        }

        public void AddUser(string mailAddress, AccesPermissionEnum accesPerm)
        {
            GeneralPermission.Add((new UserPermission(mailAddress,accesPerm)));
        }

        public void AddUser(User user, AccesPermissionEnum accesPerm)
        {
            GeneralPermission.Add(new UserPermission(user,accesPerm));
        }

        //Not sure if we need some other SubFolder recursive thing
        public void AddSubFolder(string name)
        {
            Subfolders.Add(new Folder());
        }

        //recursive function to get the height
        public int GetHeight()
        {
            int height=0;

            if (Subfolders != null)
            {
                foreach (var iter in Subfolders)
                {
                    int tmp = iter.GetHeight()+1;
                    if (tmp > height)
                    {
                        height = tmp;
                    }
                }
            }

            return height;
        }

    }
}