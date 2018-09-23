namespace Soccer.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Plugin.Connectivity;
    using Soccer.Models;
    using Soccer.Services;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    public class MatchViewModel : BaseViewModel
    {
        #region Attributes
        private ApiService apiService;
        private DialogService dialogService;
        private DataService dataService;
        private bool isRefreshing;
        private int tournamentId;
        #endregion

        #region Properties
        public ObservableCollection<MatchItemViewModel> Matches { get; set; }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { SetValue(ref this.isRefreshing, value); }
        }
        #endregion

        #region Constructor
        public MatchViewModel(int tournamentId)
        {
            instance = this;

            this.tournamentId = tournamentId;

            apiService = new ApiService();
            dialogService = new DialogService();
            dataService = new DataService();

            Matches = new ObservableCollection<MatchItemViewModel>();
        }
        #endregion

        #region Singleton
        private static MatchViewModel instance;

        public static MatchViewModel GetInstance()
        {
            return instance;
        }
        #endregion

        #region Methods
        private async void LoadMatches()
        {
            this.IsRefreshing = true;

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRefreshing = false;
                await dialogService.ShowMessage("Error", "Check you internet connection.");

                //await Application.Current.MainPage.DisplayAlert(
                //    "Error",
                //    connection.Message,
                //    "Accept");
                //return;
            }

            var parameters = dataService.First<Parameter>(false);
            var user = dataService.First<User>(false);
            var controller = string.Format("/Tournaments/GetMatchesToPredict/{0}/{1}", tournamentId, user.UserId);
            var response = await apiService.Get<Match>(
                parameters.UrlAPI, "/api", controller, user.TokenType, user.AccessToken);
            IsRefreshing = false;

            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage("Error", response.Message);

                //

                return;
            }

            ReloadMatches((List<Match>)response.Result);
        }

        private void ReloadMatches(List<Match> matches)
        {
            Matches.Clear();
            foreach (var match in matches)
            {
                Matches.Add(new MatchItemViewModel
                {
                    DateId = match.DateId,
                    DateTime = match.DateTime,
                    Local = match.Local,
                    LocalGoals = match.LocalGoals,
                    LocalId = match.LocalId,
                    MatchId = match.MatchId,
                    StatusId = match.StatusId,
                    TournamentGroupId = match.TournamentGroupId,
                    Visitor = match.Visitor,
                    VisitorGoals = match.VisitorGoals,
                    VisitorId = match.VisitorId,
                    WasPredicted = match.WasPredicted,
                });
            }
        }
        #endregion

        #region Commmands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(Refresh);
            }
        }

        private void Refresh()
        {
            LoadMatches();
        }
        #endregion
    }
}
