using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PilotMaster.Application.Interfaces;
using PilotMaster.Application.Services;
using PilotMaster.Infrastructure.Data;
using System.Reflection;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Configuração do DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("PilotMaster.Infrastructure")));


// ADICIONE ESTA LINHA: Habilita o suporte a Controllers MVC/API
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Esta linha garante que o servidor entenda e use o padrão camelCase,
        // alinhando-se ao Fronten (JavaScript).
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PilotMaster API", Version = "v1" });
    // Configuração para incluir comentários XML 
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    // Configuração de Segurança JWT no Swagger 
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Insira o token JWT no formato: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

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

// Injeção de Dependência
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddAuthorization();

// Configure a Autenticação JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // Valida a chave do emissor (Key)
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),

            // Valida o Emissor (Issuer)
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],

            // Valida a Audiência (Audience)
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],

            // Valida o tempo de expiração (Expiry)
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });
const string AllowPilotMasterOrigin = "_AllowPilotMasterOrigin";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowPilotMasterOrigin,
        policy =>
        {
            // Permite o acesso APENAS do endereço do Frontend da Gemima
            // O Frontend da Gemima está rodando em http://localhost:5173
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});



var app = builder.Build();
app.UseCors(AllowPilotMasterOrigin);

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapControllers();

app.Run();

