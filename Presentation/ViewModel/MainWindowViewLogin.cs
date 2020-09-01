using Presentation.View;
using System;
using System.Windows;


namespace Presentation
{
    internal class MainWindowViewLogin : NotifiableObject
    {
        public BackendController Controller { get; private set; }
        public MainWindowViewLogin() //default ctor
        {
            Controller = new BackendController();
            Email = "";
            Password = "";
        }
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
        public Model.User Login()
        {
            try
            {
                var res = Controller.Login(Email, Password);
                return res;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        public void Register()
        {
            var reg = new RegisterWindow(Controller);
            reg.ShowDialog();
        }
        public void SafeLogout()
        {
            try
            {
                Controller.LogOut(Email);
                Email = "";
                Password = "";
            }

            catch (Exception) {
                Email = "";
                Password = "";
            }
        }
    }
}
