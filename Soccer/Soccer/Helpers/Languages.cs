namespace Soccer.Helpers
{
    using Xamarin.Forms;
    using Interfaces;
    using Resources;

    public static class Languages
    {
        static Languages()
        {
            var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resource.Culture = ci;
            DependencyService.Get<ILocalize>().SetLocale(ci);
        }

        public static string Accept
        {
            get { return Resource.Accept; }
        }

        public static string Error
        {
            get { return Resource.Error; }
        }

        public static string EmailValidation
        {
            get { return Resource.EmailValidation; }
        }

        public static string PasswordValidation
        {
            get { return Resource.PasswordValidation; }
        }

        public static string LoginError
        {
            get { return Resource.LoginError; }
        }

        public static string SomethingWrong
        {
            get { return Resource.SomethingWrong; }
        }

        public static string CurrentPasswordValidation
        {
            get { return Resource.CurrentPasswordValidation; }
        }

        public static string ConfirmValidation
        {
            get { return Resource.ConfirmValidation; }
        }

        public static string NewPasswordValidation
        {
            get { return Resource.NewPasswordValidation; }
        }

        public static string ValidationPassword
        {
            get { return Resource.ValidationPassword; }
        }

        public static string ValidationPassword2
        {
            get { return Resource.ValidationPassword2; }
        }

        public static string ValidationConfirmPassword
        {
            get { return Resource.ValidationConfirmPassword; }
        }

        public static string ConfirmValidation2
        {
            get { return Resource.ConfirmValidation2; }
        }

        public static string PasswordChanged
        {
            get { return Resource.PasswordChanged; }
        }

        public static string Confirm
        {
            get { return Resource.Confirm; }
        }

        public static string ValidationFirstName
        {
            get { return Resource.ValidationFirstName; }
        }

        public static string ValidationLastName
        {
            get { return Resource.ValidationLastName; }
        }

        public static string ValidationNickName
        {
            get { return Resource.ValidationNickName; }
        }

        public static string ValidationFavoriteTeam
        {
            get { return Resource.ValidationFavoriteTeam; }
        }

        public static string NoCamera
        {
            get { return Resource.NoCamera; }
        }

        public static string ForgotPasswordError
        {
            get { return Resource.ForgotPasswordError; }
        }

        public static string ConfirmValidation3
        {
            get { return Resource.ConfirmValidation3; }
        }

        public static string Confirmation
        {
            get { return Resource.Confirmation; }
        }

        public static string ValidationGoalsLocal
        {
            get { return Resource.ValidationGoalsLocal; }
        }

        public static string ValidationGoalsVisitor
        {
            get { return Resource.ValidationGoalsVisitor; }
        }

        public static string UserCreate
        {
            get { return Resource.UserCreate; }
        }

        public static string Soccer
        {
            get { return Resource.Soccer; }
        }

        public static string Login
        {
            get { return Resource.Login; }
        }

        public static string EmailPlaceHolder
        {
            get { return Resource.EmailPlaceHolder; }
        }

        public static string PasswordPlaceHolder
        {
            get { return Resource.PasswordPlaceHolder; }
        }

        public static string Rememberme
        {
            get { return Resource.Rememberme; }
        }

        public static string LoginFacebook
        {
            get { return Resource.LoginFacebook; }
        }

        public static string Email
        {
            get { return Resource.Email; }
        }

        public static string Password
        {
            get { return Resource.Password; }
        }

        public static string Register
        {
            get { return Resource.Register; }
        }

        public static string Forgot
        {
            get { return Resource.Forgot; }
        }

    }
}
