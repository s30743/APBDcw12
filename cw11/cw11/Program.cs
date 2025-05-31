using cw11.Data;
using cw11.Service;
using Microsoft.EntityFrameworkCore;

namespace cw11;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();
        builder.Services.AddControllers();
        builder.Services.AddDbContext<EfdatabaseContext>(opts => 
            opts.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
        builder.Services.AddScoped<ITripsService, TripsService>();
        builder.Services.AddScoped<IClientService, ClientService>();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}