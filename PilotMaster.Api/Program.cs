using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging; // 🔑 NECESSÁRIO para o bloco try/catch do Logger
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PilotMaster.Application.Interfaces;
using PilotMaster.Application.Services;
using PilotMaster.Domain.Entities;
// 🔑 Namespaces dos seus projetos
using PilotMaster.Infrastructure.Data;
using System.Text;

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
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
        ),

        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"]
    };
});

builder.Services.AddSwaggerGen(c =>
{
    // Define o esquema de segurança JWT para o Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Insira o token JWT (apenas o token, sem 'Bearer ')",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    // Garante que os endpoints protegidos usem o esquema "Bearer"
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// ----------------------------------------------------
// 3. Injeção de Dependência (DI) e MVC
// ----------------------------------------------------

builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


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
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    // 1 - Se não existir a role Admin, cria
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }

    // 2 - Cria o usuário Admin se ele não existir
    var user = await userManager.FindByEmailAsync("admin@pilotmaster.com");

    if (user == null)
    {
        var admin = new ApplicationUser
        {
            UserName = "admin@pilotmaster.com",
            Email = "admin@pilotmaster.com",
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(admin, "Admin@123");

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}

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
        // ... (O resto do código que injeta ApplicationUser)

        var context = services.GetRequiredService<AppDbContext>();

        // Aplica migrations pendentes
        context.Database.Migrate();

        // 🔑 CHAMADA CORRETA: Passando apenas o services
        await IdentitySeed.SeedAsync(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocorreu um erro durante a criação/semeadura do banco de dados.");
    }
}

app.Run();