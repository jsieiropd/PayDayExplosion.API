using PayDayExplosion.Domain.Data.Entities;

namespace PayDayExplosion.Infrastructure.Persistence
{
    public class SeedDb
    {
        private readonly ApplicationDbContext _dbContext;

        public SeedDb(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SeedAsync()
        {
            await _dbContext.Database.EnsureCreatedAsync();

            await CheckCountriesAsync();
            await CheckEmployeeTypesAsync();
            await CheckWorkshiftTypesAsync();
            await CheckWorkdayTypesAsync();
            await CheckSpanDetailTypesAsync();
            await CheckSpanTypesAsync();
            await CheckSubspanDetailTypesAsync();
            await CheckPayCategoriesAsync();
            await CheckPayTypesAsync();
        }

        private async Task CheckEmployeeTypesAsync()
        {
            if (!_dbContext.EmployeeTypes.Any())
            {
                _dbContext.EmployeeTypes.Add(new EmployeeType { Name = "Normal", Code = "Normal" });
                _dbContext.EmployeeTypes.Add(new EmployeeType { Name = "Construcción", Code = "Construction" });
                _dbContext.EmployeeTypes.Add(new EmployeeType { Name = "Agrícola", Code = "Agricultural" });
                _dbContext.EmployeeTypes.Add(new EmployeeType { Name = "Marino", Code = "Maritime" });
                await _dbContext.SaveChangesAsync();
            }
        }

        private async Task CheckCountriesAsync()
        {
            if (!_dbContext.Countries.Any())
            {
                _dbContext.Countries.Add(new Country { Name = "Panamá", Code = "PA" });
                await _dbContext.SaveChangesAsync();
            }
        }

        private async Task CheckPayTypesAsync()
        {
            if (!_dbContext.PayTypes.Any())
            {
                _dbContext.PayTypes.Add(new PayType { Name = "Mensual", Code = "Monthly" });
                _dbContext.PayTypes.Add(new PayType { Name = "Por Hora", Code = "Hourly" });
                await _dbContext.SaveChangesAsync();
            }
        }

        private async Task CheckPayCategoriesAsync()
        {
            if (!_dbContext.PayCategories.Any())
            {
                _dbContext.PayCategories.Add(new PayCategory { Name = "Jornada Regular", Code = "RegularShift" });
                _dbContext.PayCategories.Add(new PayCategory { Name = "Jornada Extraordinaria", Code = "OvertimeShift" });
                await _dbContext.SaveChangesAsync();
            }
        }

        private async Task CheckWorkshiftTypesAsync()
        {
            if (!_dbContext.WorkshiftTypes.Any())
            {
                _dbContext.WorkshiftTypes.Add(new WorkshiftType { Name = "Diurna",                Code = "Day",           RegularFactor = 1,         OvertimeFactor = 1.25m, CountryCode = "PA" });
                _dbContext.WorkshiftTypes.Add(new WorkshiftType { Name = "Mixta Diurna-Nocturna", Code = "MixedDayNight", RegularFactor = 1.066667m, OvertimeFactor = 1.50m, CountryCode = "PA" });
                _dbContext.WorkshiftTypes.Add(new WorkshiftType { Name = "Mixta Nocturna-Diurna", Code = "MixedNightDay", RegularFactor = 1.066667m, OvertimeFactor = 1.75m, CountryCode = "PA" });
                _dbContext.WorkshiftTypes.Add(new WorkshiftType { Name = "Nocturna",              Code = "Night",         RegularFactor = 1.142857m, OvertimeFactor = 1.75m, CountryCode = "PA" });

                await _dbContext.SaveChangesAsync();
            }
        }

        private async Task CheckSpanTypesAsync()
        {
            if (!_dbContext.SpanTypes.Any())
            {
                _dbContext.SpanTypes.Add(new SpanType { Name = "Tramo Normal",          Code = "Normal",                RegularFactor = 1,  OvertimeFactor = 1, CountryCode = "PA" });
                _dbContext.SpanTypes.Add(new SpanType { Name = "Tardanza",              Code = "LateArrival",           RegularFactor = 1,  OvertimeFactor = 1, CountryCode = "PA" });
                _dbContext.SpanTypes.Add(new SpanType { Name = "Ausencia Pagada",       Code = "PaidAbsence",           RegularFactor = 1,  OvertimeFactor = 1, CountryCode = "PA" });
                _dbContext.SpanTypes.Add(new SpanType { Name = "Ausencia NO Pagada",    Code = "UnpaidAbsence",         RegularFactor = -1, OvertimeFactor = 0, CountryCode = "PA" });
                _dbContext.SpanTypes.Add(new SpanType { Name = "Certificado Médico",    Code = "MedicalCertificate",    RegularFactor = 1,  OvertimeFactor = 1, CountryCode = "PA" });
                _dbContext.SpanTypes.Add(new SpanType { Name = "Horas de Ajuste",       Code = "AdjustmentHours",       RegularFactor = 1,  OvertimeFactor = 1, CountryCode = "PA" });
                _dbContext.SpanTypes.Add(new SpanType { Name = "Descanso (Comida)",     Code = "Break",                 RegularFactor = 0,  OvertimeFactor = 1, CountryCode = "PA" });


                await _dbContext.SaveChangesAsync();
            }
        }








        private async Task CheckWorkdayTypesAsync()
        {
            if (!_dbContext.WorkdayTypes.Any())
            {
                _dbContext.WorkdayTypes.Add(new WorkdayType { Name = "Día Normal",                       Code = "NormalDay",                 Factor = 1,    CountryCode = "PA" });
                _dbContext.WorkdayTypes.Add(new WorkdayType { Name = "Descanso Obligatorio",             Code = "MandatoryRest",             Factor = 1.5m, CountryCode = "PA" });
                _dbContext.WorkdayTypes.Add(new WorkdayType { Name = "Día Libre Trabajado",              Code = "WorkedDayOff",              Factor = 1.5m, CountryCode = "PA" });
                _dbContext.WorkdayTypes.Add(new WorkdayType { Name = "Fiesta Nacional Trabajada",        Code = "Holiday",                   Factor = 2.5m, CountryCode = "PA" });
                _dbContext.WorkdayTypes.Add(new WorkdayType { Name = "Día Compensatorio Trabajado",      Code = "Compensatory",              Factor = 1.5m, CountryCode = "PA" });
                _dbContext.WorkdayTypes.Add(new WorkdayType { Name = "Descanso + Recargo Compensatorio", Code = "CompensatoryWithSurcharge", Factor = 2,    CountryCode = "PA" });
                await _dbContext.SaveChangesAsync();
            }
        }








        private async Task CheckSpanDetailTypesAsync()
        {
            if (!_dbContext.SpanDetailTypes.Any())
            {
                _dbContext.SpanDetailTypes.Add(new SpanDetailType { Name = "Normal",                    Code = "Normal",            Factor = 1.0m,  CountryCode = "PA" });

                _dbContext.SpanDetailTypes.Add(new SpanDetailType { Name = "Lluvia",                    Code = "Rain",              Factor = 0.5m,  CountryCode = "PA" });
                _dbContext.SpanDetailTypes.Add(new SpanDetailType { Name = "Paralización por lluvia",   Code = "RainStoppage",      Factor = -0.5m, CountryCode = "PA" });
                _dbContext.SpanDetailTypes.Add(new SpanDetailType { Name = "Altura hasta 125 metros",   Code = "Height125mOrUnder", Factor = 0.16m, CountryCode = "PA" });
                _dbContext.SpanDetailTypes.Add(new SpanDetailType { Name = "Altura mayor a 125 metros", Code = "HeightOver125m",    Factor = 0.2m,  CountryCode = "PA" });
                _dbContext.SpanDetailTypes.Add(new SpanDetailType { Name = "Profundidad",               Code = "Depth",             Factor = 0.15m, CountryCode = "PA" });
                _dbContext.SpanDetailTypes.Add(new SpanDetailType { Name = "Túnel",                     Code = "Tunnel",            Factor = 0.2m,  CountryCode = "PA" });
                _dbContext.SpanDetailTypes.Add(new SpanDetailType { Name = "Martilleo",                 Code = "Hammering",         Factor = 0.1m,  CountryCode = "PA" });
                _dbContext.SpanDetailTypes.Add(new SpanDetailType { Name = "Rastrilleo",                Code = "Raking",            Factor = 0.15m, CountryCode = "PA" });

                await _dbContext.SaveChangesAsync();
            }
        }



        private async Task CheckSubspanDetailTypesAsync()
        {
            if (!_dbContext.SubspanDetailTypes.Any())
            {
                _dbContext.SubspanDetailTypes.Add(new SubspanDetailType { Name = "Sobretiempo exceso de 3 horas diarias o 9 horas semanal", Code = "OvertimeExcess", Factor = 1.750000m, CountryCode = "PA" });

                await _dbContext.SaveChangesAsync();
            }
        }


    }
}
