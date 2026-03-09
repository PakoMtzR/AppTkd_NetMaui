using Microsoft.Extensions.Logging;
using MauiApp1.Data;
using MauiApp1.Viewmodels;
using MauiApp1.Services;
using MauiApp1.Pages;
using CommunityToolkit.Maui;

namespace MauiApp1
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Registrar registros para la Inyeccion de Dependencias
            builder.Services.AddSingleton<DatabaseContext>();
            builder.Services.AddSingleton<StudentService>();
            builder.Services.AddSingleton<InventoryService>();
            builder.Services.AddSingleton<SalesService>();
            builder.Services.AddSingleton<CartService>();

            // Registrar Viewmodels
            builder.Services.AddSingleton<StudentListVM>();
            builder.Services.AddTransient<StudentDetailVM>();
            builder.Services.AddSingleton<CategoryListVM>();
            builder.Services.AddTransient<CategoryDetailVM>();
            builder.Services.AddSingleton<ProductListVM>(); 
            builder.Services.AddTransient<ProductDetailVM>();
            builder.Services.AddSingleton<ProductSelectionVM>();
            builder.Services.AddSingleton<CartVM>();
            builder.Services.AddSingleton<SalesHistoryVM>();

            // Registrar Paginas
            builder.Services.AddSingleton<StudentListPage>();
            builder.Services.AddTransient<StudentDetailPage>();
            builder.Services.AddSingleton<CategoryListPage>();
            builder.Services.AddTransient<CategoryDetailPage>();
            builder.Services.AddSingleton<ProductListPage>(); 
            builder.Services.AddTransient<ProductDetailPage>(); 
            builder.Services.AddSingleton<ProductSelectionPage>();
            builder.Services.AddSingleton<CartPage>();
            builder.Services.AddSingleton<SalesHistoryPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // Asegurar que la base de datos es creada
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
