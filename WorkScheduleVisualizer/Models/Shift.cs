using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkScheduleVisualizer.Models
{
    public class Shift
    {
        public enum ShiftType { Day, Evening, Night }
        public ShiftType Type { get; set; }
        public int Hours { get; set; }
        public DateTime Date { get; set; }
    }
}
