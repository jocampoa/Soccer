namespace Soccer.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Soccer.Models;
    using Soccer.Services;
    using System.Windows.Input;
    using Xamarin.Forms;
    using Views;
    using Soccer.Helpers;

    public class LoginViewModel : BaseViewModel
    {
        #region Attributes
        private ApiService apiService;

        private DataService dataService;

        private bool isRunning;

        private bool isEnabled;
        #endregion

        #region Properties
        public string Email { get; set; }

        public string Password { get; set; }

        public bool IsRemembered { get; set; }

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

        #region Constructors
        public LoginViewModel()
        {
            this.apiService = new ApiService();
            this.dataService = new DataService();

            this.IsEnabled = true;
            this.IsRemembered = true;
        }
        #endregion

        #region Commands
        public ICommand ForgotPasswordCommand
        {
            get
            {
                return new RelayCommand(ForgotPassword);
            }
        }

        private async void ForgotPassword()
        {
            MainViewModel.GetInstance().ForgotPassword = new ForgotPasswordViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new ForgotPasswordPage());

        }

        public ICommand LoginFacebookComand
        {
            get
            {
                return new RelayCommand(LoginFacebook);
            }
        }

        private async void LoginFacebook()
        {
            await Application.Current.MainPage.Navigation.PushAsync(
                new LoginFacebookPage());
        }

        public ICommand RegisterCommand
        {
            get
            {
                return new RelayCommand(Register);
            }
        }

        private async void Register()
        {
            MainViewModel.GetInstance().Register = new RegisterViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new RegisterPage());
        }


        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand(Login);
            }
        }

        private async void Login()
        {
            if (string.IsNullOrEmpty(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.EmailValidation,
                    Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(this.Password))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PasswordValidation,
                    Languages.Accept);
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(Languages.Error, connection.Message, Languages.Accept);
                return;
            }

            var apiSecurity = Application.Current.Resources["UrlAPI"].ToString();
            var token = await this.apiService.GetToken(apiSecurity, this.Email, this.Password);

            if (token == null)
            {
                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.SomethingWrong,
                    Languages.Accept);

                return;
            }

            if (string.IsNullOrEmpty(token.AccessToken))
            {
                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.LoginError,
                    Languages.Accept);

                this.Password = string.Empty;

                return;
            }

            var response = await this.apiService.GetUserByEmail(apiSecurity,
                "/api",
                "/Users/GetUserByEmail",
                token.TokenType,
                token.AccessToken,
                this.Email);

            var user = (User)response.Result;

            user.AccessToken = token.AccessToken;
            user.TokenType = token.TokenType;
            user.TokenExpires = token.Expires;
            user.IsRemembered = IsRemembered;
            user.Password = Password;

            dataService.DeleteAllAndInsert(user.FavoriteTeam);
            dataService.DeleteAllAndInsert(user.UserType);
            dataService.DeleteAllAndInsert(user);

            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.CurrentUser = user;
            mainViewModel.RegisterDevice();
            Application.Current.MainPage = new MasterPage();

            this.Email = null;
            this.Password = null;         
        }
        #endregion
    }
}
