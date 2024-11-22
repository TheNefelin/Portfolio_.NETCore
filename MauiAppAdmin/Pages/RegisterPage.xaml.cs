using ClassLibrary_DTOs.Auth;
using MauiAppAdmin.Services;

namespace MauiAppAdmin.Pages;

public partial class RegisterPage : ContentPage
{
    private readonly ApiAuthService _authService;

    public RegisterPage(ApiAuthService apiAuthService)
	{
		InitializeComponent();

		_authService = apiAuthService;
	}

	private async void OnSave(object sender, EventArgs e)
	{
		var email = EmainEntry.Text;
		var pass01 = Pass01Entry.Text;
		var pass02 = Pass02Entry.Text;

		if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(pass01) || string.IsNullOrWhiteSpace(pass02)) {
            await DisplayAlert("Error", "Debes Rellenar Todos los Campos.", "Ok");
            return;
        }

        if (!pass01.Equals(pass02))
        {
            await DisplayAlert("Error", "Las Contraseñas No Coincide.", "Ok");
            return;
        }

		var user = new RegisterDTO()
		{
			Email = email,
			Password1 = pass01,
			Password2 = pass02,
		};

        this.IsEnabled = false;

        var apiResult = await _authService.Register(user);

		if (!apiResult.Success) {
            await DisplayAlert("", apiResult.Message, "Ok");
		} 
		else
		{
            await DisplayAlert("Naizu", apiResult.Message, "OK");

            EmainEntry.Text = "";
            Pass01Entry.Text = "";
            Pass02Entry.Text = "";

            await Navigation.PopModalAsync();
        }

		this.IsEnabled = true;
    }

	private async void GoBack(object sender, EventArgs e)
	{
		await Navigation.PopModalAsync();
	}
}