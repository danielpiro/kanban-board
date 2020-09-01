using IntroSE.Kanban.Backend.ServiceLayer;
using Presentation.View;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Presentation
{
    internal class BoardWindowView : NotifiableObject
    {
        public BackendController Controller { get; private set; }
        public BoardWindowView(Model.User user) //default ctor
        {
            Email = user.Email;
            Filter = "";
            board = new Model.Board(user.Controller, user, Filter);
            Controller = user.Controller;
            Sorted = "Due Date";
            Welcome = "Hello " + user.NickName;
            IsHost = Controller.IsHost(Email);
        }

        private string filter;
        public string Filter //binding with filter textbox
        {
            get { return filter; }
            set
            {
                filter = value;
                RaisePropertyChanged("Filter");
            }
        }
        public string welcome;
        public string Welcome
        {
            get { return welcome; }
            set
            {
                welcome = value;
                RaisePropertyChanged("Welcome");
            }
        }
        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                RaisePropertyChanged("Email");
            }
        }
        private Model.Task task; 
        public Model.Task Task //binding with selected task
        {
            get { return task; }
            set
            {
                task = value;
                RaisePropertyChanged("Task");
            }
        }
        private string sorted;
        public string Sorted //changes between creation date and duedate
        {
            get { return sorted; }
            set
            {
                sorted = value;
                RaisePropertyChanged("Sorted");
            }
        }
        private bool isHost;
        public bool IsHost //changes between creation date and duedate
        {
            get { return isHost; }
            set
            {
                isHost = value;
                RaisePropertyChanged("IsHost");
            }
        }


        private Model.Board board;
        public Model.Board Board 
        {
            get { return board; }
            set
            {
                board = value;
                RaisePropertyChanged("Board");
            }
        }
        public void ReOrganize() //sort the task by either duedate or creation date
        {
            if (Sorted.Equals("Due Date"))
            {
                for (int i = 0; i < Board.Columns.Count; i++)
                {
                    var tasks = Board.Columns.ElementAt(i).Tasks.ToList();
                    tasks.Sort((x, y) => DateTime.Compare(x.DueDate, y.DueDate));
                    Board.Columns.ElementAt(i).Tasks = new ObservableCollection<Model.Task>(tasks.
                   Select((c, j) => tasks[j]).ToList());
                }
                Sorted = "Creation Date";
            }
            else
            {
                for (int i = 0; i < Board.Columns.Count; i++)
                {
                    var tasks = Board.Columns.ElementAt(i).Tasks.ToList();
                    tasks.Sort((x, y) => DateTime.Compare(x.CreationDate, y.CreationDate));
                    Board.Columns.ElementAt(i).Tasks = new ObservableCollection<Model.Task>(tasks.
                   Select((c, j) => tasks[j]).ToList());
                }
                Sorted = "Due Date";
            }
        }

        public void Logout()
        {
            Controller.LogOut(Email);
        }
        public void AddColumn()
        {
            if (IsHost)
            {
                var newCol = new AddColumnWindow(Controller, Email);
                newCol.ShowDialog();
                ReLoad();
            }
            else
            {
                MessageBox.Show("You are not the host of this board!");
            }
        }
        public void ReLoad() //update the display board after changes
        {
            Board = new Model.Board(Controller, Email, Filter);
            if (Sorted.Equals("Creation Date"))
            {
                for (int i = 0; i < Board.Columns.Count; i++)
                {
                    var tasks = Board.Columns.ElementAt(i).Tasks.ToList();
                    tasks.Sort((x, y) => DateTime.Compare(x.DueDate, y.DueDate));
                    Board.Columns.ElementAt(i).Tasks = new ObservableCollection<Model.Task>(tasks.
                   Select((c, j) => tasks[j]).ToList());
                }
            }
        }

        public void RemoveColumn(int ordinal)
        {
            try
            {
                Controller.RemoveColumn(Email, ordinal);
                MessageBox.Show("Column removed successfully!");
                ReLoad();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public void Move(int columnOrdinal, int direction)
        {
            try
            {
                Controller.Move(Email, columnOrdinal, direction);
                if (direction == 1)
                {
                    MessageBox.Show("Column moved right successfully!");
                }
                else
                {
                    MessageBox.Show("Column moved left successfully!");
                }

                ReLoad();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public void AddTask()
        {
            var newTask = new AddTaskWindow(Controller, Email);
            newTask.ShowDialog();
            ReLoad();
        }
        public void AdvanceTask()
        {
            try
            {
                if (Task == null)
                {
                    throw new Exception("No task selected!");
                }

                Controller.AdvanceTask(Task);
                MessageBox.Show("Task Advanced successfully!");
                ReLoad();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public void ChangeName(int ordinal, Model.Column column)
        {
            if (IsHost)
            {
                if (column != null)
                {
                    var col = new ReNameWindow(Controller, Email, ordinal, column.Name);
                    col.ShowDialog();
                    ReLoad();
                }
                else
                {
                    MessageBox.Show("Please select column!");
                }
            }
            else
            {
                MessageBox.Show("You are not the host of this board!");
            }
        }
        public void SetLimit(int ordinal, Model.Column column)
        {
            if (IsHost)
            {
                if (column != null)
                {
                    var col = new SetLimitWindow(Controller, Email, ordinal, column.Limit.ToString());
                    col.ShowDialog();
                    ReLoad();
                }
                else
                {
                    MessageBox.Show("Please select column!");
                }
            }
            else
            {
                MessageBox.Show("You are not the host of this board!");
            }
        }
        public void OpenTask() //open task window for selected task
        {
            if (Task != null)
            {
                var taskwindow = new TaskWindow(Controller, Email, Task);
                taskwindow.ShowDialog();
                ReLoad();
            }
        }
        public void DeleteTask()
        {
            try
            {
                if (Task == null)
                {
                    throw new Exception("No task selected!");
                }

                Controller.DeleteTask(Task);
                MessageBox.Show("Task removed successfully!");
                ReLoad();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

    }

}
