using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace IntroSE.Kanban.Backend.DataAccessLayer.Controllers
{
    internal interface IDalController
    {
        bool Update(int ID, string attributeName, int attributeValue);
        bool Update(int ColumnOrdinal, string attributeName, int attributeValue, string Email);
        bool Insert(DTOs.ColumnDTO Columns);
        bool Insert(DTOs.TaskDTO Tasks);
    }
}
