using MahApps.Metro.Controls;
using System;
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

        private void Shift_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) {
                var draggedItem = sender as FrameworkElement;
                if (draggedItem != null && draggedItem.DataContext is Shift shift && !string.IsNullOrEmpty(shift.Name)) {
                    DragDrop.DoDragDrop(draggedItem, shift, DragDropEffects.Move);
                }
            }
        }

        private void Shift_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Employee)) || e.Data.GetDataPresent(typeof(Shift))) {
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
            var border = sender as Border;
            if (border != null) {
                border.Background = new SolidColorBrush(Colors.LightBlue);
                var schedule = border.DataContext as Schedule;

                if (schedule != null) {
                    if (e.Data.GetDataPresent(typeof(Employee))) {
                        var employee = e.Data.GetData(typeof(Employee)) as Employee;
                        if (employee != null) {
                            var date = DateTime.Now;  // Пример для установки корректной даты
                            switch (border.Name) {
                                case "MorningShift":
                                    schedule.MorningShift = new Shift { Name = employee.Name, Type = Shift.ShiftType.Day, Hours = 8, Date = date };
                                    break;
                                case "EveningShift":
                                    schedule.EveningShift = new Shift { Name = employee.Name, Type = Shift.ShiftType.Evening, Hours = 8, Date = date };
                                    break;
                                case "NightShift":
                                    schedule.NightShift = new Shift { Name = employee.Name, Type = Shift.ShiftType.Night, Hours = 8, Date = date };
                                    break;
                            }
                        }
                    } else if (e.Data.GetDataPresent(typeof(Shift))) {
                        switch (border.Name) {
                            case "MorningShift":
                                schedule.MorningShift = new Shift();
                                break;
                            case "EveningShift":
                                schedule.EveningShift = new Shift();
                                break;
                            case "NightShift":
                                schedule.NightShift = new Shift();
                                break;
                        }
                    }
                }
            }
        }

        private void EditEmployee_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem != null) {
                var contextMenu = menuItem.Parent as ContextMenu;
                if (contextMenu != null) {
                    var border = contextMenu.PlacementTarget as Border;
                    if (border != null) {
                        var employee = border.DataContext as Employee;
                        if (employee != null) {
                            // Здесь можно добавить логику для редактирования сотрудника
                            MessageBox.Show($"Edit Employee: {employee.Name}");
                        }
                    }
                }
            }
        }

        private void DeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem != null) {
                var contextMenu = menuItem.Parent as ContextMenu;
                if (contextMenu != null) {
                    var border = contextMenu.PlacementTarget as Border;
                    if (border != null) {
                        var employee = border.DataContext as Employee;
                        if (employee != null) {
                            var viewModel = DataContext as WorkScheduleVisualizer.ViewModels.MainViewModel;
                            if (viewModel != null) {
                                // Удаляем сотрудника из коллекции
                                viewModel.Employees.Remove(employee);
                                // Удаляем сотрудника из расписания
                                foreach (var schedule in viewModel.WeeklySchedule) {
                                    if (schedule.MorningShift.Name == employee.Name) {
                                        schedule.MorningShift = new Shift();
                                    }
                                    if (schedule.EveningShift.Name == employee.Name) {
                                        schedule.EveningShift = new Shift();
                                    }
                                    if (schedule.NightShift.Name == employee.Name) {
                                        schedule.NightShift = new Shift();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
