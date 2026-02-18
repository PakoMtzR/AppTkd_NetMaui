using MauiApp1.Viewmodels;

namespace MauiApp1.Pages;

public partial class StudentListPage : ContentPage
{
	public StudentListPage(StudentListVM viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}