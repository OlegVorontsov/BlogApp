using BlogApp.Server.Data;
using BlogApp.Server.Models;
using BlogApp.Server.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BlogApp.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                            .AddJwtBearer( options =>
                            {
                                options.RequireHttpsMetadata = false;
                                options.TokenValidationParameters = new TokenValidationParameters
                                {
                                    ValidateIssuer = true,
                                    ValidIssuer = AuthOptions.ISSUER,
                                    ValidateAudience = true,
                                    ValidAudience = AuthOptions.AUDIENCE,
                                    ValidateLifetime = true,
                                    IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                                    ValidateIssuerSigningKey = true
                                };
                            });
            builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(
                "Server=(localdb)\\MSSQLLocalDB;Database=BlogAppDB;Trusted_Connection=True;"));
            builder.Services.AddTransient<UsersService>();
            builder.Services.AddTransient<NewsService>();
            builder.Services.AddTransient<NoSQLDataService>();

            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
