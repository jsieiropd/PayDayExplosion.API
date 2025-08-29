using PayDayExplosion.Application.Dtos.Panama;
using PayDayExplosion.Domain.Data.Entities;

namespace PayDayExplosion.Application.Interfaces.Panama
{
    public interface IPayPAService
    {
        public List<SubspanPADto> CalculateAmounts(List<SubspanPADto> subspans, decimal hourlyRate);
    }
}
