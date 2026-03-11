using MauiApp1.Viewmodels;

namespace MauiApp1.Pages;

public partial class PurchaseCartPage : ContentPage
{
	public PurchaseCartPage(PurchaseCartVM viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
