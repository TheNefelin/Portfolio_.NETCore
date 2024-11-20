using Foundation;
using UIKit;

namespace MauiAppAdmin
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, false);
            UINavigationBar.Appearance.BarTintColor = UIColor.FromRGB(0, 0, 0); // Cambia a tu color deseado
            UITabBar.Appearance.BarTintColor = UIColor.FromRGB(0, 0, 0); // Cambia a tu color deseado
            return base.FinishedLaunching(app, options);
        }
    }
}
