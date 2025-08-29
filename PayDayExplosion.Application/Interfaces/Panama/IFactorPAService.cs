using PayDayExplosion.Application.Dtos;
using PayDayExplosion.Application.Dtos.Panama;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayDayExplosion.Application.Interfaces.Panama
{
    public interface IFactorPAService
    {
        List<SubspanPADto> DetermineSpanTypeWorkshiftTypeFactor(List<SubspanPADto> subspans, List<PayCategoryDto> payCategories, List<SpanTypeDto> spanTypes,  List<WorkshiftTypeDto> workshiftTypes, bool strictShiftRules, string payTypeCode, string employeeTypeCode, bool isSuntracsRuleContract);



        List<SubspanPADto> DetermineWorkdayTypeFactor(List<SubspanPADto> subspans, List<PayCategoryDto> payCategories, WorkdayDto workday, List<WorkdayTypeDto> workdayTypes);



        List<SubspanPADto> DetermineSpanDetailTypeFactor(List<SubspanPADto> subspans, List<SpanDetailTypeDto> spanDetailTypes);




        List<SubspanPADto> DetermineSubspanDetailTypeFactor(List<SubspanPADto> subspans, List<SubspanDetailTypeDto> subspanDetailTypes);



        List<SubspanPADto> CalculateFinalFactor(List<SubspanPADto> subspans, List<PayCategoryDto> payCategories);

    }
}
