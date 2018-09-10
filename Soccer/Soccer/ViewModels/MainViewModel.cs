using System;
using System.Collections.Generic;
using System.Text;

namespace Soccer.ViewModels
{
    public class MainViewModel
    {
        #region Properties
        public LoginViewModel Login { get; set; }
        #endregion

        #region Constructor
        public MainViewModel()
        {
            instance = this;

            //apiService = new ApiService();
            //dataService = new DataService();

            Login = new LoginViewModel();

            //LoadMenu();
        }
        #endregion

        #region Singleton
        private static MainViewModel instance;

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                instance = new MainViewModel();
            }

            return instance;
        }
        #endregion
    }
}
