using PayDayExplosion.Domain.Data.Common;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PayDayExplosion.Domain.Data.Entities
{
    public class SpanType : Base
    {
        [Display(Name = "Código")]
        public string Code { get; set; }

        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "Factor Regular")]
        [Column(TypeName = "decimal(18, 6)")]
        public decimal RegularFactor { get; set; }

        [Display(Name = "Factor Sobretiempo")]
        [Column(TypeName = "decimal(18, 6)")]
        public decimal OvertimeFactor { get; set; }

        [Display(Name = "País")]
        public string CountryCode { get; set; }


    }
}
