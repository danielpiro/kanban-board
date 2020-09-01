namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    internal class UserDTO
    {
        public const string UsersEmailColumn = "Email";
        public const string UsersNicknameColumn = "Nickname";
        public const string UsersPasswordColumn = "Password";
        public const string UsersHostColumn = "EmailHost";
        private readonly Controllers.UserControl _controller;

        public UserDTO(string email, string nickName, string password, string emailHost)
        {
            Email = email;
            Nickname = nickName;
            Password = password;
            EmailHost = emailHost;
            _controller = new Controllers.UserControl();

        }

        public string Email { get; set; }
        public string Nickname { get; set; }
        public string Password { get; set; }
        public string EmailHost { get; set; }
    }
}
