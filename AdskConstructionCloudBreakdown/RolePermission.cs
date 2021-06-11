namespace AdskConstructionCloudBreakdown
{
    public class RolePermission : Permission
    {

        public string Role { get; set; }


        //Constructor
        public RolePermission()
        {
            Role = string.Empty;
            AccessPermission = AccessPermissionEnum.Viewonly;
        }


        public RolePermission(string role, AccessPermissionEnum accessPermission)
        {
            this.Role = role;
            this.AccessPermission = accessPermission;
        }

    }
}