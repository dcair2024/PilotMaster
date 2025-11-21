using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Logging; // 🔑 NECESSÁRIO para o bloco try/catch do Logger

// 🔑 Namespaces dos seus projetos
using PilotMaster.Infrastructure.Data;
using PilotMaster.Domain.Entities;
using PilotMaster.Application.Interfaces;
using PilotMaster.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// ----------------------------------------------------
// 1. Configuração do Banco de Dados e Identity
// ----------------------------------------------------

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔑 Identity configurado com ApplicationUser
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();


// ----------------------------------------------------
// 2. Configuração do JWT
// ----------------------------------------------------

var jwtKey = builder.Configuration["Jwt:Key"];

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// ----------------------------------------------------
// 3. Injeção de Dependência (DI) e MVC
// ----------------------------------------------------

builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ----------------------------------------------------
// 4. Configuração do CORS (Para permitir o Frontend na 5173)
// ----------------------------------------------------

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

var app = builder.Build();

// ----------------------------------------------------
// 5. Middleware e Execução
// ----------------------------------------------------

// Middleware CORS deve ser chamado antes de UseAuthentication/UseAuthorization
app.UseCors("CorsPolicy");
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// ----------------------------------------------------
// 6. Bloco de Seed (Criação do Admin)
// ----------------------------------------------------

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // 🔑 CORREÇÃO 1: Usando IdentityUser para ser consistente com o IdentitySeed.cs
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var context = services.GetRequiredService<AppDbContext>();

        // Aplica migrations pendentes
        context.Database.Migrate();

        // 🔑 CORREÇÃO 2: Usando o nome correto do método (SeedAsync) e passando o parâmetro correto (services)
        await IdentitySeed.SeedAsync(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocorreu um erro durante a criação/semeadura do banco de dados.");
    }
}

app.Run();