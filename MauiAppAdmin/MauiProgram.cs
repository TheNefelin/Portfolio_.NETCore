using MauiAppAdmin.Pages;
using MauiAppAdmin.Services;
using Microsoft.Extensions.Logging;
using Plugin.Maui.Biometric;

namespace MauiAppAdmin
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Dependencies Injection
            builder.Services.AddSingleton<IBiometric>(BiometricAuthenticationService.Default);
            builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri("https://artema.bsite.net/") });
            builder.Services.AddSingleton<ApiAuthService>();

            builder.Services.AddTransient<LoginPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
