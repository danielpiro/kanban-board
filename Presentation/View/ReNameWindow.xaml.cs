using Presentation.ViewModel;
using System.Windows;

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for ReNameWindow.xaml
    /// </summary>
    public partial class ReNameWindow : Window
    {
        private SetBoardWindowView vm;
        public ReNameWindow(BackendController controller, string email, int ordinal, string name)
        {
            InitializeComponent();
            vm = new SetBoardWindowView(controller, email, ordinal, name);
            DataContext = vm;
        }

        private void ChangeName_Click(object sender, RoutedEventArgs e)
        {
            vm.ChangeName(this);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
