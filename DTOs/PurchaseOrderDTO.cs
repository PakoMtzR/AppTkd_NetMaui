using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace MauiApp1.DTOs
{
    public partial class PurchaseOrderDTO : ObservableObject
    {
        public int IdPurchaseOrder { get; set; }

        [ObservableProperty]
        private string purchaseOrderNumber = string.Empty;

        [ObservableProperty]
        private DateTime registrationDate;

        [ObservableProperty]
        private decimal orderCost;

        [ObservableProperty]
        private decimal shipmentCost;

        // Propiedad calculada
        public decimal GrandTotal => OrderCost + ShipmentCost;

        // Lista de detalles de la orden
        public List<PurchaseOrderDetailDTO> PurchaseOrderDetails { get; set; } = new();
    }
}
