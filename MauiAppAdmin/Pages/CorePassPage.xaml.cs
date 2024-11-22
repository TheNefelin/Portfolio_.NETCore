using MauiAppAdmin.Services;

namespace MauiAppAdmin.Pages;

public partial class CorePassPage : ContentPage
{
	private readonly ApiCoreService _apiCoreService;

	public CorePassPage(ApiCoreService apiCoreService)
	{
		InitializeComponent();

		_apiCoreService = apiCoreService;
	}

	private async void OnSaveClicked(object sender, EventArgs e)
    {
		var pass01 = Password01Entry.Text;
        var pass02 = Password02Entry.Text;

        if (string.IsNullOrWhiteSpace(pass01) || string.IsNullOrWhiteSpace(pass02)) {
			await DisplayAlert("Error", "Debes Rellenar Todos los Campos.", "Ok");
			return;
		}

		if (!pass01.Equals(pass02))
		{
            await DisplayAlert("Error", "Las Contraseñas No Coincide.", "Ok");
            return;
        }

		var apiResult = await _apiCoreService.Register(pass01);

		if (!apiResult.Success)
		{
            await DisplayAlert($"Error {apiResult.StatusCode}", apiResult.Message, "Ok");
            return;
        }

		await DisplayAlert($"IV: {apiResult.Data.IV}", apiResult.Message, "OK");

		await Navigation.PopAsync();
	}
}