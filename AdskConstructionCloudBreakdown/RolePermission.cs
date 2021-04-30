using System.Collections.Generic;

namespace AdskConstructionCloudBreakdown
{
    public class RolePermission
    {

        public string Role { get; set; }

        public AccessPermissionEnum AccesPermission { get; set; }



        //Constructor
        public RolePermission()
        {
            Role = string.Empty;
            AccesPermission = AccessPermissionEnum.Viewonly;
        }

        public RolePermission(string role, AccessPermissionEnum accesPermission)
        {
            this.Role = role;
            this.AccesPermission = accesPermission;
        }

    }
}