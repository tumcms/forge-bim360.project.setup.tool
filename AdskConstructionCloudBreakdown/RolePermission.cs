using System.Collections.Generic;

namespace AdskConstructionCloudBreakdown
{
    public class RolePermission
    {

        public string Role { get; set; }

        public AccesPermissionEnum AccesPermission { get; set; }



        //Constructor
        public RolePermission()
        {
            Role = string.Empty;
            AccesPermission = AccesPermissionEnum.Viewonly;
        }

        public RolePermission(string role, AccesPermissionEnum accesPermission)
        {
            this.Role = role;
            this.AccesPermission = accesPermission;
        }

    }
}