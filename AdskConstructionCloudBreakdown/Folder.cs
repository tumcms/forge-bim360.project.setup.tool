using System.Collections.Generic;

namespace AdskConstructionCloudBreakdown
{
    public class Folder
    {
        public bool isTopFolder { get; set; } // set to true if RootFolder value is assigned
        public string RootFolder { get; set; } // "Plans" or "Project files"

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

    }
}