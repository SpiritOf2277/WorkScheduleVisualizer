using System.Collections.Generic;

namespace WorkScheduleVisualizer.Models
{
    public class Employee
    {
        public string Name { get; set; }
        public List<Shift> Shifts { get; set; } = new List<Shift>();
    }
}
