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
        private List<ProductDTO> _allProducts = new();  // Lista completa de productos cargados desde el servicio, sin filtrar

        // Lista de productos filtrados para mostrar en la UI
        [ObservableProperty]
        private ObservableCollection<ProductDTO> products = new();

        // Para indicar que se están cargando los productos
        [ObservableProperty]
        private bool isRefreshing;

        // Texto de búsqueda para filtrar productos por descripción
        [ObservableProperty]
        private string searchText;

        // Lista de categorías para el filtro
        [ObservableProperty]
        private ObservableCollection<CategoryDTO> categories = new();

        // Categoría seleccionada para filtrar productos
        [ObservableProperty]
        private CategoryDTO selectedCategory;

        public ProductSelectionVM(InventoryService inventoryService, CartService cartService)
        {
            _inventoryService = inventoryService;   // Inyectamos el servicio de inventario para cargar productos y categorías
            _cartService = cartService;             // Inyectamos el servicio de carrito para añadir productos al carrito
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

        [RelayCommand]
        public void AddToCart(ProductDTO product)
        {
            if (product.Stock <= 0)
            {
                Shell.Current.DisplayAlert("Sin Stock", "Este producto no tiene existencias disponibles.", "OK");
                return;
            }

            _cartService.AddProduct(product);
            Shell.Current.DisplayAlert("Añadido", $"{product.Description} se añadió al carrito.", "OK");
        }
    }
}
