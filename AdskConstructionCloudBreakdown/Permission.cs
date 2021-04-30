using System;
using System.Collections.Generic;

namespace AdskConstructionCloudBreakdown
{
    public abstract class Permission 
    {
        public AccessPermissionEnum AccessPermission { get; set; }

        //Not sure if this is the right place for it
        public static AccessPermissionEnum SelectPermission(string intput)
        {

            //Dictionary to link String tko AccessPermission
            Dictionary<string, AccessPermissionEnum> dictinoaryPermission =
                new Dictionary<string, AccessPermissionEnum>();
            dictinoaryPermission.Add("V", AccessPermissionEnum.Viewonly);
            dictinoaryPermission.Add("V+D", AccessPermissionEnum.ViewandDownload);
            dictinoaryPermission.Add("U", AccessPermissionEnum.Publishonly);
            dictinoaryPermission.Add("V+D+U", AccessPermissionEnum.ViewDownladandPublish);
            dictinoaryPermission.Add("V+D+U+E", AccessPermissionEnum.ViewDownlaodPusblishandEdit);
            dictinoaryPermission.Add("FULL", AccessPermissionEnum.Full);
            try
            {
                return dictinoaryPermission[intput];
            }
            catch
            {
                throw new Exception("Unknown AccessPermission found");
            }

        }

    }
}