
using AA.Server.WS.Application.Contracts;
using AA.Server.WS.Infrastructure.Context;
using AA.Server.WS.Infrastructure.Repositories;
using AA.Server.WS.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AA.Server.WS.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            #region Configuration
            builder.Configuration
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            #endregion

            #region Swagger settings
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "AA.Server.WS",
                    Version = "v1",
                    Description = "This is the API documentation for the AA.Server.WS.",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Name = "Support AA Team",
                        Email = "support@aa.com",
                        Url = new Uri("https://example.support.aa.com")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense
                    {
                        Name = "Use under AA License",
                        Url = new Uri("https://example.support.aa.com/license")
                    }
                });
            });
            #endregion

            #region Cors Policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("allow-all", builder =>
                {
                    builder.AllowAnyOrigin() // Allow all origins
                           .AllowAnyMethod() // Allow all HTTP methods
                           .AllowAnyHeader(); // Allow all headers
                });
            });
            #endregion

            #region Logging
            builder.Logging.AddFile(Path.Combine(builder.Configuration["LogFilePath"], "AA.Server.WS-{Date}.txt"));
            #endregion

            #region Jwt Auth
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                    ValidAudience = builder.Configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
                };
            });
            #endregion

            #region Added Services
            builder.Services.AddScoped<DapperContext>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IDbUserRepository, DbUserRepository>();
            builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();

            builder.Services.AddScoped<TokenService>();
            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Apply cors policy globally
            app.UseCors("allow-all");

            // Add for Jwt Auth
            app.UseAuthentication();

            // Add for Jwt Auth
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
