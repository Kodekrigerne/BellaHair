using BellaHair.Application;
using BellaHair.Application.Invoices;
using BellaHair.Infrastructure;
using BellaHair.Presentation.WebUI.Components;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using MudBlazor.Services;
using Radzen;

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

            builder.Services.AddDbContext<BellaHairContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("SimplyConnectionString"))
            );

            // Dette fjerner væggen af sql i konsollen så vi kan se vores consone writelines
            // Kommenter den ud hvis du skal se den sql der bliver kørt
            builder.Logging.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.None);

            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices();

            builder.Services.AddScoped<DataProvider>();

            builder.Services.Configure<BusinessInfoSettings>(
                builder.Configuration.GetSection(BusinessInfoSettings.SectionName));

            builder.Services.Configure<OpeningTimesSettings>(
                builder.Configuration.GetSection(OpeningTimesSettings.SectionName));

            builder.Services.AddMudServices(config =>
            {
                config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;
                config.SnackbarConfiguration.PreventDuplicates = false;
                config.SnackbarConfiguration.NewestOnTop = false;
                config.SnackbarConfiguration.ShowCloseIcon = true;
                config.SnackbarConfiguration.VisibleStateDuration = 10000;
                config.SnackbarConfiguration.HideTransitionDuration = 500;
                config.SnackbarConfiguration.ShowTransitionDuration = 500;
                config.SnackbarConfiguration.SnackbarVariant = MudBlazor.Variant.Filled;
            });

            builder.Services.AddRadzenComponents();

            var app = builder.Build();

            //using (var scope = app.Services.CreateScope())
            //{
            //    var context = scope.ServiceProvider.GetRequiredService<BellaHairContext>();
            //    context.Database.EnsureDeleted();
            //    context.Database.EnsureCreated();

            //    var commandHandler = scope.ServiceProvider.GetRequiredService<IBookingCommand>();

            //    context.Database.ExecuteSqlRaw("PRAGMA journal_mode=DELETE;");

            //    var dataProvider = new DataProvider(context, scope.ServiceProvider);
            //    dataProvider.AddData().Wait();
            //}

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
