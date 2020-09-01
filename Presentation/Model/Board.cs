using System.Collections.ObjectModel;
using System.Linq;

namespace Presentation.Model
{
    public class Board : NotifiableModelObject
    {
        private Board(BackendController controller, ObservableCollection<Column> columnsNames) : base(controller) //default ctor
        {
            Columns = columnsNames;
        }

        public Board(BackendController controller, User user, string filter) : base(controller) //ctor for user input
        {
            userEmail = user.Email;
            Columns = new ObservableCollection<Column>(controller.GetAllColumnNames(user.Email).
                Select((c, i) => new Column(controller, i, controller.GetTasks(user.Email, i, filter), controller.GetColumn(user.Email, i).Name, controller.GetColumn(user.Email, i).Limit, user.Email)).ToList());

        }
        public Board(BackendController controller, string email, string filter) : base(controller) //ctor for email input
        {
            userEmail = email;
            Columns = new ObservableCollection<Column>(controller.GetAllColumnNames(email).
                Select((c, i) => new Column(controller, i, controller.GetTasks(email, i, filter), controller.GetColumn(email, i).Name, controller.GetColumn(email, i).Limit, email)).ToList());
        }
        public string userEmail { get; set; }
        public ObservableCollection<Column> _columns;
        public ObservableCollection<Column> Columns
        {
            get => _columns;
            private set
            {
                _columns = value;
                RaisePropertyChanged("Columns");
            }
        }

    }
}
