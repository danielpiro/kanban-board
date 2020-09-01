using Presentation.View;
using System;
using System.Windows;

namespace Presentation.ViewModel
{
    internal class SetBoardWindowView : NotifiableObject //helper viewModel for board update windows
    {
        public BackendController Controller { get; private set; }
        public SetBoardWindowView(BackendController controller, string email) //default ctor for AddTaskWindow and AddColumnWindow
        {
            Controller = controller;
            Email = email;
            Name = "";
            ColumnOrdinal = "";
            Title = "";
            Description = "";
            dueDate = DateTime.Now;
            Limit = "";
        }
        public SetBoardWindowView(BackendController controller, string email, int columnOrdinal, string name) //default ctor for ReNameWindow
        {
            Controller = controller;
            Ordinal = columnOrdinal;
            ColumnOrdinal = "";
            Email = email;
            Name = name;
            Title = "";
            Description = "";
            dueDate = DateTime.Now;
            Limit = "";
        }
        public SetBoardWindowView(BackendController controller, string email, string limit, int columnOrdinal) //default ctor for SetLimitWindow
        {
            Controller = controller;
            Ordinal = columnOrdinal;
            ColumnOrdinal = "";
            Email = email;
            Name = "";
            Title = "";
            Description = "";
            dueDate = DateTime.Now;
            Limit = limit;
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                RaisePropertyChanged("Name");
            }
        }
        private string columnOrdinal;
        public string ColumnOrdinal
        {
            get { return columnOrdinal; }
            set
            {
                columnOrdinal = value;
                RaisePropertyChanged("ColumnOrdinal");
            }
        }
        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                RaisePropertyChanged("Title");
            }
        }
        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                RaisePropertyChanged("Description");
            }
        }
        private DateTime dueDate;
        public DateTime DueDate
        {
            get { return dueDate; }
            set
            {
                dueDate = value;
                RaisePropertyChanged("DueDate");
            }
        }
        private string limit;
        public string Limit
        {
            get { return limit; }
            set
            {
                limit = value;
                RaisePropertyChanged("Limit");
            }
        }
        private int ordinal;
        public int Ordinal
        {
            get { return ordinal; }
            set
            {
                ordinal = value;
                RaisePropertyChanged("Ordinal");
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
        public void AddColumn(AddColumnWindow newCol) //action for AddColumnWindow
        {
            try
            {
                Controller.AddColumn(Email, Name, ColumnOrdinal);
                MessageBox.Show("Column added successfully!");
                newCol.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void AddTask(AddTaskWindow newTask) //action for AddTaskWindow
        {
            try
            {
                Controller.AddTask(Email, Title, Description, DueDate);
                MessageBox.Show("Task added successfully!");
                newTask.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void ChangeName(ReNameWindow rename) //action for ReNameWindow
        {
            try
            {
                Controller.ChangeColumnName(Email, Ordinal, Name);
                MessageBox.Show("Column name changed to " + Name);
                rename.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void setLimit(SetLimitWindow setLimit) //action for SetLimitWindow
        {
            try
            {
                Controller.SetLimit(Email, Ordinal, Limit);
                if (Limit.Equals("-1"))
                {
                    MessageBox.Show("The limit of this column was disabled");
                }
                else
                {
                    MessageBox.Show("The limit of this column was set to " + Limit);
                }

                setLimit.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
