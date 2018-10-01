namespace Soccer.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Plugin.Connectivity;
    using Soccer.Helpers;
    using Soccer.Models;
    using Soccer.Services;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class TournamentViewModel : BaseViewModel
    {
        #region Services
        private ApiService apiService;
        private DataService dataService;
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

                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error, 
                    connection.Message, 
                    Languages.Accept);
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
                await Application.Current.MainPage.DisplayAlert(Languages.Error,
                    response.Message,
                    Languages.Accept);
                return;
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
