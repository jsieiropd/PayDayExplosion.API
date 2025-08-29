using PayDayExplosion.Domain.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayDayExplosion.Domain.Data.Entities
{
    public class WorkshiftType : Base
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







        public ICollection<Subspan> Subspans { get; set; } = new List<Subspan>();

    }
}
