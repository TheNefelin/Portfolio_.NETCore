using ClassLibrary_DTOs.Auth;

namespace MauiAppAdmin.Pages;

public partial class ProfilePage : ContentPage
{
    private DateTime loginDate;
    private int expireMinutes;

    public ProfilePage()
	{
		InitializeComponent();

        LoadLoginData();
    }
    private async void LoadLoginData()
    {
        // Obtener datos de SecureStorage
        var userId = await SecureStorage.GetAsync("id");
        var userRole = await SecureStorage.GetAsync("role");
        var loginDateStr = await SecureStorage.GetAsync("date_login");
        var expireMinStr = await SecureStorage.GetAsync("expire_min");
        var sqlTokenStr = await SecureStorage.GetAsync("sql_token");
        var jwtTokenStr = await SecureStorage.GetAsync("jwt_token");

        // Mostrar los datos
        UserIdLabel.Text = userId ?? "No disponible";
        UserRoleLabel.Text = userRole ?? "No disponible";
        LoginDateLabel.Text = loginDateStr ?? "No disponible";
        ExpireMinLabel.Text = expireMinStr ?? "No disponible";
        SqlTokenLabel.Text = sqlTokenStr ?? "No disponible";
        JwtTokenLabel.Text = jwtTokenStr ?? "No disponible";

        // Si los datos de fecha de login y expiración están disponibles, inicia la cuenta regresiva
        if (DateTime.TryParse(loginDateStr, out loginDate) && int.TryParse(expireMinStr, out expireMinutes))
        {
            StartCountdown(); // Iniciar el temporizador
        }
        else
        {
            CountdownLabel.Text = "Información no disponible.";
        }
    }

    private void StartCountdown()
    {
        bool isCountdownRunning;

        isCountdownRunning = true;
        Dispatcher.StartTimer(TimeSpan.FromSeconds(1), () =>
        {
            if (!isCountdownRunning) return false;

            var timeElapsed = DateTime.Now - loginDate;
            var timeRemaining = TimeSpan.FromMinutes(expireMinutes) - timeElapsed;

            if (timeRemaining.TotalSeconds > 0)
            {
                // Mostrar la cuenta regresiva
                CountdownLabel.Text = $"{timeRemaining.Minutes:D2}:{timeRemaining.Seconds:D2} minutos restantes";
            }
            else
            {
                // Token ha expirado
                CountdownLabel.Text = "Token expirado.";
                isCountdownRunning = false; // Detener el temporizador

                // Llamar al logout automáticamente
                var shell = Application.Current.MainPage as AppShell;
                shell?.PerformLogout(); // Invoca el logout desde AppShell
            }

            return isCountdownRunning; // Mantener el temporizador corriendo hasta que el token expire
        });
    }
}