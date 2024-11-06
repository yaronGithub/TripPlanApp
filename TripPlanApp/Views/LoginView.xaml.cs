using TripPlanApp.ViewModels;

namespace TripPlanApp.Views;

public partial class LoginView : ContentPage
{
	public LoginView(LoginViewModel vm)
	{
		BindingContext = vm;
        InitializeComponent();
	}
}