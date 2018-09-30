[assembly: Xamarin.Forms.Dependency(typeof(Soccer.Droid.Implementations.RegistrationDevice))]

namespace Soccer.Droid.Implementations
{
    using Gcm.Client;
    using Android.Util;
    using Soccer.Interfaces;

    public class RegistrationDevice : IRegisterDevice
    {
        #region Methods
        public void RegisterDevice()
        {
            var mainActivity = MainActivity.GetInstance();
            GcmClient.CheckDevice(mainActivity);
            GcmClient.CheckManifest(mainActivity);

            Log.Info("MainActivity", "Registering...");
            GcmClient.Register(mainActivity, Soccer.Droid.Constants.SenderID);
        }
        #endregion
    }
}