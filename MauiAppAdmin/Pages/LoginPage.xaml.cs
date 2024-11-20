using Plugin.Maui.Biometric;

namespace MauiAppAdmin.Pages;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}

    private async void OnLoginButtonClicked(object sender, EventArgs e)
    {
        string user = UserEntry.Text;
        string password = PasswordEntry.Text;

        if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password))
        {
            await DisplayAlert("Error", "Por favor ingresa un correo y contrase�a", "OK");
            return;
        }

        //App.Current.MainPage = new LoadingPage();
        //await Navigation.PushModalAsync(new LoadingPage());

        //// L�gica para autenticaci�n aqu� (ejemplo: llamada a un servicio de API)
        //ResponseApiDTO<LoggedinDTO> responseApi = await _authService.Login(UsernameEntry.Text, PasswordEntry.Text);

        //await Navigation.PopModalAsync();

        //if (responseApi != null)
        //{
        //    // Cambiar la p�gina principal a AppShell despu�s de la autenticaci�n exitosa
        //    Application.Current.MainPage = new AppShell();
        //}
        //else
        //{
        //    await DisplayAlert("Error", "Usuario o contrase�a incorrectos", "OK");
        //}
    }

    private async void OnAuthClicked(object sender, EventArgs e)
    {
        var result = await BiometricAuthenticationService.Default.AuthenticateAsync(
                new AuthenticationRequest()
                {
                    Title = "Obtener Usuario",
                    NegativeText = "Cancelar Autenticaci�n"
                }, 
                CancellationToken.None
            );

        if (result.Status == BiometricResponseStatus.Success)
            await DisplayAlert("Naizu", "Usuario Recuperado", "Ok");
        else
            await DisplayAlert("Error", result.ErrorMsg, "Ok");
    }

    private void OnSizeChanged(object sender, EventArgs e)
    {
        // Obtener ancho y alto de la p�gina
        var width = this.Width;
        var height = this.Height;

        if (width > height) // Horizontal
        {
            DynamicFrame.Margin = new Thickness(10);
            DynamicFrame.VerticalOptions = LayoutOptions.Center;
        }
        else // Vertical
        {
            DynamicFrame.Margin = new Thickness(30, 50);
            DynamicFrame.VerticalOptions = LayoutOptions.Start;
        }
    }
}