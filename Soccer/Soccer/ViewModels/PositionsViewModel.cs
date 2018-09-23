﻿namespace Soccer.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Soccer.Models;
    using Soccer.Services;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    public class PositionsViewModel : BaseViewModel
    {
        #region Attributes
        private ApiService apiService;
        private DataService dataService;
        private DialogService dialogService;
        private bool isRefreshing;
        private int tournamentGroupId;
        #endregion

        #region Properties
        public ObservableCollection<TournamentTeamItemViewModel> TournamentTeams { get; set; }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { SetValue(ref this.isRefreshing, value); }
        }
        #endregion

        #region Constructor
        public PositionsViewModel(int tournamentGroupId)
        {
            this.tournamentGroupId = tournamentGroupId;

            apiService = new ApiService();
            dataService = new DataService();
            dialogService = new DialogService();

            TournamentTeams = new ObservableCollection<TournamentTeamItemViewModel>();

            LoadTournamentTeams();
        }
        #endregion

        #region Methods
        private async void LoadTournamentTeams()
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
            var response = await apiService.Get<TournamentTeam>(
                parameters.UrlAPI, "/api", "/TournamentTeams", user.TokenType, user.AccessToken,
                tournamentGroupId);
            IsRefreshing = false;

            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage("Error", response.Message);
                return;
            }

            ReloadTournamentTeams((List<TournamentTeam>)response.Result);
        }

        private void ReloadTournamentTeams(List<TournamentTeam> tournamentTeams)
        {
            TournamentTeams.Clear();
            foreach (var tournamentTeam in tournamentTeams)
            {
                TournamentTeams.Add(new TournamentTeamItemViewModel
                {
                    AgainstGoals = tournamentTeam.AgainstGoals,
                    FavorGoals = tournamentTeam.FavorGoals,
                    MatchesLost = tournamentTeam.MatchesLost,
                    MatchesPlayed = tournamentTeam.MatchesPlayed,
                    MatchesTied = tournamentTeam.MatchesTied,
                    MatchesWon = tournamentTeam.MatchesWon,
                    Points = tournamentTeam.Points,
                    Position = tournamentTeam.Position,
                    Team = tournamentTeam.Team,
                    TeamId = tournamentTeam.TeamId,
                    TournamentGroupId = tournamentTeam.TournamentGroupId,
                    TournamentTeamId = tournamentTeam.TournamentTeamId,
                });
            }
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(Refresh);
            }
        }

        private void Refresh()
        {
            LoadTournamentTeams();
        }
        #endregion
    }
}
