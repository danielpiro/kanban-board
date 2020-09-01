using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace IntroSE.Kanban.Backend.DataAccessLayer.Controllers
{
    internal class BoardControl : DalController
    {
        public BoardControl() : base("Boards") //default ctor
        {

        }


        public List<DTOs.BoardDTO> Select() //Returns all boards.
        {
            var boardsList = new List<DTOs.BoardDTO>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand(connection);
                command.CommandText = $"SELECT * FROM {_tableName}";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        boardsList.Add(new DTOs.BoardDTO(dataReader.GetString(0), dataReader.GetInt32(1)));
                    }

                }
                catch (Exception)
                {
                    log.Debug("an error occured while getting all boards");
                }
                finally
                {

                    command.Dispose();
                    connection.Close();
                }


            }
            return boardsList;
        }

        public bool Delete(DTOs.BoardDTO DTOObj) //Deletes a specific board.
        {
            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"DELETE FROM {_tableName} WHERE [{DTOs.BoardDTO.BoardEmailColumnEmail}]=@Email"
                };
                var emailParam = new SQLiteParameter(@"Email", DTOObj.Email);
                command.Parameters.Add(emailParam);
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    log.Debug("an error occured while deleting this board");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }

        public bool Insert(DTOs.BoardDTO Board) //creates a new board in the database.
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand(connection);
                int res = -1;
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {_tableName}  ({DTOs.BoardDTO.BoardEmailColumnEmail},{DTOs.BoardDTO.BoardDeletedTaskColumn})  " +
                        $"VALUES (@EmailVal,@DeletedTaskVal);";

                    var emailParam = new SQLiteParameter(@"EmailVal", Board.Email);
                    var deletedTasksParam = new SQLiteParameter(@"DeletedTaskVal", Board.DeletedTasks);

                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(deletedTasksParam);

                    command.Prepare();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    log.Debug("an error occured while inserting a new board");
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
