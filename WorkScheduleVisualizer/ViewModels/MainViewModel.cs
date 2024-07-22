using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WorkScheduleVisualizer.Commands;
using WorkScheduleVisualizer.Models;

namespace WorkScheduleVisualizer.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>();
        public ObservableCollection<Schedule> WeeklySchedule { get; set; }

        public MainViewModel()
        {
            InitializeSchedule();
            AddEmployeeCommand = new RelayCommand(AddEmployee, CanAddEmployee);
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

        private string _employeeName;
        public string EmployeeName
        {
            get => _employeeName;
            set { _employeeName = value; OnPropertyChanged(); }
        }

        public ICommand AddEmployeeCommand { get; }

        private bool CanAddEmployee()
        {
            return !string.IsNullOrWhiteSpace(EmployeeName);
        }

        private void AddEmployee()
        {
            var employee = new Employee { Name = EmployeeName };
            Employees.Add(employee);
            EmployeeName = string.Empty;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
