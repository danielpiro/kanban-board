using System;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public struct Task
    {
        public readonly int Id;
        public readonly DateTime CreationTime;
        public readonly DateTime DueDate;
        public readonly string Title;
        public readonly string Description;
        public readonly string emailAssignee;
        internal Task(int id, DateTime creationTime, DateTime dueDate, string title, string description, string emailAssignee)
        {
            Id = id;
            CreationTime = creationTime;
            DueDate = dueDate;
            Title = title;
            Description = description;
            this.emailAssignee = emailAssignee;
        }
        public int GetId() { return Id; }
    }
}
