using CommunityToolkit.Maui;
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
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Dependencies Injection
            builder.Services.AddSingleton<IBiometric>(BiometricAuthenticationService.Default);
            builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7005/") });
            //builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri("https://artema.bsite.net/") });

            builder.Services.AddSingleton<ApiAuthService>();
            builder.Services.AddSingleton<ApiCoreService>();

            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<CorePage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
