using MauiApp1.Viewmodels;

namespace MauiApp1.Pages;

public partial class SalesHistoryPage : ContentPage
{
    private readonly SalesHistoryVM _viewModel;
    public SalesHistoryPage(SalesHistoryVM viewModel)
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
