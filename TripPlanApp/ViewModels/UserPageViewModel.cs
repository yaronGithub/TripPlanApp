using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TripPlanApp.Services;
using TripPlanApp.Views;

namespace TripPlanApp.ViewModels
{
    public class UserPageViewModel : ViewModelBase
    {
        TripPlanWebAPIProxy proxy;
        private IServiceProvider serviceProvider;
        public UserPageViewModel(TripPlanWebAPIProxy proxy, IServiceProvider serviceProvider)
        {
            this.proxy = proxy;
            this.serviceProvider = serviceProvider;
            AddPlanningCommand = new Command(OnAddPlanning);
        }

        public ICommand AddPlanningCommand { get; }

        private async void OnAddPlanning()
        {
            ((App)Application.Current).MainPage.Navigation.PushAsync(serviceProvider.GetService<PlanningPageView>());
        }
    }
}
