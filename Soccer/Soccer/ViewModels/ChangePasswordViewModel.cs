namespace Soccer.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Soccer.Models;
    using Soccer.Services;
    using System.Windows.Input;

    public class ChangePasswordViewModel : BaseViewModel
    {
        #region Attributes
        private ApiService apiService;
        private DialogService dialogService;
        private DataService dataService;
        private bool isRunning;
        private bool isEnabled;
        #endregion

        #region Properties
        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { SetValue(ref this.isEnabled, value); }
        }

        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }

        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }
        #endregion

        #region Constructor
        public ChangePasswordViewModel()
        {
            apiService = new ApiService();
            dialogService = new DialogService();
            dataService = new DataService();

            IsEnabled = true;
        }
        #endregion

        #region Commands
        public ICommand ChangePasswordCommand
        {
            get
            {
                return new RelayCommand(ChangePassword);
            }
        }

        private async void ChangePassword()
        {
            if (string.IsNullOrEmpty(CurrentPassword))
            {
                await dialogService.ShowMessage("Error", "You must enter the current password.");
                return;
            }

            var mainViewModel = MainViewModel.GetInstance();

            if (mainViewModel.CurrentUser.Password != CurrentPassword)
            {
                await dialogService.ShowMessage("Error", "The current password doesn´t  match.");
                return;
            }

            if (string.IsNullOrEmpty(NewPassword))
            {
                await dialogService.ShowMessage("Error", "You must enter a new password.");
                return;
            }

            if (NewPassword == CurrentPassword)
            {
                await dialogService.ShowMessage("Error", "The current password is equal to new password.");
                return;
            }

            if (NewPassword.Length < 6)
            {
                await dialogService.ShowMessage("Error", "The new password must have at least 6 characters.");
                return;
            }

            if (string.IsNullOrEmpty(ConfirmPassword))
            {
                await dialogService.ShowMessage("Error", "You must enter a password confirm.");
                return;
            }

            if (NewPassword != ConfirmPassword)
            {
                await dialogService.ShowMessage("Error", "The new password and confirm does not match.");
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            var parameters = dataService.First<Parameter>(false);
            var user = dataService.First<User>(false);

            var request = new ChangePasswordRequest
            {
                CurrentPassword = CurrentPassword,
                Email = user.Email,
                NewPassword = NewPassword,
            };

            var response = await apiService.ChangePassword(parameters.UrlAPI, "/api", "/Users/ChangePassword",
                user.TokenType, user.AccessToken, request);

            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage("Error", response.Message);
                return;
            }

            await dialogService.ShowMessage("Confirm", "Your password has been changed successfully.");
            await App.Navigator.PopAsync();
        }
        #endregion
    }
}
