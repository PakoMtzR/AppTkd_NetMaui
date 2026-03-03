using CommunityToolkit.Maui.Views;
using MauiApp1.Viewmodels; // Importar el namespace del ViewModel

namespace MauiApp1.Pages;

public partial class CategoryDetailPage : Popup
{
	public CategoryDetailPage(CategoryDetailVM viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
		// Establecer la acción para que el ViewModel pueda cerrar este Popup
		viewModel.ClosePopupAction = (result) => Close(result);
	}
}
