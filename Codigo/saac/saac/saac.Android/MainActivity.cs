using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.Gms.Ads;
using Android.OS;
using Prism;
using Prism.Ioc;
using saac.Droid.Services;
using saac.Interfaces;
using Android.Runtime;
using Xamarin.Essentials;

namespace saac.Droid
{
    [Activity(Label = "saac", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            //plugin do Azure
            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();

            //Plugin de dialogs
            UserDialogs.Init(this);

            //plugin de publicidade
            MobileAds.Initialize(ApplicationContext, Preferences.Get("publicidade", ""));

            //Xamarin Essentials
            Xamarin.Essentials.Platform.Init(this, bundle);

            LoadApplication(new App(new AndroidInitializer()));
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry container)
        {
            container.Register<IFacebookLogout, FacebookLogout>();
        }
    }
}

