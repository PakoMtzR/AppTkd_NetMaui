using CommunityToolkit.Mvvm.ComponentModel;
using MauiApp1.DTOs;
using MauiApp1.Services;

namespace MauiApp1.Viewmodels
{
    public partial class SaleDetailVM : ObservableObject, IQueryAttributable
    {
        private readonly SalesService _salesService;
        
        [ObservableProperty]
        private SaleDTO? sale;

        public SaleDetailVM(SalesService salesService)
        {
            _salesService = salesService;
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("IdSale"))
            {
                // El ID viene como string desde la URL de navegación
                if (int.TryParse(query["IdSale"].ToString(), out int idSale))
                {
                    Sale = await _salesService.GetSaleById(idSale);
                }
            }
        }
    }
}
