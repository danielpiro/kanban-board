using Presentation.ViewModel;
using System.Windows;

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for AddColumnWindow.xaml
    /// </summary>
    public partial class AddColumnWindow : Window
    {
        private SetBoardWindowView vm;
        public AddColumnWindow(BackendController controller, string email)
        {
            InitializeComponent();
            vm = new SetBoardWindowView(controller, email);
            DataContext = vm;
        }

        private void AddColumn_Click(object sender, RoutedEventArgs e)
        {
            vm.AddColumn(this);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
