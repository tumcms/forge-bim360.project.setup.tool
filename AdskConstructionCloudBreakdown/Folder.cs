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
        public string GeneralPermission { get; set; }
        public string RolePermission { get; set; }

        // user and company assignment
        public List<User> AssignedUsers { get; set; }
        public Company AssignedCompany { get; set; }
        
        // nested Folders
        public List<Folder> Subfolders { get; set; }
        
        // file path to sample files
        public string SampleFilesDirectory { get; set; }

        public Folder()
        {
            AssignedUsers = new List<User>();
            Subfolders = new List<Folder>();
        }

        public Folder(string name)
        {
            this.Name = name;
            AssignedUsers = new List<User>();
            Subfolders = new List<Folder>();
        }
    }
}