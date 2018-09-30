namespace Soccer
{
    using Helpers;
    using Models;
    using Soccer.Services;
    using Soccer.ViewModels;
    using Soccer.Views;
    using System;
    using Xamarin.Forms;
    using System.Threading.Tasks;

    public partial class App : Application
	{
        #region Attributes
        private DataService dataService;
        #endregion

        #region Properties
        public static NavigationPage Navigator { get; internal set; }

        public static MasterPage Master { get; internal set; }
        #endregion

        #region Constructors
        public App ()
		{
			InitializeComponent();

            var dataService = new DataService();

            this.LoadParameters();

            var user = dataService.First<User>(false);
            if (user != null && user.IsRemembered && user.TokenExpires > DateTime.Now)
            {
                var favoriteTeam = dataService.Find<Team>(user.FavoriteTeamId, false);
                user.FavoriteTeam = favoriteTeam;
                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.CurrentUser = user;
                mainViewModel.RegisterDevice();
                Application.Current.MainPage = new MasterPage();
            }
            else
            {
                this.MainPage = new NavigationPage(new LoginPage());
            }       
		}
        #endregion

        #region Methods
        public static Action HideLoginView
        {
            get
            {
                return new Action(() => Application.Current.MainPage =
                                  new NavigationPage(new LoginPage()));
            }
        }

        public static async Task NavigateToProfile(FacebookResponse profile)
        {
            if (profile == null)
            {
                Application.Current.MainPage = new NavigationPage(new LoginPage());
                return;
            }

            var apiService = new ApiService();
            var dataService = new DataService();
            var dialogService = new DialogService();

            var parameters = dataService.First<Parameter>(false);
            var token = await apiService.LoginFacebook(
                parameters.UrlAPI,
                "/api",
                "/Users/LoginFacebook",
                profile);

            if (token == null)
            {
                Application.Current.MainPage = new NavigationPage(new LoginPage());
                return;
            }

            var response = await apiService.GetUserByEmail(
                parameters.UrlAPI,
                "/api",
                "/Users/GetUserByEmail",
                token.TokenType,
                token.AccessToken,
                token.UserName);

            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage("Error", "Problem ocurred retrieving user information, try latter.");
                return;
            }

            var user = (User)response.Result;
            user.AccessToken = token.AccessToken;
            user.TokenType = token.TokenType;
            user.TokenExpires = token.Expires;
            user.IsRemembered = true;
            user.Password = profile.Id;
            dataService.DeleteAllAndInsert(user.FavoriteTeam);
            dataService.DeleteAllAndInsert(user.UserType);
            dataService.DeleteAllAndInsert(user);

            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.CurrentUser = user;
            mainViewModel.RegisterDevice();
            Application.Current.MainPage = new MasterPage();
        }

        private void LoadParameters()
        {
            var urlAPI = Application.Current.Resources["UrlAPI"].ToString();
            var urlBackend = Application.Current.Resources["URLBackend"].ToString();
            var parameter = dataService.First<Parameter>(false);
            if (parameter == null)
            {
                parameter = new Parameter
                {
                    UrlAPI = urlAPI,
                    URLBackend = urlBackend,
                };

                dataService.Insert(parameter);
            }
            else
            {
                parameter.UrlAPI = urlAPI;
                parameter.URLBackend = urlBackend;
                dataService.Update(parameter);
            }
        }

        protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
        #endregion
    }
}
