using System;
using System.Collections.Generic;
using System.Linq;

namespace AdskConstructionCloudBreakdown
{
    public class Folder
    {
        public string Name { get; set; }

        // Permission management 
        // user and company
        public List<UserPermission> UserPermissions { get; set; }

        //maybe need to change role permission
        public List<RolePermission> RolePermissions { get; set; }

        // nested Folders
        public List<Folder> Subfolders { get; set; }

        // file path to sample files
        public string SampleFilesDirectory { get; set; }

        //Infos only for construction purpose
        public Folder RootFolder { get; set; }
        public int level { get; set; }

        //Constructor
        public Folder()
        {
            UserPermissions = new List<UserPermission>();
            Subfolders = new List<Folder>();
            RolePermissions = new List<RolePermission>();
            level = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public Folder(string name)
        {
            this.Name = name;
            UserPermissions = new List<UserPermission>();
            Subfolders = new List<Folder>();
            RolePermissions = new List<RolePermission>();
            level = 0;
        }


        /// <summary>
        /// Adds a role permission to the current folder
        /// </summary>
        /// <param name="role"></param>
        /// <param name="accessPerm"></param>
        public void AddRolePermission(string role, AccessPermissionEnum accessPerm)
        {
            RolePermissions.Add(new RolePermission(role, accessPerm));
        }

        /// <summary>
        /// Adds a user to the current folder
        /// </summary>
        /// <param name="mailAddress"></param>
        /// <param name="accessPerm">User permission </param>
        public void AddUser(string mailAddress, AccessPermissionEnum accessPerm)
        {
            UserPermissions.Add((new UserPermission(mailAddress, accessPerm)));
        }

        public void AddUser(User user, AccessPermissionEnum accessPerm)
        {
            UserPermissions.Add(new UserPermission(user, accessPerm));
        }

        //Not sure if we need some other SubFolder recursive thing
        public void AddSubFolder(string name)
        {
            Subfolders.Add(new Folder());
            Subfolders.Last().RootFolder = this;
            Subfolders.Last().level = this.level + 1;
        }

        public void AddSubFolder(Folder name)
        {
            Subfolders.Add(name);
            name.RootFolder = this;
            name.level = this.level + 1;
        }

        //recursive function to get the height
        public int GetHeight()
        {
            int height = 0;

            if (Subfolders != null)
            {
                foreach (var iter in Subfolders)
                {
                    int tmp = iter.GetHeight() + 1;
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