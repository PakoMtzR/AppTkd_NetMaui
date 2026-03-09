using MauiApp1.Viewmodels;

namespace MauiApp1.Pages;

public partial class ProductDetailPage : ContentPage
{
    public ProductDetailPage(ProductDetailVM viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
