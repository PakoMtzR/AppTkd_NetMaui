using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.DTOs;
using MauiApp1.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics; // For Debug.WriteLine

namespace MauiApp1.Viewmodels
{
    [QueryProperty(nameof(ProductId), "IdProduct")]
    public partial class ProductDetailVM : ObservableObject, IQueryAttributable
    {
        private readonly InventoryService _inventoryService;

        [ObservableProperty]
        public ProductDTO product;

        [ObservableProperty]
        public int productId;

        [ObservableProperty]
        public string title;

        [ObservableProperty]
        public bool isLoading;

        [ObservableProperty]
        public List<CategoryDTO> categories;

        [ObservableProperty]
        public List<BrandDTO> brands;

        [ObservableProperty]
        public CategoryDTO selectedCategory;

        [ObservableProperty]
        public BrandDTO selectedBrand;

        public ProductDetailVM(InventoryService inventoryService)
        {
            _inventoryService = inventoryService;
            Product = new ProductDTO { Stock = 0, SalePrice = 0, PurchasePrice = 0 }; // Initialize with default values
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            ProductId = 0; // Reset before processing new query

            if (query.TryGetValue("IdProduct", out object productIdParam))
            {
                if (productIdParam is string productIdString && int.TryParse(productIdString, out int id))
                {
                    ProductId = id;
                }
                else if (productIdParam is int productIdInt)
                {
                    ProductId = productIdInt;
                }
            }

            await LoadProductDetails();
        }

        private async Task LoadPickerData()
        {
            Categories = await _inventoryService.GetAllCategories();
            Brands = await _inventoryService.GetAllBrands();
        }

        private async Task LoadProductDetails()
        {
            IsLoading = true;
            try
            {
                await LoadPickerData(); // Load reference data first

                if (ProductId > 0) // Existing product
                {
                    Title = "Editar Producto";
                    Debug.WriteLine($"Attempting to load product with ID: {ProductId}");
                    var loadedProduct = await _inventoryService.GetProductById(ProductId);

                    if (loadedProduct == null)
                    {
                        Debug.WriteLine($"Product with ID {ProductId} not found. Initializing new product as fallback.");
                        Product = new ProductDTO { Stock = 0, SalePrice = 0, PurchasePrice = 0 };
                        Title = "Nuevo Producto"; // Revert to new if not found
                    }
                    else
                    {
                        Product = loadedProduct;
                        // Set selected picker items based on loaded product's FK IDs
                        SelectedCategory = Categories?.FirstOrDefault(c => c.IdCategory == Product.IdCategory);
                        SelectedBrand = Brands?.FirstOrDefault(b => b.IdBrand == Product.IdBrand);
                    }
                }
                else // New product
                {
                    Title = "Nuevo Producto";
                    Debug.WriteLine("No valid ProductId, initializing new product.");
                    Product = new ProductDTO { Stock = 0, SalePrice = 0, PurchasePrice = 0 };
                    // Ensure pickers are cleared for a new product entry
                    SelectedCategory = null;
                    SelectedBrand = null;
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        partial void OnSelectedCategoryChanged(CategoryDTO value)
        {
            if (Product != null && value != null)
            {
                Product.IdCategory = value.IdCategory;
            }
        }

        partial void OnSelectedBrandChanged(BrandDTO value)
        {
            if (Product != null && value != null)
            {
                Product.IdBrand = value.IdBrand;
            }
        }

        [RelayCommand]
        private async Task SaveProduct()
        {
            if (Product == null)
                return;

            // Ensure FKs are set if selected by the user
            if (SelectedCategory != null) Product.IdCategory = SelectedCategory.IdCategory;
            if (SelectedBrand != null) Product.IdBrand = SelectedBrand.IdBrand;

            ProductDTO savedProduct;
            if (Product.IdProduct == 0)
            {
                // New product
                savedProduct = await _inventoryService.AddProduct(Product);
            }
            else
            {
                // Existing product
                await _inventoryService.UpdateProduct(Product);
                savedProduct = Product; // For updates, the passed DTO is already updated
            }

            // Pass the saved product back to the previous page
            await Shell.Current.GoToAsync("..", new Dictionary<string, object>
            {
                { "SavedProduct", savedProduct }
            });
        }

        [RelayCommand]
        private async Task Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
