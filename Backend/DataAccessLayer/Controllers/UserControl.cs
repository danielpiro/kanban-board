using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace IntroSE.Kanban.Backend.DataAccessLayer.Controllers
{
    internal class UserControl : DalController
    {

        public UserControl() : base("Users")
        {

        }


        public bool Update(string Email, string attributeName, string attributeValue) //updates user with specific email (attribute is string).
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"UPDATE {_tableName} SET [{attributeName}]=@{attributeName} WHERE Email=@Email "
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
                    log.Debug("an error occured while updating this user.");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }


        public List<DTOs.UserDTO> Select() //Returns all users.
        {
            var userList = new List<DTOs.UserDTO>();
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
                        userList.Add(new DTOs.UserDTO(dataReader.GetString(0), dataReader.GetString(1), dataReader.GetString(2), dataReader.GetString(3)));
                    }

                }
                catch (Exception)
                {
                    log.Debug("an error occured while getting all user.");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }


            }
            return userList;
        }

        public bool Delete(DTOs.UserDTO DTOObj) //Deletes a specific user (based on email)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"DELETE FROM {_tableName} WHERE [{DTOs.UserDTO.UsersEmailColumn}]=@emailVal"
                };
                command.Parameters.Add(new SQLiteParameter(@"emailVal", DTOObj.Email));
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    log.Debug("an error occured while updating this user.");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }
        public bool Insert(DTOs.UserDTO User) //creates a new User in the database.
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand(connection);
                int res = -1;
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {_tableName}  ({DTOs.UserDTO.UsersEmailColumn} ,{DTOs.UserDTO.UsersNicknameColumn},{DTOs.UserDTO.UsersPasswordColumn},{DTOs.UserDTO.UsersHostColumn}) " +
                        $"VALUES (@emailVal,@nickNameVal,@passwordVal,@emailHostVal);";

                    command.Parameters.Add(new SQLiteParameter(@"emailVal", User.Email));
                    command.Parameters.Add(new SQLiteParameter(@"nickNameVal", User.Nickname));
                    command.Parameters.Add(new SQLiteParameter(@"passwordVal", User.Password));
                    command.Parameters.Add(new SQLiteParameter(@"emailHostVal", User.EmailHost));
                    command.Prepare();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    log.Debug("an error occured while creating this user.");
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
