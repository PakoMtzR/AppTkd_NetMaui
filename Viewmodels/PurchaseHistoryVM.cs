using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.DTOs;
using MauiApp1.Services;
using System.Collections.ObjectModel;
using MauiApp1.Pages;

namespace MauiApp1.Viewmodels
{
    public partial class PurchaseHistoryVM : ObservableObject, IQueryAttributable
    {
        private readonly PurchaseService _purchaseService;
        
        // Lista de pedidos realizados
        [ObservableProperty]
        private ObservableCollection<PurchaseOrderDTO> orders = new();

        [ObservableProperty]
        private bool isRefreshing;

        public PurchaseHistoryVM(PurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
        }

        [RelayCommand]
        public async Task LoadHistory()
        {
            if (IsRefreshing) return;

            IsRefreshing = true;
            try
            {
                var list = await _purchaseService.GetPurchaseHistory();
                Orders = new ObservableCollection<PurchaseOrderDTO>(list);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", "No se pudo cargar el historial de compras: " + ex.Message, "OK");
            }
            finally
            {
                IsRefreshing = false;
            }
        }
        
        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            await LoadHistory();
            query.Clear();
        }

        [RelayCommand]
        private async Task GoToDetails(PurchaseOrderDTO order)
        {
            if (order == null) return;
            
            // Navegación a la página de detalles pasando el ID de la orden
            await Shell.Current.GoToAsync($"{nameof(PurchaseDetailPage)}?IdPurchase={order.IdPurchaseOrder}");
        }
    }
}
