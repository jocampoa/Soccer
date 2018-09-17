namespace Soccer.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Models;
    using Services;
    using Soccer.Views;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class TournamentItemViewModel : Tournament
    {
        private DataService dataService;

        public ICommand SelectTournamentCommand
        {
            get
            {
                return new RelayCommand(SelectTournament);
            }
        }

        private async void SelectTournament()
        {
            //var mainViewModel = MainViewModel.GetInstance();
            //var parameters = dataService.First<Parameter>(false);
            //if (parameters.Option == "Predictions")
            //{
            //    mainViewModel.Match = new MatchViewModel(TournamentId);
            //    Application.Current.MainPage = new MatchPage();
            //}
            //else
            //{
            //    mainViewModel.Group = new GroupViewModel(Groups);
            //    Application.Current.MainPage = new GroupPage();
            //}
        }
    }
}
