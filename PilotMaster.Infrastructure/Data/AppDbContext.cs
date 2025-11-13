using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PilotMaster.Domain.Entities;
using System.Security.Cryptography;


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
            base.OnModelCreating(modelBuilder);

            // Cria hash da senha
            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes("Admin@123"));
            var senhaHash = BitConverter.ToString(hash).Replace("-", "");

            modelBuilder.Entity<Usuario>().HasData(
                new Usuario
                {
                    Id = 1,
                    Email = "admin@pilotmaster.com",
                    SenhaHash = senhaHash,
                    Role = "Supervisor"
                }
            );
        }
    }
}