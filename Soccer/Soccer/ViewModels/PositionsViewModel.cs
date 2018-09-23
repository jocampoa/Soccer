namespace Soccer.ViewModels
{
    public class PositionsViewModel : BaseViewModel
    {
        private int tournamentGroupId;

        public PositionsViewModel(int tournamentGroupId)
        {
            this.tournamentGroupId = tournamentGroupId;
        }
    }
}
