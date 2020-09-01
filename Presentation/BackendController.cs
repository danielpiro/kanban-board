using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Presentation
{
    public class BackendController
    {
        public IService Service { get; private set; }
        public BackendController(IService service) //default ctor
        {
            Service = service;
        }

        public BackendController()
        {
            Service = new Service();
            Service.LoadData();
        }
        internal void LogOut(string email)
        {
            var res = Service.Logout(email);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }
        internal Model.User Login(string email, string password)
        {
            Response<IntroSE.Kanban.Backend.ServiceLayer.User> user = Service.Login(email, password);
            if (user.ErrorOccured)
            {
                throw new Exception(user.ErrorMessage);
            }
            return new Model.User(this, email, user.Value.Nickname);
        }
        internal void Register(string email, string password, string nickName, string host)
        {
            Response reg;
            if (string.IsNullOrWhiteSpace(host))
            {
                reg = Service.Register(email, password, nickName);
            }
            else
            {
                reg = Service.Register(email, password, nickName, host);
            }
            if (reg.ErrorOccured)
            {
                throw new Exception(reg.ErrorMessage);
            }
        }
        internal void Move(string email, int columnOrdinal, int direction)
        {
            Response res;
            if (direction == 1)
            {
                res = Service.MoveColumnRight(email, columnOrdinal);
            }
            else
            {
                res = Service.MoveColumnLeft(email, columnOrdinal);
            }
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }
        internal void AddColumn(string email, string colName, string colOrdinal)
        {
            int k = 0;
            if (!int.TryParse(colOrdinal, out k))
            {
                throw new Exception("Column Ordinal must be an integer.");
            }
            var res = Service.AddColumn(email, k, colName);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }
        internal void RemoveColumn(string email, int columnOrdinal)
        {
            var res = Service.RemoveColumn(email, columnOrdinal);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }
        internal void SetLimit(string email, int columnOrdinal, string limit)
        {
            int k = 0;
            if (!int.TryParse(limit, out k))
            {
                throw new Exception("Limit must be an integer.");
            }
            var res = Service.LimitColumnTasks(email, columnOrdinal, k);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }
        internal void AddTask(string email, string title, string description, DateTime dueDate)
        {
            var res = Service.AddTask(email, title, description, dueDate);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }
        internal void ChangeColumnName(string email, int columnOrdinal, string newName)
        {
            if (!(columnOrdinal >= 0 && columnOrdinal < Service.GetBoard(email).Value.ColumnsNames.Count))
            {
                throw new Exception("Please select one of the columns in the list before clicking this botton.");
            }
            else
            {
                var res = Service.ChangeColumnName(email, columnOrdinal, newName);
                if (res.ErrorOccured)
                {
                    throw new Exception(res.ErrorMessage);
                }
            }

        }
        internal void DeleteTask(Model.Task deltask)
        {
            var res = Service.DeleteTask(deltask.userEmail, deltask.column, deltask.TaskId);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }
        internal void AssignTask(Model.Task task1)
        {
            var res = Service.AssignTask(task1.userEmail, task1.column, task1.TaskId, task1.EmailAssignee);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }
        public void AdvanceTask(Model.Task deltask)
        {
            var res = Service.AdvanceTask(deltask.userEmail, deltask.column, deltask.TaskId);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }
        public void UpdateTitle(Model.Task task)
        {
            var res = Service.UpdateTaskTitle(task.userEmail, task.column, task.TaskId, task.Title);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }
        public void UpdateDescription(Model.Task task)
        {
            var res = Service.UpdateTaskDescription(task.userEmail, task.column, task.TaskId, task.Description);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }

        }
        internal void UpdateDueDate(Model.Task task)
        {
            var res = Service.UpdateTaskDueDate(task.userEmail, task.column, task.TaskId, task.DueDate);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }
        internal Task GetTask(string email, int column, int index)
        {
            var res = Service.GetColumn(email, column);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
            return res.Value.Tasks.ElementAt(index);
        }
        internal List<string> GetAllColumnNames(string email)
        {
            var col = Service.GetBoard(email).Value.ColumnsNames;
            return new List<string>(col);
        }
        internal List<Task> GetTasks(string email, int column, string filter) 
        {
            var tasks = Service.GetColumn(email, column).Value.Tasks;
            if (string.IsNullOrWhiteSpace(filter))
            {
                return new List<Task>(tasks);
            }
            else
            //if the filter is not empty , the return list<tasks> will inculde only tasks that contain the filter
            {
                var tasks1 = new List<Task>();
                foreach (var task in tasks)
                {
                    if (task.Description.Contains(filter) || task.Title.Contains(filter))
                    {
                        tasks1.Add(task);
                    }
                }
                return tasks1;
            }
        }
        internal Column GetColumn(string email, int columnOrdinal)
        {
            return Service.GetColumn(email, columnOrdinal).Value;
        }
        internal bool IsHost(string email)
        {
            return email.Equals(Service.GetBoard(email).Value.emailCreator);
        }

    }
}
