using MauiAppAdmin.Pages;
using MauiAppAdmin.Services;

namespace MauiAppAdmin
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new LoadingPage();
                      
            ApiAuthService.RemoveUser();

            var toolbarItem = this.ToolbarItems.FirstOrDefault(item => item.Text == "Logout");
            if (toolbarItem != null)
            {
                this.ToolbarItems.Remove(toolbarItem);
            }

            var loginPage = App._serviceProvider.GetService<LoginPage>();
            App.Current.MainPage = loginPage;
        }
    }
}
