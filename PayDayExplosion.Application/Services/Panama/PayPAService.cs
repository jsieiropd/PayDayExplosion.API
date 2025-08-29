using PayDayExplosion.Application.Dtos.Panama;
using PayDayExplosion.Application.Interfaces.Panama;
using PayDayExplosion.Domain.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayDayExplosion.Application.Services.Panama
{
    public class PayPAService : IPayPAService
    {
        public List<SubspanPADto> CalculateAmounts(List<SubspanPADto> subspans, decimal hourlyRate)
        {
            foreach (var subspan in subspans)
            {
                subspan.Amount = Math.Round(hourlyRate * subspan.Factor * subspan.ClockedHours, 2);
            }

            return subspans;
        }




    }
}
