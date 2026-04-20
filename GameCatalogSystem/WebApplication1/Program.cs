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

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

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
