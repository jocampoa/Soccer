namespace Soccer.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Soccer.Models;
    using System.Windows.Input;

    public class MatchViewModel : Match
    {
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
        }
        #endregion
    }
}
