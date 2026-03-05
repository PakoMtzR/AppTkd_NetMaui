using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.DTOs;
using MauiApp1.Services;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MauiApp1.Pages;

namespace MauiApp1.Viewmodels
{
    public partial class ProductListVM : ObservableObject, IQueryAttributable
    {
        private readonly InventoryService _inventoryService;
        private List<ProductDTO> _allProducts = new();

        [ObservableProperty]
        public ObservableCollection<ProductDTO> products;

        [ObservableProperty]
        public bool isBusy;

        [ObservableProperty]
        string searchText;

        [ObservableProperty]
        public ObservableCollection<CategoryDTO> categories;

        [ObservableProperty]
        public CategoryDTO selectedCategory;

        public ProductListVM(InventoryService inventoryService)
        {
            _inventoryService = inventoryService;
            Products = new ObservableCollection<ProductDTO>();
            Categories = new ObservableCollection<CategoryDTO>(); // Initialize categories
        }

        partial void OnSearchTextChanged(string value)
        {
            PerformSearch();
        }

        partial void OnSelectedCategoryChanged(CategoryDTO value)
        {
            PerformSearch();
        }

        private void PerformSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchText) && (SelectedCategory == null || SelectedCategory.IdCategory == 0))
            {
                Products.Clear();
                foreach (var product in _allProducts)
                {
                    Products.Add(product);
                }
            }
            else
            {
                var filteredProducts = _allProducts
                    .Where(p => (string.IsNullOrWhiteSpace(SearchText) ||
                                (p.Description ?? "").Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                                (p.CategoryDescription ?? "").Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                                (p.BrandName ?? "").Contains(SearchText, StringComparison.OrdinalIgnoreCase)))
                    .Where(p => SelectedCategory == null || SelectedCategory.IdCategory == 0 || p.IdCategory == SelectedCategory.IdCategory) // Add category filter
                    .ToList();

                Products.Clear();
                foreach (var product in filteredProducts)
                {
                    Products.Add(product);
                }
            }
        }

        [RelayCommand]
        public async Task LoadProductsAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                _allProducts = await _inventoryService.GetAllProducts();
                
                // Load categories for the filter picker
                Categories = new ObservableCollection<CategoryDTO>(await _inventoryService.GetAllCategories());
                Categories.Insert(0, new CategoryDTO { IdCategory = 0, Description = "Todas las Categorías" }); // Add an "All" option
                SelectedCategory = Categories.FirstOrDefault(); // Select "All" by default

                PerformSearch();
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            await LoadProductsAsync();
            query.Clear();
        }

        [RelayCommand]
        private async Task AddNewProduct()
        {
            await Shell.Current.GoToAsync($"{nameof(ProductDetailPage)}?IdProduct=0");
        }

        [RelayCommand]
        private async Task EditProduct(ProductDTO product)
        {
            if (product == null) return;
            await Shell.Current.GoToAsync($"{nameof(ProductDetailPage)}?IdProduct={product.IdProduct}");
        }

        [RelayCommand]
        private async Task DeleteProduct(ProductDTO product)
        {
            if (product == null) return;

            bool confirm = await Shell.Current.DisplayAlert("Confirmar Eliminación", $"¿Estás seguro de que quieres eliminar el producto '{product.Description}'?", "Sí", "No");
            if (confirm)
            {
                await _inventoryService.DeleteProduct(product.IdProduct);
                
                var productToRemoveFromAll = _allProducts.FirstOrDefault(p => p.IdProduct == product.IdProduct);
                if(productToRemoveFromAll != null)
                {
                    _allProducts.Remove(productToRemoveFromAll);
                }
                Products.Remove(product);
            }
        }
    }
}
