using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayDayExplosion.Application.Dtos.Panama
{
    public class ExplosionTestPADto
    {
        public decimal HourlyRate { get; set; }
        public string CountryCode { get; set; }
        public string PayTypeCode { get; set; }
        public string EmployeeTypeCode { get; set; }
        public bool IsSuntracsRuleContract { get; set; }
        public DayOfWeek FirstDayOfTheWeek { get; set; }
        public decimal OvertimeHoursPreviouslyWorkedInCurrentWeek { get; set; }

        public ShiftDto Shift { get; set; }
        public WorkdayDto Workday { get; set; }
        public List<PayCategoryDto> PayCategories { get; set; }
        public List<WorkdayTypeDto> WorkdayTypes { get; set; }
        public List<WorkshiftTypeDto> WorkshiftTypes { get; set; }
        public List<SpanTypeDto> SpanTypes { get; set; }
        public List<SpanDetailTypeDto> SpanDetailTypes { get; set; }
        public List<SubspanDetailTypeDto> SubspanDetailTypes { get; set; }



        public List<SpanPADto> Spans { get; set; }
    }
}
