using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.DTOs;
using MauiApp1.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MauiApp1.Viewmodels
{
    public partial class PurchaseProductSelectionVM : ObservableObject
    {
        private readonly InventoryService _inventoryService;
        private readonly PurchaseCartService _cartService;
        
        // Lista completa de productos cargados desde el servicio, sin filtrar
        private List<ProductDTO> _allProducts = new();  

        // Lista de productos filtrados para mostrar en la UI
        [ObservableProperty]
        private ObservableCollection<ProductDTO> products = new();
        
        // Para indicar que se están cargando los productos
        [ObservableProperty]
        private bool isRefreshing;
        
        // Lista de categorías para el filtro
        [ObservableProperty]
        private ObservableCollection<CategoryDTO> categories = new();
        
        // Categoría seleccionada para filtrar productos
        [ObservableProperty]
        private CategoryDTO selectedCategory;
        
        // Texto de búsqueda para filtrar productos por descripción
        [ObservableProperty]
        private string searchText;

        public PurchaseProductSelectionVM(InventoryService inventoryService, PurchaseCartService cartService)
        {
            _inventoryService = inventoryService;
            _cartService = cartService;
        }
        
        // Estos métodos se ejecutan cada vez que cambian el texto de búsqueda o la categoría seleccionada, para actualizar la lista de productos mostrados
        partial void OnSearchTextChanged(string value) => PerformSearch();
        partial void OnSelectedCategoryChanged(CategoryDTO value) => PerformSearch();

        private void PerformSearch()
        {
            var filtered = _allProducts.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                filtered = filtered.Where(p => p.Description.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
            }

            if (SelectedCategory != null && SelectedCategory.IdCategory != 0)
            {
                filtered = filtered.Where(p => p.IdCategory == SelectedCategory.IdCategory);
            }

            Products = new ObservableCollection<ProductDTO>(filtered);
        }

        [RelayCommand]
        public async Task LoadProducts()
        {
            IsRefreshing = true;
            _allProducts = await _inventoryService.GetAllProducts();
            var categoriesList = await _inventoryService.GetAllCategories();
            categoriesList.Insert(0, new CategoryDTO { IdCategory = 0, Description = "Todas las Categorías" });
            Categories = new ObservableCollection<CategoryDTO>(categoriesList);
            SelectedCategory = Categories.FirstOrDefault();
            PerformSearch();
            IsRefreshing = false;
        }

        // [RelayCommand]
        // public async Task FilterProducts()
        // {
        //     var list = await _inventoryService.GetAllProducts();
        //
        //     var filtered = list.Where(p => 
        //         (SelectedCategory == null || SelectedCategory.IdCategory == 0 || p.IdCategory == SelectedCategory.IdCategory) &&
        //         (string.IsNullOrEmpty(SearchText) || p.Description.ToLower().Contains(SearchText.ToLower()))
        //     ).ToList();
        //
        //     Products = new ObservableCollection<ProductDTO>(filtered);
        // }

        [RelayCommand]
        public async Task AddToCart(ProductDTO product)
        {
            _cartService.AddProduct(product);
            await Shell.Current.DisplayAlert("Agregado", $"{product.Description} se añadió a la orden de compra.", "OK");
        }

        // [RelayCommand]
        // public async Task GoToCart()
        // {
        //     await Shell.Current.GoToAsync("PurchaseCartPage");
        // }
    }
}
