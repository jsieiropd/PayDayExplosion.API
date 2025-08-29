using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayDayExplosion.Application.Dtos
{
    public class SpanTypeDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public decimal RegularFactor { get; set; }
        public decimal OvertimeFactor { get; set; }

    }
}
