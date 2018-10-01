namespace Soccer.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Soccer.Helpers;
    using Soccer.Models;
    using Soccer.Services;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class ChangePasswordViewModel : BaseViewModel
    {
        #region Attributes
        private ApiService apiService;

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
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PasswordValidation,
                    Languages.Accept);
                return;
            }

            var mainViewModel = MainViewModel.GetInstance();

            if (mainViewModel.CurrentUser.Password != CurrentPassword)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.ConfirmValidation,
                    Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(NewPassword))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.NewPasswordValidation,
                    Languages.Accept);
                return;
            }

            if (NewPassword == CurrentPassword)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.ValidationPassword,
                    Languages.Accept);
                return;
            }

            if (NewPassword.Length < 6)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.ValidationPassword2,
                    Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(ConfirmPassword))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.ValidationConfirmPassword,
                    Languages.Accept);
                return;
            }

            if (NewPassword != ConfirmPassword)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.ConfirmValidation2,
                    Languages.Accept);
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
                await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            await Application.Current.MainPage.DisplayAlert(
                Languages.Confirm,
                Languages.PasswordChanged,
               Languages.Accept);
            return;
        }
        #endregion
    }
}
