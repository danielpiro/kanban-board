using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public struct Column
    {
        public readonly IReadOnlyCollection<Task> Tasks;
        public readonly string Name;
        public readonly int Limit;
        internal Column(IReadOnlyCollection<Task> tasks, string name, int limit)
        {
            Tasks = tasks;
            Name = name;
            Limit = limit;
        }
        public int GetLimit() { return Limit; }
    }
}
