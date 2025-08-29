using PayDayExplosion.Domain.Data.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PayDayExplosion.Domain.Data.Entities
{
    public class Subspan : Base
    {

        [Display(Name = "Tramo")]
        public int SpanId { get; set; }

        [Display(Name = "Descripción")]
        public string DisplayText { get; set; } = string.Empty;

        [Display(Name = "Desde")]
        public DateTime? SubTimeIn { get; set; }

        [Display(Name = "Hasta")]
        public DateTime? SubTimeOut { get; set; }

        [Display(Name = "Tipo de turno")]
        public int PayCategoryId { get; set; }

        [Display(Name = "Tipo de Jornada")]
        public int WorkshiftTypeId { get; set; }

        [Display(Name = "Tipo de Pago Extra")]
        public int? SpanDetailTypeId { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal Factor1 { get; set; } = 1.000000m;

        [Column(TypeName = "decimal(18,6)")]
        public decimal Factor2 { get; set; } = 1.000000m;

        [Column(TypeName = "decimal(18,6)")]
        public decimal Factor3 { get; set; } = 1.000000m;

        [Column(TypeName = "decimal(18,6)")]
        public decimal Factor4 { get; set; } = 1.000000m;

        [Column(TypeName = "decimal(18,6)")]
        public decimal Substract { get; set; } = 0.00m;
        [Column(TypeName = "decimal(18,6)")]
        public decimal Factor { get; set; } = 1.000000m;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; } = 0.00m;

        [Column(TypeName = "decimal(18,6)")]
        public decimal ClockedHours { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal MealHours { get; set; }

        [Display(Name = "¿Jornada Regular?")]
        public bool IsInSchedule { get; set; }

        [Display(Name = "Código de País")]
        public string CountryCode { get; set; } = string.Empty;










        [ForeignKey("WorkshiftTypeId")] //Diurna, Mixta Diurna-Nocturna, Mixta Nocturna-Diurna, Nocturna
        public WorkshiftType WorkshiftType { get; set; }

        [ForeignKey("PayCategoryId")] //Ordinaria, Extraordinaria
        public PayCategory PayCategory { get; set; }



        public ICollection<SubspanDetail> SubspanDetails { get; set; } = new List<SubspanDetail>();
    }
}
