using FluentValidation;
using GameCatalogSystem.Application.Services;
using GameCatalogSystem.Application.Services.Interfaces;
using GameCatalogSystem.Domain.Repositories;
using GameCatalogSystem.Infrastructure.Context;
using GameCatalogSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Separa as variáveis para não dar confusão
            var envDbUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            var localDbUrl = builder.Configuration.GetConnectionString("DefaultConnection");

            var envMongoUrl = Environment.GetEnvironmentVariable("MONGODB_URI");
            var localMongoUrl = builder.Configuration.GetSection("MongoDbSettings:ConnectionString").Value;

            // 2. A lógica blindada do Entity Framework
            builder.Services.AddDbContext<CatalogDbContext>(options =>
            {
                // Se a variável do Render não for nula/vazia, obrigatoriamente usa o Postgres
                if (!string.IsNullOrEmpty(envDbUrl))
                {
                    options.UseNpgsql(envDbUrl);
                }
                else
                {
                    // Se estiver vazia, significa que estamos rodando no seu computador (Localhost)
                    options.UseSqlServer(localDbUrl);
                }
            });

            builder.Services.AddScoped<GameCatalogSystem.Domain.Repositories.IGenreRepository, GameCatalogSystem.Infrastructure.Repositories.GenreRepository>();
            builder.Services.AddScoped<GameCatalogSystem.Application.Services.Interfaces.IGenreService, GameCatalogSystem.Application.Services.GenreService>();
            builder.Services.AddScoped<GameCatalogSystem.Domain.Repositories.IGameRepository, GameCatalogSystem.Infrastructure.Repositories.GameRepository>();
            builder.Services.AddScoped<GameCatalogSystem.Application.Services.Interfaces.IGameService, GameCatalogSystem.Application.Services.GameService>();
            builder.Services.AddValidatorsFromAssemblyContaining<GameCatalogSystem.Application.Validators.CreateGameValidator>();
            builder.Services.AddScoped<GameCatalogSystem.Application.Services.Interfaces.IAuditService, GameCatalogSystem.Infrastructure.Services.MongoAuditService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GameCatalogSystem API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Insira o token JWT desta maneira: Bearer {seu token}",
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
            Array.Empty<string>()
        }
    });
            });

            builder.Services.AddCors();

            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
                };
            });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<GameCatalogSystem.Infrastructure.Context.CatalogDbContext>();

                    if (!context.Genres.Any())
                    {
                        context.Genres.AddRange(
                            new GameCatalogSystem.Domain.Entities.Genre("RPG de Ação", "Combina elementos de RPG com combate em tempo real."),
                            new GameCatalogSystem.Domain.Entities.Genre("Ação e Aventura", "Foco em exploração, resolução de quebra-cabeças e combate."),
                            new GameCatalogSystem.Domain.Entities.Genre("FPS (Tiro em Primeira Pessoa)", "Combate com armas de fogo na perspectiva do personagem."),
                            new GameCatalogSystem.Domain.Entities.Genre("Estratégia em Tempo Real (RTS)", "Controle de exércitos e recursos sem turnos."),
                            new GameCatalogSystem.Domain.Entities.Genre("Sobrevivência / Terror", "Foco em escassez de recursos e atmosfera de medo."),
                            new GameCatalogSystem.Domain.Entities.Genre("Esportes e Corrida", "Simulações de esportes do mundo real ou veículos."),
                            new GameCatalogSystem.Domain.Entities.Genre("MMORPG", "RPG massivo online para milhares de jogadores.")
                        );

                        context.SaveChanges();
                        Console.WriteLine("Banco de dados populado com Gêneros padrão com sucesso!");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao rodar o Data Seeding: {ex.Message}");
                }
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseMiddleware<GameCatalogSystem.Middlewares.GlobalExceptionMiddleware>();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseCors(options => options
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
                dbContext.Database.EnsureCreated();
            }

            app.Run();
        }
    }
}