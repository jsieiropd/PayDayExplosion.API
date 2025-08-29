

namespace PayDayExplosion.Application.Dtos
{
    public class SubspanDto
    {
        public int SpanId { get; set; }
        public string DisplayText { get; set; } = string.Empty;
        public DateTime? SubTimeIn { get; set; }
        public DateTime? SubTimeOut { get; set; }
        public int PayCategoryId { get; set; }
        public int WorkshiftTypeId { get; set; }
        public int SpanTypeId { get; set; }
        public int SpanDetailTypeId { get; set; }
        public decimal Factor1 { get; set; } = 1.000000m;
        public decimal Factor2 { get; set; } = 1.000000m;
        public decimal Factor3 { get; set; } = 1.000000m;
        public decimal Factor4 { get; set; } = 1.000000m;
        public decimal Substract { get; set; } = 0.00m;
        public decimal Factor { get; set; } = 1.000000m;
        public decimal Amount { get; set; } = 0.00m;
        public decimal ClockedHours { get; set; }
        public decimal MealHours { get; set; }
        public bool IsInSchedule { get; set; }
        public string CountryCode { get; set; } = string.Empty;

    }
}
