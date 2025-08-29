using Microsoft.EntityFrameworkCore;
using PayDayExplosion.Application.Interfaces;
using PayDayExplosion.Domain.Data.Entities;

namespace PayDayExplosion.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<EmployeeType> EmployeeTypes { get; set; }
        public DbSet<PayCategory> PayCategories { get; set; }
        public DbSet<PayType> PayTypes { get; set; }
        public DbSet<SpanDetailType> SpanDetailTypes { get; set; }
        public DbSet<Subspan> Subspans { get; set; }
        public DbSet<SubspanDetail> SubspanDetails { get; set; }
        public DbSet<SubspanDetailType> SubspanDetailTypes { get; set; }
        public DbSet<WorkdayType> WorkdayTypes { get; set; }
        public DbSet<WorkshiftType> WorkshiftTypes { get; set; }
        public DbSet<SpanType> SpanTypes { get; set; }

    }
}
