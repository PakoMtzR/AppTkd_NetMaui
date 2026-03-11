using CommunityToolkit.Mvvm.ComponentModel;
using MauiApp1.DTOs;
using MauiApp1.Services;
using System.Collections.Generic;

namespace MauiApp1.Viewmodels
{
    public partial class PurchaseDetailVM : ObservableObject, IQueryAttributable
    {
        private readonly PurchaseService _purchaseService;

        [ObservableProperty]
        private PurchaseOrderDTO? order;

        public PurchaseDetailVM(PurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("IdPurchase"))
            {
                if (int.TryParse(query["IdPurchase"].ToString(), out int idPurchase))
                {
                    Order = await _purchaseService.GetPurchaseById(idPurchase);
                }
            }
        }
    }
}
