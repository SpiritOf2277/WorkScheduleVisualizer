using MahApps.Metro.Controls;
using System.Windows.Controls;
using WorkScheduleVisualizer.ViewModels;
using WorkScheduleVisualizer.Models;

namespace WorkScheduleVisualizer.Views
{
    /// <summary>
    /// Логика взаимодействия для EmployeeWindow.xaml
    /// </summary>
    public partial class EmployeeWindow
    {
        public EmployeeWindow(EmployeeViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
