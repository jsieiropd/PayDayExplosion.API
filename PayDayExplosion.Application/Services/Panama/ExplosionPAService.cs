using Microsoft.EntityFrameworkCore;
using PayDayExplosion.Application.Dtos.Panama;
using PayDayExplosion.Application.Interfaces.Panama;
using PayDayExplosion.Domain.Data.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PayDayExplosion.Application.Services.Panama
{
    public class ExplosionPAService : IExplosionPAService
    {
        private readonly ISubspanPAService _subspanService;
        private readonly IFactorPAService _factorService;
        private readonly IPayPAService _payService;


        public ExplosionPAService(ISubspanPAService subspanService, IFactorPAService factorService, IPayPAService payService)
        {
            _subspanService = subspanService;
            _factorService = factorService;
            _payService = payService;
        }






        public async Task<List<SubspanPADto>> ExplosionTestPAAsync(ExplosionTestPADto explosion)
        {
            var spanCount = 1;
            foreach (var span in explosion.Spans)
            {
                Debug.WriteLine("#:                 " + spanCount);
                Debug.WriteLine("TimeIn:            " + span.TimeIn);
                Debug.WriteLine("TimeOut:           " + span.TimeOut);
                Debug.WriteLine("Rain:              " + span.Rain);
                Debug.WriteLine("RainStoppage:      " + span.RainStoppage);
                Debug.WriteLine("Height125mOrUnder: " + span.Height125mOrUnder);
                Debug.WriteLine("HeightOver125m:    " + span.HeightOver125m);
                Debug.WriteLine("Depth:             " + span.Depth);
                Debug.WriteLine("Tunnel:            " + span.Tunnel);
                Debug.WriteLine("Hammering:         " + span.Hammering);
                Debug.WriteLine("Raking:            " + span.Raking);
                Debug.WriteLine("");
                spanCount++;
            }

            var overtimeHours = explosion.OvertimeHoursPreviouslyWorkedInCurrentWeek;


            //CREANDO LOS SUBSPANS
            //separando los spans que caen en el corte de jornada programada
            var subspans = _subspanService.SplitSpansByShift(explosion.Spans, explosion.Shift, explosion.SpanTypes, explosion.PayCategories, explosion.CountryCode, explosion.Workday.OnlyCaptureOvertime);

            //dejar o quitar sobretiempos dependiendo del caso de:
            //antes de entrada, despues de salida o en hora de comida 
            subspans = _subspanService.FilterSpansByAuthorization(subspans, explosion.Shift, explosion.Workday.AuthOvertimePreShift, explosion.Workday.AuthOvertimePostShift, explosion.Workday.AuthOvertimeDuringMeal, explosion.SpanTypes, explosion.PayCategories);
            
            //separando los subspans de ser necesario en los cambios de horario (6am y 6pm), y en el cambio de día
            subspans = _subspanService.SplitSpansAtKeyTimes(subspans);

            //determinando que tipo de jornada es, entre:
            //regular diurna, regular mixta (mixta diurna-nocturna / mixta nocturna-diurna), regular nocturna,
            //extraordinaria diurna, extraordinaria mixta diurna-nocturna, extraordinaria mixta nocturna-diurna, extraordinaria nocturna
            subspans = _subspanService.ClassifySpansByWorkshiftType(subspans, explosion.WorkshiftTypes, explosion.PayCategories);

            subspans.OrderBy(ss => ss.SubTimeIn).ToList();

            //Determinando el exceso de 3 horas diarias o 9 horas semanales en los subspans de jornada extraordinaria
            subspans = _subspanService.DetermineOvertimeExcess(subspans, overtimeHours, explosion.PayCategories);

            //obteniendo un subspan por cada spandetailtype (de construcción) que fue enviado
            var finalSubspans = _subspanService.ExpandSpanDetailTypeSubspans(subspans, explosion.SpanDetailTypes, explosion.EmployeeTypeCode);

            finalSubspans.OrderBy(ss => ss.SubTimeIn).ToList();







            //FACTORES
            //Factor de tipo de Jornada (Regular Diurna, Extraordinaria Nocturna, etc) + tipo de incidencia en el trampo (Normal, Ausencia, Tardanza, etc) "WorkshiftTypeSpanTypeFactor"
            finalSubspans = _factorService.DetermineSpanTypeWorkshiftTypeFactor(finalSubspans, explosion.PayCategories, explosion.SpanTypes,  explosion.WorkshiftTypes, explosion.Workday.StrictShiftRules, explosion.PayTypeCode, explosion.EmployeeTypeCode, explosion.IsSuntracsRuleContract);

            //Factor de Tipo de Día (Normal, feriado, Descanso, compensatorio, etc) "WorkdayTypeFactor"
            finalSubspans = _factorService.DetermineWorkdayTypeFactor(finalSubspans, explosion.PayCategories, explosion.Workday, explosion.WorkdayTypes);

            //Factor de exceso de sobretiempo de 3 horas diarias o 9 horas semanales "SubspanDetailTypeFactor"
            finalSubspans = _factorService.DetermineSubspanDetailTypeFactor(finalSubspans, explosion.SubspanDetailTypes);

            //Factor de Construcción "SpanDetailTypeFactor"
            finalSubspans = _factorService.DetermineSpanDetailTypeFactor(finalSubspans, explosion.SpanDetailTypes);

            //Factor Final "Factor"
            finalSubspans = _factorService.CalculateFinalFactor(finalSubspans, explosion.PayCategories);





            //CALCULO MONTO RESULTANTE (Amount)
            finalSubspans = _payService.CalculateAmounts(finalSubspans, explosion.HourlyRate);


            return finalSubspans;
        }





    }
}
