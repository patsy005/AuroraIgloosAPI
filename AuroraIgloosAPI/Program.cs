using System.Security.Policy;
using System.Text;
using AuroraIgloosAPI.BussinessLogic;
using AuroraIgloosAPI.Models.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using AuroraIgloosAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

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
            
            // password hasher
            builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

            var jwt = builder.Configuration.GetSection("Jwt");
            var keyBytes = Encoding.UTF8.GetBytes(jwt["Key"]);
            
            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwt["Issuer"],
                        ValidAudience = jwt["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                        ClockSkew = TimeSpan.FromSeconds(30)
                    };
                });

            // builder.Services.AddAuthentication();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("FrontendCors", policy =>
                {
                    policy
                        .WithOrigins(
                            "http://localhost:5173",
                            "http://localhost:5174"
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    // .AllowCredentials();
                });
            });



            var app = builder.Build();

            // Konfiguracja �rodowiska
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseHttpsRedirection();
            }

            app.UseStaticFiles();
            
            app.UseRouting();
            
            app.UseCors("FrontendCors");
            
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.MapControllers();
            app.Run();
        }
    }
}
