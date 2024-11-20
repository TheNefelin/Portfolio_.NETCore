# MauiApp Admin .NET 8

## Edit SVG Icons
* [Photopea](https://www.photopea.com/)
* [Maui Controls](https://learn.microsoft.com/en-us/dotnet/maui/user-interface/controls/?view=net-maui-8.0)

## Dependencies
```
ClassLibrary_DTOs
Newtonsoft.Json
Plugin.Maui.Biometric
```

## Color Palette
```
backgroundDarkHard = HEX: #000000, RGB: 0,0,0:
backgroundDarkHard = HEX: #000000, RGB: 0,0,0:
backgroundDarkHard = HEX: #000000, RGB: 0,0,0:
backgroundDarkHard = HEX: #000000, RGB: 0,0,0:
```

## Config
### Change top bar color
* MainActivity.cs (Android)
```
protected override void OnCreate(Bundle savedInstanceState)
{
    base.OnCreate(savedInstanceState);
    Window.SetStatusBarColor(Android.Graphics.Color.ParseColor("#000000"));
    Window.SetNavigationBarColor(Android.Graphics.Color.ParseColor("#000000"));
}
```
* AppDelegate.cs (iOS)
```
public override bool FinishedLaunching(UIApplication app, NSDictionary options)
{
    UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, false);
    UINavigationBar.Appearance.BarTintColor = UIColor.FromRGB(0, 0, 0); // Cambia a tu color deseado
    UITabBar.Appearance.BarTintColor = UIColor.FromRGB(0, 0, 0); // Cambia a tu color deseado
    return base.FinishedLaunching(app, options);
}
```

### Change welcome screen logo and background color
* Modify icon logo on welcome screen MauiAppAdmin
```
.\Resources\Splash\splash.svg (Photopea)

<!-- Splash Screen -->
<MauiSplashScreen Include="Resources\Splash\thundercats.png" Color="#000000" />
```
* Modify icon app
```
icon: .\Resources\AppIcon\appiconfg.svg (Photopea)
background: .\Resources\AppIcon\appicon.svg (Photopea)
```
### Forze Dark or Light theme in App.xaml.cs
```
Application.Current.UserAppTheme = AppTheme.Light;
or
App.Current.UserAppTheme = AppTheme.Dark;
```

### Color
```
.\Platforms\Android\Resources\values\colors.xml
```
# Docs

## Biometric
* Add permission in AndroidManifest.xml (Android)
```
<uses-permission android:name="android.permission.USE_BIOMETRIC" />
```
* Add permission in Info.plist (iOS)
```
<key>NSFaceIDUsageDescription</key>
<string>Escanear Rostro</string>
```
* Add dependency injection in MauiProgram.cs
```
builder.Services.AddSingleton<IBiometric>(BiometricAuthenticationService.Default);
```
* Implementation
```
var result = await BiometricAuthenticationService.Default.AuthenticateAsync(
    new AuthenticationRequest()
    {
        Title = "Obtener Usuario",
        NegativeText = "Cancelar Autenticación"
    }, 
    CancellationToken.None
);

if (result.Status == BiometricResponseStatus.Success)
    await DisplayAlert("Naizu", "Usuario Recuperado", "Ok");
else
    await DisplayAlert("Error", result.ErrorMsg, "Ok");
```

## HttpClient
* Add dependency injection in MauiProgram.cs
```
builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7005/") });
```
* Add LoginPage as dependency injection in MauiProgram.cs
```
builder.Services.AddTransient<LoginPage>();
```
* If LoginPage is loaded on (App.xaml.cs), add Service Provider
```
public static IServiceProvider _serviceProvider;

public App(IServiceProvider serviceProvider)
{
    InitializeComponent();
    _serviceProvider = serviceProvider;
    MainPage = new NavigationPage(_serviceProvider.GetService<LoginPage>());
}
```
* Implementation
```
private readonly ApiAuthService _authService;

public LoginPage(ApiAuthService authService)
{
	InitializeComponent();
    _authService = authService;
}
```

## XAML
> [Maui Doc](https://learn.microsoft.com/es-es/dotnet/maui/whats-new/dotnet-8?view=net-maui-8.0)

* Single container
```
<Button />
<Label />
<Image />
<ContentPage />
<Frame />
<ScrollView />
```
* Multiple container
```
<ScrollView />
<StackLayout />
<FlexLayout />
<AbsoluteLayout />
<VerticalStackLayout />
<HorizontalStackLayout />

<CollectionView ItemsSource="{Binding Products}">
    <CollectionView.ItemsLayout>
        <GridItemsLayout Orientation="Vertical" />
    </CollectionView.ItemsLayout>

    <CollectionView.ItemTemplate>
        <DataTemplate>
            <!-- Aqui van lo Elementos a iterar de Products -->
        </DataTemplate>
    </CollectionView.ItemTemplate>
</CollectionView>
```

