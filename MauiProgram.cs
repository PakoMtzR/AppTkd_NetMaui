using Microsoft.Extensions.Logging;
using MauiApp1.Data;
using MauiApp1.Viewmodels;
using MauiApp1.Services;
using MauiApp1.Pages;

namespace MauiApp1
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Register services for Dependency Injection
            builder.Services.AddSingleton<DatabaseContext>();
            builder.Services.AddSingleton<StudentService>();

            // Register Viewmodels
            builder.Services.AddSingleton<StudentListVM>();
            builder.Services.AddSingleton<StudentDetailVM>();

            // Register Pages
            builder.Services.AddSingleton<StudentListPage>();
            builder.Services.AddSingleton<StudentDetailPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // Ensure database is created
            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<DatabaseContext>();
                context.Database.EnsureCreated();
            }

            return app;
        }
    }
}
