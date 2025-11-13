using BellaHair.Application;
using BellaHair.Infrastructure;
using BellaHair.Presentation.WebUI.Components;
using Microsoft.EntityFrameworkCore;

namespace BellaHair.Presentation.WebUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddDbContext<BellaHairContext>(options
                => options.UseSqlite(builder.Configuration.GetConnectionString("BellaHairContext")));

            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices();

            var app = builder.Build();

            var dataProvider = new DataProvider(app.Services.GetRequiredService<BellaHairContext>());
            dataProvider.AddData();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseAntiforgery();

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
