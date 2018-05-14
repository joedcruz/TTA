namespace TTAServer
{
    /// <summary>
    /// Used by API AssignUserRoles
    /// </summary>
    public class AssignUserRoles
    {
        // Username of the user to authenticate
        public string Username { get; set; }

        // List of roles to be assigned to the user
        public string[] NewRoles { get; set; }
    }
}
