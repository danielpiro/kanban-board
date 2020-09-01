using System;
using System.Collections.Generic;
using System.Data.SQLite;


namespace IntroSE.Kanban.Backend.DataAccessLayer.Controllers
{
    internal class TaskControl : DalController
    {

        public TaskControl() : base("Tasks")
        {
        }



        public List<DTOs.TaskDTO> SelectTasks(string email, int ColumnOridnal) //returns all tasks from the same column (same ordinal and email).
        {
            var taskList = new List<DTOs.TaskDTO>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand(connection);
                command.CommandText = $"SELECT * FROM {_tableName} WHERE [{DTOs.TaskDTO.TasksEmailColumnEmail}]=@Email AND [{DTOs.TaskDTO.TasksColumnIdColumnColumnId}]=@ColumnOridnal";
                command.Parameters.Add(new SQLiteParameter(@"Email", email));
                command.Parameters.Add(new SQLiteParameter(@"ColumnOridnal", ColumnOridnal));
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        taskList.Add(new DTOs.TaskDTO(dataReader.GetInt32(0), dataReader.GetString(1), dataReader.IsDBNull(2) ? null : dataReader.GetString(2), dataReader.GetDateTime(3), dataReader.GetDateTime(4), email, ColumnOridnal, dataReader.GetString(7)));
                    }
                }
                catch (Exception)
                {
                    log.Debug("an error occured while getting all tasks from this board's column");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return taskList;
        }

        public bool Delete(int taskId) //Deletes a specific task.
        {
            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"DELETE FROM {_tableName} WHERE [{DTOs.TaskDTO.TasksIdColumnId}]=@taskId"
                };
                command.Parameters.Add(new SQLiteParameter(@"taskId", taskId));
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    log.Debug("an error occured while deleting this task");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }

        public bool Insert(DTOs.TaskDTO Tasks) //Creates a new task in the database.
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand(connection);
                int res = -1;
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {_tableName}  ({DTOs.TaskDTO.TasksIdColumnId} ,{DTOs.TaskDTO.TasksTitleColumnTitle},{DTOs.TaskDTO.TasksDescriptionColumnDescription},{DTOs.TaskDTO.TasksDueDateColumnDueDate},{DTOs.TaskDTO.TasksCreationDateColumnCreationDate},{DTOs.TaskDTO.TasksEmailColumnEmail},{DTOs.TaskDTO.TasksColumnIdColumnColumnId},{DTOs.TaskDTO.TasksEmailAssigneeColumn}) " +
                        $"VALUES (@idVal,@titleVal,@descriptionVal,@dueDateTimeVal,@creationTimeVal,@emailVal,@columnOridnalVal,@emailAssigneeVal);";

                    var idParam = new SQLiteParameter(@"idVal", Tasks.TaskId);
                    var titleParam = new SQLiteParameter(@"titleVal", Tasks.Title);
                    var descriptionParam = new SQLiteParameter(@"descriptionVal", Tasks.Description);
                    var dueDateParam = new SQLiteParameter(@"dueDateTimeVal", Tasks.DueDate);
                    var creationTimeParam = new SQLiteParameter(@"creationTimeVal", Tasks.CreationTime);
                    var emailParam = new SQLiteParameter(@"emailVal", Tasks.Email);
                    var columnOridnalParam = new SQLiteParameter(@"columnOridnalVal", Tasks.ColumnOridnal);
                    var emailAssigneeParam = new SQLiteParameter(@"emailAssigneeVal", Tasks.EmailAssignee);

                    command.Parameters.Add(idParam);
                    command.Parameters.Add(titleParam);
                    command.Parameters.Add(descriptionParam);
                    command.Parameters.Add(dueDateParam);
                    command.Parameters.Add(creationTimeParam);
                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(columnOridnalParam);
                    command.Parameters.Add(emailAssigneeParam);
                    command.Prepare();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    log.Debug("an error occured while creating this task.");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();

                }
                return res > 0;

            }
        }
    }
}
