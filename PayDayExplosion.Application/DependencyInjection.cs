using Microsoft.Extensions.DependencyInjection;
using PayDayExplosion.Application.Interfaces.Panama;
using PayDayExplosion.Application.Services.Panama;

namespace PayDayExplosion.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IExplosionPAService, ExplosionPAService>();
            services.AddScoped<ISubspanPAService, SubspanPAService>();
            services.AddScoped<IFactorPAService, FactorPAService>();
            services.AddScoped<IPayPAService, PayPAService>();

            return services;
        }
    }
}
