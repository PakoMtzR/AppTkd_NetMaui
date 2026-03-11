using MauiApp1.Pages;

namespace MauiApp1
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            
            // Rutas para alumnos
            Routing.RegisterRoute(nameof(StudentListPage), typeof(StudentListPage));
            Routing.RegisterRoute(nameof(StudentDetailPage), typeof(StudentDetailPage));
            // Rutas para inventario
            Routing.RegisterRoute(nameof(CategoryListPage), typeof(CategoryListPage));
            Routing.RegisterRoute(nameof(CategoryDetailPage), typeof(CategoryDetailPage));
            Routing.RegisterRoute(nameof(ProductListPage), typeof(ProductListPage));
            Routing.RegisterRoute(nameof(ProductDetailPage), typeof(ProductDetailPage));
            // Rutas para la tienda
            Routing.RegisterRoute(nameof(SalesHistoryPage), typeof(SalesHistoryPage));
            Routing.RegisterRoute(nameof(SaleDetailPage), typeof(SaleDetailPage));
            // Rutas para los pedidos
            Routing.RegisterRoute(nameof(PurchaseProductSelectionPage), typeof(PurchaseProductSelectionPage));
            Routing.RegisterRoute(nameof(PurchaseCartPage), typeof(PurchaseCartPage));
            Routing.RegisterRoute(nameof(PurchaseHistoryPage), typeof(PurchaseHistoryPage));
            Routing.RegisterRoute(nameof(PurchaseDetailPage), typeof(PurchaseDetailPage));
        }
    }
}
