using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace IntroSE.Kanban.Backend.BusinessLayer.UserPackage
{
    internal class UserController
    {
        private DataAccessLayer.Controllers.UserControl newUser = new DataAccessLayer.Controllers.UserControl();
        private bool HasLogged;
        private Dictionary<string, User> Users;
        private readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const int MaxPasswordLength = 25;
        private const int MinPasswordLength = 5;
        public bool GetHasLogged() { return HasLogged; }
        public UserController()
        {
            HasLogged = false;
            Users = new Dictionary<string, User>();

        }
        public Dictionary<string, User> GetUsers()
        {
            return Users;
        }
        public void LoadData() //Loads all the data while starting the program.
        {
            var user = new DataAccessLayer.Controllers.UserControl();
            var dalUser = user.Select(); //Gets all users from the database.
            foreach (var dal in dalUser)
            {
                Users.Add(dal.Email, new User(dal.Email, dal.Password, dal.Nickname, dal.EmailHost));    //Adds all users to the users dictionary.
            }
        }
        public User GetUser(string email) //Gets a specific user from the dictionary.
        {
            if (Users.ContainsKey(email))
            {
                return Users[email];
            }
            else
            {
                log.Debug("Tried getting unregistered user " + email);
                throw new Exception("This email is not registered.");

            }
        }

        public void Register(string email, string password, string nickname, string hostEmail) //Adds a new user to the dictionary and inserts it to the database.
        {
            RegisterHelper(email, hostEmail);
            if (!string.IsNullOrWhiteSpace(nickname))
            {
                if (EmailVerify(email))
                {
                    Users.Add(email, new User(email, password, nickname, hostEmail));
                    newUser.Insert(new DataAccessLayer.DTOs.UserDTO(email, nickname, password, hostEmail)); //creates the new user in the database.
                    log.Debug("User " + email + " was created.");
                }
                else
                {
                    log.Debug("Invalid email");
                    throw new Exception("Invalid Email");
                }
            }
            else
            {
                log.Debug("Error: nickName cant be empty");
                throw new Exception("nickName cant be empty");
            }

        }
        private void RegisterHelper (string email,string hostEmail)
        {
            if (Users.ContainsKey(email)) //Checks if this email is unused by another user.
            {
                log.Debug("Tried registering with an existing email.");
                throw new Exception("email already in use.");
            }
            if (!email.Equals(hostEmail))
            {
                if (!Users.ContainsKey(hostEmail))
                {
                    log.Debug("The host email not registerd in the system");
                    throw new Exception("The host email is not registerd in the system");
                }
                if (!GetUser(hostEmail).GetEmailHost().Equals(hostEmail))
                {
                    log.Debug("The host email is not host");
                    throw new Exception("The host email is not host");
                }
            }
        }
        public bool IsLogged(string email) //Checks if a specific user is logged in.
        {
            return GetUser(email).GetLoggedIn();
        }
        public void Login(string email, string password) //Tries logging in a user.
        {
            if (!IsLogged(email)) //checks if this user is already logged in.
            {
                if (HasLogged == false) //User can only log in if everybody else is logged out.
                {
                    GetUser(email).Login(password); //Throws exception if password doesn't match.
                    HasLogged = true;
                }
                else
                {
                    log.Debug("Error: User " + email + " tried logging in while another user was already logged in.");
                    throw new Exception("Someone is already logged in.");
                }
            }
            else
            {
                log.Debug("User " + email + " already is logged in");
                throw new Exception("user is already logged in");
            }

        }
        public void Logout(string email) //Logs a user out.
        {
            GetUser(email).Logout();
            HasLogged = false;
        }
        public bool EmailVerify(string email) //Makes sure that the input email is valid.
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    var domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        public void PasswordVerify(string password) //akes sure the input password is valid.
        {
            if (string.IsNullOrWhiteSpace(password)) //checks if the password is null.
            {
                log.Debug("empty password");
                throw new Exception("Password should not be empty");
            }


            if (!password.Any(char.IsLower)) //checks if it contains a lowercase letter.
            {
                log.Debug("Register password without lower case letter");
                throw new Exception("Password should contain at least one lower case letter.");
            }
            else
            {
                if (!password.Any(char.IsUpper)) //checks if it contains an uppercase letter.
                {
                    log.Debug("Register password without upper case letter");
                    throw new Exception("Password should contain at least one upper case letter.");
                }
                else
                {
                    if (password.Length < MinPasswordLength || password.Length > MaxPasswordLength) //checks if it fits the required length.
                    {
                        log.Debug("Register password out of bounds.");
                        throw new Exception("Password should not be lesser than 4 or greater than 20 characters.");
                    }
                    else
                    {
                        if (!password.Any(char.IsDigit)) //checks contains it has a number.
                        {
                            log.Debug("Register password without a number");
                            throw new Exception("Password should contain at least one numeric value.");
                        }
                    }
                }
            }
        }
        public void Delete() //deletes all the users from the database and dictionary, sets haslogged to false.
        {
            newUser.DeleteTable();
            Users.Clear();
            HasLogged = false;
        }
    }
}