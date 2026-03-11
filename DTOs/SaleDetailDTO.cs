using CommunityToolkit.Mvvm.ComponentModel;

namespace MauiApp1.DTOs
{
    public partial class SaleDetailDTO : ObservableObject
    {
        public int IdSaleDetail { get; set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TotalPrice))]
        private int quantity;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TotalPrice))]
        private decimal unitPriceAtSale;

        // Propiedad calculada automáticamente
        public decimal TotalPrice => Quantity * UnitPriceAtSale;

        // Llaves Foráneas
        public int IdSale { get; set; }
        public int IdProduct { get; set; }

        // Propiedades informativas para la UI
        public string? ProductName { get; set; }

        // Mantenemos ProductPrice por compatibilidad con los bindings actuales de la UI
        public decimal ProductPrice => UnitPriceAtSale;

        public int AvailableStock { get; set; } // Stock máximo permitido para este producto
    }
}
