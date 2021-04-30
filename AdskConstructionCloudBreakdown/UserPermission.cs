namespace AdskConstructionCloudBreakdown
{
    public class UserPermission : Permission
    {

        public User AssignedUsers { get; set; }
        

        //Constructor
        public UserPermission()
        {
            AssignedUsers = new User();
            AccesPermission = AccesPermissionEnum.Viewonly;
        }

        public UserPermission(string mailAddress, AccesPermissionEnum accesPermission)
        {
            AssignedUsers = new User(mailAddress);
            AccesPermission = accesPermission;
        }

        public UserPermission(User user, AccesPermissionEnum accesPermission)
        {
            AssignedUsers = user;
            AccesPermission = accesPermission;
        }

    }
}
