using DemoAppleSignIn.Dependency;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DemoAppleSignIn
{
    public partial class MainPage : ContentPage
    {
        private IAppleSignInService appleSignInService;

        public MainPage()
        {
            InitializeComponent();
            appleSignInService = DependencyService.Get<IAppleSignInService>();
        }

        async void AppleSignInButton_SignIn(System.Object sender, System.EventArgs e)
        {
            if (appleSignInService != null)
            {
                var account = await appleSignInService.SignInAsync();

                if (account != null)
                {
                    await SecureStorage.SetAsync(App.APPLE_ID_EMAIL, account.Email);
                    await SecureStorage.SetAsync(App.APPLE_ID_NAME, account.Name);
                    await SecureStorage.SetAsync(App.APPLE_ID_TOKEN, account.Token);

                    Preferences.Set(App.APPLE_ID_LOGGED_KEY, true);

                    NavigationPage nvp = (NavigationPage)App.Current.MainPage;

                    await nvp.PushAsync(new LoggedPage());
                }
            }
        }
    }
}
