namespace Soccer.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Soccer.Models;
    using Soccer.Services;
    using Soccer.Views;
    using System.Windows.Input;

    public class GroupItemViewModel : Group
    {
        private DataService dataService;

        public GroupItemViewModel()
        {
            dataService = new DataService();
        }

        public ICommand SelectGroupCommand { get { return new RelayCommand(SelectGroup); } }

        private async void SelectGroup()
        {
            var mainViewModel = MainViewModel.GetInstance();
            var parameters = dataService.First<Parameter>(false);
            if (parameters.Option == "Tournaments")
            {
                mainViewModel.Positions = new PositionsViewModel(TournamentGroupId);
                await App.Navigator.PushAsync(new PositionsPage());
            }
            else
            {
                mainViewModel.MyResults = new MyResultsViewModel(TournamentGroupId);
                await App.Navigator.PushAsync(new MyResultsPage());
            }
        }
    }
}
