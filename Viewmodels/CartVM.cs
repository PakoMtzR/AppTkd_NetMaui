using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.DTOs;
using MauiApp1.Services;
using System.Collections.ObjectModel;
using System.Linq;

namespace MauiApp1.Viewmodels
{
    public partial class CartVM : ObservableObject
    {
        private readonly CartService _cartService;
        private readonly SalesService _salesService;

        [ObservableProperty]
        private ObservableCollection<SaleDetailDTO> items;

        [ObservableProperty]
        private decimal total;

        [ObservableProperty]
        private string? clientName;

        public CartVM(CartService cartService, SalesService salesService)
        {
            _cartService = cartService;
            _salesService = salesService;
            Items = _cartService.CartItems;
            UpdateTotal();

            // Suscribirse a cambios en la colección para actualizar el total
            Items.CollectionChanged += (s, e) => UpdateTotal();
        }

        private void UpdateTotal()
        {
            Total = _cartService.TotalCart;
        }

        [RelayCommand]
        public void RemoveItem(SaleDetailDTO item)
        {
            _cartService.RemoveItem(item);
        }

        [RelayCommand]
        public async Task FinalizeSale()
        {
            if (Items.Count == 0)
            {
                await Shell.Current.DisplayAlert("Error", "El carrito está vacío.", "OK");
                return;
            }

            var saleNumber = await _salesService.GenerateSaleNumber();
            var saleDto = new SaleDTO
            {
                SaleNumber = saleNumber,
                ClientName = ClientName,
                TotalPrice = Total,
                SaleDetails = Items.ToList()
            };

            var success = await _salesService.SaveSale(saleDto);

            if (success)
            {
                await Shell.Current.DisplayAlert("Venta Exitosa", $"Venta registrada con el folio: {saleNumber}", "OK");
                _cartService.ClearCart();
                ClientName = string.Empty;
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "No se pudo completar la venta. Verifique el stock.", "OK");
            }
        }
    }
}
