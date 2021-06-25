using DemoAppleSignIn.Model;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DemoAppleSignIn
{
    public partial class LoggedPage : ContentPage
    {
        public LoggedPage()
        {
            InitializeComponent();
            LlenarLoginDatos();
        }

        private async void LlenarLoginDatos()
        {
            if (Preferences.Get(App.APPLE_ID_LOGGED_KEY, false))
            {
                string Correo = await SecureStorage.GetAsync(App.APPLE_ID_EMAIL);
                string Nombre = await SecureStorage.GetAsync(App.APPLE_ID_NAME);
                lblBienvenido.Text = $"Bienvenido: {Nombre}, {Correo}";
            }
        }
    }
}
