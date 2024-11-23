using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripPlanApp.Models;
using TripPlanApp.Services;
using static Android.App.ActivityManager;

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

        #region Save Planning
        //define a command to save the task
        public Command SavePlanningCommand { get; set; }
        //define a method to perform the save operation
        private async void SavePlanning()
        {
            //Validate the task fields
            ValidateTaskDescription();
            ValidateTaskDueDate();
            ValidateTaskActualDate();
            //If there are errors, return
            if (ShowTaskDescriptionError || ShowTaskDueDateError || ShowTaskActualDateError)
                return;
            InServerCall = true;
            PlanGroup? updatedUserPlanning = new PlanGroup();
            updatedUserPlanning.PlanId = planning.PlanId;
            updatedUserPlanning.GroupDescription = TaskDescription;
            updatedUserPlanning.TaskDueDate = taskDueDate;
            updatedUserPlanning.TaskActualDate = taskActualDate;
            updatedUserPlanning.TaskComments = TaskComments.ToList();
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
                UserPageViewModel tasksViewModel = serviceProvider.GetService<UserPageViewModel>();
                tasksViewModel.Refresh();
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
