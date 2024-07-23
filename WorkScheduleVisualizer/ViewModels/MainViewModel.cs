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
            set
            {
                _selectedEmployee = value;
                OnPropertyChanged();
                ((RelayCommand)EditEmployeeCommand).RaiseCanExecuteChanged();
                ((RelayCommand)DeleteEmployeeCommand).RaiseCanExecuteChanged();
            }
        }

        public ICommand AddEmployeeCommand { get; }
        public ICommand EditEmployeeCommand { get; }
        public ICommand DeleteEmployeeCommand { get; }
        public ICommand DropCommand { get; }

        public MainViewModel()
        {
            InitializeSchedule();
            AddEmployeeCommand = new RelayCommand(OpenAddEmployeeWindow);
            EditEmployeeCommand = new RelayCommand(OpenEditEmployeeWindow);
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
                Console.WriteLine($"Employee added: {args.EmployeeName}");
            };

            var employeeWindow = new EmployeeWindow(employeeViewModel);
            employeeWindow.ShowDialog();
        }

        private void OpenEditEmployeeWindow()
        {
            if (SelectedEmployee == null) return;

            var employeeViewModel = new EmployeeViewModel
            {
                EmployeeName = SelectedEmployee.Name,
                IsEditing = true
            };

            employeeViewModel.EmployeeUpdated += (sender, args) =>
            {
                SelectedEmployee.Name = args.EmployeeName;
                Console.WriteLine($"Employee updated: {args.EmployeeName}");
            };

            var employeeWindow = new EmployeeWindow(employeeViewModel);
            employeeWindow.ShowDialog();
        }

        private bool CanEditOrDeleteEmployee()
        {
            var canExecute = SelectedEmployee != null;
            Console.WriteLine($"CanEditOrDeleteEmployee: {canExecute}");
            return canExecute;
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
                Console.WriteLine($"Employee deleted: {SelectedEmployee.Name}");
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
                    var date = DateTime.Now;
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
                    Console.WriteLine($"Employee {employee.Name} dropped to {shiftType}");
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
                    Console.WriteLine($"Shift cleared for {shiftType}");
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
