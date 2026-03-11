using MauiApp1.Viewmodels;

namespace MauiApp1.Pages;

public partial class PurchaseDetailPage : ContentPage
{
	public PurchaseDetailPage(PurchaseDetailVM viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
