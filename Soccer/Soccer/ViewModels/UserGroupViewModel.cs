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

    public class UserGroupViewModel : BaseViewModel
    {
        #region Attributes
        private ApiService apiService;
        private DialogService dialogService;
        private DataService dataService;
        private bool isRefreshing;
        private bool isRunning;
        private bool isEnabled;
        #endregion

        #region Properties
        public ObservableCollection<UserGroupItemViewModel> UserGroups { get; set; }

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
        public UserGroupViewModel()
        {
            apiService = new ApiService();
            dialogService = new DialogService();
            dataService = new DataService();

            UserGroups = new ObservableCollection<UserGroupItemViewModel>();

            LoadUserGroups();
        }
        #endregion

        #region Methods
        private async void LoadUserGroups()
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
            var response = await apiService.Get<UserGroup>(
                parameters.UrlAPI, "/api", "/Groups",
                user.TokenType, user.AccessToken, user.UserId);
            IsRefreshing = false;

            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage("Error", response.Message);
                return;
            }

            ReloadUserGroups((List<UserGroup>)response.Result);
        }

        private void ReloadUserGroups(List<UserGroup> userGroups)
        {
            UserGroups.Clear();
            foreach (var userGroup in userGroups)
            {
                UserGroups.Add(new UserGroupItemViewModel
                {
                    GroupId = userGroup.GroupId,
                    GroupUsers = userGroup.GroupUsers,
                    Logo = userGroup.Logo,
                    Name = userGroup.Name,
                    Owner = userGroup.Owner,
                    OwnerId = userGroup.OwnerId,
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
            LoadUserGroups();
        }
        #endregion
    }
}
