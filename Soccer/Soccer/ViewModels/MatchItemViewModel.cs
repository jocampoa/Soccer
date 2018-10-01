namespace Soccer.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Soccer.Models;
    using Soccer.Views;
    using System.Windows.Input;

    public class MatchItemViewModel : Match
    {
        #region Constructor
        public MatchItemViewModel()
        {

        }
        #endregion

        #region Commmands
        public ICommand SelectMatchCommand
        {
            get
            {
                return new RelayCommand(SelectMatch);
            }
        }

        private async void SelectMatch()
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Prediction = new PredictionViewModel(this);

            await App.Navigator.PushAsync(new PredictionPage());
        }
        #endregion
    }
}
