// EM PilotMaster.Infrastructure/Data/IdentitySeed.cs

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection; // Necessário para IServiceProvider
using PilotMaster.Domain.Entities;

namespace PilotMaster.Infrastructure.Data
{
    public static class IdentitySeed
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            // 🔑 1. RESTAURAÇÃO E CORREÇÃO DE TIPOS: Recupere os Managers
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            // 🔑 2. DEFINIÇÃO DAS VARIÁVEIS (que você adicionou)
            var email = "admin@pilotmaster.com";
            var password = "Admin@123";

            // 3. Cria a role (Supervisor)
            if (!await roleManager.RoleExistsAsync("Supervisor"))
                await roleManager.CreateAsync(new IdentityRole("Supervisor"));

            // 4. Cria o usuário (admin)
            if (await userManager.FindByEmailAsync(email) == null)
            {
                var user = new ApplicationUser // <<<<<< Usa o tipo ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                    await userManager.AddToRoleAsync(user, "Supervisor");
            }
        }
    }
}