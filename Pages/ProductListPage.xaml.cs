using MauiApp1.Viewmodels; 

namespace MauiApp1.Pages;

public partial class ProductListPage : ContentPage
{
    public ProductListPage(ProductListVM viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ProductListVM viewModel)
        {
            viewModel.LoadProductsCommand.Execute(null);
        }
    }
}
