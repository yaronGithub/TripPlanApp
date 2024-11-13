using TripPlanApp.ViewModels;

namespace TripPlanApp.Views;

public partial class PlanningPageView : ContentPage
{
	public PlanningPageView(PlanningPageViewModel vm)
	{
		this.BindingContext = vm;
		InitializeComponent();
	}
}