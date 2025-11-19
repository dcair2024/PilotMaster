using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PilotMaster.Domain.Entities;

namespace PilotMaster.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Seus DbSets de domínio
        public DbSet<Usuario> Usuarios { get; set; } = null!;
        public DbSet<Navio> Navios { get; set; } = null!;
        public DbSet<Manobra> Manobras { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Mapeamentos adicionais (se tiver)
        }
    }
}
