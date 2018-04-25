using saac.Helpers;
using Xamarin.Forms;

namespace saac.Views
{
    public partial class NotificacoesPage : ContentPage
    {
        public NotificacoesPage()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.Android)
                adMobView.AdUnitId = Settings.IdAdMobAndroid;

        }
    }
}
