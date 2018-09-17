namespace Soccer
{
    using Soccer.Helpers;
    using Soccer.Models;
    using Soccer.Services;
    using Soccer.ViewModels;
    using Soccer.Views;
    using System;
    using Xamarin.Forms;

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
                //mainViewModel.RegisterDevice();
                Application.Current.MainPage = new MasterPage();
            }
            else
            {
                this.MainPage = new NavigationPage(new LoginPage());
            }       
		}
        #endregion

        #region Methods
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
