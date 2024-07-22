using ControlzEx.Theming;
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
                            var date = DateTime.Now;
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
    }
}
