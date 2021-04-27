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

        public List<Folder> Plans { get; set; }
        public List<Folder> ProjectFiles { get; set; }

        public Bim360Project(string projectName)
        {
            ProjectName = projectName;
            // init arrays
            Plans = new List<Folder>();
            ProjectFiles = new List<Folder>();
        }

        public Bim360Project(string projectName, ProjectTypeEnum projectType)
        {
            ProjectName = projectName;
            ProjectType = projectType;
            // init arrays
            Plans = new List<Folder>();
            ProjectFiles = new List<Folder>();
        }


        public int Findsubheight()
        {
            int height=0;

            //find the height of the subfolder of both roots
            foreach (var iter in Plans)
            {
                int tmp = iter.Getheight();
                if (tmp > height)
                {
                    height = tmp;
                }
            }

            foreach (var iter in ProjectFiles)
            {
                int tmp = iter.Getheight();
                if (tmp > height)
                {
                    height = tmp;
                }
            }

            return height;
        }

    }

    
}
