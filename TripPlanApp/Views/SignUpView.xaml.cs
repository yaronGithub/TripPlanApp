using TripPlanApp.ViewModels;

namespace TripPlanApp.Views;

public partial class SignUpView : ContentPage
{
	public SignUpView(SignUpViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}