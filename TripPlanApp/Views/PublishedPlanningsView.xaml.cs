using TripPlanApp.ViewModels;

namespace TripPlanApp.Views;

public partial class PublishedPlanningsView : ContentPage
{
	public PublishedPlanningsView(PublishedPlanningsViewModel vm)
	{
		this.BindingContext = vm;
		InitializeComponent();
	}
}