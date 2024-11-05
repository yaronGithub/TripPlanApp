using TripPlanApp.ViewModels;

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
            //Routing.RegisterRoute("", typeof());
        }
    }
}
