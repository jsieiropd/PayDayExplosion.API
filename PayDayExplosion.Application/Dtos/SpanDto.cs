using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayDayExplosion.Application.Dtos
{
    public class SpanDto
    {
        public int Id { get; set; }
        public int WorkdayId { get; set; }
        public int SpanTypeId { get; set; }
        public DateTime TimeIn { get; set; }
        public DateTime TimeOut { get; set; }
    }
}
