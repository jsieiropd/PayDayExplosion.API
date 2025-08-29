using System.ComponentModel.DataAnnotations;
using PayDayExplosion.Domain.Data.Common;

namespace PayDayExplosion.Domain.Data.Entities
{
    public class Country : Base
    {
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "Código")]
        public string Code { get; set; }


        //public ICollection<SpanType> SpanTypes { get; set; } = new List<SpanType>();
        //public ICollection<WorkshiftType> WorkshiftTypes { get; set; } = new List<WorkshiftType>();

        //public ICollection<SpanDetailType> SpanDetailTypes { get; set; } = new List<SpanDetailType>();


    }
}
