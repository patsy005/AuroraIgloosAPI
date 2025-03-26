using AuroraIgloosAPI.BussinessLogic;
using AuroraIgloosAPI.Models.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace AuroraIgloosAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Pobierz connection string z appsettings.json
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            //builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

            builder.Services.Configure<JsonOptions>(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
            });


            // Zarejestruj CompanyContext w DI
            builder.Services.AddDbContext<CompanyContext>(options =>
                options.UseSqlServer(connectionString));

            // Dodaj kontrolery i inne serwisy
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            var app = builder.Build();

            // Konfiguracja œrodowiska
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
