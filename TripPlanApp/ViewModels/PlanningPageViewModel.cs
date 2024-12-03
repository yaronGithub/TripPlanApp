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
        public PlanGroup Planning
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
            SavePlanningCommand = new Command(SavePlanning);
            CancelCommand = new Command(Cancel);
        }


        //Define properties for each field in the task form including error messages and validation logic
        #region PlanningDescription
        private bool showPlanningDescriptionError;
        public bool ShowPlanningDescriptionError
        {
            get => showPlanningDescriptionError;
            set
            {
                showPlanningDescriptionError = value;
                OnPropertyChanged(nameof(ShowPlanningDescriptionError));
            }
        }
        private string planningDescription;
        public string PlanningDescription
        {
            get => planningDescription;
            set
            {
                planningDescription = value;
                ValidatePlanningDescription();
                OnPropertyChanged(nameof(PlanningDescription));
            }
        }
        private string planningDescriptionError;
        public string PlanningDescriptionError
        {
            get => planningDescriptionError;
            set
            {
                planningDescriptionError = value;
                OnPropertyChanged(nameof(PlanningDescriptionError));
            }
        }
        public void ValidatePlanningDescription()
        {
            this.PlanningDescriptionError = "Description is required";
            this.ShowPlanningDescriptionError = string.IsNullOrEmpty(PlanningDescription);
        }

        #endregion PlanningDescription

        #region PlanningGroupName
        private bool showGroupNameError;
        public bool ShowGroupNameError
        {
            get => showGroupNameError;
            set
            {
                showGroupNameError = value;
                OnPropertyChanged(nameof(ShowGroupNameError));
            }
        }
        private string groupName;
        public string GroupName
        {
            get => groupName;
            set 
            { 
                this.groupName = value;
                ValidateGroupName();
                OnPropertyChanged(nameof(GroupName));
            }
        }

        private string planningGroupNameError;
        public string PlanningGroupNameError
        {
            get => planningGroupNameError;
            set
            {
                planningGroupNameError = value;
                OnPropertyChanged(nameof(PlanningGroupNameError));
            }
        }
        public void ValidateGroupName()
        {
            this.PlanningGroupNameError = "Group name is required";
            this.ShowGroupNameError = string.IsNullOrEmpty(GroupName);
        }
        #endregion PlanningGroupName

        #region Places collection
        //Define an ObservableCollection of TaskComment to hold the comments for the task
        private ObservableCollection<PlanPlace> planningPlaces;
        public ObservableCollection<PlanPlace> PlanningPlaces
        {
            get => planningPlaces;
            set
            {
                planningPlaces = value;
                OnPropertyChanged(nameof(PlanningPlaces));
            }
        }
        
        #endregion Places collection

        #region Save Planning
        //define a command to save the task
        public Command SavePlanningCommand { get; set; }
        //define a method to perform the save operation
        private async void SavePlanning()
        {
            //Validate the planning fields
            ValidatePlanningDescription();
            ValidateGroupName();
            //If there are errors, return
            if (ShowPlanningDescriptionError || ShowGroupNameError)
                return;
            InServerCall = true;
            PlanGroup? updatedUserPlanning = new PlanGroup();
            updatedUserPlanning.PlanId = planning.PlanId;
            updatedUserPlanning.GroupName = planning.GroupName;
            updatedUserPlanning.GroupDescription = PlanningDescription;
            updatedUserPlanning.UserId = ((App)Application.Current).LoggedInUser.UserId;
            //If the task is new, add it to the database
            if (planning.PlanId == 0)
            {
                updatedUserPlanning = await proxy.AddPlanning(updatedUserPlanning);
            }
            //If the task is existing, update it in the database
            else
            {
                updatedUserPlanning = await proxy.UpdatePlanning(updatedUserPlanning);
            }

            if (updatedUserPlanning != null)
            {
                //Update the Logged in user with the updated planning!
                if (planning.PlanId != 0)
                {
                    //((App)Application.Current).LoggedInUser.UserTasks.Remove(UserTask);
                }
                //((App)Application.Current).LoggedInUser.UserTasks.Add(updatedUserPlanning);
                //Refresh tasks list 
                UserPageViewModel planningsViewModel = serviceProvider.GetService<UserPageViewModel>();
                planningsViewModel.Refresh();
                //Navigate back to the main page
                await Shell.Current.Navigation.PopAsync();
            }
            else
            {
                InServerCall = false;

                //If we are here, the update failed!
                await Shell.Current.DisplayAlert("Error", "Task update failed. Please try again or Cancel", "Ok");

            }

        }
        #endregion Save Planning
        #region Cancel
        //Define a coomand for Cancel button
        public Command CancelCommand { get; set; }
        //Define a method to perform the cancel operation
        private async void Cancel()
        {
            await Shell.Current.Navigation.PopAsync();
        }
        #endregion Camcel
    }
}
