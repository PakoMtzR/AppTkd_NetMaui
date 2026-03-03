using MauiApp1.Viewmodels;

namespace MauiApp1.Pages;

public partial class CategoryListPage : ContentPage
{
    public CategoryListPage(CategoryListVM viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is CategoryListVM viewModel)
        {
            viewModel.LoadCategoriesCommand.Execute(null);
        }
    }
}
