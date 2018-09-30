[assembly: Xamarin.Forms.ExportRenderer(
    typeof(Soccer.Views.LoginFacebookPage),
    typeof(Soccer.Droid.Implementations.LoginPageRenderer))]

namespace Soccer.Droid.Implementations
{
    using Android.App;
    using Newtonsoft.Json;
    using Soccer.Models;
    using Soccer.Views;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Xamarin.Auth;
    using Xamarin.Forms;
    using Soccer.Services;
    using Xamarin.Forms.Platform.Android;

    public class LoginPageRenderer : PageRenderer
    {
        public LoginPageRenderer()
        {
            var activity = this.Context as Activity;

            var auth = new OAuth2Authenticator(
                clientId: "457494911249915",
                scope: "email",
                authorizeUrl: new Uri("https://www.facebook.com/dialog/oauth/"),
                redirectUrl: new Uri("https://www.facebook.com/connect/login_success.html"));

            auth.Completed += async (sender, eventArgs) =>
            {
                if (eventArgs.IsAuthenticated)
                {
                    var accessToken = eventArgs.Account.Properties["access_token"].ToString();
                    var profile = await GetFacebookProfileAsync(accessToken);
                    await App.NavigateToProfile(profile);
                }
                else
                {
                    App.HideLoginView();
                }
            };

            activity.StartActivity(auth.GetUI(activity));
        }

        private async Task<FacebookResponse> GetFacebookProfileAsync(string accessToken)
        {
            var requestUrl = "https://graph.facebook.com/v2.8/me/?fields=name,picture.width(999),cover," +
                "age_range,devices,email,gender,is_verified,birthday,languages,work,website,religion," +
                "location,locale,link,first_name,last_name,hometown&access_token=" + accessToken;
            var apiService = new ApiService();
            return await apiService.GetFacebook(requestUrl);
        }

    }
}