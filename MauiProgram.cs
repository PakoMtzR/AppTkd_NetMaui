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

            // Registrar servicios para la Inyeccion de Dependencias
            builder.Services.AddSingleton<DatabaseContext>();
            builder.Services.AddSingleton<StudentService>();
            builder.Services.AddSingleton<InventoryService>();
            builder.Services.AddSingleton<SalesService>();
            builder.Services.AddSingleton<CartService>();
            builder.Services.AddSingleton<PurchaseService>();
            builder.Services.AddSingleton<PurchaseCartService>();

            // Registrar Viewmodels
            // ------------------------------------------------------
            // ViewModels de Alumnos
            builder.Services.AddSingleton<StudentListVM>();
            builder.Services.AddTransient<StudentDetailVM>();
            // ViewModels del Inventario
            builder.Services.AddSingleton<CategoryListVM>();
            builder.Services.AddTransient<CategoryDetailVM>();
            builder.Services.AddSingleton<ProductListVM>(); 
            builder.Services.AddTransient<ProductDetailVM>();
            // ViewModels de la tienda
            builder.Services.AddSingleton<ProductSelectionVM>();
            builder.Services.AddSingleton<CartVM>();
            builder.Services.AddSingleton<SalesHistoryVM>();
            builder.Services.AddTransient<SaleDetailVM>();
            // Viewmodels de Compras
            builder.Services.AddSingleton<PurchaseProductSelectionVM>();
            builder.Services.AddSingleton<PurchaseCartVM>();
            builder.Services.AddSingleton<PurchaseHistoryVM>();
            builder.Services.AddTransient<PurchaseDetailVM>();

            // Registrar Paginas
            // ------------------------------------------------------
            // Paginas de Alumnos
            builder.Services.AddSingleton<StudentListPage>();
            builder.Services.AddTransient<StudentDetailPage>();
            // Paginas del Inventario
            builder.Services.AddSingleton<CategoryListPage>();
            builder.Services.AddTransient<CategoryDetailPage>();
            builder.Services.AddSingleton<ProductListPage>(); 
            builder.Services.AddTransient<ProductDetailPage>();
            // Paginas de la Tienda
            builder.Services.AddSingleton<ProductSelectionPage>();
            builder.Services.AddSingleton<CartPage>();
            builder.Services.AddSingleton<SalesHistoryPage>();
            builder.Services.AddTransient<SaleDetailPage>();
            // Paginas de Compras
            builder.Services.AddSingleton<PurchaseProductSelectionPage>();
            builder.Services.AddSingleton<PurchaseCartPage>();
            builder.Services.AddSingleton<PurchaseHistoryPage>();
            builder.Services.AddTransient<PurchaseDetailPage>();

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
