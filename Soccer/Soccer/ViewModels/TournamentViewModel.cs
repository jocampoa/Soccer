namespace Soccer.ViewModels
{
    using Services;
    using System.Collections.ObjectModel;
    public class TournamentViewModel : BaseViewModel
    {
        #region Services
        private ApiService apiService;
        private DataService dataService;
        #endregion

        #region Attributes
        private bool isRefreshing;
        #endregion

        #region Properties
        public ObservableCollection<TournamentItemViewModel> Tournaments { get; set; }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { SetValue(ref this.isRefreshing, value); }
        }
        #endregion

        #region Constructor
        public TournamentViewModel()
        {
            apiService = new ApiService();
            dataService = new DataService();
            Tournaments = new ObservableCollection<TournamentItemViewModel>();
            LoadTournaments();
        }
        #endregion

        #region Methods
        private async void LoadTournaments()
        {
        //    if (!CrossConnectivity.Current.IsConnected)
        //    {
        //        await dialogService.ShowMessage("Error", "Check you internet connection.");
        //        return;
        //    }

        //    var isReachable = await CrossConnectivity.Current.IsRemoteReachable("google.com");
        //    if (!isReachable)
        //    {
        //        await dialogService.ShowMessage("Error", "Check you internet connection.");
        //        return;
        //    }

        //    IsRefreshing = true;
        //    var parameters = dataService.First<Parameter>(false);
        //    var user = dataService.First<User>(false);
        //    var response = await apiService.Get<Tournament>(
        //        parameters.URLBase, "/api", "/Tournaments", user.TokenType, user.AccessToken);
        //    IsRefreshing = false;

        //    if (!response.IsSuccess)
        //    {
        //        await dialogService.ShowMessage("Error", response.Message);
        //        return;
        //    }

        //    ReloadTournaments((List<Tournament>)response.Result);
        //}

        //private void ReloadTournaments(List<Tournament> tournaments)
        //{
        //    Tournaments.Clear();
        //    foreach (var tournament in tournaments)
        //    {
        //        Tournaments.Add(new TournamentItemViewModel
        //        {
        //            Dates = tournament.Dates,
        //            Groups = tournament.Groups,
        //            Logo = tournament.Logo,
        //            Name = tournament.Name,
        //            TournamentId = tournament.TournamentId,
        //        });
        //    }
        //}
        //#endregion

        //#region Commands
        //public ICommand RefreshCommand
        //{
        //    get { return new RelayCommand(Refresh); }
        //}

        //private void Refresh()
        //{
        //    LoadTournaments();
        }
        #endregion
    }
}
