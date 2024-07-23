using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using WorkScheduleVisualizer.Commands;

namespace WorkScheduleVisualizer.ViewModels
{
    public class EmployeeViewModel : INotifyPropertyChanged
    {
        private string _employeeName;
        public string EmployeeName
        {
            get => _employeeName;
            set { _employeeName = value; OnPropertyChanged(); }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public event EventHandler<EmployeeEventArgs> EmployeeAdded;

        public EmployeeViewModel()
        {
            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Cancel);
        }

        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(EmployeeName);
        }

        private void Save()
        {
            EmployeeAdded?.Invoke(this, new EmployeeEventArgs { EmployeeName = EmployeeName });
            CloseWindow();
        }

        private void Cancel()
        {
            CloseWindow();
        }

        private void CloseWindow()
        {
            Application.Current.Windows[Application.Current.Windows.Count - 1].Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class EmployeeEventArgs : EventArgs
    {
        public string EmployeeName { get; set; }
    }
}
