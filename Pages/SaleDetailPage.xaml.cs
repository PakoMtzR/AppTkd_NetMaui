using MauiApp1.Viewmodels;

namespace MauiApp1.Pages;

public partial class SaleDetailPage : ContentPage
{
	public SaleDetailPage(SaleDetailVM viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
