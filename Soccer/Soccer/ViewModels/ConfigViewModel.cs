namespace Soccer.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Plugin.Media;
    using Plugin.Media.Abstractions;
    using Soccer.Helpers;
    using Soccer.Models;
    using Soccer.Services;
    using Soccer.Views;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class ConfigViewModel : BaseViewModel
    {
        #region Attributes
        private ApiService apiService;
        private DataService dataService;
        private bool isRunning;
        private bool isEnabled;
        private bool allowToModify;
        private List<League> leagues;
        private int favoriteLeagueId;
        private int favoriteTeamId;
        private ImageSource imageSource;
        private MediaFile file;
        private User currentUser;
        #endregion

        #region Properties
        public ObservableCollection<LeagueItemViewModel> Leagues { get; set; }

        public ObservableCollection<TeamItemViewModel> Teams { get; set; }

        public int FavoriteLeagueId
        {
            get { return this.favoriteLeagueId; }
            set
            {
                SetValue(ref this.favoriteLeagueId, value);
                ReloadTeams(favoriteLeagueId);
            }
        }

        public int FavoriteTeamId
        {
            get { return this.favoriteTeamId; }
            set { SetValue(ref this.favoriteLeagueId, value);}
        }

        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { SetValue(ref this.isEnabled, value); }
        }

        public bool AllowToModify
        {
            get { return this.allowToModify; }
            set { SetValue(ref this.allowToModify, value); }
        }

        public ImageSource ImageSource
        {
            get { return this.imageSource; }
            set { SetValue(ref this.imageSource, value); }
        }

        public User User
        {
            get;
            set;
        }
        #endregion

        #region Constructor
        public ConfigViewModel(User currentUser)
        {
            this.currentUser = currentUser;

            apiService = new ApiService();
            dataService = new DataService();

            Leagues = new ObservableCollection<LeagueItemViewModel>();
            Teams = new ObservableCollection<TeamItemViewModel>();
            
            User.FirstName = currentUser.FirstName;
            User.LastName = currentUser.LastName;
            User.Picture = currentUser.Picture;
            User.Email = currentUser.Email;
            User.NickName = currentUser.NickName;

            IsEnabled = true;
            AllowToModify = currentUser.UserTypeId == 1;

            LoadLeagues();
        }
        #endregion

        #region Methods
        private async void LoadLeagues()
        {
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                IsRunning = true;
                IsEnabled = false;

                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Accept);
                return;
            }

            var parameters = dataService.First<Parameter>(false);
            var response = await apiService.Get<League>(parameters.UrlAPI, "/api", "/Leagues");

            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.Accept);
                return;
            }

            leagues = (List<League>)response.Result;
            ReloadLeagues(leagues);
            ReloadTeams(currentUser.FavoriteTeam.LeagueId);

            FavoriteLeagueId = currentUser.FavoriteTeam.LeagueId;
            FavoriteTeamId = currentUser.FavoriteTeamId;
        }

        private void ReloadLeagues(List<League> leagues)
        {
            Leagues.Clear();
            foreach (var league in leagues.OrderBy(l => l.Name))
            {
                Leagues.Add(new LeagueItemViewModel
                {
                    LeagueId = league.LeagueId,
                    Logo = league.Logo,
                    Name = league.Name,
                    Teams = league.Teams,
                });
            }
        }

        private void ReloadTeams(int favoriteLeagueId)
        {
            var teams = leagues.Where(l => l.LeagueId == favoriteLeagueId).FirstOrDefault().Teams;
            Teams.Clear();
            foreach (var team in teams.OrderBy(t => t.Name))
            {
                Teams.Add(new TeamItemViewModel
                {
                    Fans = team.Fans,
                    Initials = team.Initials,
                    LeagueId = team.LeagueId,
                    Logo = team.Logo,
                    Name = team.Name,
                    TeamId = team.TeamId,
                });
            }
        }
        #endregion

        #region Commands
        public ICommand ChangePasswordCommand
        {
            get
            {
                return new RelayCommand(ChangePassword);
            }
        }

        private async void ChangePassword()
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.ChangePassword = new ChangePasswordViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new ChangePasswordPage());
        }

        public ICommand SaveCommand
        {
            get
            {
                return new RelayCommand(Save);
            }
        }

        private async void Save()
        {
            if (string.IsNullOrEmpty(User.FirstName))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.ValidationFirstName,
                    Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(User.LastName))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.ValidationLastName,
                    Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(User.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.EmailValidation,
                    Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(User.NickName))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.ValidationNickName,
                    Languages.Accept);
                return;
            }

            if (FavoriteTeamId == 0)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.ValidationFavoriteTeam,
                    Languages.Accept);
                return;
            }

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

            IsRunning = true;
            IsEnabled = false;

            byte[] imageArray = null;
            if (file != null)
            {
                imageArray = FilesHelper.ReadFully(file.GetStream());
                file.Dispose();
            }

            var user = new User
            {
                Email = User.Email,
                FavoriteTeamId = FavoriteTeamId,
                FirstName = User.FirstName,
                ImageArray = imageArray,
                LastName = User.LastName,
                NickName = User.NickName,
                Password = User.Password,
                UserTypeId = 1,
                UserId = currentUser.UserId,
            };

            var parameters = dataService.First<Parameter>(false);
            var response = await apiService.Put(parameters.UrlAPI, "/api", "/Users",
                currentUser.TokenType, currentUser.AccessToken, user);

            if (!response.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.Accept);
                return;
            }

            response = await apiService.GetUserByEmail(
                parameters.UrlAPI,
                "/api",
                "/Users/GetUserByEmail",
                currentUser.TokenType,
                currentUser.AccessToken,
                User.Email);

            var newUser = (User)response.Result;
            newUser.AccessToken = currentUser.AccessToken;
            newUser.TokenType = currentUser.TokenType;
            newUser.TokenExpires = currentUser.TokenExpires;
            newUser.IsRemembered = currentUser.IsRemembered;
            newUser.Password = currentUser.Password;
            dataService.DeleteAllAndInsert(newUser.FavoriteTeam);
            dataService.DeleteAllAndInsert(newUser.UserType);
            dataService.DeleteAllAndInsert(newUser);

            IsRunning = false;
            IsEnabled = true;

            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.CurrentUser = newUser;
            await App.Navigator.PopAsync();
        }

        public ICommand CancelCommand
        {
            get
            {
                return new RelayCommand(Cancel);
            }
        }

        private void Cancel()
        {
            Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
        }

        public ICommand TakePictureCommand
        {
            get
            {
                return new RelayCommand(TakePicture);
            }
        }

        private async void TakePicture()
        {
            if (!AllowToModify)
            {
                return;
            }

            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.NoCamera,
                    Languages.Accept);
                return;
            }

            IsRunning = true;

            file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = "Sample",
                Name = "test.jpg",
                PhotoSize = PhotoSize.Small,
            });

            if (file != null)
            {
                ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });
            }

            IsRunning = false;
        }
        #endregion
    }
}
