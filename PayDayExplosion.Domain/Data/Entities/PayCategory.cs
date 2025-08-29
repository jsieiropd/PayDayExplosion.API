using System.ComponentModel.DataAnnotations;
using PayDayExplosion.Domain.Data.Common;

namespace PayDayExplosion.Domain.Data.Entities
{
    public class PayCategory : Base
    {
        [Display(Name = "Código")]
        public string Code { get; set; }

        [Display(Name = "Nombre")]
        public string Name { get; set; }



        public ICollection<Subspan> Subspans { get; set; } = new List<Subspan>();
    }
}
