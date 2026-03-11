using MauiApp1.Viewmodels;

namespace MauiApp1.Pages;

public partial class PurchaseHistoryPage : ContentPage
{
	private readonly PurchaseHistoryVM _viewModel;
	public PurchaseHistoryPage(PurchaseHistoryVM viewModel)
	{
		InitializeComponent();
		BindingContext = _viewModel = viewModel;
	}
	
	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await _viewModel.LoadHistory();
	}
}
