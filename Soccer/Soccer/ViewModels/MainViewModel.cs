﻿namespace Soccer.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Soccer.Models;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using Soccer.Services;

    public class MainViewModel : BaseViewModel
    {
        #region Attibrutes
        private ApiService apiService;
        private DataService dataService;
        private User currentUser;
        #endregion

        #region Properties
        public LoginViewModel Login { get; set; }

        public ObservableCollection<MenuItemViewModel> Menu { get; set; }

        public User CurrentUser
        {
            get { return this.currentUser; }
            set { SetValue(ref this.currentUser, value); }
        }

        public TokenResponse Token { get; set; }

        public ConfigViewModel Config { get; set; }

        public TournamentViewModel Tournament { get; set; }

        public MatchViewModel Match { get; set; }

        public GroupViewModel Group { get; set; }

        public UserGroupViewModel UserGroup { get; set; }

        public PredictionViewModel Prediction { get; set; }

        public PositionsViewModel Positions { get; set; }

        public MyResultsViewModel MyResults { get; set; }

        public RegisterViewModel Register { get; set; }

        public ForgotPasswordViewModel ForgotPassword { get; set; }

        public ChangePasswordViewModel ChangePassword { get; set; }

        public UsersGroupViewModel UsersGroup { get; set; }
        #endregion

        #region Constructor
        public MainViewModel()
        {
            instance = this;

            apiService = new ApiService();
            dataService = new DataService();

            Login = new LoginViewModel();
            this.LoadMenu();
        }
        #endregion

        #region Singleton
        private static MainViewModel instance;

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                instance = new MainViewModel();
            }

            return instance;
        }
        #endregion

        #region Methods
        //public void RegisterDevice()
        //{
        //    var register = DependencyService.Get<IRegisterDevice>();
        //    register.RegisterDevice();
        //}

        private void LoadMenu()
        {
            Menu = new ObservableCollection<MenuItemViewModel>();

            Menu.Add(new MenuItemViewModel
            {
                Icon = "predictions.png",
                PageName = "TournamentPage",
                Title = "Predictions",
            });

            Menu.Add(new MenuItemViewModel
            {
                Icon = "groups.png",
                PageName = "UserGroupPage",
                Title = "Groups",
            });

            Menu.Add(new MenuItemViewModel
            {
                Icon = "tournaments.png",
                PageName = "TournamentPage",
                Title = "Tournaments",
            });

            Menu.Add(new MenuItemViewModel
            {
                Icon = "myresults.png",
                PageName = "TournamentPage",
                Title = "My Results",
            });

            Menu.Add(new MenuItemViewModel
            {
                Icon = "config.png",
                PageName = "ConfigPage",
                Title = "Config",
            });

            Menu.Add(new MenuItemViewModel
            {
                Icon = "logut.png",
                PageName = "LoginPage",
                Title = "Logut",
            });
        }
        #endregion

        #region Commands
        public ICommand RefreshPointsCommand
        {
            get
            {
                return new RelayCommand(RefreshPoints);
            }
        }

        private async void RefreshPoints()
        {
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {              
                return; // Do nichim
            }

            var parameters = dataService.First<Parameter>(false);
            var user = dataService.First<User>(false);
            var response = await apiService.GetPoints(
                parameters.UrlAPI, "/api", "/Users/GetPoints",
                user.TokenType, user.AccessToken, user.UserId);

            if (!response.IsSuccess)
            {
                return; // Do nichim
            }

            var point = (Point)response.Result;
            if (CurrentUser.Points != point.Points)
            {
                CurrentUser.Points = point.Points;
                dataService.Update(CurrentUser);
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentUser"));
            }
        }
        #endregion
    }
}
