namespace Soccer.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Plugin.Connectivity;
    using Soccer.Models;
    using Soccer.Services;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Input;
    using System;
    using Plugin.Media;
    using Plugin.Media.Abstractions;
    using Xamarin.Forms;
    using Soccer.Helpers;
    using Soccer.Views;

    public class RegisterViewModel : User, INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Attributes
        private ApiService apiService;
        private DataService dataService;
        private bool isRunning;
        private bool isEnabled;
        private List<League> leagues;
        private int favoriteLeagueId;
        private ImageSource imageSource;
        private MediaFile file;
        #endregion

        #region Properties
        public ObservableCollection<LeagueItemViewModel> Leagues { get; set; }

        public ObservableCollection<TeamItemViewModel> Teams { get; set; }

        public int FavoriteLeagueId
        {
            set
            {
                if (favoriteLeagueId != value)
                {
                    favoriteLeagueId = value;
                    ReloadTeams(favoriteLeagueId);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FavoriteLeagueId"));
                }
            }
            get
            {
                return favoriteLeagueId;
            }
        }

        public bool IsRunning
        {
            set
            {
                if (isRunning != value)
                {
                    isRunning = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRunning"));
                }
            }
            get
            {
                return isRunning;
            }
        }

        public bool IsEnabled
        {
            set
            {
                if (isEnabled != value)
                {
                    isEnabled = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsEnabled"));
                }
            }
            get
            {
                return isEnabled;
            }
        }

        public ImageSource ImageSource
        {
            set
            {
                if (imageSource != value)
                {
                    imageSource = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ImageSource"));
                }
            }
            get
            {
                return imageSource;
            }
        }

        public string PasswordConfirm { get; set; }
        #endregion

        #region Constructor
        public RegisterViewModel()
        {
            apiService = new ApiService();
            dataService = new DataService();

            Leagues = new ObservableCollection<LeagueItemViewModel>();
            Teams = new ObservableCollection<TeamItemViewModel>();

            IsEnabled = true;

            LoadLeagues();
        }
        #endregion

        #region Methods
        private async void LoadLeagues()
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

            IsRunning = true;
            IsEnabled = false;

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
        public ICommand SaveCommand
        {
            get
            {
                return new RelayCommand(Save);
            }
        }

        private async void Save()
        {
            if (string.IsNullOrEmpty(this.FirstName))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.ValidationFirstName,
                    Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(this.LastName))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.ValidationLastName,
                    Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(this.Password))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PasswordValidation,
                    Languages.Accept);
                return;
            }

            if (this.Password.Length < 6)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.ValidationPassword2,
                    Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(PasswordConfirm))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.ValidationConfirmPassword,
                    Languages.Accept);
                return;
            }

            if (Password != PasswordConfirm)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.ConfirmValidation,
                    Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.EmailValidation,
                    Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(NickName))
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

            this.IsRunning = true;
            this.IsEnabled = false;

            var checkConnetion = await this.apiService.CheckConnection();
            if (!checkConnetion.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    checkConnetion.Message,
                    Languages.Accept);
                return;
            }

            var imageArray = FilesHelper.ReadFully(file.GetStream());
            file.Dispose();

            var user = new User
            {
                Email = Email,
                FavoriteTeamId = FavoriteTeamId,
                FirstName = FirstName,
                ImageArray = imageArray,
                LastName = LastName,
                NickName = NickName,
                Password = Password,
                UserTypeId = 1,
            };

            var parameters = dataService.First<Parameter>(false);
            var response = await apiService.Post(parameters.UrlAPI, "/api", "/Users", user);

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

            await Application.Current.MainPage.DisplayAlert(
                Languages.Confirmation,
                Languages.UserCreate,
                Languages.Accept);

            await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());

            return;          
        }

        public ICommand CancelCommand
        {
            get
            {
                return new RelayCommand(Cancel);
            }
        }

        private async void Cancel()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
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
