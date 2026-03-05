namespace MauiApp1.Pages;

using Microsoft.Maui.Controls;
using MauiApp1.Viewmodels; // Ensure this is correctly referenced

public partial class ProductDetailPage : ContentPage
{
    public ProductDetailPage(ProductDetailVM viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
