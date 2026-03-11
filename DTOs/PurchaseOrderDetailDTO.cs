using CommunityToolkit.Mvvm.ComponentModel;

namespace MauiApp1.DTOs
{
    public partial class PurchaseOrderDetailDTO : ObservableObject
    {
        public int IdPurchaseOrderDetail { get; set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TotalCost))]
        private int quantity;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TotalCost))]
        private decimal unitPriceAtPurchase;

        // Propiedad calculada para la UI
        public decimal TotalCost => Quantity * UnitPriceAtPurchase;

        public int IdPurchaseOrder { get; set; }
        public int IdProduct { get; set; }

        // Propiedades informativas para la UI
        public string? ProductName { get; set; }
        public int CurrentStock { get; set; } 
    }
}
