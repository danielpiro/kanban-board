using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Tests")]


namespace IntroSE.Kanban.Backend.BusinessLayer.BoardPackage
{
    internal class Board
    {
        private DataAccessLayer.Controllers.ColumnControl ColumnCon = new DataAccessLayer.Controllers.ColumnControl();
        private DataAccessLayer.Controllers.TaskControl TaskCon = new DataAccessLayer.Controllers.TaskControl();
        private readonly string email;
        private List<string> boardEmails;
        private List<Column> columns;
        private const int MinColumn = 2;
        private readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int deletedTasks;

        public Board(string email)
        {
            this.email = email;
            var backlog = new Column(0, "backlog");
            var in_progress = new Column(1, "in progress");
            var done = new Column(2, "done");
            columns = new List<Column>();
            columns.Add(backlog);
            columns.Add(in_progress);
            columns.Add(done);
            boardEmails = new List<string>();
            boardEmails.Add(email);
            deletedTasks = 0;
        }
        public Board() { }
        public List<string> GetBoardEmail()
        {
            return boardEmails;
        }
        public void SetDeletedTasks()
        {
            deletedTasks++;
        }

        public int GetDeletedTasks()
        {
            return deletedTasks;
        }
        public Board(string email, List<Column> columns, List<string> boardEmails, int deletedTasks)
        {
            this.email = email;
            this.columns = columns;
            this.boardEmails = boardEmails;
            this.deletedTasks = deletedTasks;
        }
        public String GetEmail() { return email; }
        public List<Column> GetColumns() { return columns; }
        public Column GetColumn(int columnOrdinal) // we get the key of the column and we return the column with this key
        {
            if (columnOrdinal >= 0 && columnOrdinal < columns.Count)
            {
                return columns[columnOrdinal];
            }

            log.Debug("Tried getting an illegal column ordinal.");
            throw new Exception("Column ordinal is illegal.");

        }

        public Column GetColumn(string columnName) // we get the name of the column and we return the column with this name
        {
            foreach (var column in columns)
            {
                if (column.GetColumnName().Equals(columnName))  // we check the columnName
                {
                    return column;
                }
            }
            log.Debug("Tried getting an illegal column name.");
            throw new Exception("Column Name is illegal");
        }
        public void AddTask(int taskId, string title, string description, DateTime dueDate, string emailAssignee)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                log.Debug("Tried adding new task with an empty title");
                throw new Exception("Title can't be empty.");
            }
            if (title.Length > 50)
            {
                log.Debug("Tried adding new task with an title longer than 50 characters.");
                throw new Exception("Title can't be longer than 50 characters.");
            }
            if (description != null)
            {
                if (description.Length > 300)
                {
                    log.Debug("Tried adding new task with an description longer than 300 characters.");
                    throw new Exception("Description can't be longer than 300 characters.");
                }
            }
            if (!(DateTime.Compare(dueDate, DateTime.Now) > 0))
            {
                log.Debug("Tried adding new task with a non futuristic due date.");
                throw new Exception("Due date is required to be a futuristic date.");
            }
            GetColumn(0).AddTask(new Task(taskId, title, description, dueDate, emailAssignee));
            TaskCon.Insert(new DataAccessLayer.DTOs.TaskDTO(taskId, title, description, dueDate, GetColumn(0).GetTask(taskId).GetCreationDate(), email, 0, emailAssignee));
            log.Debug("Task " + (taskId) + " was created by user " + email + ".");
        }
        public int TotalTask()
        {
            int sum = 0;
            foreach (var col in columns)
            {
                sum = sum + col.GetTasks().Count;
            }

            return sum;
        }
        public Column AddColumn(int columnOrdinal, string Name)
        {
            if (string.IsNullOrWhiteSpace(Name) || Name.Length > 15)
            {
                log.Debug("Name out of bounds!");
                throw new Exception("Name out of bounds!");
            }
            foreach (Column col in columns)
            {
                if (col.GetColumnName().Equals(Name))
                {
                    log.Debug("This column name already exists");
                    throw new Exception("This column name already exists");
                }
            }
            if (columnOrdinal != columns.Count)
            {
                checkOrdinal(columnOrdinal);
            }

            var newCol = new Column(columnOrdinal, Name);
            columns.Insert(columnOrdinal, newCol);
            for (int i = columns.Count - 1; i > columnOrdinal; i = i - 1) // we update the columnOrdinal we moved
            {
                GetColumn(i).SetOrdinal(i);
                ColumnCon.Update(i - 1, DataAccessLayer.DTOs.ColumnDTO.ColumnOrdinalColumnOrdinal, i, email);
                foreach (Task tasks in GetColumn(i).GetTasks())
                {
                    TaskCon.Update(tasks.GetTaskID(), DataAccessLayer.DTOs.TaskDTO.TasksColumnIdColumnColumnId, i);
                }
            }
            ColumnCon.Insert(new DataAccessLayer.DTOs.ColumnDTO(newCol.GetColumnOrdinal(), newCol.GetColumnName(), newCol.GetLimit(), email));
            return newCol;
        }
        public Column MoveColumn(int columnOrdinal, int direction)
        {
            checkOrdinal(columnOrdinal);
            if (direction == 1 && columnOrdinal == columns.Count - 1) //validity check.
            {
                log.Debug("error: tried moving the last column right");
                throw new Exception("Last column can not be moved right!");
            }
            if (direction == -1 && columnOrdinal == 0) //validity check.
            {
                log.Debug("error: tried moving the first column left");
                throw new Exception("First column can not be moved Left!");
            }
            return SwapCol(columnOrdinal, columnOrdinal + direction);

        }
        private Column SwapCol(int columnOrdinal1, int columnOrdinal2) //swaps 2 columns, helper function to movecolumn.
        {
            var sCol = GetColumn(columnOrdinal1); //temp placeholder column.
            columns[columnOrdinal1] = GetColumn(columnOrdinal2); //switches places in the list.
            columns[columnOrdinal2] = sCol;
            GetColumn(columnOrdinal1).SetOrdinal(columnOrdinal2); //sets new ordinal for each column.
            GetColumn(columnOrdinal2).SetOrdinal(columnOrdinal1);
            ColumnCon.Update(columnOrdinal1, DataAccessLayer.DTOs.ColumnDTO.ColumnOrdinalColumnOrdinal, -1, email); //ordinal+email is unique so we used -1 as temp placeholder ordinal value since it's an illegal value so no column has it.
            ColumnCon.Update(columnOrdinal2, DataAccessLayer.DTOs.ColumnDTO.ColumnOrdinalColumnOrdinal, columnOrdinal1, email);
            ColumnCon.Update(-1, DataAccessLayer.DTOs.ColumnDTO.ColumnOrdinalColumnOrdinal, columnOrdinal2, email);
            foreach (Task tasks in GetColumn(columnOrdinal2).GetTasks()) //updates all tasks in database.
            {
                TaskCon.Update(tasks.GetTaskID(), DataAccessLayer.DTOs.TaskDTO.TasksColumnIdColumnColumnId, columnOrdinal2);
            }
            foreach (Task tasks in GetColumn(columnOrdinal1).GetTasks()) //updates all tasks in database.
            {
                TaskCon.Update(tasks.GetTaskID(), DataAccessLayer.DTOs.TaskDTO.TasksColumnIdColumnColumnId, columnOrdinal1);
            }
            return GetColumn(columnOrdinal2);
        }
        private void checkOrdinal(int columnOrdinal) //checks the ordinal is valid, throws exeption if not. helper function.
        {
            if ((columns.Count < columnOrdinal) || (columnOrdinal < 0))
            {
                log.Debug("error: illegal column ordinal");
                throw new Exception("The columnOrdinal out of bounds!");
            }
        }
        public void RemoveColumn(int columnOrdinal) //removes column from db.
        {
            if (columns.Count <= MinColumn)
            {
                log.Debug("error: a board cannot have less than 2 columns");
                throw new Exception("Cannot have less than 2 columns!");
            }
            checkOrdinal(columnOrdinal);
            if (columnOrdinal == 0)
            {
                if (GetColumn(1).GetLimit() < GetColumn(1).GetTasks().Count + GetColumn(0).GetTasks().Count && GetColumn(1).GetLimit() != -1)
                { //checks if the next column has enough space for all the tasks from this one
                    log.Debug("error: there is not enough free space in the next column for all the tasks from this one");
                    throw new Exception("The next column cannot hold all the tasks!");
                }
                GetColumn(1).GetTasks().AddRange(GetColumn(columnOrdinal).GetTasks());
            }
            else
            {
                if (GetColumn(columnOrdinal - 1).GetLimit() < GetColumn(columnOrdinal).GetTasks().Count + GetColumn(columnOrdinal - 1).GetTasks().Count && GetColumn(columnOrdinal - 1).GetLimit() != -1)
                { //checks if the previous column has enough space for all the tasks from this one
                    log.Debug("error: there is not enough free space in the previous column for all the tasks from this one");
                    throw new Exception("The previous column cannot hold all the tasks!");
                }
                GetColumn(columnOrdinal - 1).GetTasks().AddRange(GetColumn(columnOrdinal).GetTasks());
                foreach (var tasks in GetColumn(columnOrdinal).GetTasks()) //updates ordinal for all tasks of the column in database (irrelevant if ordinal is 0 since they will stay at 0 post-removal.
                {
                    TaskCon.Update(tasks.GetTaskID(), DataAccessLayer.DTOs.TaskDTO.TasksColumnIdColumnColumnId, columnOrdinal - 1);
                }
            }
            columns.Remove(GetColumn(columnOrdinal));
            ColumnCon.Delete(email, columnOrdinal);
            for (int i = columnOrdinal; i < columns.Count; i = i + 1) // iterates on all the columns following this one.
            {
                GetColumn(i).SetOrdinal(i);
                ColumnCon.Update(i + 1, DataAccessLayer.DTOs.ColumnDTO.ColumnOrdinalColumnOrdinal, i, email); //update ordinal.
                foreach (var tasks in GetColumn(i).GetTasks()) //updates the ordinal of all the tasks of each column in the database.
                {
                    TaskCon.Update(tasks.GetTaskID(), DataAccessLayer.DTOs.TaskDTO.TasksColumnIdColumnColumnId, i);
                }
            }
        }
        public void ChangeColumnName(int columnOrdinal, string Name)
        {
            if (GetColumn(columnOrdinal).GetColumnName().Equals(Name))
            {
                log.Debug("This column's name is already this name!");
                throw new Exception("This column's name is already this name!");
            }
            if (string.IsNullOrWhiteSpace(Name) || Name.Length > 15)
            {
                log.Debug("Name out of bounds!");
                throw new Exception("Name out of bounds!");
            }
            foreach (Column col in columns)
            {
                if (col.GetColumnName().Equals(Name))
                {
                    log.Debug("This column name already exists");
                    throw new Exception("This column name already exists");
                }
            }
            GetColumn(columnOrdinal).ChangeName(Name);
            ColumnCon.Update(columnOrdinal, DataAccessLayer.DTOs.ColumnDTO.ColumnNameColumnName, Name, email);
        }
    }
}
