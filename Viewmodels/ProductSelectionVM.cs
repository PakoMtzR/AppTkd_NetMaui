using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.DTOs;
using MauiApp1.Services;
using System.Collections.ObjectModel;

namespace MauiApp1.Viewmodels
{
    public partial class ProductSelectionVM : ObservableObject
    {
        private readonly InventoryService _inventoryService;
        private readonly CartService _cartService;

        [ObservableProperty]
        private ObservableCollection<ProductDTO> products = new();

        [ObservableProperty]
        private bool isRefreshing;

        public ProductSelectionVM(InventoryService inventoryService, CartService cartService)
        {
            _inventoryService = inventoryService;
            _cartService = cartService;
        }

        [RelayCommand]
        public async Task LoadProducts()
        {
            IsRefreshing = true;
            var list = await _inventoryService.GetAllProducts();
            Products = new ObservableCollection<ProductDTO>(list);
            IsRefreshing = false;
        }

        [RelayCommand]
        public void AddToCart(ProductDTO product)
        {
            if (product.Stock <= 0)
            {
                Shell.Current.DisplayAlert("Sin Stock", "Este producto no tiene existencias disponibles.", "OK");
                return;
            }

            _cartService.AddProduct(product, 1);
            Shell.Current.DisplayAlert("Añadido", $"{product.Description} se añadió al carrito.", "OK");
        }
    }
}
