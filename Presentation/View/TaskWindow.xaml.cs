using Presentation.ViewModel;
using System.Windows;

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        private TaskViewModel vm;
        public TaskWindow(BackendController controller, string email, Model.Task task)
        {
            InitializeComponent();
            vm = new TaskViewModel(controller, email, task);
            DataContext = vm;
        }

        private void Title_Button_Click(object sender, RoutedEventArgs e)
        {
            vm.UpdateTitle();
        }

        private void Description_Button_Click(object sender, RoutedEventArgs e)
        {
            vm.UpdateDescription();
        }

        private void DueDate_Button_Click(object sender, RoutedEventArgs e)
        {
            vm.UpdateDueDate();
        }

        private void Assignee_Button_Click(object sender, RoutedEventArgs e)
        {
            vm.UpdateEmailAssignee();
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
