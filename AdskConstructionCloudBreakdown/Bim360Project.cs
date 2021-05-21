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

        /// <summary>
        /// Creates a BIM360Project instance with a project name
        /// </summary>
        /// <param name="projectName"></param>
        public Bim360Project(string projectName)
        {
            ProjectName = projectName;
            // init arrays
            Plans = new Folder();
            Plans.Name = "Plans";
            ProjectFiles = new Folder();
            ProjectFiles.Name = "ProjectFiles";
        }

        /// <summary>
        /// Creates a BIM360Project instance with project name and type
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="projectType"></param>
        public Bim360Project(string projectName, ProjectTypeEnum projectType)
        {
            ProjectName = projectName;
            ProjectType = projectType;
            // init arrays
            Plans = new Folder();
            ProjectFiles = new Folder();
        }

        public Bim360Project(Bim360Project tocopyproject)
        {
            this.ProjectName = tocopyproject.ProjectName;
            this.ProjectType = tocopyproject.ProjectType;
            // init arrays
            Plans = new Folder();
            ProjectFiles = new Folder();
            Plans.Clone(tocopyproject.Plans);
            ProjectFiles.Clone(tocopyproject.ProjectFiles);

        }

        /// <summary>
        /// Returns the max height of folder hierarchy 
        /// </summary>
        /// <returns></returns>
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
