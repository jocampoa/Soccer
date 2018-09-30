namespace Soccer.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Soccer.Services;
    using System.ComponentModel;
    using System.Windows.Input;
    using System;
    using Plugin.Connectivity;
    using Soccer.Models;
    using Soccer.Views;
    using Android.App;

    public class ForgotPasswordViewModel : BaseViewModel
    {
        #region Attributes
        private ApiService apiService;

        private DataService dataService;

        private DialogService dialogService; // borrar

        private bool isRunning;

        private bool isEnabled;
        #endregion

        #region Properties
        public string Email { get; set; }

        public bool IsRunning
        {
            get { return this.isRunning; }
            set { this.SetValue(ref this.isRunning, value); }
        }

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { this.SetValue(ref this.isEnabled, value); }
        }
        #endregion

        #region Constructor
        public ForgotPasswordViewModel()
        {
            apiService = new ApiService();
            dialogService = new DialogService();
            dataService = new DataService();

            IsEnabled = true;
        }
        #endregion

        #region Commands
        public ICommand SendNewPasswordCommand
        {
            get
            {
                return new RelayCommand(SendNewPassword);
            }
        }

        private async void SendNewPassword()
        {
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await dialogService.ShowMessage("Error", "Check you internet connection.");

                //await Application.Current.MainPage.DisplayAlert(Languages.Error, connection.Message, Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(this.Email))
            {
                await dialogService.ShowMessage("Error", "You must enter a email.");
                return;

                //await Application.Current.MainPage.DisplayAlert(
                //    Languages.Error,
                //    Languages.EmailValidation,
                //    Languages.Accept);
                //return;
            }

            IsRunning = true;
            IsEnabled = false;

            var parameters = dataService.First<Parameter>(false);
            var response = await apiService.PasswordRecovery(
                parameters.UrlAPI, "/api", "/Users/PasswordRecovery", Email);

            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage("Error", "Your password can't be recovered.");
                return;
            }

            await dialogService.ShowMessage("Confirmation", "Your new password has been sent, check the new password in your email.");
            App.Current.MainPage = new LoginPage();
        }

        public ICommand CancelCommand
        {
            get
            { return new RelayCommand(Cancel);
            }
        }

        private void Cancel()
        {
            App.Current.MainPage = new LoginPage();
        }
        #endregion
    }
}
