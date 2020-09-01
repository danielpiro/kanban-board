using System;

namespace IntroSE.Kanban.Backend.BusinessLayer.UserPackage
{
    internal class User
    {
        private DataAccessLayer.Controllers.UserControl newUser = new DataAccessLayer.Controllers.UserControl();
        private readonly string email;
        private readonly string nickname;
        private readonly string password;
        private bool LoggedIn;
        private readonly string host;
        private readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public User() { }
        public User(string email, string password, string nickname, string host) //default ctor
        {
            this.email = email;
            this.password = password;
            this.nickname = nickname;
            LoggedIn = false;
            this.host = host;
        }
        public string GetEmailHost() { return host; }
        public string GetEmail() { return email; }
        public string GetNickname() { return nickname; }
        public string GetPassword() { return password; }
        public bool GetLoggedIn() { return LoggedIn; }

        public void Login(string password) //tries logging in the user.
        {
            if (this.password.Equals(password))  //verify if the password matches the user's.
            {
                LoggedIn = true;
                log.Debug("User " + email + " has logged in.");
            }
            else
            {
                log.Debug("User " + email + " tried logging in with an incorrect password.");
                throw new Exception("email and password does not match.");
            }
        }
        public void Logout() //updated the user status
        {
            if (GetLoggedIn()) //checks if the user is logged in, else throws an exception.
            {
                LoggedIn = false;
                log.Debug("User " + email + " has logged out.");
            }
            else
            {
                log.Debug("Error: User " + email + " tried logging out while being logged out.");
                throw new Exception("This user is not logged in");
            }
        }




    }
}