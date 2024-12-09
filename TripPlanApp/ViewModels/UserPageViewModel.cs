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

        //This is a List containing all of the user plannings
        private List<PlanGroup>? userPlannings;
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
                //FilterPlannings();
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
                    SelectedPlanning = userPlannings.FirstOrDefault(t => t.PlanId == id);
                }
                else
                    SelectedPlanning = null;
                OnPropertyChanged();
            }
        }

        private PlanGroup? selectedPlanning;
        public PlanGroup? SelectedPlanning
        {
            get => selectedPlanning;
            set
            {
                selectedPlanning = value;
                OnPlanningSelected(selectedPlanning);
                OnPropertyChanged();
            }
        }


        public UserPageViewModel(TripPlanWebAPIProxy proxy)
        {
            this.proxy = proxy;
            AddPlanningCommand = new Command(OnAddPlanning);

            SearchText = "";
            FilteredUserPlannings = new ObservableCollection<PlanningDisplay>();

            // Initialize userPlannings asynchronously
            InitializeUserPlannings();
        }

        //This is a public method that should be called when the page needs to be refreshed
        public void Refresh()
        {
            
        }

        private async void InitializeUserPlannings()
        {
            try
            {
                InServerCall = true;
                // Get all user plannings
                userPlannings = await proxy.GetAllPlannings(((App)Application.Current).LoggedInUser.Email);

                // Optionally, update the filtered list after retrieving the data
                FilteredUserPlannings = new ObservableCollection<PlanningDisplay>(
                    userPlannings.Select(p => new PlanningDisplay
                    {
                        PlanId = p.PlanId,
                        GroupName = p.GroupName,
                        GroupDescription = p.GroupDescription,
                        StartDate = p.StartDate,
                        EndDate = p.EndDate
                    }));
                InServerCall = false;
            }
            catch (Exception ex)
            {
                // Handle any errors during initialization
                Console.WriteLine($"Error fetching plannings: {ex.Message}");
            }
        }

        //this method will be triggered upon SelectedPlanning property change
        private async void OnPlanningSelected(PlanGroup planning)
        {
            if (planning != null)
            {
                var navParam = new Dictionary<string, object>
                {
                    { "selectedPlanning", planning }
                };
                //Navigate to the task details page
                await Shell.Current.GoToAsync("planningPage", navParam);
                SelectedObject = null;
            }
        }


        public ICommand AddPlanningCommand { get; }

        private async void OnAddPlanning()
        {
            PlanGroup planning = new PlanGroup()
            {
                PlanId = 0,
                GroupName = "",
                GroupDescription = "",
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = null,
                UserId = ((App)Application.Current).LoggedInUser.UserId,
                Reviews = new List<Review>(),
                Pictures = new List<Picture>(),
                PlanPlaces = new List<PlanPlace>(),
                UsersNavigation = new List<User>(),

            };
            var navParam = new Dictionary<string, object>
                {
                    { "selectedPlanning", planning }
                };
            await Shell.Current.GoToAsync("planningPage", navParam);
        }
    }

    public class PlanningDisplay
    {
        public int PlanId { get; set; }
        public string GroupName { get; set; } = null!;
        public string GroupDescription { get; set; } = null!;
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
    }
}
