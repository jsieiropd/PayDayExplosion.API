using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PayDayExplosion.Application.Dtos
{
    public class SpanDetailTypeDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public decimal Factor { get; set; }
    }
}
