using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WorkScheduleVisualizer.Models
{
    public class Shift
    {
        public enum ShiftType { Day, Evening, Night }
        public string Name { get; set; }
        public ShiftType Type { get; set; }
        public int Hours { get; set; }
        public DateTime Date { get; set; }
    }

    public class Schedule : INotifyPropertyChanged
    {
        private Shift _morningShift;
        private Shift _eveningShift;
        private Shift _nightShift;

        public DayOfWeek Day { get; set; }

        public Shift MorningShift
        {
            get => _morningShift;
            set
            {
                _morningShift = value;
                OnPropertyChanged();
            }
        }

        public Shift EveningShift
        {
            get => _eveningShift;
            set
            {
                _eveningShift = value;
                OnPropertyChanged();
            }
        }

        public Shift NightShift
        {
            get => _nightShift;
            set
            {
                _nightShift = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}