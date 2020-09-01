using System;
using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    /// <summary>
    /// The service for using the Kanban board.
    /// It allows executing all of the required behaviors by the Kanban board.
    /// You are not allowed (and can't due to the interfance) to change the signatures
    /// Do not add public methods\members! Your client expects to use specifically these functions.
    /// You may add private, non static fields (if needed).
    /// You are expected to implement all of the methods.
    /// Good luck.
    /// </summary>
    public class Service : IService
    {
        private BusinessLayer.UserPackage.UserController UserController;
        private BusinessLayer.BoardPackage.BoardController BoardController;
        private readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Simple public constructor.
        /// </summary>
        public Service()
        {
            UserController = new BusinessLayer.UserPackage.UserController();
            BoardController = new BusinessLayer.BoardPackage.BoardController();

        }

        /// <summary>        
        /// Loads the data. Intended be invoked only when the program starts
        /// </summary>
        /// <returns>A response object. The response should contain a error message in case of an error.</returns>
        public Response LoadData()
        {
            try
            {
                UserController.LoadData();
                BoardController.LoadData();
                log.Debug("Data loaded successfully");
                return new Response();
            }
            catch (Exception ex)
            {
                return new Response<Object>(ex.Message);
            }
        }


        ///<summary>Remove all persistent data.</summary>
        public Response DeleteData()
        {
            try
            {
                UserController.Delete();
                BoardController.Delete();
                log.Debug("All persisted data deleted");
                return new Response();
            }
            catch (Exception ex)
            {
                return new Response<Object>(ex.Message);
            }
        }


        /// <summary>
        /// Registers a new user and creates a new board for him.
        /// </summary>
        /// <param name="email">The email address of the user to register</param>
        /// <param name="password">The password of the user to register</param>
        /// <param name="nickname">The nickname of the user to register</param>
        /// <returns>A response object. The response should contain a error message in case of an error<returns>
        public Response Register(string email, string password, string nickname)
        {
            try
            {
                UserController.PasswordVerify(password);
                UserController.Register(email, password, nickname, email);
                BoardController.Register(email);
                return new Response();
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
                return new Response<Object>(ex.Message);
            }
        }


        /// <summary>
        /// Registers a new user and joins the user to an existing board.
        /// </summary>
        /// <param name="email">The email address of the user to register</param>
        /// <param name="password">The password of the user to register</param>
        /// <param name="nickname">The nickname of the user to register</param>
        /// <param name="emailHost">The email address of the host user which owns the board</param>
        /// <returns>A response object. The response should contain a error message in case of an error<returns>
        public Response Register(string email, string password, string nickname, string emailHost)
        {
            try
            {
                if (email.Equals(emailHost))
                    throw new Exception("The host email cannot be identical to your email");
                UserController.PasswordVerify(password);
                UserController.Register(email, password, nickname, emailHost);
                BoardController.AddToBoard(email, emailHost);
                return new Response();
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
                return new Response<Object>(ex.Message);
            }
        }



        /// <summary>
        /// Assigns a task to a user
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>        
        /// <param name="emailAssignee">Email of the user to assign to task to</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response AssignTask(string email, int columnOrdinal, int taskId, string emailAssignee)
        {
            if (UserController.IsLogged(email))
            {
                try
                {
                    BoardController.AssignTask(email, columnOrdinal, taskId, emailAssignee, UserController.GetUser(email).GetEmailHost());
                    return new Response();
                }
                catch (Exception ex)
                {
                    return new Response<Object>(ex.Message);
                }
            }
            else
            {
                log.Debug("This user is not logged in");
                return new Response<Object>("This user is not logged in");
            }
        }

        /// <summary>
        /// Delete a task
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>        		
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response DeleteTask(string email, int columnOrdinal, int taskId)
        {
            if (UserController.IsLogged(email))
            {
                try
                {
                    BoardController.DeleteTask(email, columnOrdinal, taskId, UserController.GetUser(email).GetEmailHost());
                    return new Response();
                }
                catch (Exception ex)
                {
                    return new Response<Object>(ex.Message);
                }
            }
            else
            {
                log.Debug("This user is not logged in");
                return new Response<Board>("This user is not logged in");
            }
        }



        /// <summary>
        /// Log in an existing user
        /// </summary>
        /// <param name="email">The email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns>A response object with a value set to the user, instead the response should contain a error message in case of an error</returns>
        public Response<User> Login(string email, string password)
        {
            try
            {
                UserController.Login(email, password);
                var user = new User(email, UserController.GetUser(email).GetNickname());
                return new Response<User>(user);
            }
            catch (Exception ex)
            {
                return new Response<User>(ex.Message);
            }
        }

        /// <summary>        
        /// Log out an logged in user. 
        /// </summary>
        /// <param name="email">The email of the user to log out</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response Logout(string email)
        {
            try
            {
                UserController.Logout(email);
                return new Response();
            }
            catch (Exception ex)
            {
                return new Response<Object>(ex.Message);
            }
        }

        /// <summary>
        /// Returns the board of a user. The user must be logged in
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <returns>A response object with a value set to the board, instead the response should contain a error message in case of an error</returns>
        public Response<Board> GetBoard(string email)
        {
            if (UserController.IsLogged(email))
            {
                try
                {
                    var ColumnsNames = new List<string>();
                    foreach (var column in BoardController.GetBoard(UserController.GetUser(email).GetEmailHost()).GetColumns())
                    {
                        ColumnsNames.Add(column.GetColumnName());
                    }
                    var board = new Board((IReadOnlyCollection<string>)ColumnsNames, UserController.GetUser(email).GetEmailHost());
                    return new Response<Board>(board);
                }
                catch (Exception ex)
                {
                    return new Response<Board>(ex.Message);
                }
            }
            else
            {
                log.Debug("This user is not logged in");
                return new Response<Board>("This user is not logged in");
            }
        }

        /// <summary>
        /// Limit the number of tasks in a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response LimitColumnTasks(string email, int columnOrdinal, int limit)
        {
            if (UserController.IsLogged(email))
            {
                try
                {
                    BoardController.LimitColumnTasks(email, columnOrdinal, limit);
                    return new Response();
                }
                catch (Exception ex)
                {
                    return new Response<Object>(ex.Message);
                }
            }
            else
            {
                log.Debug("This user is not logged in");
                return new Response<Object>("This user is not logged in");
            }
        }

        /// <summary>
        /// Change the name of a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="newName">The new name.</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response ChangeColumnName(string email, int columnOrdinal, string newName)
        {
            if (UserController.IsLogged(email))
            {
                try
                {
                    BoardController.ChangeColumnName(email, columnOrdinal, newName);
                    return new Response();
                }
                catch (Exception ex)
                {
                    return new Response<Object>(ex.Message);
                }
            }
            else
            {
                log.Debug("This user is not logged in");
                return new Response<Object>("This user is not logged in");
            }
        }


        /// <summary>
        /// Add a new task.
        /// </summary>
        /// <param name="email">Email of the user. The user must be logged in.</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        /// <returns>A response object with a value set to the Task, instead the response should contain a error message in case of an error</returns>
        public Response<Task> AddTask(string email, string title, string description, DateTime dueDate)
        {
            if (UserController.IsLogged(email))
            {
                try
                {
                    BoardController.AddTask(email, title, description, dueDate, UserController.GetUser(email).GetEmailHost());
                    return new Response<Task>(new Task(BoardController.GetTotalTasks() - 1, DateTime.Now, dueDate, title, description, email));
                }
                catch (Exception ex)
                {
                    return new Response<Task>(ex.Message);
                }
            }
            else
            {
                log.Debug("This user is not logged in");
                return new Response<Task>("This user is not logged in");
            }
        }

        /// <summary>
        /// Update the due date of a task
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="dueDate">The new due date of the column</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response UpdateTaskDueDate(string email, int columnOrdinal, int taskId, DateTime dueDate)
        {
            if (UserController.IsLogged(email))
            {
                try
                {
                    BoardController.UpdateTaskDueDate(email, UserController.GetUser(email).GetEmailHost(), columnOrdinal, taskId, dueDate);
                    return new Response();
                }
                catch (Exception ex)
                {
                    return new Response<Object>(ex.Message);
                }
            }
            else
            {
                log.Debug("This user is not logged in");
                return new Response<Object>("This user is not logged in");
            }
        }

        /// <summary>
        /// Update task title
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="title">New title for the task</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response UpdateTaskTitle(string email, int columnOrdinal, int taskId, string title)
        {
            if (UserController.IsLogged(email))
            {
                try
                {
                    BoardController.UpdateTaskTitle(email, UserController.GetUser(email).GetEmailHost(), columnOrdinal, taskId, title);
                    return new Response();
                }
                catch (Exception ex)
                {
                    return new Response<Object>(ex.Message);
                }
            }
            else
            {
                log.Debug("This user is not logged in");
                return new Response<Object>("This user is not logged in");
            }
        }

        /// <summary>
        /// Update the description of a task
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="description">New description for the task</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response UpdateTaskDescription(string email, int columnOrdinal, int taskId, string description)
        {
            if (UserController.IsLogged(email))
            {
                try
                {
                    BoardController.UpdateTaskDescription(email, UserController.GetUser(email).GetEmailHost(), columnOrdinal, taskId, description);
                    return new Response();
                }
                catch (Exception ex)
                {
                    return new Response<Object>(ex.Message);
                }
            }
            else
            {
                log.Debug("This user is not logged in");
                return new Response<Object>("This user is not logged in");
            }
        }

        /// <summary>
        /// Advance a task to the next column
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response AdvanceTask(string email, int columnOrdinal, int taskId)
        {
            if (UserController.IsLogged(email))
            {
                try
                {
                    BoardController.AdvanceTask(email, UserController.GetUser(email).GetEmailHost(), columnOrdinal, taskId);
                    return new Response();
                }
                catch (Exception ex)
                {
                    return new Response<Object>(ex.Message);
                }
            }
            else
            {
                log.Debug("This user is not logged in");
                return new Response<Object>("This user is not logged in");
            }
        }


        /// <summary>
        /// Returns a column given it's name
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnName">Column name</param>
        /// <returns>A response object with a value set to the Column, The response should contain a error message in case of an error</returns>
        public Response<Column> GetColumn(string email, string columnName)
        {
            if (UserController.IsLogged(email))
            {
                try
                {
                    var Tasks = new List<Task>();
                    foreach (var task in BoardController.GetColumn(UserController.GetUser(email).GetEmailHost(), columnName).GetTasks())
                    {
                        Tasks.Add(new Task(task.GetTaskID(), task.GetCreationDate(), task.GetDueDate(), task.GetTitle(), task.GetDescription(), task.GetEmailAssignee()));
                    }
                    var column = new Column((IReadOnlyCollection<Task>)Tasks, columnName, BoardController.GetColumn(UserController.GetUser(email).GetEmailHost(), columnName).GetLimit());
                    return new Response<Column>(column);
                }
                catch (Exception ex)
                {
                    return new Response<Column>(ex.Message);
                }
            }
            else
            {
                log.Debug("This user is not logged in");
                return new Response<Column>("This user is not logged in");
            }
        }

        /// <summary>
        /// Returns a column given it's identifier.
        /// The first column is identified by 0, the ID increases by 1 for each column
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Column ID</param>
        /// <returns>A response object with a value set to the Column, The response should contain a error message in case of an error</returns>

        public Response<Column> GetColumn(string email, int columnOrdinal)
        {
            if (UserController.IsLogged(email))
            {
                try
                {
                    var Tasks = new List<Task>();
                    foreach (var task in BoardController.GetColumn(UserController.GetUser(email).GetEmailHost(), columnOrdinal).GetTasks())
                    {
                        Tasks.Add(new Task(task.GetTaskID(), task.GetCreationDate(), task.GetDueDate(), task.GetTitle(), task.GetDescription(), task.GetEmailAssignee()));
                    }
                    var column = new Column((IReadOnlyCollection<Task>)Tasks, BoardController.GetColumn(UserController.GetUser(email).GetEmailHost(), columnOrdinal).GetColumnName(), BoardController.GetColumn(UserController.GetUser(email).GetEmailHost(), columnOrdinal).GetLimit());
                    return new Response<Column>(column);
                }
                catch (Exception ex)
                {
                    return new Response<Column>(ex.Message);
                }
            }
            else
            {
                log.Debug("This user is not logged in");
                return new Response<Column>("This user is not logged in");
            }
        }

        /// <summary>
        /// Removes a column given it's identifier.
        /// The first column is identified by 0, the ID increases by 1 for each column
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Column ID</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response RemoveColumn(string email, int columnOrdinal)
        {
            if (UserController.IsLogged(email))
            {
                try
                {
                    BoardController.RemoveColumn(email, columnOrdinal);
                    log.Info("user " + email + " deleted column number " + columnOrdinal + " from his board.");
                    return new Response();
                }
                catch (Exception ex)
                {
                    return new Response<Object>(ex.Message);
                }
            }
            else
            {
                log.Debug("This user is not logged in");
                return new Response<Object>("This user is not logged in");
            }
        }

        /// <summary>
        /// Adds a new column, given it's name and a location to place it.
        /// The first column is identified by 0, the ID increases by 1 for each column        
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Location to place to column</param>
        /// <param name="Name">new Column name</param>
        /// <returns>A response object with a value set to the new Column, the response should contain a error message in case of an error</returns>
        public Response<Column> AddColumn(string email, int columnOrdinal, string Name)
        {
            if (UserController.IsLogged(email))
            {
                try
                {
                    BoardController.AddColumn(email, columnOrdinal, Name);
                    var Tasks = new List<Task>();
                    var column = new Column((IReadOnlyCollection<Task>)Tasks, BoardController.GetColumn(email, columnOrdinal).GetColumnName(), BoardController.GetColumn(email, columnOrdinal).GetLimit());
                    log.Debug("user " + email + " added " + Name + " column to his board as column number " + columnOrdinal);
                    return new Response<Column>(column);
                }
                catch (Exception ex)
                {
                    return new Response<Column>(ex.Message);
                }
            }
            else
            {
                log.Debug("This user is not logged in");
                return new Response<Column>("This user is not logged in");
            }

        }

        /// <summary>
        /// Moves a column to the right, swapping it with the column wich is currently located there.
        /// The first column is identified by 0, the ID increases by 1 for each column        
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Current location of the column</param>
        /// <returns>A response object with a value set to the moved Column, the response should contain a error message in case of an error</returns>
        public Response<Column> MoveColumnRight(string email, int columnOrdinal)
        {
            if (UserController.IsLogged(email))
            {
                try
                {
                    BoardController.MoveColumn(email, columnOrdinal, 1);
                    var Tasks = new List<Task>();
                    foreach (var task in BoardController.GetColumn(email, columnOrdinal + 1).GetTasks())
                    {
                        Tasks.Add(new Task(task.GetTaskID(), task.GetCreationDate(), task.GetDueDate(), task.GetTitle(), task.GetDescription(), task.GetEmailAssignee()));
                    }
                    var column = new Column((IReadOnlyCollection<Task>)Tasks, BoardController.GetColumn(email, columnOrdinal + 1).GetColumnName(), BoardController.GetColumn(email, columnOrdinal + 1).GetLimit());
                    log.Debug("user " + email + " moved his " + column.Name + " column right.");
                    return new Response<Column>(column);
                }
                catch (Exception ex)
                {
                    return new Response<Column>(ex.Message);
                }
            }
            else
            {
                log.Debug("This user is not logged in");
                return new Response<Column>("This user is not logged in");
            }

        }

        /// <summary>
        /// Moves a column to the left, swapping it with the column wich is currently located there.
        /// The first column is identified by 0, the ID increases by 1 for each column.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Current location of the column</param>
        /// <returns>A response object with a value set to the moved Column, the response should contain a error message in case of an error</returns>
        public Response<Column> MoveColumnLeft(string email, int columnOrdinal)
        {
            if (UserController.IsLogged(email))
            {
                try
                {
                    BoardController.MoveColumn(email, columnOrdinal, -1);
                    var Tasks = new List<Task>();
                    foreach (var task in BoardController.GetColumn(email, columnOrdinal - 1).GetTasks())
                    {
                        Tasks.Add(new Task(task.GetTaskID(), task.GetCreationDate(), task.GetDueDate(), task.GetTitle(), task.GetDescription(), task.GetEmailAssignee()));
                    }
                    var column = new Column((IReadOnlyCollection<Task>)Tasks, BoardController.GetColumn(email, columnOrdinal - 1).GetColumnName(), BoardController.GetColumn(email, columnOrdinal - 1).GetLimit());
                    log.Debug("user " + email + " moved his " + column.Name + " column left.");
                    return new Response<Column>(column);
                }
                catch (Exception ex)
                {
                    return new Response<Column>(ex.Message);
                }
            }
            else
            {
                log.Debug("This user is not logged in");
                return new Response<Column>("This user is not logged in");
            }

        }

    }
}
