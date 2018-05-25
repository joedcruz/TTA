namespace TTAServer
{
    /// <summary>
    /// Used by API AssignUserRoles and GetUserRoles
    /// </summary>
    public class UserRolesModel
    {
        public string UserId { get; set; }

        // Username of the user to authenticate
        public string Username { get; set; }

        public string[] RoleIds { get; set; }

        // List of roles to be assigned to the user
        public string[] RoleNames { get; set; }
    }
}
