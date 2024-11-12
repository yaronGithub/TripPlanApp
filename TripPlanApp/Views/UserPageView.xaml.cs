using TripPlanApp.ViewModels;

namespace TripPlanApp.Views;

public partial class UserPageView : ContentPage
{
	public UserPageView(UserPageViewModel vm)
	{
		this.BindingContext = vm;
		InitializeComponent();
	}
}