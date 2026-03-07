using Android.App;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.View;

namespace TrainzInfoMAUI
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // 1. Зсуваємо WebView вниз, щоб він не ховався під шторку
            WindowCompat.SetDecorFitsSystemWindows(Window, true);

            // 2. Фарбуємо системну шторку (фон під годинником) у колір твоєї CSS-шапки
            Window!.SetStatusBarColor(Android.Graphics.Color.ParseColor("#1e1e1e"));

            // 3. Робимо системні іконки (годинник, батарея, Wi-Fi) світлими
            WindowCompat.GetInsetsController(Window, Window.DecorView).AppearanceLightStatusBars = false;
        }
    }
}
