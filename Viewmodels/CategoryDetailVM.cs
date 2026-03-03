using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.DTOs;
using MauiApp1.Services;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MauiApp1.Viewmodels
{
    [QueryProperty(nameof(CategoryId), "IdCategory")]
    public partial class CategoryDetailVM : ObservableObject, IQueryAttributable
    {
        private readonly InventoryService _inventoryService;

        [ObservableProperty]
        public CategoryDTO category;

        [ObservableProperty]
        public int categoryId;

        [ObservableProperty]
        public string title;

        [ObservableProperty]
        public bool isLoading;

        public Action<object> ClosePopupAction { get; set; } // Acción para cerrar el Popup con un resultado

        public CategoryDetailVM(InventoryService inventoryService)
        {
            _inventoryService = inventoryService;
            Category = new CategoryDTO();
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            CategoryId = 0; 

            if (query.TryGetValue("IdCategory", out object categoryIdParam))
            {
                if (categoryIdParam is string categoryIdString && int.TryParse(categoryIdString, out int id))
                {
                    CategoryId = id;
                }
                else if (categoryIdParam is int categoryIdInt)
                {
                    CategoryId = categoryIdInt;
                }
            }

            await LoadCategoryDetails();
        }

        private async Task LoadCategoryDetails()
        {
            IsLoading = true;
            try
            {
                if (CategoryId > 0) // Existing category
                {
                    Title = "Editar Categoría";
                    var loadedCategory = await _inventoryService.GetCategoryById(CategoryId);
                    if (loadedCategory == null)
                    {
                        Debug.WriteLine($"Category with ID {CategoryId} not found. Initializing new category as fallback.");
                        Category = new CategoryDTO();
                        Title = "Nueva Categoría"; // Revert to new if not found
                    }
                    else
                    {
                        Category = loadedCategory;
                    }
                }
                else // New category
                {
                    Title = "Nueva Categoría";
                    Category = new CategoryDTO(); // Ensure it's a fresh instance
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task SaveCategory()
        {
            if (Category == null)
                return;

            CategoryDTO savedCategory;
            if (Category.IdCategory == 0)
            {
                // New category
                savedCategory = await _inventoryService.AddCategory(Category);
            }
            else
            {
                // Existing category
                await _inventoryService.UpdateCategory(Category);
                savedCategory = Category;
            }

            ClosePopupAction?.Invoke(savedCategory); // Cerrar el popup y pasar la categoría guardada
        }

        [RelayCommand]
        private async Task Cancel()
        {
            ClosePopupAction?.Invoke(null); // Cerrar el popup sin pasar resultado (o un resultado de cancelación)
        }
    }
}
