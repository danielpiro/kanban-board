using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public struct Board
    {
        public readonly IReadOnlyCollection<string> ColumnsNames;
        public readonly string emailCreator;
        internal Board(IReadOnlyCollection<string> columnsNames, string emailCreator)
        {
            ColumnsNames = columnsNames;
            this.emailCreator = emailCreator;
        }
        public IReadOnlyCollection<string> GetColumnsNames()
        {
            return ColumnsNames;
        }
    }
}
