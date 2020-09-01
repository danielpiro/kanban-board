namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    internal class BoardEmailsDTO
    {

        public const string EmailColumn = "Email";
        public const string HostColumn = "Host";
        private readonly Controllers.BoardEmailsControl _controller;
        public BoardEmailsDTO(string email, string host)
        {
            Email = email;
            Host = host;
            _controller = new Controllers.BoardEmailsControl();
        }

        public string Email { get; set; }
        public string Host { get; set; }
    }
}
