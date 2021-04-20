using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdskConstructionCloudBreakdown
{
    public class Bim360Project
    {
        public string ProjectName { get; set; }
        public ProjectTypeEnum ProjectType { get; set; }

        public List<Folder> Plans { get; set; }
        public List<Folder> ProjectFiles { get; set; }

        public Bim360Project(string projectName)
        {
            ProjectName = projectName;
            // init arrays
            Plans = new List<Folder>();
            ProjectFiles = new List<Folder>();
        }
    }

    
}
