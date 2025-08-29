using PayDayExplosion.Application.Dtos;
using PayDayExplosion.Application.Dtos.Panama;
using PayDayExplosion.Domain.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayDayExplosion.Application.Interfaces.Panama
{
    public interface ISubspanPAService
    {
        List<SubspanWithSpanDetailTypesPADto> SplitSpansByShift(List<SpanPADto> spans, ShiftDto shift, List<SpanTypeDto> spanTypes, List<PayCategoryDto> payCategories, string countryCode, bool onlyCaptureOvertime);

        List<SubspanWithSpanDetailTypesPADto> FilterSpansByAuthorization(List<SubspanWithSpanDetailTypesPADto> subspans, ShiftDto shift, bool authOvertimePreShift, bool authOvertimePostShift, bool authOvertimeDuringMeal, List<SpanTypeDto> spanTypes, List<PayCategoryDto> payCategories);

        List<SubspanWithSpanDetailTypesPADto> SplitSpansAtKeyTimes(List<SubspanWithSpanDetailTypesPADto> subspans);

        List<SubspanWithSpanDetailTypesPADto> ClassifySpansByWorkshiftType(List<SubspanWithSpanDetailTypesPADto> subspans, List<WorkshiftTypeDto> workshiftTypes, List<PayCategoryDto> payCategories);

        List<SubspanWithSpanDetailTypesPADto> DetermineOvertimeExcess(List<SubspanWithSpanDetailTypesPADto> subspans, decimal overtimeHours, List<PayCategoryDto> payCategories);

        List<SubspanPADto> ExpandSpanDetailTypeSubspans(List<SubspanWithSpanDetailTypesPADto> processedSubspans, List<SpanDetailTypeDto> spanDetailTypes, string employeeTypeCode);
    }
}
