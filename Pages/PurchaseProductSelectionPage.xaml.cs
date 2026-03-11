using MauiApp1.Viewmodels;
namespace MauiApp1.Pages;

public partial class PurchaseProductSelectionPage : ContentPage
{
	public PurchaseProductSelectionPage(PurchaseProductSelectionVM viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
	protected override void OnAppearing()
	{
		base.OnAppearing();
		if (BindingContext is PurchaseProductSelectionVM viewModel)
		{
			viewModel.LoadProductsCommand.Execute(null);
		}
	}
}
