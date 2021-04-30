using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AdskConstructionCloudBreakdown
{
    public class Bim360Project
    {
        public string ProjectName { get; set; }
        public ProjectTypeEnum ProjectType { get; set; }

        public Folder Plans { get; set; }
        public Folder ProjectFiles { get; set; }

        public Bim360Project(string projectName)
        {
            ProjectName = projectName;
            // init arrays
            Plans = new Folder();
            ProjectFiles = new Folder();
        }

        public Bim360Project(string projectName, ProjectTypeEnum projectType)
        {
            ProjectName = projectName;
            ProjectType = projectType;
            // init arrays
            Plans = new Folder();
            ProjectFiles = new Folder();
        }


        public int FindSubHeight()
        {
            //find the height of the subfolder of both roots
            int heightpl = Plans.GetHeight();
            int heightpf = ProjectFiles.GetHeight();
            if (heightpl > heightpf)
            {
                return heightpl;
            }
            return heightpf;
        }

    }

    
}
