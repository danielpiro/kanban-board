using System;
using System.Data.SQLite;
using System.IO;

namespace IntroSE.Kanban.Backend.DataAccessLayer.Controllers
{
    internal abstract class DalController
    {
        protected readonly string _connectionString;
        protected readonly string _tableName;
        protected readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public DalController(string tableName)
        {
            var path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "database.db"));
            _connectionString = $"Data Source={path}; Version=3;";
            _tableName = tableName;
            Create(path);
        }
        public void Create(string path)
        {
            if (!File.Exists(path)) //if the file does not exist, it will create a new db file with all the tables.
            {
                SQLiteConnection.CreateFile("database.db");

                using (var connection = new SQLiteConnection(_connectionString))
                {
                    var BoardEmails = "CREATE TABLE \"BoardEmails\"(\"Email\" TEXT NOT NULL UNIQUE,\"Host\"  TEXT NOT NULL,PRIMARY KEY(\"Email\"))";
                    var UserTable = "CREATE TABLE \"Users\"( \"Email\" TEXT NOT NULL UNIQUE, \"Nickname\"  TEXT NOT NULL, \"Password\"  TEXT NOT NULL,\"EmailHost\"  TEXT NOT NULL,PRIMARY KEY (\"Email\"))";
                    var BoardTable = "CREATE TABLE \"Boards\" (\"Email\" TEXT NOT NULL UNIQUE,\"DeletedTasks\" INTEGER NOT NULL, PRIMARY KEY(\"Email\"))";
                    var ColumnTable = "CREATE TABLE \"Columns\" (\"ColumnOrdinal\" INTEGER NOT NULL,\"ColumnName\"    TEXT NOT NULL,\"ColumnLimit\" INTEGER NOT NULL,\"Email\" TEXT NOT NULL,PRIMARY KEY(\"ColumnOrdinal\", \"Email\"))";
                    var TaskTable = "CREATE TABLE \"Tasks\"(\"ID\"    INTEGER NOT NULL UNIQUE, \"Title\" TEXT NOT NULL, \"Description\"   TEXT,\"DueDate\"   TEXT NOT NULL, \"CreationDate\"  TEXT NOT NULL, \"Email\" TEXT NOT NULL, \"ColumnOrdinal\" INTEGER NOT NULL,\"EmailAssignee\" TEXT NOT NULL, PRIMARY KEY(\"ID\"))";
                    var command = new SQLiteCommand(null, connection);

                    try //Creates all tables.
                    {
                        connection.Open();
                        command.CommandText = UserTable;
                        command.ExecuteNonQuery();
                        command.CommandText = BoardTable;
                        command.ExecuteNonQuery();
                        command.CommandText = ColumnTable;
                        command.ExecuteNonQuery();
                        command.CommandText = TaskTable;
                        command.ExecuteNonQuery();
                        command.CommandText = BoardEmails;
                        command.ExecuteNonQuery();

                    }
                    catch (Exception ex)
                    {
                        log.Debug(ex.Message);
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        command.Dispose();
                        connection.Close();
                    }
                }

            }
        }
        public bool Update(int ID, string attributeName, string attributeValue) //updates a task with a specific ID (attribute is string).
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"UPDATE {_tableName} SET [{attributeName}]=@{attributeName} WHERE ID=@ID"
                };
                try
                {
                    connection.Open();
                    command.Parameters.Add(new SQLiteParameter(@"ID", ID));
                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    command.Prepare();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    log.Debug("an error occured while updating this task.");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }
        public bool Update(int ID, string attributeName, int attributeValue) //updates a task with a specific ID (attribute is int).
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"UPDATE {_tableName} SET [{attributeName}]=@{attributeName} WHERE ID=@ID"
                };
                try
                {
                    connection.Open();
                    command.Parameters.Add(new SQLiteParameter(@"ID", ID));
                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    command.Prepare();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    log.Debug("an error occured while updating this task.");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();

                }

            }
            return res > 0;
        }
        public bool Update(int id, string attributeName, DateTime attributeValue) //updates a task with a specific ID (attribute is DateTime).
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"UPDATE {_tableName} SET [{attributeName}]=@{attributeName} WHERE id={id}"
                };
                try
                {

                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    log.Debug("an error occured while updating this task.");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }
        public bool DeleteTable() //Deletes all tasks from the database.
        {
            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"DELETE FROM {_tableName} "
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    log.Debug("an error occured while deleting all tasks.");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }
        public bool Update(string Email, string attributeName, int attributeValue) //updates a task with a specific ID (attribute is int).
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"UPDATE {_tableName} SET [{attributeName}]=@{attributeName} WHERE Email=@Email"
                };
                try
                {
                    connection.Open();
                    command.Parameters.Add(new SQLiteParameter(@"Email", Email));
                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    command.Prepare();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    log.Debug("an error occured while updating this task.");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();

                }

            }
            return res > 0;
        }
        public bool Update(int ColumnOrdinal, string attributeName, int attributeValue, string Email) //updates column with specific ordinal and name (attribute is int).
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"UPDATE {_tableName} SET [{attributeName}]=@attributeName WHERE [{DTOs.ColumnDTO.ColumnEmailColumnEmail}]=@Email AND [{DTOs.ColumnDTO.ColumnOrdinalColumnOrdinal}]=@ColumnOrdinal"
                };
                try
                {
                    connection.Open();
                    command.Parameters.Add(new SQLiteParameter(@"attributeName", attributeValue));
                    command.Parameters.Add(new SQLiteParameter(@"ColumnOrdinal", ColumnOrdinal));
                    command.Parameters.Add(new SQLiteParameter(@"Email", Email));
                    command.Prepare();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    log.Debug("an error occured while updating this column.");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }
    }
}
