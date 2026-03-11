using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.DTOs;
using MauiApp1.Services;
using System.Collections.ObjectModel;
using System.Linq;

namespace MauiApp1.Viewmodels
{
    public partial class PurchaseCartVM : ObservableObject
    {
        private readonly PurchaseCartService _cartService;
        private readonly PurchaseService _purchaseService;
        
        // Lista de productos en el carrito
        [ObservableProperty]
        private ObservableCollection<PurchaseOrderDetailDTO> items;
        
        // Precio total del carrito
        [ObservableProperty]
        private decimal totalOrder;

        [ObservableProperty]
        private decimal shipmentCost;

        [ObservableProperty]
        private decimal grandTotal;

        public PurchaseCartVM(PurchaseCartService cartService, PurchaseService purchaseService)
        {
            _cartService = cartService;
            _purchaseService = purchaseService;
            
            // Enlazar la colección del servicio directamente a la propiedad Items para reflejar cambios en la UI
            Items = _cartService.CartItems;
            UpdateTotals();

            // Suscribirse a cambios en la colección para actualizar el total
            Items.CollectionChanged += (s, e) => UpdateTotals();
        }

        // Llamar manualmente cuando cambia el costo de envío o cantidades
        [RelayCommand]
        private void UpdateTotals()
        {
            TotalOrder = _cartService.TotalOrder;
            GrandTotal = TotalOrder + ShipmentCost;
        }

        partial void OnShipmentCostChanged(decimal value)
        {
            UpdateTotals();
        }

        [RelayCommand]
        public void IncreaseQuantity(PurchaseOrderDetailDTO item)
        {
            _cartService.IncrementItem(item);
            UpdateTotals();
        }

        [RelayCommand]
        public void DecreaseQuantity(PurchaseOrderDetailDTO item)
        {
            _cartService.DecrementItem(item);
            UpdateTotals();
        }

        [RelayCommand]
        public void RemoveItem(PurchaseOrderDetailDTO item)
        {
            _cartService.RemoveItem(item);
        }

        [RelayCommand]
        public async Task FinalizePurchase()
        {
            if (Items.Count == 0)
            {
                await Shell.Current.DisplayAlert("Error", "La orden está vacía.", "OK");
                return;
            }

            var orderNumber = _purchaseService.GeneratePurchaseOrderNumber();
            var orderDto = new PurchaseOrderDTO
            {
                PurchaseOrderNumber = orderNumber,
                OrderCost = TotalOrder,
                ShipmentCost = ShipmentCost,
                PurchaseOrderDetails = Items.ToList()
            };

            var success = await _purchaseService.SavePurchase(orderDto);

            if (success)
            {
                await Shell.Current.DisplayAlert("Éxito", $"Orden de compra registrada con folio: {orderNumber}. El stock ha sido actualizado.", "OK");
                _cartService.ClearCart();
                ShipmentCost = 0;
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "No se pudo registrar la compra.", "OK");
            }
        }
    }
}
