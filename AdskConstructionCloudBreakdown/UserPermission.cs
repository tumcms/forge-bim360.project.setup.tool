namespace AdskConstructionCloudBreakdown
{
    public class UserPermission
    {

        public User AssignedUsers { get; set; }

        public AccessPermissionEnum AccesPermission { get; set; }


        //Constructor
        public UserPermission()
        {
            AssignedUsers = new User();
            AccesPermission = AccessPermissionEnum.Viewonly;
        }

        public UserPermission(string mailAddress, AccessPermissionEnum accesPermission)
        {
            AssignedUsers = new User(mailAddress);
            AccesPermission = accesPermission;
        }

        public UserPermission(User user, AccessPermissionEnum accesPermission)
        {
            AssignedUsers = user;
            AccesPermission = accesPermission;
        }

    }
}
