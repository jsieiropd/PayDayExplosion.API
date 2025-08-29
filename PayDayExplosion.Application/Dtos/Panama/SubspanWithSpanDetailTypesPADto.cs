using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayDayExplosion.Application.Dtos.Panama
{
    public class SubspanWithSpanDetailTypesPADto : SubspanDto
    {
        //SpanDetailTypes
        public bool Rain { get; set; }
        public bool RainStoppage { get; set; }
        public bool Height125mOrUnder { get; set; }
        public bool HeightOver125m { get; set; }
        public bool Depth { get; set; }
        public bool Tunnel { get; set; }
        public bool Hammering { get; set; }
        public bool Raking { get; set; }

        //SubspanDetailTypes
        public bool OvertimeExcess { get; set; }
    }
}
