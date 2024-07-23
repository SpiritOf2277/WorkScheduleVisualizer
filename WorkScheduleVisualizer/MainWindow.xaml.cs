using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WorkScheduleVisualizer.Models;
using WorkScheduleVisualizer.ViewModels;

namespace WorkScheduleVisualizer
{
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Employee_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            var border = sender as Border;
            if (border != null) {
                var employee = border.DataContext as Employee;
                var viewModel = DataContext as WorkScheduleVisualizer.ViewModels.MainViewModel;
                if (employee != null && viewModel != null) {
                    viewModel.SelectedEmployee = employee;
                }
            }
        }
    }
}
