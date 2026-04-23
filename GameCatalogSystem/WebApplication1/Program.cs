using FluentValidation;
using GameCatalogSystem.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<CatalogDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<GameCatalogSystem.Domain.Repositories.IGenreRepository, GameCatalogSystem.Infrastructure.Repositories.GenreRepository>();
            builder.Services.AddScoped<GameCatalogSystem.Application.Services.Interfaces.IGenreService, GameCatalogSystem.Application.Services.GenreService>();
            builder.Services.AddScoped<GameCatalogSystem.Domain.Repositories.IGameRepository, GameCatalogSystem.Infrastructure.Repositories.GameRepository>();
            builder.Services.AddScoped<GameCatalogSystem.Application.Services.Interfaces.IGameService, GameCatalogSystem.Application.Services.GameService>();
            builder.Services.AddValidatorsFromAssemblyContaining<GameCatalogSystem.Application.Validators.CreateGameValidator>();
            builder.Services.AddScoped<GameCatalogSystem.Application.Services.Interfaces.IAuditService, GameCatalogSystem.Infrastructure.Services.MongoAuditService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    // Pegamos a instância do nosso banco de dados
                    var context = services.GetRequiredService<GameCatalogSystem.Infrastructure.Context.CatalogDbContext>();

                    // Verifica se já existe algum gênero cadastrado. Se NÃO existir, nós populamos!
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

                        // Salva as alterações no banco
                        context.SaveChanges();
                        Console.WriteLine("Banco de dados populado com Gêneros padrão com sucesso!");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao rodar o Data Seeding: {ex.Message}");
                }
            }

            // Configure the HTTP request pipeline.
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

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
