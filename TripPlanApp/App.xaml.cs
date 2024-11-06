using TripPlanApp.Models;
using TripPlanApp.Services;
using TripPlanApp.ViewModels;
using TripPlanApp.Views;

namespace TripPlanApp
{
    public partial class App : Application
    {
        public User? LoggedInUser { get; set; }
        private TripPlanWebAPIProxy proxy;
        public App(IServiceProvider serviceProvider, TripPlanWebAPIProxy proxy)
        {
            this.proxy = proxy;
            InitializeComponent();
            LoggedInUser = null;
            MainPage = new NavigationPage(serviceProvider.GetService<LoginView>());
        }
    }
}
