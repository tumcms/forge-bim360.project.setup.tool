namespace AdskConstructionCloudBreakdown
{
    public class UserPermission : Permission
    {

        public User AssignedUsers { get; set; }


        //Constructor
        public UserPermission()
        {
            AssignedUsers = new User();
            AccessPermission = AccessPermissionEnum.Viewonly;
        }


        public UserPermission(string mailAddress, AccessPermissionEnum accessPermission)
        {
            AssignedUsers = new User(mailAddress);
            AccessPermission = accessPermission;
        }


        public UserPermission(User user, AccessPermissionEnum accesPermission)

        {
            AssignedUsers = user;
            AccessPermission = accessPermission;
        }

    }
}
