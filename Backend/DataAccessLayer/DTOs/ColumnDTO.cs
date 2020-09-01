namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    internal class ColumnDTO
    {
        public const string ColumnOrdinalColumnOrdinal = "ColumnOrdinal";
        public const string ColumnNameColumnName = "ColumnName";
        public const string ColumnLimitColumnLimit = "ColumnLimit";
        public const string ColumnEmailColumnEmail = "Email";
        private readonly Controllers.ColumnControl _controller;

        public ColumnDTO(int columnOrdinal, string columnName, int limit, string email)
        {
            ColumnOrdinal = columnOrdinal;
            ColumnName = columnName;
            Limit = limit;
            Email = email;
            _controller = new Controllers.ColumnControl();
        }

        public int ColumnOrdinal { get; set; }
        public string ColumnName { get; set; }
        public int Limit { get; set; }
        public string Email { get; set; }
    }
}
