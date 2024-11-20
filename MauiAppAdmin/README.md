# MauiApp Admin .NET 8

## Edit SVG Icons
* [Photopea](https://www.photopea.com/)
* [Maui Controls](https://learn.microsoft.com/en-us/dotnet/maui/user-interface/controls/?view=net-maui-8.0)

## Dependencies
```
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
* Modify icon logo on welcome screen
```
.\Resources\Splash\splash.svg (Photopea)
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
