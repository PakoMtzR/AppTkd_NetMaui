using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.DTOs;
using MauiApp1.Services;
using System.Collections.ObjectModel;
using MauiApp1.Pages;

namespace MauiApp1.Viewmodels
{
    public partial class SalesHistoryVM : ObservableObject, IQueryAttributable
    {
        private readonly SalesService _salesService;

        [ObservableProperty]
        private ObservableCollection<SaleDTO> sales = new();

        [ObservableProperty]
        private bool isRefreshing;

        public SalesHistoryVM(SalesService salesService)
        {
            _salesService = salesService;
        }

        [RelayCommand]
        public async Task LoadHistory()
        {
            IsRefreshing = true;
            var list = await _salesService.GetSalesHistory();
            Sales = new ObservableCollection<SaleDTO>(list);
            IsRefreshing = false;
        }
        
        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            await LoadHistory();
            query.Clear();
        }

        [RelayCommand]
        private async Task GoToDetails(SaleDTO sale)
        {
            if (sale == null) return;
            await Shell.Current.GoToAsync($"{nameof(SaleDetailPage)}?IdSale={sale.IdSale}");
        }
    }
}
