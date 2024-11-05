using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TripPlanApp.Models;
using TripPlanApp.Services;
using TripPlanApp.Views;

namespace TripPlanApp.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private TripPlanWebAPIProxy proxy;
        private IServiceProvider serviceProvider;
        public LoginViewModel(TripPlanWebAPIProxy proxy, IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.proxy = proxy;
            LoginCommand = new Command(OnLogin);
            RegisterCommand = new Command(OnRegister);
            email = "";
            password = "";
            InServerCall = false;
            errorMsg = "";
        }

        #region Email
        private bool showEmailError;//if needed to show the email error
        public bool ShowEmailError
        {
            get => showEmailError;
            set
            {
                showEmailError = value;
                OnPropertyChanged("ShowEmailError");
            }
        }

        private string email;
        public string Email
        {
            get => email;
            set
            {
                if (email != value)
                {
                    email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        private string emailError;//email error
        public string EmailError
        {
            get => emailError;
            set
            {
                emailError = value;
                OnPropertyChanged("EmailError");
            }
        }
        private void ValidateEmail()//validate if the email fild is not empty longer than 5 chars and contains @
        {
            this.ShowEmailError = (string.IsNullOrEmpty(Email) || Email.Length < 5 || !Email.Contains('@'));
            EmailError = "Invalid Email";
        }
        #endregion Email

        #region Password
        private bool showPasswordError;//if needed to show the password error
        public bool ShowPasswordError
        {
            get => showPasswordError;
            set
            {
                showPasswordError = value;
                OnPropertyChanged("ShowPasswordError");
            }
        }

        private string password;
        public string Password
        {
            get => password;
            set
            {
                if (password != value)
                {
                    password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        private string passwordError;//password error
        public string PasswordError
        {
            get => passwordError;
            set
            {
                passwordError = value;
                OnPropertyChanged("PasswordError");
            }
        }
        private void ValidatePassword()//validates if the password field is full and there are more the 4 characters in it
        {
            this.ShowPasswordError = (string.IsNullOrEmpty(Password) || Password.Length < 4);
            PasswordError = "Invalid Password";
        }
        #endregion Password


        private string errorMsg;
        public string ErrorMsg
        {
            get => errorMsg;
            set
            {
                if (errorMsg != value)
                {
                    errorMsg = value;
                    OnPropertyChanged(nameof(ErrorMsg));
                }
            }
        }


        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }



        private async void OnLogin()
        {
            //Choose the way you want to blobk the page while indicating a server call
            InServerCall = true;
            ErrorMsg = "";
            //Call the server to login
            LoginInfo loginInfo = new LoginInfo { Email = Email, Passwd = Password };
            User? u = await this.proxy.LoginAsync(loginInfo);

            InServerCall = false;

            //Set the application logged in user to be whatever user returned (null or real user)
            ((App)Application.Current).LoggedInUser = u;
            if (u == null)
            {
                ErrorMsg = "Invalid email or password";
            }
            else
            {
                ErrorMsg = "";
                await Application.Current.MainPage.DisplayAlert("Login", $"Login Succeeded!", "ok");//if the check returned not null means that the user exist, shows a message
                
                //Navigate to the main page
                AppShell shell = serviceProvider.GetService<AppShell>();

                
                ((App)Application.Current).MainPage = shell;
                Shell.Current.FlyoutIsPresented = false; //close the flyout
                //Shell.Current.GoToAsync("Tasks"); //Navigate to the Tasks tab page
            }
        }

        private void OnRegister()
        {
            ErrorMsg = "";
            Email = "";
            Password = "";
            // Navigate to the Register View page
            ((App)Application.Current).MainPage.Navigation.PushAsync(serviceProvider.GetService<SignUpView>());
        }
    }
}
