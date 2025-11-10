using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PilotMaster.Domain.Entities;

namespace PilotMaster.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        // Mapeamento das Entidades (Mantido o mesmo, pois as propriedades são simples)
        public DbSet<Navio> Navios => Set<Navio>();
        public DbSet<Manobra> Manobras => Set<Manobra>();
        public DbSet<Usuario> Usuarios => Set<Usuario>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuração do relacionamento Manobra-Navio
            modelBuilder.Entity<Manobra>()
                .HasOne(m => m.Navio)
                .WithMany(n => n.Manobras)
                .HasForeignKey(m => m.NavioId);

            base.OnModelCreating(modelBuilder);
        }
    }
}