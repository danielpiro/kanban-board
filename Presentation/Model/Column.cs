using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Presentation.Model
{
    public class Column : NotifiableModelObject
    {


        public readonly string UserEmail;
        public Column(BackendController controller, int ordinal, List<IntroSE.Kanban.Backend.ServiceLayer.Task> tasks, string name, int limit, string userEmail) : base(controller)
        {
            Tasks = new ObservableCollection<Task>(tasks.
            Select((c, i) => new Task(controller, ordinal, userEmail, tasks[i])).ToList());
            UserEmail = userEmail;
            Name = name;
            Limit = limit;
            ColumnOrdinal = ordinal;
        }
        private ObservableCollection<Task> _tasks;
        public ObservableCollection<Task> Tasks
        {
            get => _tasks;
            set
            {
                _tasks = value;
                RaisePropertyChanged("Tasks");
            }
        }
        private Model.Task task;
        public Model.Task Task
        {
            get { return task; }
            set
            {
                task = value;
                RaisePropertyChanged("Task");
            }
        }
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }
        private int _limit;
        public int Limit
        {
            get => _limit;
            set
            {
                _limit = value;
                RaisePropertyChanged("Limit");
            }
        }
        private int _columnOrdinal;
        public int ColumnOrdinal
        {
            get => _columnOrdinal;
            set
            {
                _columnOrdinal = value;
                RaisePropertyChanged("ColumnOrdinal");
            }
        }
    }
}
