using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripPlanApp.Models;
using TripPlanApp.Services;

namespace TripPlanApp.ViewModels
{
    [QueryProperty(nameof(PlanGroup), "selectedPlanning")]
    public class PlanningPageViewModel : ViewModelBase
    {
        private PlanGroup planning;
        public PlanGroup UserTask
        {
            get => planning;
            set
            {
                if (planning != value)
                {
                    planning = value;
                    OnPropertyChanged(nameof(PlanGroup));
                }
            }
        }
        private TripPlanWebAPIProxy proxy;
        private IServiceProvider serviceProvider;
        public PlanningPageViewModel(TripPlanWebAPIProxy proxy, IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.proxy = proxy;
        }
    }
}
