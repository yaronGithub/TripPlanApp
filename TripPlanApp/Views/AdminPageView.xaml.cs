using TripPlanApp.ViewModels;

namespace TripPlanApp.Views;

public partial class AdminPageView : ContentPage
{
	public AdminPageView(AdminPageViewModel vm)
	{
		this.BindingContext = vm;
		InitializeComponent();
	}
}