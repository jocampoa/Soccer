namespace Soccer.ViewModels
{
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Views;
    using Xamarin.Forms;
    using Helpers;
    using Soccer.Services;

    public class MenuItemViewModel
    {
        #region Attributes
        private DataService dataService;
        #endregion

        #region Properties
        public string Icon { get; set; }

        public string Title { get; set; }

        public string PageName { get; set; }
        #endregion

        #region Constructor
        public MenuItemViewModel()
        {
            dataService = new DataService();
        }
        #endregion

        #region Commands
        public ICommand NavigateCommand
        {
            get
            {
                return new RelayCommand(Navigate);
            }
        }

        private void Navigate()
        {
            App.Master.IsPresented = false;
            var mainViewModel = MainViewModel.GetInstance();

            if (this.PageName == "LoginPage")
            {               
                mainViewModel.CurrentUser.IsRemembered = false;
                dataService.Update(mainViewModel.CurrentUser);
                Application.Current.MainPage = new NavigationPage(new LoginPage());
            }

            else if (this.PageName == "TournamentPage")
            {
                MainViewModel.GetInstance().Tournament = new TournamentViewModel();
                App.Navigator.PushAsync(new TournamentPage());
            }

            else if (this.PageName == "ConfigPage")
            {
                MainViewModel.GetInstance().Config = new ConfigViewModel();
                App.Navigator.PushAsync(new ConfigPage());
            }

            
            else if (this.PageName == "UserGroupPage")
            {
                MainViewModel.GetInstance().UserGroup = new UsersGroupViewModel();
                App.Navigator.PushAsync(new UserGroupPage());
            }
        }
        #endregion
    }
}
