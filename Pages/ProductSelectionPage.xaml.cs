namespace MauiApp1.Pages;
using MauiApp1.Viewmodels;

public partial class ProductSelectionPage : ContentPage
{
    private readonly ProductSelectionVM _viewModel;
    public ProductSelectionPage(ProductSelectionVM viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadProducts();
    }
}
