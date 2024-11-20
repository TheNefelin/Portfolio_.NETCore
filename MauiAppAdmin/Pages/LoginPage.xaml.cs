using ClassLibrary_DTOs.Auth;
using ClassLibrary_DTOs;
using MauiAppAdmin.Services;
using Plugin.Maui.Biometric;

namespace MauiAppAdmin.Pages;

public partial class LoginPage : ContentPage
{
    private readonly ApiAuthService _authService;

    public LoginPage(ApiAuthService authService)
	{
		InitializeComponent();
        _authService = authService;
    }

    private async void OnLoginButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new LoadingPage());

        string user = UserEntry.Text;
        string password = PasswordEntry.Text;

        if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password))
        {
            await DisplayAlert("Error", "Por favor ingresa un correo y contraseña", "OK");
            await Navigation.PopModalAsync();
            return;
        }

        ResultApiDTO<LoggedinDTO> resultApi = await _authService.Login(UserEntry.Text, PasswordEntry.Text);

        if (!resultApi.Success) 
        {
            await DisplayAlert($"Error {resultApi.StatusCode}", resultApi.Message, "OK");
            await Navigation.PopModalAsync();
            return;
        }

        Application.Current.MainPage = new AppShell();
        await Navigation.PopModalAsync();
    }

    private async void OnAuthClicked(object sender, EventArgs e)
    {
        var result = await BiometricAuthenticationService.Default.AuthenticateAsync(
                new AuthenticationRequest()
                {
                    Title = "Obtener Usuario",
                    NegativeText = "Cancelar Autenticación"
                }, 
                CancellationToken.None
            );

        if (result.Status == BiometricResponseStatus.Success)
            UserEntry.Text = await ApiAuthService.GetUser();
        else
            await DisplayAlert("Error", result.ErrorMsg, "Ok");
    }

    private void OnSizeChanged(object sender, EventArgs e)
    {
        var width = this.Width;
        var height = this.Height;

        if (width > height) // Horizontal
        {
            DynamicFrame.Margin = new Thickness(10);
            DynamicFrame.VerticalOptions = LayoutOptions.Center;
        }
        else // Vertical
        {
            DynamicFrame.Margin = new Thickness(30, 80);
            DynamicFrame.VerticalOptions = LayoutOptions.Start;
        }
    }
}