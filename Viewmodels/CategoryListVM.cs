using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.DTOs;
using MauiApp1.Services;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MauiApp1.Pages;
using CommunityToolkit.Maui.Views; // Needed for ShowPopupAsync and Popup

namespace MauiApp1.Viewmodels
{
    public partial class CategoryListVM : ObservableObject, IQueryAttributable
    {
        private readonly InventoryService _inventoryService;
        private readonly IServiceProvider _serviceProvider; // Inyectar IServiceProvider
        private List<CategoryDTO> _allCategories = new();   // Lista de todas las categorias

        [ObservableProperty]
        public ObservableCollection<CategoryDTO> categories;

        [ObservableProperty]
        public bool isBusy;

        [ObservableProperty]
        string searchText;

        public CategoryListVM(InventoryService inventoryService, IServiceProvider serviceProvider) // Recibir IServiceProvider
        {
            _inventoryService = inventoryService;
            _serviceProvider = serviceProvider; // Asignar
            Categories = new ObservableCollection<CategoryDTO>();
        }

        partial void OnSearchTextChanged(string value)
        {
            PerformSearch();
        }

        private void PerformSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                Categories.Clear();
                foreach (var category in _allCategories)
                {
                    Categories.Add(category);
                }
            }
            else
            {
                var filteredCategories = _allCategories
                    .Where(c => (c.Description ?? "").Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                Categories.Clear();
                foreach (var category in filteredCategories)
                {
                    Categories.Add(category);
                }
            }
        }

        [RelayCommand]
        public async Task LoadCategoriesAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                _allCategories = await _inventoryService.GetAllCategories();
                PerformSearch();
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            await LoadCategoriesAsync(); // Cargar siempre las categorías al navegar a la página

            // Opcional: Podrías hacer algo con el SavedCategory aquí si lo necesitas
            if (query.ContainsKey("SavedCategory"))
            {
                // Por ejemplo, notificar al usuario que la categoría se guardó.
            }
            query.Clear();
        }

        private async Task ShowCategoryPopup(int idCategory = 0)
        {
            var categoryDetailVM = _serviceProvider.GetService<CategoryDetailVM>();
            if (categoryDetailVM == null)
            {
                // Manejar error si el VM no puede ser resuelto
                await Shell.Current.DisplayAlert("Error", "No se pudo cargar el detalle de la categoría.", "OK");
                return;
            }

            // Manualmente aplicar query attributes para el VM
            var query = new Dictionary<string, object>();
            if (idCategory > 0)
            {
                query.Add("IdCategory", idCategory.ToString());
            }
            categoryDetailVM.ApplyQueryAttributes(query);

            var categoryDetailPage = new CategoryDetailPage(categoryDetailVM);
            object result = await Shell.Current.CurrentPage.ShowPopupAsync(categoryDetailPage);

            if (result is CategoryDTO savedCategory)
            {
                // Refrescar la lista si una categoría fue guardada
                await LoadCategoriesAsync();
            }
        }

        [RelayCommand]
        private async Task AddNewCategory()
        {
            await ShowCategoryPopup(0); // 0 indica una nueva categoría
        }

        [RelayCommand]
        private async Task EditCategory(CategoryDTO category)
        {
            if (category == null) return;
            await ShowCategoryPopup(category.IdCategory);
        }

        [RelayCommand]
        private async Task DeleteCategory(CategoryDTO category)
        {
            if (category == null) return;

            bool confirm = await Shell.Current.DisplayAlert("Confirmar Eliminación", $"¿Estás seguro de que quieres eliminar la categoría '{category.Description}'?", "Sí", "No");
            if (confirm)
            {
                await _inventoryService.DeleteCategory(category.IdCategory);
                
                // Remove from both lists to keep UI and master list in sync
                var categoryToRemoveFromAll = _allCategories.FirstOrDefault(c => c.IdCategory == category.IdCategory);
                if(categoryToRemoveFromAll != null)
                {
                    _allCategories.Remove(categoryToRemoveFromAll);
                }
                Categories.Remove(category);
            }
        }
    }
}
