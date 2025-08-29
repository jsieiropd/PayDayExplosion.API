using PayDayExplosion.Application.Dtos.Panama;
using PayDayExplosion.Domain.Data.Entities;

namespace PayDayExplosion.Application.Interfaces.Panama
{
    public interface IExplosionPAService
    {
        Task<List<SubspanPADto>> ExplosionTestPAAsync(ExplosionTestPADto explosion);
    }
}
