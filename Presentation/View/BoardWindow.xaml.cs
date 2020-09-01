using System.Windows;
using System.Windows.Input;

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for BoardWindow.xaml
    /// </summary>
    public partial class BoardWindow : Window
    {
        private BoardWindowView vm;
        public BoardWindow(Model.User user)
        {
            InitializeComponent();
            vm = new BoardWindowView(user);
            DataContext = vm;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            vm.Logout();
            Close();
        }

        private void AddColumn_Click(object sender, RoutedEventArgs e)
        {
            vm.AddColumn();
        }

        private void RemoveColumn_Click(object sender, RoutedEventArgs e)
        {
            vm.RemoveColumn(ColumnList.SelectedIndex);
        }

        private void MoveRight_Click(object sender, RoutedEventArgs e)
        {
            vm.Move(ColumnList.SelectedIndex, 1);
        }

        private void MoveLeft_Click(object sender, RoutedEventArgs e)
        {
            vm.Move(ColumnList.SelectedIndex, -1);
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            vm.AddTask();
        }

        private void AdvanceTask_Click(object sender, RoutedEventArgs e)
        {
            vm.AdvanceTask();
        }

        private void RenameColumn_Click(object sender, RoutedEventArgs e)
        {
            vm.ChangeName(ColumnList.SelectedIndex, ((Model.Column)ColumnList.SelectedItem));
        }

        private void SetLimit_Click(object sender, RoutedEventArgs e)
        {
            vm.SetLimit(ColumnList.SelectedIndex, ((Model.Column)ColumnList.SelectedItem));
        }
        private void TasksList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            vm.OpenTask();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            vm.ReLoad();
        }

        private void RemoveTask_Click(object sender, RoutedEventArgs e)
        {
            vm.DeleteTask();
        }

        private void SortedBy_Click(object sender, RoutedEventArgs e)
        {
            vm.ReOrganize();
        }
    }
}
