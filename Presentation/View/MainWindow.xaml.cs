using System.Windows;


namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewLogin vm;
        public MainWindow()
        {
            InitializeComponent();
            vm = new MainWindowViewLogin();
            DataContext = vm;
        }



        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Model.User u = vm.Login();
            if (u != null)
            {
                var boardView = new BoardWindow(u);
                boardView.ShowDialog();
                vm.SafeLogout();
            }

        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            vm.Register();
        }
    }
}
