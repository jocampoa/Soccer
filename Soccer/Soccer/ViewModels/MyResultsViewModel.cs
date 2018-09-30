namespace Soccer.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Soccer.Models;
    using Soccer.Services;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;

    public class MyResultsViewModel : BaseViewModel
    {
        #region Attributes
        private ApiService apiService;

        private DataService dataService;

        private DialogService dialogService; // borrar

        private bool isRefreshing;

        private bool isRunning;

        private bool isEnabled;

        private int tournamentGroupId;

        private string filter;

        private List<Result> results;
        #endregion

        #region Properties
        public ObservableCollection<ResultItemViewModel> Results { get; set; }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
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

        public string Filter
        {
            get { return this.filter; }
            set
            {
                this.SetValue(ref this.filter, value);

                if (string.IsNullOrEmpty(filter))
                {
                    ReloadResults(results);
                }
            }
        }
        #endregion

        #region Constructor
        public MyResultsViewModel(int tournamentGroupId)
        {
            this.tournamentGroupId = tournamentGroupId;

            apiService = new ApiService();
            dialogService = new DialogService();
            dataService = new DataService();

            Results = new ObservableCollection<ResultItemViewModel>();

            LoadResults();
        }
        #endregion

        #region Methods
        private async void LoadResults()
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
            var controller = string.Format("/Tournaments/GetResults/{0}/{1}", tournamentGroupId, user.UserId);
            var response = await apiService.Get<Result>(
                parameters.UrlAPI, "/api", controller, user.TokenType, user.AccessToken);
            IsRefreshing = false;

            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage("Error", response.Message);
                return;
            }

            results = (List<Result>)response.Result;
            ReloadResults(results);
        }

        private void ReloadResults(List<Result> results)
        {
            Results.Clear();
            foreach (var result in results)
            {
                Results.Add(new ResultItemViewModel
                {
                    LocalGoals = result.LocalGoals,
                    Match = result.Match,
                    MatchId = result.MatchId,
                    Points = result.Points,
                    PredictionId = result.PredictionId,
                    UserId = result.UserId,
                    VisitorGoals = result.VisitorGoals,
                });
            }
        }
        #endregion

        #region Commands
        public ICommand SearchResultCommand
        {
            get
            {
                return new RelayCommand(SearchResult);
            }
        }

        private void SearchResult()
        {
            var list = results
                .Where(r => r.Match.Local.Initials.ToUpper() == Filter.ToUpper() ||
                            r.Match.Visitor.Initials.ToUpper() == Filter.ToUpper())
                .ToList();
            ReloadResults(list);
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(Refresh);
            }
        }

        private void Refresh()
        {
            LoadResults();
        }
        #endregion
    }
}
