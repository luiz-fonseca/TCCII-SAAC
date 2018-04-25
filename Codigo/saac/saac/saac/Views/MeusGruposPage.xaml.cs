using saac.Helpers;
using Xamarin.Forms;

namespace saac.Views
{
    public partial class MeusGruposPage : ContentPage
    {
        public MeusGruposPage()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.Android)
                adMobView.AdUnitId = Settings.IdAdMobAndroid;
        }
    }
}
