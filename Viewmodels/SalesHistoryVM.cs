using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.DTOs;
using MauiApp1.Services;
using System.Collections.ObjectModel;

namespace MauiApp1.Viewmodels
{
    public partial class SalesHistoryVM : ObservableObject
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
    }
}
