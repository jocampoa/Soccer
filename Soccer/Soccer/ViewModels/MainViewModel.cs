namespace Soccer.ViewModels
{
    using Soccer.Models;
    using System.Collections.ObjectModel;

    public class MainViewModel : BaseViewModel
    {
        #region Attibrutes
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

        #endregion

        #region Constructor
        public MainViewModel()
        {
            instance = this;
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
                PageName = "SelectTournamentPage",
                Title = "Predictions",
            });

            Menu.Add(new MenuItemViewModel
            {
                Icon = "groups.png",
                PageName = "SelectUserGroupPage",
                Title = "Groups",
            });

            Menu.Add(new MenuItemViewModel
            {
                Icon = "tournaments.png",
                PageName = "SelectTournamentPage",
                Title = "Tournaments",
            });

            Menu.Add(new MenuItemViewModel
            {
                Icon = "myresults.png",
                PageName = "SelectTournamentPage",
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
    }
}
