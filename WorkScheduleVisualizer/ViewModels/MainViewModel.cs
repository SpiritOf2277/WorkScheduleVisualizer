using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using WorkScheduleVisualizer.Commands;
using WorkScheduleVisualizer.Models;
using WorkScheduleVisualizer.Views;

namespace WorkScheduleVisualizer.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>();
        public ObservableCollection<Schedule> WeeklySchedule { get; set; }

        private Employee _selectedEmployee;
        public Employee SelectedEmployee
        {
            get => _selectedEmployee;
            set { _selectedEmployee = value; OnPropertyChanged(); }
        }

        public ICommand AddEmployeeCommand { get; }
        public ICommand EditEmployeeCommand { get; }
        public ICommand DeleteEmployeeCommand { get; }
        public ICommand DropCommand { get; }

        public MainViewModel()
        {
            InitializeSchedule();
            AddEmployeeCommand = new RelayCommand(OpenAddEmployeeWindow);
            EditEmployeeCommand = new RelayCommand(EditEmployee);
            DeleteEmployeeCommand = new RelayCommand(DeleteEmployee);
            DropCommand = new RelayCommand<object>(ExecuteDrop);
        }

        private void InitializeSchedule()
        {
            WeeklySchedule = new ObservableCollection<Schedule>
            {
                new Schedule { Day = DayOfWeek.Monday },
                new Schedule { Day = DayOfWeek.Tuesday },
                new Schedule { Day = DayOfWeek.Wednesday },
                new Schedule { Day = DayOfWeek.Thursday },
                new Schedule { Day = DayOfWeek.Friday },
                new Schedule { Day = DayOfWeek.Saturday },
                new Schedule { Day = DayOfWeek.Sunday },
            };
        }

        private void OpenAddEmployeeWindow()
        {
            var employeeViewModel = new EmployeeViewModel();
            employeeViewModel.EmployeeAdded += (sender, args) =>
            {
                var employee = new Employee { Name = args.EmployeeName };
                Employees.Add(employee);
            };

            var employeeWindow = new EmployeeWindow(employeeViewModel);
            employeeWindow.ShowDialog();
        }

        private void EditEmployee()
        {
            // Логика для редактирования сотрудника
        }

        private void DeleteEmployee()
        {
            if (SelectedEmployee != null) {
                Employees.Remove(SelectedEmployee);
                foreach (var schedule in WeeklySchedule) {
                    if (schedule.MorningShift.Name == SelectedEmployee.Name) {
                        schedule.MorningShift = new Shift();
                    }
                    if (schedule.EveningShift.Name == SelectedEmployee.Name) {
                        schedule.EveningShift = new Shift();
                    }
                    if (schedule.NightShift.Name == SelectedEmployee.Name) {
                        schedule.NightShift = new Shift();
                    }
                }
                SelectedEmployee = null;
            }
        }

        private void ExecuteDrop(object parameter)
        {
            var data = parameter as Tuple<Schedule, string, object>;
            if (data != null) {
                var schedule = data.Item1;
                var shiftType = data.Item2;
                var droppedData = data.Item3;

                if (droppedData is Employee employee) {
                    var date = DateTime.Now;  // Пример для установки корректной даты
                    switch (shiftType) {
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
                } else if (droppedData is Shift shift) {
                    switch (shiftType) {
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
