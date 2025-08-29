using PayDayExplosion.Domain.Data.Common;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PayDayExplosion.Domain.Data.Entities
{
    public class WorkdayType : Base
    {
        [Display(Name = "Código")]
        public string Code { get; set; }

        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "Factor")]
        [Column(TypeName = "decimal(18, 6)")]
        public decimal Factor { get; set; }

        [Display(Name = "País")]
        public string CountryCode { get; set; }

    }
}
