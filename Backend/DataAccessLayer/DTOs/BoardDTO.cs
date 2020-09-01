namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    internal class BoardDTO
    {
        public const string BoardEmailColumnEmail = "Email";
        public const string BoardDeletedTaskColumn = "DeletedTasks";
        private readonly Controllers.BoardControl _controller;
        public BoardDTO(string email, int deletedTasks)
        {
            Email = email;
            DeletedTasks = deletedTasks;
            _controller = new Controllers.BoardControl();
        }

        public string Email { get; set; }
        public int DeletedTasks { get; set; }
    }
}
