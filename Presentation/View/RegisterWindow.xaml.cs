using Presentation.ViewModel;
using System.Windows;

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private RegisterWindowView vm;
        public RegisterWindow(BackendController controller)
        {
            InitializeComponent();
            vm = new RegisterWindowView(controller);
            DataContext = vm;
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            vm.Register(this);
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
