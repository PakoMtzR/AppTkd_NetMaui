using MauiApp1.Viewmodels;

namespace MauiApp1.Pages;

public partial class StudentDetailPage : ContentPage
{
    public StudentDetailPage(StudentDetailVM viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
