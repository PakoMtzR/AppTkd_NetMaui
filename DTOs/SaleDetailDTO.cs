using CommunityToolkit.Mvvm.ComponentModel;

namespace MauiApp1.DTOs
{
    public partial class SaleDetailDTO : ObservableObject
    {
        public int IdSaleDetail { get; set; }

        [ObservableProperty]
        private int quantity;

        [ObservableProperty]
        private decimal totalPrice;

        // Llaves Foráneas
        public int IdSale { get; set; }
        public int IdProduct { get; set; }

        // Propiedades informativas para la UI
        public string? ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public int AvailableStock { get; set; } // Stock máximo permitido para este producto
    }
}
