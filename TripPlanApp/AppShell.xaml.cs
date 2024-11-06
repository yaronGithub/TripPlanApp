using TripPlanApp.ViewModels;
using TripPlanApp.Views;

namespace TripPlanApp
{
    public partial class AppShell : Shell
    {
        public AppShell(AppShellViewModel vm)
        {
            this.BindingContext = vm;
            InitializeComponent();
            RegisterRoutes();
        }

        void RegisterRoutes()
        {
            Routing.RegisterRoute("publishedPlannings", typeof(PublishedPlanningsView));
            Routing.RegisterRoute("editProfile", typeof(EditProfileView));
        }
    }
}
