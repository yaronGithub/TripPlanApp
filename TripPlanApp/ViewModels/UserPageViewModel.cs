using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TripPlanApp.Models;
using TripPlanApp.Services;
using TripPlanApp.Views;

namespace TripPlanApp.ViewModels
{
    public class UserPageViewModel : ViewModelBase
    {
        private TripPlanWebAPIProxy proxy;
        private IServiceProvider serviceProvider;

        //This is a List containing all of the user plannings
        private List<PlanGroup> userPlannings;
        //THis is a list of only the tasks that should be displayed on screen
        private ObservableCollection<PlanningDisplay> filteredUserPlannings;
        public ObservableCollection<PlanningDisplay> FilteredUserPlannings
        {
            get => filteredUserPlannings;
            set
            {
                filteredUserPlannings = value;
                OnPropertyChanged();
            }
        }

        //Search bar text
        private string searchText;
        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                FilterPlannings();
                OnPropertyChanged();
            }
        }

        private PlanningDisplay selectedObject;
        public PlanningDisplay SelectedObject
        {
            get => selectedObject;
            set
            {
                selectedObject = value;
                if (value != null)
                {
                    // Extract the Id property by from the task object
                    int id = value.PlanId;
                    SelectedPlanning = userPlannings.Where(t => t.PlanId == id).FirstOrDefault();
                }
                else
                    SelectedPlanning = null;
                OnPropertyChanged();
            }
        }

        private PlanGroup selectedPlanning;
        public PlanGroup SelectedPlanning
        {
            get => selectedPlanning;
            set
            {
                selectedPlanning = value;
                OnPlanningSelected(selectedTask);
                OnPropertyChanged();
            }
        }


        public UserPageViewModel(TripPlanWebAPIProxy proxy, IServiceProvider serviceProvider)
        {
            this.proxy = proxy;
            this.serviceProvider = serviceProvider;
            AddPlanningCommand = new Command(OnAddPlanning);
            //this.userPlannings = ((App)Application.Current).LoggedInUser.;
            FilteredUserTasks = new ObservableCollection<TaskDisplay>();
            SearchText = "";
            showDoneTasks = false;
            showNotDoneTasks = true;
            AddNewTaskCommand = new Command(AddNewTask);
            FilterTasks();
        }

        public ICommand AddPlanningCommand { get; }

        private async void OnAddPlanning()
        {
            ((App)Application.Current).MainPage.Navigation.PushAsync(serviceProvider.GetService<PlanningPageView>());
        }
    }

    public class PlanningDisplay
    {
        public int PlanId { get; set; }
        public string GroupDescription { get; set; } = null!;
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
    }
}
