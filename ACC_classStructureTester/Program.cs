using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdskConstructionCloudBreakdown;

namespace ACC_classStructureTester
{
    class Program
    {
        static void Main(string[] args)
        {

            var project1 = new Bim360Project("MyProject01")
            {
                ProjectType = ProjectTypeEnum.Office
            };

            
            project1.Plans.Add(new Folder("Level1"));
            project1.ProjectFiles.Add(new Folder("Level1"));
            project1.ProjectFiles[0].Subfolders.Add(new Folder("Level1"));

            Console.WriteLine(project1);


        }
    }
}
