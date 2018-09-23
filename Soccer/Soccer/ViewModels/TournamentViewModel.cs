namespace Soccer.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Plugin.Connectivity;
    using Soccer.Models;
    using Soccer.Services;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Input;

    public class TournamentViewModel : BaseViewModel
    {
        #region Services
        private ApiService apiService;
        private DataService dataService;
        private DialogService dialogService;
        #endregion

        #region Attributes
        private bool isRefreshing;
        private bool isRunning;
        private bool isEnabled;

        #endregion

        #region Properties
        public ObservableCollection<TournamentItemViewModel> Tournaments { get; set; }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { SetValue(ref this.isRefreshing, value); }
        }

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

        #region Constructor
        public TournamentViewModel()
        {
            apiService = new ApiService();
            dataService = new DataService();
            dialogService = new DialogService();
            Tournaments = new ObservableCollection<TournamentItemViewModel>();
            LoadTournaments();
        }
        #endregion

        #region Methods
        private async void LoadTournaments()
        {
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await dialogService.ShowMessage("Error", "Check you internet connection.");

                //await Application.Current.MainPage.DisplayAlert(Languages.Error, connection.Message, Languages.Accept);
                return;
            }

            IsRefreshing = true;

            var parameters = dataService.First<Parameter>(false);
            var user = dataService.First<User>(false);
            var response = await apiService.Get<Tournament>(
                parameters.UrlAPI, "/api", "/Tournaments", user.TokenType, user.AccessToken);
            IsRefreshing = false;

            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage("Error", response.Message);
                return;

                //await Application.Current.MainPage.DisplayAlert(
                //    Languages.Error,
                //    Languages.SomethingWrong,
                //    Languages.Accept);
                //return;
            }

            ReloadTournaments((List<Tournament>)response.Result);
        }

        private void ReloadTournaments(List<Tournament> tournaments)
        {
            Tournaments.Clear();
            foreach (var tournament in tournaments)
            {
                Tournaments.Add(new TournamentItemViewModel
                {
                    Dates = tournament.Dates,
                    Groups = tournament.Groups,
                    Logo = tournament.Logo,
                    Name = tournament.Name,
                    TournamentId = tournament.TournamentId,
                });
            }
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand
        {
            get { return new RelayCommand(Refresh); }
        }

        private void Refresh()
        {
            LoadTournaments();
        }
        #endregion
    }
}
