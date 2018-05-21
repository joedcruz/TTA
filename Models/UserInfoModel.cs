namespace TTAServer
{
    /// <summary>
    /// Used by API UserDetailsController
    /// </summary>
    public class UserInfoModel
    {
        // Username of the user
        public string Username { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
