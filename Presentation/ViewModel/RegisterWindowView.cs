using Presentation.View;
using System;
using System.Windows;

namespace Presentation.ViewModel
{
    internal class RegisterWindowView : NotifiableObject
    {
        public RegisterWindowView(BackendController controller) //default ctor
        {
            Controller = controller;
            Email = "";
            Password = "";
            NickName = "";
            Host = "";
        }
        public void Register(RegisterWindow register)
        {
            try
            {
                Controller.Register(Email, Password, NickName, Host);
                MessageBox.Show("User created successfully!");
                register.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }
        public BackendController Controller { get; private set; }
        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                RaisePropertyChanged("Email");
            }
        }
        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                RaisePropertyChanged("Password");
            }
        }
        private string nickName;
        public string NickName
        {
            get { return nickName; }
            set
            {
                nickName = value;
                RaisePropertyChanged("NickName");
            }
        }
        private string host;
        public string Host
        {
            get { return host; }
            set
            {
                host = value;
                RaisePropertyChanged("Host");
            }
        }
    }
}
