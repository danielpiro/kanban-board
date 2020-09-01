using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace IntroSE.Kanban.Backend.DataAccessLayer.Controllers
{
    internal class ColumnControl:DalController
    {
      
        public ColumnControl():base("Columns")
        {
           
        }

        public bool Update(int ColumnOrdinal, string attributeName, string attributeValue, string Email) //updates column with specific ordinal and name (attribute is string).
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

       

        public List<DTOs.ColumnDTO> SelectColumn(string Email) //Returns all columns with a specific email.
        {
            var columnsList = new List<DTOs.ColumnDTO>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand(connection);
                command.CommandText = $"SELECT * FROM {_tableName} WHERE [{DTOs.ColumnDTO.ColumnEmailColumnEmail}]=@Email";
                command.Parameters.Add(new SQLiteParameter(@"Email", Email));
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        columnsList.Add(new DTOs.ColumnDTO(dataReader.GetInt32(0), dataReader.GetString(1), dataReader.GetInt32(2), Email));
                    }
                }
                catch (Exception)
                {
                    log.Debug("an error occured while getting all columns from this board.");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return columnsList;
        }
       

        public bool Delete(string email, int columnOrdinal) //Deletes a specific column (keys are email and ordinal).
        {
            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"DELETE FROM {_tableName} WHERE [{DTOs.ColumnDTO.ColumnEmailColumnEmail}]=@Email AND [{DTOs.ColumnDTO.ColumnOrdinalColumnOrdinal}]=@ColumnOrdinal"
                };
                var emailParam = new SQLiteParameter(@"Email", email);
                var colOrdinalParam = new SQLiteParameter(@"ColumnOrdinal", columnOrdinal);
                command.Parameters.Add(emailParam);
                command.Parameters.Add(colOrdinalParam);
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    log.Debug("an error occured while deleting this column.");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }
        public bool Insert(DTOs.ColumnDTO Columns) //Creates a new column.
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(connection);
                int res = -1;
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {_tableName}  ({DTOs.ColumnDTO.ColumnOrdinalColumnOrdinal} ,{DTOs.ColumnDTO.ColumnNameColumnName},{DTOs.ColumnDTO.ColumnLimitColumnLimit},{DTOs.ColumnDTO.ColumnEmailColumnEmail}) " +
                        $"VALUES (@columnOridnalVal,@columnNameVal,@limitVal,@email);";

                    var idParam = new SQLiteParameter(@"columnOridnalVal", Columns.ColumnOrdinal);
                    var NameParam = new SQLiteParameter(@"columnNameVal", Columns.ColumnName);
                    var limitParam = new SQLiteParameter(@"limitVal", Columns.Limit);
                    var emailParam = new SQLiteParameter(@"email", Columns.Email);


                    command.Parameters.Add(idParam);
                    command.Parameters.Add(NameParam);
                    command.Parameters.Add(limitParam);
                    command.Parameters.Add(emailParam);

                    command.Prepare();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    log.Debug("an error occured while inserting a new column");
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
