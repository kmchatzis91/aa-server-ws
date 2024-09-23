
using AA.Server.WS.Application.Contracts;
using AA.Server.WS.Infrastructure.Context;
using AA.Server.WS.Infrastructure.Repositories;

namespace AA.Server.WS.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            // swagger settings
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

            #region Added Services
            // configuration
            builder.Configuration
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            // logging
            builder.Logging.AddFile(Path.Combine(builder.Configuration["LogFilePath"], "AA.Server.WS-{Date}.txt"));

            // context & services
            builder.Services.AddScoped<DapperContext>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IDbUserRepository, DbUserRepository>();
            builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
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
