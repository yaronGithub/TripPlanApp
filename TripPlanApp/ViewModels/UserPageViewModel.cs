using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripPlanApp.Services;

namespace TripPlanApp.ViewModels
{
    public class UserPageViewModel : ViewModelBase
    {
        TripPlanWebAPIProxy proxy;
        public UserPageViewModel(TripPlanWebAPIProxy proxy) 
        {
            this.proxy = proxy;
        }
    }
}
