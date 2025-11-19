using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace PilotMaster.Infrastructure.Data
{
    public static class IdentitySeed
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            // Cria a role
            if (!await roleManager.RoleExistsAsync("Supervisor"))
                await roleManager.CreateAsync(new IdentityRole("Supervisor"));

            // Cria o usuário
            var email = "admin@pilotmaster.com";
            var senha = "Admin@123";

            if (await userManager.FindByEmailAsync(email) == null)
            {
                var user = new IdentityUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, senha);

                if (result.Succeeded)
                    await userManager.AddToRoleAsync(user, "Supervisor");
            }
        }
    }
}
