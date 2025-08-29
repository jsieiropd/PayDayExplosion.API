using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayDayExplosion.Application.Dtos
{
    public class WorkdayDto
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public int ShiftId { get; set; }
        public DateTime Date { get; set; }
        public int WorkdayTypeId { get; set; }
        public int? NextWorkdayTypeId { get; set; }
        public bool AuthOvertimePreShift { get; set; }
        public bool AuthOvertimeDuringMeal { get; set; }
        public bool AuthOvertimePostShift { get; set; }
        public bool OnlyCaptureOvertime { get; set; }
        public bool StrictShiftRules { get; set; }
        public bool SubtractMealTime { get; set; }
    }
}
