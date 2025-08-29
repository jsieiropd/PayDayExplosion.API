using System.ComponentModel.DataAnnotations;
using PayDayExplosion.Domain.Data.Common;

namespace PayDayExplosion.Domain.Data.Entities
{
    public class PayType : Base
    {
        [Display(Name = "Código")]
        public string Code { get; set; } = string.Empty;

        [Display(Name = "Nombre")]
        public string Name { get; set; } = string.Empty;


    }
}
