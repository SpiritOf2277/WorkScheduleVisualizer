using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WorkScheduleVisualizer.Commands;
using WorkScheduleVisualizer.Models;

namespace WorkScheduleVisualizer.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>();
        public ObservableCollection<Shift.ShiftType> ShiftTypes { get; set; } = new ObservableCollection<Shift.ShiftType>
        {
            Shift.ShiftType.Day,
            Shift.ShiftType.Evening,
            Shift.ShiftType.Night
        };

        private Employee _selectedEmployee;
        public Employee SelectedEmployee
        {
            get => _selectedEmployee;
            set { _selectedEmployee = value; OnPropertyChanged(); }
        }

        private Shift.ShiftType _selectedShiftType;
        public Shift.ShiftType SelectedShiftType
        {
            get => _selectedShiftType;
            set { _selectedShiftType = value; OnPropertyChanged(); }
        }

        private string _employeeName;
        public string EmployeeName
        {
            get => _employeeName;
            set { _employeeName = value; OnPropertyChanged(); }
        }

        private int _hours;
        public int Hours
        {
            get => _hours;
            set { _hours = value; OnPropertyChanged(); }
        }

        private DateTime _shiftDate = DateTime.Now;
        public DateTime ShiftDate
        {
            get => _shiftDate;
            set { _shiftDate = value; OnPropertyChanged(); }
        }

        public ICommand AddEmployeeCommand { get; }
        public ICommand AddShiftCommand { get; }
        public ICommand RemoveEmployeeCommand { get; }

        public MainViewModel()
        {
            AddEmployeeCommand = new RelayCommand(AddEmployee, CanAddEmployee);
            AddShiftCommand = new RelayCommand(AddShift, CanAddShift);
            RemoveEmployeeCommand = new RelayCommand(RemoveEmployee, CanRemoveEmployee);
        }

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

        private bool CanAddShift()
        {
            return SelectedEmployee != null && Hours > 0;
        }

        private void AddShift()
        {
            var shift = new Shift
            {
                Type = SelectedShiftType,
                Hours = Hours,
                Date = ShiftDate
            };

            SelectedEmployee.Shifts.Add(shift);
            OnPropertyChanged(nameof(SelectedEmployee));

            var totalHours = SelectedEmployee.Shifts.Sum(s => s.Hours);
            if (totalHours > 40) {
                // Implement overwork warning logic here
            }
        }

        private bool CanRemoveEmployee()
        {
            return SelectedEmployee != null;
        }

        private void RemoveEmployee()
        {
            Employees.Remove(SelectedEmployee);
            SelectedEmployee = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
