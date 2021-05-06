using System;
using System.Collections.Generic;

namespace AdskConstructionCloudBreakdown
{
    public abstract class Permission 
    {
        public AccessPermissionEnum AccessPermission { get; set; }

        //Not sure if this is the right place for it
        public static AccessPermissionEnum SelectPermission(string input)
        {

            //Dictionary to link String tko AccessPermission
            Dictionary<string, AccessPermissionEnum> permissionDict =
                new Dictionary<string, AccessPermissionEnum>();
            permissionDict.Add("V", AccessPermissionEnum.Viewonly);
            permissionDict.Add("V+D", AccessPermissionEnum.ViewandDownload);
            permissionDict.Add("U", AccessPermissionEnum.Publishonly);
            permissionDict.Add("V+D+U", AccessPermissionEnum.ViewDownladandPublish);
            permissionDict.Add("V+D+U+E", AccessPermissionEnum.ViewDownlaodPusblishandEdit);
            permissionDict.Add("FULL", AccessPermissionEnum.Full);
            try
            {
                return permissionDict[input];
            }
            catch
            {
                throw new Exception("Unknown AccessPermission found");
            }

        }

    }
}