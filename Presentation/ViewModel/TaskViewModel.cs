using IntroSE.Kanban.Backend.ServiceLayer;
using Presentation.Model;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Presentation.ViewModel
{
    internal class TaskViewModel : NotifiableObject
    {
        public BackendController Controller { get; private set; }
        public TaskViewModel(BackendController controller, string email, Model.Task task)
        {
            Controller = controller;
            Task = task;
            TaskName = "Task " + Task.TaskId;
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
        private string taskName;
        public string TaskName
        {
            get { return taskName; }
            set
            {
                taskName = value;
                RaisePropertyChanged("TaskName");
            }
        }

        public void UpdateTitle()
        {
            try
            {
                Controller.UpdateTitle(Task);
                MessageBox.Show("Title updated successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void UpdateDescription()
        {
            try
            {
                Controller.UpdateDescription(Task);
                MessageBox.Show("Description updated successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void UpdateDueDate()
        {
            try
            {
                Controller.UpdateDueDate(Task);
                MessageBox.Show("Due date updated successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void UpdateEmailAssignee()
        {
            try
            {
                Controller.AssignTask(task);
                MessageBox.Show("Task Assigned to " + Task.EmailAssignee);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
