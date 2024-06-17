using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySql.Data.MySqlClient;
using System.Data;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
        });

        services.AddScoped<IDbConnection>(sp =>
        {
            var dbHost = Environment.GetEnvironmentVariable("MYSQL_HOST");
            var dbUser = Environment.GetEnvironmentVariable("MYSQL_USER");
            var dbPassword = Environment.GetEnvironmentVariable("MYSQL_PWD");
            var dbName = Environment.GetEnvironmentVariable("DB_NAME");

            var connectionString = $"Server={dbHost};Database={dbName};User={dbUser};Password={dbPassword};";
            return new MySqlConnection(connectionString);
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseCors("AllowAllOrigins");

        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
