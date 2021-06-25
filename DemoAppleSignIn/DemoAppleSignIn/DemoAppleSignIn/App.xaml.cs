using Xamarin.Essentials;
using Xamarin.Forms;

namespace DemoAppleSignIn
{
    public partial class App : Application
    {
        public const string APPLE_ID_TOKEN = "AppleIdKey";
        public const string APPLE_ID_EMAIL = "AppleIdEmail";
        public const string APPLE_ID_NAME = "AppleIdName";

        public const string APPLE_ID_LOGGED_KEY = "AppleIdLogged";

        public App()
        {
            InitializeComponent();


            // si ya hay informacion de login, ya no pedirlo
            if (!Preferences.Get(APPLE_ID_LOGGED_KEY, false))
            {
                MainPage = new NavigationPage(new MainPage());
            }
            else
            {
                MainPage = new NavigationPage(new LoggedPage());
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
