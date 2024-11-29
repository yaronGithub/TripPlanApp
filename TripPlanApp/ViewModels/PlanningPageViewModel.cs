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

        #region PlanningDueDate
        private bool showPlanningDueDateError;
        public bool ShowPlanningDueDateError
        {
            get => showPlanningDueDateError;
            set
            {
                showPlanningDueDateError = value;
                OnPropertyChanged(nameof(ShowPlanningDueDateError));
            }
        }
        private DateOnly taskDueDate;
        public DateTime TaskDueDate
        {
            get => taskDueDate.ToDateTime(TimeOnly.MinValue);
            set
            {
                taskDueDate = new DateOnly(value.Year, value.Month, value.Day);
                ValidateTaskDueDate();
                OnPropertyChanged(nameof(TaskDueDate));
            }
        }

        private string taskDueDateError;
        public string TaskDueDateError
        {

            get => taskDueDateError;
            set
            {
                taskDueDateError = value;
                OnPropertyChanged(nameof(TaskDueDateError));
            }
        }
        public void ValidateTaskDueDate()
        {
            this.ShowPlanningDueDateError = taskDueDate < DateOnly.FromDateTime(DateTime.Now);
        }
        #endregion PlanningDueDate
        #region PlanningActualDate
        private bool showPlanningActualDateError;
        public bool ShowPlanningActualDateError
        {
            get => showPlanningActualDateError;
            set
            {
                showPlanningActualDateError = value;
                OnPropertyChanged(nameof(ShowPlanningActualDateError));
            }
        }
        private DateOnly? planningActualDate;
        public DateTime? PlanningActualDate
        {
            get
            {
                if (planningActualDate == null)
                    return null;
                else
                {
                    DateOnly val = planningActualDate.Value;
                    return val.ToDateTime(TimeOnly.MinValue);
                }

            }
            set
            {
                if (value == null)
                    planningActualDate = null;
                else
                {
                    DateTime val = value.Value;
                    planningActualDate = new DateOnly(val.Year, val.Month, val.Day);
                }
                ValidatePlanningActualDate();
                OnPropertyChanged(nameof(PlanningActualDate));
            }
        }
        private string taskActualDateError;
        public string TaskActualDateError
        {
            get => taskActualDateError;
            set
            {
                taskActualDateError = value;
                OnPropertyChanged(nameof(TaskActualDateError));
            }
        }
        public void ValidatePlanningActualDate()
        {
            this.ShowPlanningActualDateError = false;

        }
        #endregion PlanningActualDate

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
        //Define a property to hold the new comment text
        private string newComment;
        public string NewComment
        {
            get => newComment;
            set
            {
                newComment = value;
                OnPropertyChanged(nameof(NewComment));
                OnPropertyChanged(nameof(EnableNewCommentButton));
            }
        }
        //define a boolean property to indiczte if the new comment field is not empty
        public bool EnableNewCommentButton
        {
            get => !string.IsNullOrEmpty(NewComment);
        }

        #endregion Places collection

        #region Save Planning
        //define a command to save the task
        public Command SavePlanningCommand { get; set; }
        //define a method to perform the save operation
        private async void SavePlanning()
        {
            //Validate the task fields
            ValidatePlanningDescription();
            ValidateTaskDueDate();
            ValidatePlanningActualDate();
            //If there are errors, return
            if (ShowPlanningDescriptionError || ShowPlanningDueDateError || ShowPlanningActualDateError)
                return;
            InServerCall = true;
            PlanGroup? updatedUserPlanning = new PlanGroup();
            updatedUserPlanning.PlanId = planning.PlanId;
            updatedUserPlanning.GroupDescription = PlanningDescription;
            updatedUserPlanning.TaskDueDate = taskDueDate;
            updatedUserPlanning.TaskActualDate = planningActualDate;
            updatedUserPlanning.TaskComments = PlanningPlaces.ToList();
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
                    ((App)Application.Current).LoggedInUser.UserTasks.Remove(UserTask);
                }
                ((App)Application.Current).LoggedInUser.UserTasks.Add(updatedUserPlanning);
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
