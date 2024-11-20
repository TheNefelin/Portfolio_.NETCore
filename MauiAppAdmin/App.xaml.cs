using MauiAppAdmin.Pages;

namespace MauiAppAdmin
{
    public partial class App : Application
    {
        public static IServiceProvider _serviceProvider;

        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _serviceProvider = serviceProvider;
            App.Current.UserAppTheme = AppTheme.Dark;

            var loginPage = App._serviceProvider.GetService<LoginPage>();
            MainPage = loginPage;
        }
    }
}
