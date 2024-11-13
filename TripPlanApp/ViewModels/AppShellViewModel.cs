using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripPlanApp.Models;
using TripPlanApp.Views;
using TripPlanApp.Services;

namespace TripPlanApp.ViewModels
{
    public class AppShellViewModel : ViewModelBase
    {
        private User? currentUser;
        private IServiceProvider serviceProvider;

        private TripPlanWebAPIProxy proxy;
        public AppShellViewModel(IServiceProvider serviceProvider, TripPlanWebAPIProxy proxy)
        {
            this.serviceProvider = serviceProvider;
            this.currentUser = ((App)Application.Current).LoggedInUser;
            this.proxy = proxy;
            UserImage = proxy.GetImagesBaseAddress() + ((App)Application.Current).LoggedInUser.ProfileImagePath.Substring(1);
        }
        public AppShellViewModel() { }

        #region user image
        // image url property
        private string userImage;
        public string UserImage
        {
            get => userImage;
            set
            {
                userImage = value;
                OnPropertyChanged("UserImage");
            }
        }
        #endregion user image
        public bool IsManager
        {
            get
            {
                return this.currentUser.IsManager;
            }
        }

        //this command will be used for logout menu item
        public Command LogoutCommand
        {
            get
            {
                return new Command(OnLogout);
            }
        }
        //this method will be trigger upon Logout button click
        public void OnLogout()
        {
            ((App)Application.Current).LoggedInUser = null;

            ((App)Application.Current).MainPage = new NavigationPage(serviceProvider.GetService<LoginView>());
        }
    }
}
