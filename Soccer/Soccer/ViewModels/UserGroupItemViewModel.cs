namespace Soccer.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Soccer.Models;
    using Soccer.Services;
    using Soccer.Views;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class UserGroupItemViewModel : UserGroup
    {
        public UserGroupItemViewModel()
        {
        }

        public ICommand SelectGroupCommand
        {
            get
            {
                return new RelayCommand(SelectGroup);
            }
        }

        private async void SelectGroup()
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.UsersGroup = new UsersGroupViewModel(this);
            await App.Navigator.PushAsync(new UsersGroupPage());
        }
    }
}
