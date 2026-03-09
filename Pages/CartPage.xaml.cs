namespace MauiApp1.Pages;
using MauiApp1.Viewmodels;

public partial class CartPage : ContentPage
{
    public CartPage(CartVM viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
