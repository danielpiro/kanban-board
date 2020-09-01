using Presentation.ViewModel;
using System.Windows;

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for AddTaskWindow.xaml
    /// </summary>
    public partial class AddTaskWindow : Window
    {
        private SetBoardWindowView vm;
        public AddTaskWindow(BackendController controller, string email)
        {
            InitializeComponent();
            vm = new SetBoardWindowView(controller, email);
            DataContext = vm;
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            vm.AddTask(this);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
