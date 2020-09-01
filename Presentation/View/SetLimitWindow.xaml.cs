using Presentation.ViewModel;
using System.Windows;

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for SetLimitWindow.xaml
    /// </summary>
    public partial class SetLimitWindow : Window
    {
        private SetBoardWindowView vm;
        public SetLimitWindow(BackendController controller, string email, int ordinal, string limit)
        {
            InitializeComponent();
            vm = new SetBoardWindowView(controller, email, limit, ordinal);
            DataContext = vm;
        }

        private void SetLimit_Click(object sender, RoutedEventArgs e)
        {
            vm.setLimit(this);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
