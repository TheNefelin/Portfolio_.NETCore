# MauiApp Admin .NET 8

## Menu
- [Go to ToDo](#todo)
- [Go to Folder Structure](#folder-structure)
- [Go to Edit SVG Icons](#edit-svg-icons)
- [Go to Dependencies](#dependencies)
- [Go to Color Palette](#color-palette)
- [Go to Config](#config)
- [Go to Docs](#docs)
- [Go to HttpClient](#httpclient)
- [Go to XAML](#xaml)
- [Go to Modal with Data](#modal-with-data)

<hr>

## ToDo
- [X] Password Manager
- [ ] Metronome 
- [ ] Tuner
- [ ] Converter to .webp
- [ ] Reactive Chat
- [ ] Portfolio Manager
- [ ] Project Manager

## Folder Structure
```
/
├── MauiAdmin/
│	├── Pages/
│	│	└── LoginPage.xaml
│	├── Resources/
│	│	├── AppIcon/
│	│	│	├── appicon.svg
│	│	│	└── appiconfg.svg
│	│	├── Fonts/
│	│	│	├── OpenSans-Regular.ttf
│	│	│	└── OpenSans-Semibold.ttf
│	│	├── Images/
│	│	│	└── dotnet_bot.svg
│	│	├── Raw/
│	│	│	└── AboutAssets.txt
│	│	├── Splash/
│	│	│	└── splash.svg
│	│	└── Styles/
│	│		├── Colors.xaml
│	│		└── Styles.xam
│	├── App.xaml
│	├── AppShell.xaml
│	├── MainPage.xaml
│	├── MauiProgram.cs
│	└── README.md
└── /
```

## Edit SVG Icons
* [Photopea](https://www.photopea.com/)
* [Maui Controls](https://learn.microsoft.com/en-us/dotnet/maui/user-interface/controls/?view=net-maui-8.0)

## Dependencies
```
ClassLibrary_DTOs
CommunityToolkit.Maui
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

### Single container
```
<Button />
<Label />
<Image />
<ContentPage />
<Frame />
<ScrollView />
```

### Multiple container
```
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

### CommunityToolkit.Maui
* Add servive on Build
```
builder
    .UseMauiApp<App>()
    .UseMauiCommunityToolkit()
    ...
```
* On xaml file
```
xmlns:toolkit="http://schemas.microsoft.com/dotnet/2022/maui/toolkit"
```

# Modal with Data
* **Create a `ContentPage`**  
* **Define a `TaskCompletionSource` attribute**  
```
private TaskCompletionSource<string> _taskCompletionSource;
```
* Initialize the attribute in the constructor
```
public ConstructorName()
{
    InitializeComponent();
    _taskCompletionSource = new TaskCompletionSource<string>();
}
```
* Set the result value
```
_taskCompletionSource.SetResult("SomeValue");
```
* Create a method to return the result
```
public Task<string> GetPasswordAsync()
{
    return _taskCompletionSource.Task;
}
```
* If its there no result? then...
```
protected override void OnDisappearing()
{
    base.OnDisappearing();

    if (!_tcsCoreDTO.Task.IsCompleted)
    {
        _tcsCoreDTO.SetResult(null);
    }
}
```

### LoginPage
```
<Grid RowDefinitions=".2*,.8*">
    <Image Aspect="AspectFill" Source="background.jpg" Grid.RowSpan="2"></Image>

    <Grid Grid.Row="1">
        <RoundRectangle Opacity=".8" CornerRadius="30,30,0,0" Fill="black">
        </RoundRectangle>

        <VerticalStackLayout Margin="30,0,30,0">
            <Label TextColor="White" FontFamily="RobotoBold" FontAttributes="Bold" FontSize="Medium" Margin="0,30,0,0" Text="Iniciar Sesión"></Label>
            <Label TextColor="Gray" FontFamily="RobotoRegular" Text="Admin"></Label>

            <Grid Margin="0,30,0,0" Padding="5" ColumnDefinitions=".2*,.8*">
                <RoundRectangle 
                    Fill="#EDEBF6"
                    CornerRadius="10"
                    HeightRequest="60"
                    HorizontalOptions="Center"
                    VerticalOptions="Center"
                    WidthRequest="60">
                </RoundRectangle>

                <MauiImage Include="Resources\Images\user.png" Resize="false" />
                
        
                <Entry Margin="5,0,0,0" Grid.Column="1" Placeholder="EMAIL" VerticalOptions="Center"></Entry>
            </Grid>

            <Grid Padding="5" ColumnDefinitions=".2*,.8*">
                <RoundRectangle 
                    Fill="#EDEBF6"
                    CornerRadius="10"
                    HeightRequest="60"
                    HorizontalOptions="Center"
                    VerticalOptions="Center"
                    WidthRequest="60">
                </RoundRectangle>

                <Image
                    Source="dotnet_bot.png"
                    SemanticProperties.Description="Cute dot net bot waving hi to you!"
                    HorizontalOptions="Center" />

                <Entry Margin="5,0,0,0" Grid.Column="1" Placeholder="PASSWORD" IsPassword="True" VerticalOptions="Center"></Entry>
            </Grid>

            <Button Margin="0,30,0,0" Text="Iniciar Sesión"></Button>
            <Button Margin="0,30,0,0" Text="Registrarse" Background="Black"></Button>
        </VerticalStackLayout>
    </Grid>
</Grid>
```
