using PayDayExplosion.Domain.Data.Common;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PayDayExplosion.Domain.Data.Entities
{
    public class SubspanDetail : Base
    {
        [Display(Name = "Subtramo")]
        public int SubspanId { get; set; }

        [Display(Name = "Tipo de Subtramo")]
        public int SubspanDetailTypeId { get; set; }




        [ForeignKey("SubspanId")]
        public Subspan Subspan { get; set; }

        [ForeignKey("SubspanDetailTypeId")]
        public SubspanDetailType SubspanDetailType { get; set; }


    }
}
