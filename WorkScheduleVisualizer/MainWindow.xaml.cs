using ControlzEx.Theming;
using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WorkScheduleVisualizer.Models;

namespace WorkScheduleVisualizer
{
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            ThemeManager.Current.ThemeSyncMode = ThemeSyncMode.SyncWithAppMode;
            ThemeManager.Current.SyncTheme();
        }

        private void Employee_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) {
                var draggedItem = sender as FrameworkElement;
                if (draggedItem != null) {
                    DragDrop.DoDragDrop(draggedItem, draggedItem.DataContext, DragDropEffects.Move);
                }
            }
        }

        private void Shift_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Employee))) {
                var border = sender as Border;
                if (border != null) {
                    border.Background = new SolidColorBrush(Colors.LightCoral);
                }
            }
        }

        private void Shift_DragLeave(object sender, DragEventArgs e)
        {
            var border = sender as Border;
            if (border != null) {
                border.Background = new SolidColorBrush(Colors.LightBlue);
            }
        }

        private void Shift_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Employee))) {
                var employee = e.Data.GetData(typeof(Employee)) as Employee;
                var border = sender as Border;
                if (employee != null && border != null) {
                    border.Background = new SolidColorBrush(Colors.LightBlue);
                }
            }
        }
    }
}
