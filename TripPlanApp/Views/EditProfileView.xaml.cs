using TripPlanApp.ViewModels;

namespace TripPlanApp.Views;

public partial class EditProfileView : ContentPage
{
	public EditProfileView(EditProfileViewModel vm)
	{
		this.BindingContext = vm;
		InitializeComponent();
	}
}