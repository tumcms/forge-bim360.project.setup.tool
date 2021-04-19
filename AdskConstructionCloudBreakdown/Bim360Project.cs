using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdskConstructionCloudBreakdown
{
    public class Bim360Project
    {
        public string projectName { get; set; }
        public ProjectTypeEnum ProjectType { get; set; }

        public Folder Folders { get; set; }


    }

    
}
