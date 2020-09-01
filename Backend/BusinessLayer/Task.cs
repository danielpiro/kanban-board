using System;

namespace IntroSE.Kanban.Backend.BusinessLayer.BoardPackage
{
    internal class Task
    {
        private readonly int taskId;
        private string title;
        private string description;
        private readonly DateTime creationDate;
        private DateTime dueDate;
        private string emailAssignee;
        public Task() { }
        public Task(int taskId, string title, string description, DateTime dueDate, string emailAssignee) //default ctor
        {
            this.taskId = taskId;
            this.title = title;
            this.description = description;
            creationDate = DateTime.Now;
            this.dueDate = dueDate;
            this.emailAssignee = emailAssignee;
        }
        public Task(int taskId, string title, string description, DateTime dueDate, DateTime creationDate, string emailAssignee) //ctor for added creation date constrain
        {
            this.taskId = taskId;
            this.title = title;
            this.description = description;
            this.creationDate = creationDate;
            this.dueDate = dueDate;
            this.emailAssignee = emailAssignee;
        }
        //getters and setters.
        public string GetEmailAssignee() { return emailAssignee; }
        public void EditTaskTitle(string title) { this.title = title; }
        public void EditTaskDescription(string description) { this.description = description; }
        public void EditTaskDueDate(DateTime dueDate) { this.dueDate = dueDate; }
        public int GetTaskID() { return taskId; }
        public DateTime GetCreationDate() { return creationDate; }
        public string GetTitle() { return title; }
        public string GetDescription() { return description; }
        public DateTime GetDueDate() { return dueDate; }
        public void SetEmailAssignee(string emailAssignee) { this.emailAssignee = emailAssignee; }



    }

}
